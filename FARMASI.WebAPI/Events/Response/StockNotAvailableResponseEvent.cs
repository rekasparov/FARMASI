using FARMASI.WebAPI.Messages.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Events.Response
{
    public class StockNotAvailableResponseEvent
    {
        public string Id { get; set; }
        public StockNotAvailableResponseMessage StockNotAvailableResponseMessage { get; set; }
    }
}
