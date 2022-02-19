namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Supplier")]
    public partial class Supplier
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Slug { get; set; }

        [StringLength(250)]
        public string Img { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int? Orders { get; set; }

        public DateTime? CreatedDate { get; set; }


        public int? CreatedBy { get; set; }

        public DateTime? ModifieDate { get; set; }

 
        public int? ModifieBy { get; set; }

        public int? Status { get; set; }
    }
}
