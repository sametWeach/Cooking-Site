namespace MvcYemek.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Resim")]
    public partial class Resim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resim()
        {
            Kategori = new HashSet<Kategori>();
            Kullanici = new HashSet<Kullanici>();
            Tarif = new HashSet<Tarif>();
            Tarif1 = new HashSet<Tarif>();
        }

        public int ResimID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; }

        [StringLength(500)]
        public string Kucukresimyol { get; set; }

        [StringLength(500)]
        public string Ortaresimyol { get; set; }

        [StringLength(500)]
        public string Buyukresimyol { get; set; }

        public int? KullanıcıID { get; set; }

        public int? YoneticiID { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kategori> Kategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici> Kullanici { get; set; }

        public virtual Yonetici Yonetici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tarif> Tarif { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tarif> Tarif1 { get; set; }
    }
}
