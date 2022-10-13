using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Messages.Response
{
    public class DeletedFromCartResponseMessage
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
