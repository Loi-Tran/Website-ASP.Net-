namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Orderdetail")]
    public partial class Orderdetail
    {
       
        public int ID { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public decimal? Price { get; set; }

        public int? Qty { get; set; }

        public decimal? Amount { get; set; }
    }
}
