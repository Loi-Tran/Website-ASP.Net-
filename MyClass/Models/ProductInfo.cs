using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    public class ProductInfo
    {
        public int ID { get; set; }  
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Code { get; set; }    
        public string Slug { get; set; }  
        public string Description { get; set; }
        public string Image { get; set; }
        public string MoreImages { get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public bool? IncludedVAT { get; set; }
        public int? Quantity { get; set; }
        public int CategoryID { get; set; }
        public string Detail { get; set; }
        public int? Warranty { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifieDate { get; set; }
        public int? ModifieBy { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescriptions { get; set; }
        public int? Status { get; set; }
        public DateTime? TopHot { get; set; }
        public int? ViewCount { get; set; }
        public int SuppierId { get; set; }
    }
}
