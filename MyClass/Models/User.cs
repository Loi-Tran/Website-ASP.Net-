namespace MyClass.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }
       
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(32)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }
        public string Img { get; set; }
        public int? CountError { get; set; }
        public string Roles { get; set; }
        public string Gender { get; set; }
        public DateTime? CreatedDate { get; set; }

       
        public int? CreatedBy { get; set; }

        public DateTime? ModifieDate { get; set; }

   
        public int? ModifieBy { get; set; }

        public int? Status { get; set; }
    }
}
