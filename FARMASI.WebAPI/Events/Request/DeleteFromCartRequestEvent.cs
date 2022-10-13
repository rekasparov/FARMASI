using FARMASI.WebAPI.Messages.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Events.Request
{
    public class DeleteFromCartRequestEvent
    {
        public string Id { get; set; }
        public DeleteFromCartRequestMessage DeleteFromCartRequestMessage { get; set; }
    }
}
