﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjektSemestralny
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarDealerManagementDBEntities : DbContext
    {
        public CarDealerManagementDBEntities()
            : base("name=CarDealerManagementDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TB_ADDRESS> TB_ADDRESS { get; set; }
        public virtual DbSet<TB_CAR> TB_CAR { get; set; }
        public virtual DbSet<TB_CAR_COLOR> TB_CAR_COLOR { get; set; }
        public virtual DbSet<TB_CAR_CONDITION> TB_CAR_CONDITION { get; set; }
        public virtual DbSet<TB_CAR_COUNTRY> TB_CAR_COUNTRY { get; set; }
        public virtual DbSet<TB_CAR_MODEL> TB_CAR_MODEL { get; set; }
        public virtual DbSet<TB_CLIENT> TB_CLIENT { get; set; }
        public virtual DbSet<TB_TRANSACTIONS> TB_TRANSACTIONS { get; set; }
    }
}
