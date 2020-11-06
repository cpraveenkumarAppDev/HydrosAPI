namespace HydrosApi.Data
{     
    using System.Data.Entity;
    using Models;

    public partial class SDEContext : DbContext
    {
        public SDEContext()
            : base("name=SDEContext")
        {

            base.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<PLACE_OF_USE_VIEW> PLACE_OF_USE_VIEW { get; set; }


        public virtual DbSet<POINT_OF_DIVERSION> POINT_OF_DIVERSION { get; set; }

        public virtual DbSet<WATERSHED_FILE_REPORT_SDE> WATERSHED_FILE_REPORT_SDE { get; set; }

    }

}
