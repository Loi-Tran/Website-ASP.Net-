namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Menu")]
    public partial class Menu
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Text { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(50)]
        public string TypeMenu { get; set; }
        [StringLength(50)]
        public string Position { get; set; }

        public int? Status { get; set; }

        public int? TableID { get; set; }
        public int? ParentID { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifieDate { get; set; }

        public int? ModifieBy { get; set; }
    }
}
