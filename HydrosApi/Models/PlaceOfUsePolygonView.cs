namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Spatial;
    using System.Web.UI.WebControls;
    using Microsoft.SqlServer.Types;


    [Table("ADJ.POU_POLYGONS_ARC")]
    public partial class PlaceOfUsePolygonView
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlaceOfUsePolygonView()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        public string DWR_ID { get; set; }


        public string CADASTRAL { get; set; }

        public string POLYGONS { get; set; }
        public string POLYGON_JSON { get; set; }

        public string USE { get; set; }

         

        [NotMapped]
        public SqlGeometry SHAPE
        {

            get => SqlGeometry.STGeomFromText(new SqlChars(new SqlString(POLYGONS)), 4326);
            set
            {
                SHAPE = value;
            }

        }









        //var geometry = SqlGeometry.STGeomFromText(shape, 26912);
    }
}
