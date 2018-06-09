using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RTMGMTCore2.Models
{
    public partial class RTMGMTContext : DbContext
    {
        public virtual DbSet<BottomUpProcessedRecords> BottomUpProcessedRecords { get; set; }
        public virtual DbSet<BrokenRecords> BrokenRecords { get; set; }
        public virtual DbSet<DuplicateRtrecords> DuplicateRtrecords { get; set; }
        public virtual DbSet<FixRecords> FixRecords { get; set; }
        public virtual DbSet<ReportsToRecords> ReportsToRecords { get; set; }
        public virtual DbSet<ReportsToRecordStagings> ReportsToRecordStagings { get; set; }
        public virtual DbSet<RequiredCorrectionsSet> RequiredCorrectionsSet { get; set; }
        public virtual DbSet<TopDownProcessedRecords> TopDownProcessedRecords { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer(@"Server=WIN-CORE-I7\VS2017;Database=RTMGMT;Trusted_Connection=True;");
        //            }
        //        }

        public RTMGMTContext(DbContextOptions<RTMGMTContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BottomUpProcessedRecords>(entity =>
            {
                entity.Property(e => e.RtString)
                    .IsRequired()
                    .HasColumnName("RT_STRING")
                    .HasMaxLength(255)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<BrokenRecords>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.RtString)
                    .HasColumnName("RT_STRING")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<DuplicateRtrecords>(entity =>
            {
                entity.ToTable("DuplicateRTRecords");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<FixRecords>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.RtString)
                    .HasColumnName("RT_STRING")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ReportsToRecords>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ReportsToRecordStagings>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RequiredCorrectionsSet>(entity =>
            {
                entity.Property(e => e.ReportingId)
                    .IsRequired()
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ReportsToId)
                    .IsRequired()
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TopDownProcessedRecords>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .HasColumnName("EMPLOYEE_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportLevel).HasColumnName("REPORT_LEVEL");

                entity.Property(e => e.ReportingId)
                    .HasColumnName("REPORTING_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.ReportsToId)
                    .HasColumnName("REPORTS_TO_ID")
                    .HasMaxLength(20);

                entity.Property(e => e.RtString)
                    .HasColumnName("RT_STRING")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("TITLE")
                    .HasMaxLength(50);
            });
        }
    }
}
