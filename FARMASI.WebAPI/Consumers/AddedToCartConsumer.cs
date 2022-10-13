using FARMASI.BusinessLayer.Abstract;
using FARMASI.DataTransferObject;
using FARMASI.WebAPI.Events.Response;
using FARMASI.WebAPI.Messages.Response;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Consumers
{
    public class AddedToCartConsumer : IConsumer<AddedToCartResponseEvent>
    {
        private readonly IHubContext<NotificationHub> hubcontext;
        private readonly IShoppingCartBL shoppingCart;
        private readonly IProductBL productBL;

        public AddedToCartConsumer(IHubContext<NotificationHub> hubcontext, IShoppingCartBL shoppingCart, IProductBL productBL)
        {
            this.hubcontext = hubcontext;
            this.shoppingCart = shoppingCart;
            this.productBL = productBL;
        }

        public async Task Consume(ConsumeContext<AddedToCartResponseEvent> context)
        {
            AddedToCartResponseMessage addedToCartResponseMessage = context.Message.AddedToCartResponseMessage;

            IList<ShoppingCartDTO> shoppingCarts = await getShoppingCarts(context.Message.Id);

            await shoppingCartUpdated(context.Message.Id, shoppingCarts);

            await productStockUpdated(addedToCartResponseMessage.ProductId, await getProductQuantityInStock(addedToCartResponseMessage.ProductId));
        }

        private async Task<IList<ShoppingCartDTO>> getShoppingCarts(string id)
        {
            return await shoppingCart.GetShoppingCarts(id);
        }

        private async Task shoppingCartUpdated(string id, IList<ShoppingCartDTO> shoppingCarts)
        {
            if (Program.Connections.Any(x => x.Value == id))
            {
                KeyValuePair<string, string> connection = Program.Connections.FirstOrDefault(x => x.Value == id);

                string connectionId = connection.Key;

                await hubcontext.Clients.Client(connectionId).SendAsync("ShoppingCartUpdated");
            }
        }

        private async Task productStockUpdated(string productId, int quantityInStock)
        {
            await hubcontext.Clients.All.SendAsync("ProductStockUpdated", productId, quantityInStock);
        }

        private async Task<int> getProductQuantityInStock(string productId)
        {
            ProductDTO product = await productBL.GetProductByIdAsync(productId);

            return product.QuantityInStock;
        }
    }
}
