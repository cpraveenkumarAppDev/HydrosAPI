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
 

    [Table("ADJ_INV.TEST_FILE_UPLOAD")]
    public partial class TEST_FILE_UPLOAD : AdwrRepository<TEST_FILE_UPLOAD>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(100)]
        public string DESCR { get; set; }

        public byte[] FILE_BLOB { get; set; }

        [StringLength(50)]
        public string TYPE { get; set; }

        [StringLength(50)]
        public string MIME_TYPE { get; set; }

        public DateTime? CREATEDT { get; set; }

        public static TEST_FILE_UPLOAD UploadFile(HandleForm provider)
        {
            if (provider.Files != null)
            {
                var file = provider.Files[0];
                
                var mimeType = file.Headers.ContentType.MediaType;
                var isImage = mimeType.ToLower().IndexOf("image/") == 0 ? true : false;
                                 
                byte[] fileBlob = file.ReadAsByteArrayAsync().Result;
                var form = provider.FormData;

                var fileUpload = new TEST_FILE_UPLOAD()
                {
                    DESCR = file.Headers.ContentDisposition.FileName.Trim('\"'),
                    FILE_BLOB = fileBlob,
                    TYPE = Path.GetExtension(file.Headers.ContentDisposition.FileName.Trim('\"').ToLower()),
                    MIME_TYPE = mimeType,
                    CREATEDT =  DateTime.Now
                };

                TEST_FILE_UPLOAD.Add(fileUpload);

                return fileUpload;
            }

            return null;
        }
    }
}
