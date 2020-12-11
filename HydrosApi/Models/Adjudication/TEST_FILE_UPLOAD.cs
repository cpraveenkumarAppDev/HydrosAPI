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


        public static TEST_FILE_UPLOAD UploadFile(HandleForm provider)
        {

            if(provider.Files != null)
            { 
                byte[] fileBlob=provider.Files[0].ReadAsByteArrayAsync().Result; 

                var fileUpload = new TEST_FILE_UPLOAD() {
                    DESCR = "Testing File Upload",
                    FILE_BLOB = fileBlob
                };
                try
                {
                    TEST_FILE_UPLOAD.Add(fileUpload);
                }
                catch (Exception exception)
                {
                    return fileUpload;
                }
                    return fileUpload;
            }

            return null;
        }
    }
}
