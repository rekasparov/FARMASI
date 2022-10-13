using FARMASI.BusinessLayer.Abstract;
using FARMASI.Common.BaseSettings.Redis;
using FARMASI.DataTransferObject;
using FARMASI.Entity;
using FARMASI.UnitOfWork.Abstract.Redis;
using FARMASI.UnitOfWork.Concrete.Redis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.BusinessLayer.Concrete
{
    public class ShoppingCartBL : IShoppingCartBL
    {
        private readonly IBaseRedisUnitOfWork unitOfWork;

        public ShoppingCartBL(IOptions<RedisSettings> redisSettings)
        {
            unitOfWork = new BaseRedisUnitOfWork(redisSettings);
        }

        public async Task<IList<ShoppingCartDTO>> GetShoppingCarts(string id)
        {
            return await getShoppingCarts(id);
        }

        public async Task AddProductToShoppingCartAsync(ShoppingCartDTO dto)
        {
            IList<ShoppingCartDTO> shoppingCartItems = await getShoppingCarts(dto.Id);

            if (shoppingCartItems.Any(x => x.ProductId.Equals(dto.ProductId)))
            {
                ShoppingCartDTO shoppingCartItem = shoppingCartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);

                shoppingCartItem.Quantity += dto.Quantity;

                shoppingCartItems.Remove(shoppingCartItem);

                shoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItems.Add(dto);
            }

            IEnumerable<ShoppingCart> entities = shoppingCartItems.Select(x => new ShoppingCart()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Product = x.Product == null
                ? null
                : new Product()
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    UnitPrice = x.Product.UnitPrice,
                    QuantityInStock = x.Product.QuantityInStock
                }
            }).AsEnumerable();

            await unitOfWork.ShoppingCart.AddRangeAsync(dto.Id, entities);
        }

        public async Task DeleteProductFromShoppingCartAsync(ShoppingCartDTO dto)
        {
            IList<ShoppingCartDTO> shoppingCartItems = await getShoppingCarts(dto.Id);

            if (shoppingCartItems.Any(x => x.ProductId.Equals(dto.ProductId)))
            {
                ShoppingCartDTO shoppingCartItem = shoppingCartItems.FirstOrDefault(x => x.ProductId == dto.ProductId);

                shoppingCartItem.Quantity -= dto.Quantity;

                shoppingCartItems.Remove(shoppingCartItem);

                if (shoppingCartItem.Quantity != 0)
                    shoppingCartItems.Add(shoppingCartItem);
            }

            IEnumerable<ShoppingCart> entities = shoppingCartItems.Select(x => new ShoppingCart()
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Product = x.Product == null
                ? null
                : new Product()
                {
                    Id = x.Product.Id,
                    Name = x.Product.Name,
                    UnitPrice = x.Product.UnitPrice,
                    QuantityInStock = x.Product.QuantityInStock
                }
            }).AsEnumerable();

            await unitOfWork.ShoppingCart.AddRangeAsync(dto.Id, entities);
        }

        private async Task<IList<ShoppingCartDTO>> getShoppingCarts(string id)
        {
            return (await unitOfWork.ShoppingCart.GetListByIdAsync(id))
                .Select(x => new ShoppingCartDTO()
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Product = x.Product == null
                    ? null
                    : new ProductDTO()
                    {
                        Id = x.Product.Id,
                        Name = x.Product.Name,
                        UnitPrice = x.Product.UnitPrice,
                        QuantityInStock = x.Product.QuantityInStock
                    }
                })
                .ToList();
        }
    }
}
