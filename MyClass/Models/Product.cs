namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Slug { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }
        
        public decimal Price { get; set; }

        public decimal PromotionPrice { get; set; }

      

        public int? Quantity { get; set; }
        [Required]
        public int CategoryID { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

     

        public DateTime? CreatedDate { get; set; }

   
        public int? CreatedBy { get; set; }

        public DateTime? ModifieDate { get; set; }

    
        public int? ModifieBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        public int? Status { get; set; }

       

        public int? ViewCount { get; set; }
    
        public int SuppierId { get; set; }
    }
}
