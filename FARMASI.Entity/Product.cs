using FARMASI.Common.BaseEntities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Entity
{
    public class Product : BaseEntity
    {
        public Product()
        {
            ShoppingCarts = new List<ShoppingCart>();
        }

        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityInStock { get; set; }

        public List<ShoppingCart> ShoppingCarts { get; set; }
    }
}
