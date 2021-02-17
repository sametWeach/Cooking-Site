namespace MvcYemek.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Tarif")]
    public partial class Tarif
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tarif()
        {
            Yorum = new HashSet<Yorum>();
            Resim1 = new HashSet<Resim>();
            Etiket1 = new HashSet<Etiket>();
        }

        public int TarifID { get; set; }

        [Required]
        [StringLength(50)]
        public string Baslik { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        [AllowHtml]
        [Required]
        public string Malzemeler { get; set; }

        [AllowHtml]
        [Required]
        public string Yapilis { get; set; }

        public byte Hazirlanma { get; set; }

        public byte Pisirme { get; set; }

        public byte KacKisi { get; set; }

        [StringLength(500)]
        public string Notlar { get; set; }

        public byte? Puan { get; set; }

        public DateTime YayınTarihi { get; set; }

        public int KategoriID { get; set; }

        public int? EtiketID { get; set; }

        public int? KullaniciID { get; set; }

        public int? YoneticiID { get; set; }

        public int KapakResimID { get; set; }

        public int? Goruntulenme { get; set; }

        public virtual Etiket Etiket { get; set; }

        public virtual Kategori Kategori { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Resim Resim { get; set; }

        public virtual Yonetici Yonetici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorum> Yorum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resim> Resim1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Etiket> Etiket1 { get; set; }
    }
}
