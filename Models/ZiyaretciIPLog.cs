namespace MvcYemek.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZiyaretciIPLog")]
    public partial class ZiyaretciIPLog
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string IpAddress { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Tarih { get; set; }
    }
}
