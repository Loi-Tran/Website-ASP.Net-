namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Slider")]
    public partial class Slider
    {
        
        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public DateTime? CreatedDate { get; set; }

      
        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    
        public int? ModifiedBy { get; set; }

        public int? Status { get; set; }
    }
}
