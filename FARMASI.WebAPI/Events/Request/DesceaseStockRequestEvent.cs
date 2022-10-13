using FARMASI.WebAPI.Messages.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Events.Request
{
    public class DesceaseStockRequestEvent
    {
        public string Id { get; set; }
        public DecreaseStockRequestMessage DecreaseStockRequestMessage { get; set; }
    }
}
