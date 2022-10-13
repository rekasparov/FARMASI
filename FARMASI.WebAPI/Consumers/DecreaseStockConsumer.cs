using FARMASI.BusinessLayer.Abstract;
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
    public class DecreaseStockConsumer : IConsumer<DesceaseStockRequestEvent>
    {
        private readonly IProductBL productBL;
        private readonly IPublishEndpoint publishEndpoint;

        public DecreaseStockConsumer(IProductBL productBL, IPublishEndpoint publishEndpoint)
        {
            this.productBL = productBL;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<DesceaseStockRequestEvent> context)
        {
            DecreaseStockRequestMessage decreaseStockRequestMessage = context.Message.DecreaseStockRequestMessage;

            if (await checkStockAvailability(decreaseStockRequestMessage.ProductId, decreaseStockRequestMessage.Quantity))
            {
                await productBL.DecreaseStockAsync(decreaseStockRequestMessage.ProductId, decreaseStockRequestMessage.Quantity);

                await publishEndpoint.Publish(new AddToCartRequestEvent()
                {
                    Id = context.Message.Id,
                    AddToCartRequestMessage = new AddToCartRequestMessage()
                    {
                        ProductId = decreaseStockRequestMessage.ProductId,
                        Quantity = decreaseStockRequestMessage.Quantity,
                        Product = await productBL.GetProductByIdAsync(decreaseStockRequestMessage.ProductId)
                    }
                });
            }
            else
            {
                await publishEndpoint.Publish(new StockNotAvailableResponseEvent()
                {
                    Id = context.Message.Id,
                    StockNotAvailableResponseMessage = new StockNotAvailableResponseMessage()
                    {
                        ProductId = decreaseStockRequestMessage.ProductId
                    }
                });
            }
        }

        private async Task<bool> checkStockAvailability(string productId, int quantity)
        {
            return await productBL.IsStockAvailableAsync(productId, quantity);
        }
    }
}
