using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            string cId = Context.ConnectionId;

            string clientId = Context.GetHttpContext().Request.Query["access_token"].ToString();

            if (!Program.Connections.Any(x => x.Key == cId))
                Program.Connections.Add(cId, clientId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string cId = Context.ConnectionId;

            if (Program.Connections.Any(x => x.Key == cId))
                Program.Connections.Remove(cId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
