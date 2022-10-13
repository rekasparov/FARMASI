using FARMASI.Common.BaseEntities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Entity
{
    public class ShoppingCart : BaseEntity
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
