namespace MvcYemek.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        public int YorumID { get; set; }

        [StringLength(50)]
        public string Baslik { get; set; }

        [Required]
        [StringLength(100)]
        public string Icerik { get; set; }

        public int TarifID { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public int KullaniciID { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Tarif Tarif { get; set; }
    }
}
