using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NTierOracleIdentityExample.Dll.Entities;

namespace NTierOracleIdentityExample.Dll.Context
{
    public partial class EXAMPLE_SCHEMA_Context : IdentityDbContext<ApplicationUser>
    {
        #region Fields
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public EXAMPLE_SCHEMA_Context(DbContextOptions<EXAMPLE_SCHEMA_Context> options, IConfiguration config) : base(options)
        {
            _config = config;
        }
        public EXAMPLE_SCHEMA_Context(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region DbSet Fields
        public virtual DbSet<ExampleTable> ExampleTables { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseOracle(_config["ConnectionStrings:EXAMPLE_SCHEMA"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("EXAMPLE_SCHEMA")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<ExampleTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EXAMPLE_TABLE");

                entity.HasIndex(e => e.Pk, "IX_EXAMPLE_TABLE")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATION_DATE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pk)
                    .HasPrecision(10)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PK");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.pk).HasName("LOG_PK");
                entity.ToTable("LOG");
                entity.Property(e => e.pk)
                    .HasColumnType("NUMBER(10)")
                    .HasColumnName("PK");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATION_DATE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Value)
                    .IsUnicode(false)
                    .HasColumnName("LOG_VALUE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        #endregion
    }
}
