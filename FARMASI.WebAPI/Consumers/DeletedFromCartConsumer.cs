using FARMASI.BusinessLayer.Abstract;
using FARMASI.DataTransferObject;
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
    public class DeletedFromCartConsumer : IConsumer<DeletedFromCartResponseEvent>
    {
        private readonly IHubContext<NotificationHub> hubcontext;
        private readonly IShoppingCartBL shoppingCart;
        private readonly IProductBL productBL;

        public DeletedFromCartConsumer(IHubContext<NotificationHub> hubcontext, IShoppingCartBL shoppingCart, IProductBL productBL)
        {
            this.hubcontext = hubcontext;
            this.shoppingCart = shoppingCart;
            this.productBL = productBL;
        }

        public async Task Consume(ConsumeContext<DeletedFromCartResponseEvent> context)
        {
            DeletedFromCartResponseMessage deletedFromCartResponseMessage = context.Message.DeletedFromCartResponseMessage;

            IList<ShoppingCartDTO> shoppingCarts = await getShoppingCarts(context.Message.Id);

            await shoppingCartUpdated(context.Message.Id);

            await productStockUpdated(deletedFromCartResponseMessage.ProductId, await getProductQuantityInStock(deletedFromCartResponseMessage.ProductId));
        }

        private async Task<IList<ShoppingCartDTO>> getShoppingCarts(string id)
        {
            return await shoppingCart.GetShoppingCarts(id);
        }

        private async Task shoppingCartUpdated(string id)
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
            ProductDTO product = await getProductByProductId(productId);

            await hubcontext.Clients.All.SendAsync("ProductStockUpdated", productId, await increaseStockAsync(product));
        }

        private async Task<int> getProductQuantityInStock(string productId)
        {
            ProductDTO product = await getProductByProductId(productId);

            return product.QuantityInStock;
        }

        private async Task<ProductDTO> getProductByProductId(string productId)
        {
            ProductDTO product = await productBL.GetProductByIdAsync(productId);

            return product;
        }

        private async Task<int> increaseStockAsync(ProductDTO product)
        {
            product.QuantityInStock++;

            await productBL.EditProductAsync(product);

            return product.QuantityInStock;
        }
    }
}
