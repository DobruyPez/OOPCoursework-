using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4lab.Resources
{
    public class SubscriptionPrice
    {
        [Key]
        public int Id { get; set; }

        public decimal LitePrice { get; set; } = 10.00m;
        public decimal SemiProPrice { get; set; } = 20.00m;
        public decimal ProPrice { get; set; } = 30.00m;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
