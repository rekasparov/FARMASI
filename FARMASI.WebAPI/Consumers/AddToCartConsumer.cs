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
    public class AddToCartConsumer : IConsumer<AddToCartRequestEvent>
    {
        private readonly IShoppingCartBL shoppingCart;
        private readonly IPublishEndpoint publishEndpoint;

        public AddToCartConsumer(IShoppingCartBL shoppingCart, IPublishEndpoint publishEndpoint)
        {
            this.shoppingCart = shoppingCart;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<AddToCartRequestEvent> context)
        {
            AddToCartRequestMessage addToCartRequestMessage = context.Message.AddToCartRequestMessage;

            await addToCart(context.Message.Id, addToCartRequestMessage.ProductId, addToCartRequestMessage.Quantity, addToCartRequestMessage.Product);

            await publishEndpoint.Publish(new AddedToCartResponseEvent()
            {
                Id = context.Message.Id,
                AddedToCartResponseMessage = new AddedToCartResponseMessage()
                {
                    ProductId = addToCartRequestMessage.ProductId,
                    Quantity = addToCartRequestMessage.Quantity
                }
            });
        }

        private async Task addToCart(string id, string productId, int quantiy, ProductDTO product)
        {
            await shoppingCart.AddProductToShoppingCartAsync(new ShoppingCartDTO()
            {
                Id = id,
                ProductId = productId,
                Quantity = quantiy,
                Product = product
            });
        }
    }
}
