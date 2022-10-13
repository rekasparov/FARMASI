using FARMASI.BusinessLayer.Abstract;
using FARMASI.Common.BaseReturnTypes.Abstract;
using FARMASI.Common.BaseReturnTypes.Concrete;
using FARMASI.DataTransferObject;
using FARMASI.WebAPI.Events.Request;
using FARMASI.WebAPI.Extensions;
using FARMASI.WebAPI.Messages.Request;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartBL shoppingCartBL;
        private readonly IPublishEndpoint publishEndpoint;

        public ShoppingCartsController(IShoppingCartBL shoppingCartBL, IPublishEndpoint publishEndpoint)
        {
            this.shoppingCartBL = shoppingCartBL;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                baseReturnType.Data = await shoppingCartBL.GetShoppingCarts(id);
                baseReturnType.SuccessMessage = "OK";
            }
            catch (Exception ex)
            {
                ex = ex.GetInnerException();
                baseReturnType.ErrorOccurred = true;
                baseReturnType.ErrorMessage = ex.Message;
            }

            return Ok(baseReturnType);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ShoppingCartDTO model)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                await publishEndpoint.Publish(new DesceaseStockRequestEvent()
                {
                    Id = model.Id,
                    DecreaseStockRequestMessage = new DecreaseStockRequestMessage()
                    {
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    }
                });

                baseReturnType.SuccessMessage = "OK";
            }
            catch (Exception ex)
            {
                ex = ex.GetInnerException();
                baseReturnType.ErrorOccurred = true;
                baseReturnType.ErrorMessage = ex.Message;
            }

            return Ok(baseReturnType);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(ShoppingCartDTO model)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                await publishEndpoint.Publish(new DeleteFromCartRequestEvent()
                {
                    Id = model.Id,
                    DeleteFromCartRequestMessage = new DeleteFromCartRequestMessage()
                    {
                        ProductId = model.ProductId,
                        Quantity = model.Quantity
                    }
                });

                baseReturnType.SuccessMessage = "OK";
            }
            catch (Exception ex)
            {
                ex = ex.GetInnerException();
                baseReturnType.ErrorOccurred = true;
                baseReturnType.ErrorMessage = ex.Message;
            }

            return Ok(baseReturnType);
        }
    }
}
