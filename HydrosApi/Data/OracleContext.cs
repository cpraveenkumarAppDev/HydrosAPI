namespace HydrosApi.Data
    //*** SHOULD BE REPLACED BY ADWRCONTEXT AND SDECONTEXT WHICH MORE CLOSELY DESCRIBES THEIR SERVER/SCHEMA LOCATIONS    
{
    using System;
    using System.Data.Entity;
    using HydrosApi.Models.Adjudication;
    using HydrosApi.Models.ADWR;
    using HydrosApi.Models.Permitting.AAWS;
    using Models;    

    
        public partial class OracleContext : DbContext
    {
        public OracleContext()
            : base("name=OracleContext")
        {

            base.Configuration.ProxyCreationEnabled = false;
        }
        public virtual DbSet<V_AWS_WQ> V_AWS_WQ { get; set; }

        public virtual DbSet<V_CD_AW_AMA_INA> V_CD_AW_AMA_INA { get; set; }
        public virtual DbSet<V_CD_AW_APP_FEE_RATES> V_CD_AW_APP_FEE_RATES { get; set; }
        public virtual DbSet<CD_AW_COMMENT_TYPE> CD_AW_COMMENT_TYPE { get; set; }

        public virtual DbSet<V_AWS_GENERAL_INFO> V_AWS_GENERAL_INFO { get; set; }
        public virtual DbSet<V_AWS_OAWS> V_AWS_OAWS { get; set; }

        public virtual DbSet<AW_APP_ACTIVITY_TRK> AW_APP_ACTIVITY_TRK { get; set; }
        public virtual DbSet<CD_AW_APP_ACTIVITY> CD_AW_APP_ACTIVITY { get; set; }


        public virtual DbSet<FILE> FILES { get; set; }
        public virtual DbSet<PROPOSED_WATER_RIGHT> PROPOSED_WATER_RIGHT { get; set; }
        public virtual DbSet<PWR_POD> PWR_POD { get; set; }
        public virtual DbSet<V_AWS_PROVIDER> V_AWS_PROVIDER { get; set; }
        public virtual DbSet<V_AWS_SUBBAS> V_AWS_SUBBAS { get; set; }
        public virtual DbSet<V_AWS_HYDRO> V_AWS_HYDRO { get; set; }
        public virtual DbSet<V_AWS_COUNTY_BASIN> V_AWS_COUNTY_BASIN { get; set; }
        public virtual DbSet<CD_AW_COUNTY> CD_AW_COUNTY { get; set; }
        public virtual DbSet<AW_USERS> AW_USERS { get; set; }
        public virtual DbSet<POINT_OF_DIVERSION_VIEW> POINT_OF_DIVERSION_VIEW { get; set; }

        public virtual DbSet<WTR_RIGHT_FACILITY> WTR_RIGHT_FACILITY { get; set; }
        //public virtual DbSet<EXPLANATIONS> EXPLANATIONS { get; set; }
        //public virtual DbSet<WATERSHED_FILE_REPORT> WATERSHED_FILE_REPORT { get; set; }

        /*public virtual DbSet<CD_AQUIFER_TYPE> CD_AQUIFER_TYPE { get; set; }
        public virtual DbSet<CD_DRILLERS_LOG> CD_DRILLERS_LOG { get; set; }
        public virtual DbSet<CD_FORMATIONS> CD_FORMATIONS { get; set; }
        public virtual DbSet<CD_GEO_UNITS> CD_GEO_UNITS { get; set; }
        public virtual DbSet<CD_HYDROGEOLOGIC_UNIT> CD_HYDROGEOLOGIC_UNIT { get; set; }
        public virtual DbSet<CD_LITHO_STRAT> CD_LITHO_STRAT { get; set; }
        public virtual DbSet<CD_LITHO_STRAT_TYPE> CD_LITHO_STRAT_TYPE { get; set; }
        public virtual DbSet<CD_LOCAL_AQUIFER> CD_LOCAL_AQUIFER { get; set; }
        public virtual DbSet<CD_LOG_QUALITY> CD_LOG_QUALITY { get; set; }
        public virtual DbSet<CD_LOG_TYPE> CD_LOG_TYPE { get; set; }
        public virtual DbSet<CD_MAP_UNITS> CD_MAP_UNITS { get; set; }
        public virtual DbSet<CD_PRINCIPAL_AQUIFER> CD_PRINCIPAL_AQUIFER { get; set; }
        public virtual DbSet<CD_SECONDARY_AQUIFER> CD_SECONDARY_AQUIFER { get; set; }
        public virtual DbSet<CD_TERTIARY_AQUIFER> CD_TERTIARY_AQUIFER { get; set; }
        public virtual DbSet<CD_USCS> CD_USCS { get; set; }
        public virtual DbSet<LAYER> LAYERS { get; set; }
        public virtual DbSet<LOG_EVENTS> LOG_EVENTS { get; set; }
        public virtual DbSet<WELLS_VIEW> WELLS_VIEW { get; set; }
        public virtual DbSet<SW_AIS_VIEW> SW_AIS_VIEW { get; set; }
        public virtual DbSet<SOC_AIS_VIEW> SOC_AIS_VIEW { get; set; }
        public virtual DbSet<V_AWS_GENERAL_INFO> V_AWS_GENERAL_INFO { get; set; }
        
        public virtual DbSet<SOC_AIS_VIEW> SOC_AIS_VIEW { get; set; }*/


        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EXPLANATIONS>()
                .Property(e => e.EXP_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATIONS>()
                .Property(e => e.LOCATION)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATIONS>()
                .Property(e => e.EXPLANATION)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATIONS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATIONS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.LOCATION)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.ORIGINAL_FILE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<FILE>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.POU_ID)
                .IsUnicode(false);

         

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .HasMany(e => e.PWR_POD)
                .WithOptional(e => e.PROPOSED_WATER_RIGHT)
                .HasForeignKey(e => e.PWR_ID);

            modelBuilder.Entity<PWR_POD>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<PWR_POD>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<WATERSHED_FILE_REPORT>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<WATERSHED_FILE_REPORT>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<WATERSHED_FILE_REPORT>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_AQUIFER_TYPE>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_AQUIFER_TYPE>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_AQUIFER_TYPE>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_AQUIFER_TYPE>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_AQUIFER_TYPE>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_AQUIFER_TYPE)
            //    .HasForeignKey(e => e.AT_CODE);

            modelBuilder.Entity<CD_DRILLERS_LOG>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_DRILLERS_LOG>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_DRILLERS_LOG>()
                .Property(e => e.CATEGORY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_DRILLERS_LOG>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_DRILLERS_LOG>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_DRILLERS_LOG>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_DRILLERS_LOG)
            //    .HasForeignKey(e => e.DRL_CODE);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.FORMATION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.AGE_MA)
                .HasPrecision(25, 2);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.MU_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.GU_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.ROCK_TYPE1)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.ROCK_TYPE2)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_FORMATIONS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.AGE_MYA)
                .HasPrecision(25, 2);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.SUPEREON)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.EON)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.ERA)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.PERIOD_SYSTEM)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.EPOCH_SERIES)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_GEO_UNITS>()
                .HasMany(e => e.CD_LITHO_STRAT)
                .WithOptional(e => e.CD_GEO_UNITS)
                .HasForeignKey(e => e.GU_CODE);

            modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_HYDROGEOLOGIC_UNIT>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_HYDROGEOLOGIC_UNIT)
            //    .HasForeignKey(e => e.HU_CODE);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.AGE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.MU_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.GU_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.ROCK_TYPE1)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.ROCK_TYPE2)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_LITHO_STRAT>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_LITHO_STRAT)
            //    .HasForeignKey(e => e.LS_CODE);

            modelBuilder.Entity<CD_LITHO_STRAT_TYPE>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT_TYPE>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT_TYPE>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LITHO_STRAT_TYPE>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOCAL_AQUIFER>()
                .Property(e => e.USGS_CODE)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_LOCAL_AQUIFER>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_LOCAL_AQUIFER)
            //    .HasForeignKey(e => e.LAQ_CODE);

            modelBuilder.Entity<CD_LOG_QUALITY>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_QUALITY>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_QUALITY>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_QUALITY>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_QUALITY>()
                .HasMany(e => e.LOG_EVENTS)
                .WithOptional(e => e.CD_LOG_QUALITY)
                .HasForeignKey(e => e.LOG_QUALITY_CODE);

            modelBuilder.Entity<CD_LOG_TYPE>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_TYPE>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_TYPE>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_TYPE>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_LOG_TYPE>()
                .HasMany(e => e.LOG_EVENTS)
                .WithRequired(e => e.CD_LOG_TYPE)
                .HasForeignKey(e => e.LOG_TYPE_CODE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.AGE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.ROCK_TYPE1)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.ROCK_TYPE2)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.AGE_MA)
                .HasPrecision(25, 2);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .Property(e => e.MAP_UNIT)
                .IsUnicode(false);

            modelBuilder.Entity<CD_MAP_UNITS>()
                .HasMany(e => e.CD_LITHO_STRAT)
                .WithOptional(e => e.CD_MAP_UNITS)
                .HasForeignKey(e => e.MU_CODE);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.USGS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_PRINCIPAL_AQUIFER>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_PRINCIPAL_AQUIFER)
            //    .HasForeignKey(e => e.PAQ_CODE);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.USGS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_SECONDARY_AQUIFER>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_SECONDARY_AQUIFER)
            //    .HasForeignKey(e => e.SAQ_CODE);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.USGS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_TERTIARY_AQUIFER>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_TERTIARY_AQUIFER)
            //    .HasForeignKey(e => e.TAQ_CODE);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.DIVISION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.MAJOR_DIVISION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<CD_USCS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            //modelBuilder.Entity<CD_USCS>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.CD_USCS)
            //    .HasForeignKey(e => e.USCS_CODE);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.LS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.PAQ_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.AT_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.SAQ_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.LAQ_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.HU_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.FINES)
                .HasPrecision(7, 3);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.SAND)
                .HasPrecision(7, 3);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.GRAVEL)
                .HasPrecision(7, 3);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.MED_SIZE)
                .HasPrecision(7, 3);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.MAX_SIZE)
                .HasPrecision(7, 3);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.DRL_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.USCS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.TAQ_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.LITHO_STRAT_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<LAYER>()
                .Property(e => e.WATER_ENCOUNTERED)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.WELL_ID)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.WELL_SOURCE)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.LOG_TYPE_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.LOG_QUALITY_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.UPDATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<LOG_EVENTS>()
                .Property(e => e.DRILLER_COMMENTS)
                .IsUnicode(false);

            //modelBuilder.Entity<LOG_EVENTS>()
            //    .HasMany(e => e.LAYERS)
            //    .WithOptional(e => e.LOG_EVENTS)
            //    .HasForeignKey(e => e.LOG_EVENT_ID);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.PROGRAM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.FILE_NO)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.OWNER)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.CADASTRAL)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.PCC)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.REGISTRY_ID)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.WSHD_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<WELLS_VIEW>()
                .Property(e => e.WELL_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<SW_AIS_VIEW>()
                .Property(e => e.ART_PROGRAM)
                .IsUnicode(false);

            modelBuilder.Entity<SW_AIS_VIEW>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SW_AIS_VIEW>()
                .Property(e => e.PCC)
                .IsUnicode(false);

            modelBuilder.Entity<SW_AIS_VIEW>()
                .Property(e => e.WS_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<SW_AIS_VIEW>()
                .Property(e => e.USE)
                .IsUnicode(false);

            modelBuilder.Entity<SOC_AIS_VIEW>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SOC_AIS_VIEW>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SOC_AIS_VIEW>()
                .Property(e => e.USE)
                .IsUnicode(false);

            modelBuilder.Entity<SOC_AIS_VIEW>()
                .Property(e => e.WS)
                .IsUnicode(false);

            modelBuilder.Entity<SOC_AIS_VIEW>()
                .Property(e => e.PCC)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<PLACE_OF_USE_VIEW> PLACE_OF_USE_VIEW { get; set; }*/
    }
}
