using FARMASI.WebAPI.Events.Response;
using FARMASI.WebAPI.Messages.Response;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Consumers
{
    public class StockNotAvailableConsumer : IConsumer<StockNotAvailableResponseEvent>
    {
        private readonly IHubContext<NotificationHub> hubcontext;

        public StockNotAvailableConsumer(IHubContext<NotificationHub> hubcontext)
        {
            this.hubcontext = hubcontext;
        }

        public async Task Consume(ConsumeContext<StockNotAvailableResponseEvent> context)
        {
            StockNotAvailableResponseMessage stockNotAvailableResponseMessage = context.Message.StockNotAvailableResponseMessage;

            await stockNotAvailable(context.Message.Id, stockNotAvailableResponseMessage.ProductId);
        }

        private async Task stockNotAvailable(string id, string productId)
        {
            if (Program.Connections.Any(x => x.Value == id))
            {
                KeyValuePair<string, string> connection = Program.Connections.FirstOrDefault(x => x.Value == id);

                string connectionId = connection.Key;

                await hubcontext.Clients.Client(connectionId).SendAsync("StockNotAvailable", productId);
            }
        }
    }
}
