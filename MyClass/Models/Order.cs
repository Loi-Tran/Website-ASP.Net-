namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int ID { get; set; }

        
        public int UserId { get; set; }
        [StringLength(50)]
        public string ReceiverName { get; set; }

        [StringLength(50)]
        public string ReceiverAddress { get; set; }

        [StringLength(50)]
        public string ReceiverEmail { get; set; }

        [StringLength(50)]
        public string ReceiverPhone { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public DateTime? CreatedDate { get; set; }

 
        public int? CreatedBy { get; set; }

        public DateTime? ModifieDate { get; set; }

 
        public int? ModifieBy { get; set; }

        public int Status { get; set; }
    }
}
