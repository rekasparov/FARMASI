using FARMASI.BusinessLayer.Abstract;
using FARMASI.DataTransferObject;
using FARMASI.WebAPI.Events.Request;
using FARMASI.WebAPI.Events.Response;
using FARMASI.WebAPI.Messages.Request;
using FARMASI.WebAPI.Messages.Response;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Consumers
{
    public class DeleteFromCartConsumer : IConsumer<DeleteFromCartRequestEvent>
    {
        private readonly IShoppingCartBL shoppingCart;
        private readonly IPublishEndpoint publishEndpoint;

        public DeleteFromCartConsumer(IShoppingCartBL shoppingCart, IPublishEndpoint publishEndpoint)
        {
            this.shoppingCart = shoppingCart;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<DeleteFromCartRequestEvent> context)
        {
            DeleteFromCartRequestMessage deleteFromCartRequestMessage = context.Message.DeleteFromCartRequestMessage;

            await deleteFromCart(context.Message.Id, deleteFromCartRequestMessage.ProductId, deleteFromCartRequestMessage.Quantity);

            await publishEndpoint.Publish(new DeletedFromCartResponseEvent()
            {
                Id = context.Message.Id,
                DeletedFromCartResponseMessage = new DeletedFromCartResponseMessage()
                {
                    ProductId = deleteFromCartRequestMessage.ProductId,
                    Quantity = deleteFromCartRequestMessage.Quantity
                }
            });
        }

        private async Task deleteFromCart(string id, string productId, int quantiy)
        {
            await shoppingCart.DeleteProductFromShoppingCartAsync(new ShoppingCartDTO()
            {
                Id = id,
                ProductId = productId,
                Quantity = quantiy
            });
        }
    }
}
