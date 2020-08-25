namespace HydrosApi.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using HydrosApi.Models;
    using AdwrApi.Models.Permitting.AAWS;

    public partial class ADWRContext : DbContext
    {
        public ADWRContext()
            : base("name=OracleContext")
        {

            base.Configuration.ProxyCreationEnabled = false;
        }
        
        public virtual DbSet<EXPLANATION> EXPLANATION { get; set; }
        public virtual DbSet<FILE> FILES { get; set; }
        public virtual DbSet<PROPOSED_WATER_RIGHT> PROPOSED_WATER_RIGHT { get; set; }
        public virtual DbSet<PWR_POD> PWR_POD { get; set; }
        public virtual DbSet<WATERSHED_FILE_REPORT> WATERSHED_FILE_REPORT { get; set; }
        public virtual DbSet<CD_AQUIFER_TYPE> CD_AQUIFER_TYPE { get; set; }
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



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EXPLANATION>()
                .Property(e => e.EXP_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATION>()
                .Property(e => e.LOCATION)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATION>()
                .Property(e => e.EXPLANATION1)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATION>()
                .Property(e => e.CREATEBY)
                .IsUnicode(false);

            modelBuilder.Entity<EXPLANATION>()
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

           /* modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.WATER_STRUCTURE_FAC_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.WATER_STRUCTURE_FAC_CAP)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.WATER_STRUCTURE_FAC_PUR)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.WATER_REUSE_PROGRAM)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.DISCHARGE_OF_WAISTWATER)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.TREATMENT_OF_WAISTWATER)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.PERIODS_OF_INACTIVITY)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.EXPANSION_PLANS)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.EXPANSION_EXPLANATION)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.DRILL_DATE)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.DEPTH)
                .HasPrecision(10, 2);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.ADEQ_ID)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.PWS_ID_NO)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.CWS_ID_NO)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.ACC_DOC_NO)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.ACTIVE)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.CTY_DOC_NO)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.OLD_WFR)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.DIVERSION_STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<PROPOSED_WATER_RIGHT>()
                .Property(e => e.STATUS_SOURCE)
                .IsUnicode(false);*/

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

        public System.Data.Entity.DbSet<HydrosApi.Models.PLACE_OF_USE_VIEW> PLACE_OF_USE_VIEW { get; set; }
    }
}
