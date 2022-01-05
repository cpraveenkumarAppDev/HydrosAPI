namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;
    using System.Linq;
    using System.IO;
    using System.Configuration;
    using System.ComponentModel;

    [Table("ADJ_INV.FILES")]
    public partial class FILE:AdwrRepository<FILE>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ID { get; set; }

        public int? WFR_ID { get; set; }
        public int? POD_ID { get; set; }

        public int? PWR_ID { get; set; }
        public byte[] FILE_DATA { get; set; }
        [StringLength(100)]
        public string ORIGINAL_FILE_NAME { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }
        [StringLength(2000)]
        public string TYPE { get; set; }

        [StringLength(2000)]
        public string LOCATION { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string MIME_TYPE { get; set; }               

        [NotMapped]
        public string STATUS { get; set; }

        [NotMapped]
        public bool DeleteRecord { get; set; }



        public static FILE FileValues(HandleForm provider, string User)
        {
            var uploadFilePath = @"" + ConfigurationManager.AppSettings["FileUploadLocation"];

            var fileInfo = new FILE();
            string fieldName;
            //string newFileName="";            

            var form = provider.FormData;

            //get all the form data (ie not the file stream yet)
            //------------------------------------------------------------------
            //eventually move this block into a class or function so it can be used anywhere.
            //also use it for non-multipart/form-data types

            foreach (var key in form)
            {
                var keyValue = form.GetValues(key.ToString()).FirstOrDefault();
                fieldName = key.ToString().Trim('\"');
                var property = fileInfo.GetType().GetProperty(fieldName);

               
                

                if (property != null)
                {
                    //Convert the form value to the correct data type
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    try
                    {
                        property.SetValue(fileInfo, converter.ConvertFrom(keyValue));
                    }
                    catch
                    {
                        fileInfo.STATUS = String.Format("The value '{0}' provided for {1} is not valid. It should be a {2}", keyValue, fieldName, property.PropertyType);
                        return fileInfo;
                    }
                }
            }

            //--end eventually move this into a class or function so it can be used anywhere
            //-------------------------------------------------------------------------------

            if (fileInfo.PWR_ID == null && fileInfo.WFR_ID == null && fileInfo.POD_ID == null || provider.Files.Count() == 0)
            {
                fileInfo = new FILE();
                fileInfo.STATUS = fileInfo.PWR_ID == null || fileInfo.WFR_ID == null || fileInfo.POD_ID == null ? "An ID for Proposed Water Right or Water Facility Right was not provided." : "Please select a valid file.";
                return fileInfo;
            }


            if (fileInfo.ID > 0)
            {
                fileInfo.UPDATEBY = User;
                fileInfo.UPDATEDT = DateTime.Now;
            }

            else
            {
                fileInfo.CREATEBY = User;
                fileInfo.CREATEDT = DateTime.Now;

            }


            //FILE.Add(fileInfo);

            foreach (var file in provider.Files)
            {

                var mimeType = file.Headers.ContentType.MediaType;
                var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                var originalFileName = fileName == "blob" ? fileInfo.ORIGINAL_FILE_NAME : fileName;

                byte[] fileData = file.ReadAsByteArrayAsync().Result;

                fileInfo.ORIGINAL_FILE_NAME = originalFileName;
                fileInfo.FILE_DATA = fileData;
                fileInfo.TYPE = Path.GetExtension(originalFileName.ToLower());
                fileInfo.MIME_TYPE = mimeType;

                //FILE.Update(fileInfo);
            }

            
            return fileInfo;
        }
    
        public static FILE UploadFile(HandleForm provider, string User)
        {

            var uploadFilePath = @"" + ConfigurationManager.AppSettings["FileUploadLocation"];
                         
            var fileInfo = new FILE();            
            string fieldName;
            //string newFileName="";            

            var form = provider.FormData;

            //get all the form data (ie not the file stream yet)
            //------------------------------------------------------------------
            //eventually move this block into a class or function so it can be used anywhere.
            //also use it for non-multipart/form-data types

            foreach (var key in form)
            {
                var keyValue = form.GetValues(key.ToString()).FirstOrDefault();
                fieldName = key.ToString().Trim('\"');
                var property = fileInfo.GetType().GetProperty(fieldName);

                //Convert the form value to the correct data type
                 
                if (property != null)
                {
                    try
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        property.SetValue(fileInfo, converter.ConvertFrom(keyValue));
                    }
                    catch
                    {
                        fileInfo.STATUS = String.Format("The value '{0}' provided for {1} is not valid. It should be a {2}", keyValue,fieldName,property.PropertyType);
                        return fileInfo;
                    }
                }
            }

            //--end eventually move this into a class or function so it can be used anywhere
            //-------------------------------------------------------------------------------

            if (fileInfo.PWR_ID==null && fileInfo.WFR_ID == null &&  fileInfo.POD_ID == null || provider.Files.Count() == 0) 
            {
                fileInfo = new FILE();
                fileInfo.STATUS = fileInfo.PWR_ID==null || fileInfo.WFR_ID == null || fileInfo.POD_ID == null ? "An ID for Proposed Water Right or Water Facility Right was not provided." : "Please select a valid file.";
                return fileInfo;
            }

            if (fileInfo.ID > 0)
            {
                fileInfo.UPDATEBY = User;
                fileInfo.UPDATEDT = DateTime.Now;
            }

            else
            {
                fileInfo.CREATEBY = User;
                fileInfo.CREATEDT = DateTime.Now;

            }

            FILE.Add(fileInfo);

            foreach (var file in provider.Files)            { 

                var mimeType = file.Headers.ContentType.MediaType;
                var originalFileName = file.Headers.ContentDisposition.FileName.Trim('\"');

                byte[] fileData= file.ReadAsByteArrayAsync().Result;

                fileInfo.ORIGINAL_FILE_NAME = originalFileName;
                fileInfo.FILE_DATA = fileData;
                fileInfo.TYPE = Path.GetExtension(originalFileName.ToLower());
                fileInfo.MIME_TYPE = mimeType;

                FILE.Update(fileInfo);
            }

            //Create a descriptive filename to make removing orphans easier
            //by inserting the record and adding -FILE-{ID} at the end
            /* foreach (var file in provider.Files)
             {
                 var fileInput = file.ReadAsStreamAsync().Result;

                 fieldName = file.Headers.ContentDisposition.Name.Trim('\"');
                 var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');

                 var mimeType = file.Headers.ContentType.MediaType;

                 var field = fileInfo.GetType().GetProperty(fieldName);

                 if (field != null)
                 {
                     newFileName = Guid.NewGuid().ToString();

                     //Make the GUID a little bit smaller to accommodate the -FILE-{ID} value appended 
                     newFileName = newFileName.Substring(0,newFileName.LastIndexOf('-'))+"-FILE-"+fileInfo.ID.ToString(); 
                     var fieldType = field.PropertyType.Name;                    
                     field.SetValue(fileInfo, fileName);   

                     //field.SetValue(oneFile, fileInput);
                     fileInfo.TYPE = Path.GetExtension(fileName).ToLower();
                     fileInfo.LOCATION = Path.Combine(uploadFilePath, newFileName+fileInfo.TYPE);
                     using (Stream stream = File.OpenWrite(fileInfo.LOCATION))
                     {
                         fileInput.CopyTo(stream);
                         //close file  
                         stream.Close();
                     }
                     FILE.Update(fileInfo);
                 }
             }   */
            return fileInfo;                        
        }
    }
}
