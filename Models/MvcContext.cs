namespace MvcYemek.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MvcContext : DbContext
    {
        public MvcContext()
            : base("name=MvcContext")
        {
        }

        public virtual DbSet<Etiket> Etiket { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Resim> Resim { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tarif> Tarif { get; set; }
        public virtual DbSet<Yonetici> Yonetici { get; set; }
        public virtual DbSet<Yorum> Yorum { get; set; }
        public virtual DbSet<ZiyaretciIPLog> ZiyaretciIPLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Tarif)
                .WithOptional(e => e.Etiket)
                .HasForeignKey(e => e.EtiketID);

            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Tarif1)
                .WithMany(e => e.Etiket1)
                .Map(m => m.ToTable("TarifEtiket").MapLeftKey("EtiketID").MapRightKey("TarifID"));

            modelBuilder.Entity<Kategori>()
                .HasMany(e => e.Tarif)
                .WithRequired(e => e.Kategori)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Yorum)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resim>()
                .HasMany(e => e.Kategori)
                .WithOptional(e => e.Resim)
                .HasForeignKey(e => e.KategoriResim);

            modelBuilder.Entity<Resim>()
                .HasMany(e => e.Tarif)
                .WithRequired(e => e.Resim)
                .HasForeignKey(e => e.KapakResimID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resim>()
                .HasMany(e => e.Tarif1)
                .WithMany(e => e.Resim1)
                .Map(m => m.ToTable("TarifResim").MapLeftKey("ResimID").MapRightKey("TarifID"));

            modelBuilder.Entity<Tarif>()
                .HasMany(e => e.Yorum)
                .WithRequired(e => e.Tarif)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yonetici>()
                .Property(e => e.YoneticiSifre)
                .IsUnicode(false);

            modelBuilder.Entity<ZiyaretciIPLog>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);
        }
    }
}
