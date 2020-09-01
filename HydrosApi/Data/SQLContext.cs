 
 
using System.Data.Entity;
 
using HydrosApi.Models;
 

namespace HydrOS.Data
{
    public partial class SqlContext : DbContext
    {
        public SqlContext()
            : base("name=SqlContext")
        {
        }

        public virtual DbSet<SW_AIS_VIEW> SW_AIS_VIEW { get; set; }
        public virtual DbSet<SOC_AIS_VIEW> SOC_AIS_VIEW { get; set; }

        public virtual DbSet<WELLS_VIEW> WELLS_VIEW { get; set; }

    }
}