using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
       
        [StringLength(50)]
        public string Name { get; set; }
    }
}
