using FARMASI.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Messages.Request
{
    public class AddToCartRequestMessage
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDTO Product { get; set; }
    }
}
