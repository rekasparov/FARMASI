using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.DataTransferObject
{
    public class ShoppingCartDTO
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }

        public ProductDTO Product { get; set; }
    }
}
