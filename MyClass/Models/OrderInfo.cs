using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Models
{
    public class OrderInfo
    {
        public int ID { get; set; }
        public int UserId { get; set; }     
        public string ReceiverName { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverPhone { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifieDate { get; set; }
        public int? ModifieBy { get; set; }
        public int? Status { get; set; }
     
        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public decimal? Price { get; set; }

        public int? Qty { get; set; }

        public decimal? Amount { get; set; }
    }
}
