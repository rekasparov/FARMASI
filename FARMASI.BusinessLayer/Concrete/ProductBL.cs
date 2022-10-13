using FARMASI.BusinessLayer.Abstract;
using FARMASI.Common.BaseSettings.MongoDB;
using FARMASI.DataTransferObject;
using FARMASI.Entity;
using FARMASI.UnitOfWork.Abstract.MongoDB;
using FARMASI.UnitOfWork.Concrete.MongoDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.BusinessLayer.Concrete
{
    public class ProductBL : IProductBL
    {
        private readonly IBaseMongoDBUnitOfWork unitOfWork;

        public ProductBL(IOptions<MongoDBSettings> mongoDBSettings)
        {
            unitOfWork = new BaseMongoDBUnitOfWork(mongoDBSettings);
        }

        public IList<ProductDTO> GetProducts()
        {
            try
            {
                return unitOfWork.Product.Get().Select(x => new ProductDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UnitPrice = x.UnitPrice,
                    QuantityInStock = x.QuantityInStock
                }).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddProductAsync(ProductDTO dto)
        {
            try
            {
                Product entity = new Product()
                {
                    Name = dto.Name,
                    UnitPrice = dto.UnitPrice,
                    QuantityInStock = dto.QuantityInStock
                };

                await unitOfWork.Product.AddAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> GetProductByNameAsync(string name)
        {
            try
            {
                Product entity = await unitOfWork.Product.GetAsync(x => x.Name == name);

                if (entity is null)
                    return null;

                return new ProductDTO()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    UnitPrice = entity.UnitPrice,
                    QuantityInStock = entity.QuantityInStock
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            try
            {
                return await getProductByIdAsync(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            try
            {
                Product entity = await unitOfWork.Product.GetByIdAsync(id);

                if (!entity.Id.Equals(id))
                    entity.Id = id;

                await unitOfWork.Product.DeleteAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        public async Task EditProductAsync(ProductDTO dto)
        {
            try
            {
                await editProductAsync(dto);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IsStockAvailableAsync(string id, int quantity)
        {
            ProductDTO product = await getProductByIdAsync(id);

            if (product != null && product.QuantityInStock >= quantity)
                return true;

            return false;
        }

        public async Task DecreaseStockAsync(string id, int quantity)
        {
            ProductDTO product = await getProductByIdAsync(id);

            product.QuantityInStock -= quantity;

            await editProductAsync(product);
        }

        private async Task<ProductDTO> getProductByIdAsync(string id)
        {
            try
            {
                Product entity = await unitOfWork.Product.GetByIdAsync(id);

                if (entity is null)
                    return null;

                return new ProductDTO()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    UnitPrice = entity.UnitPrice,
                    QuantityInStock = entity.QuantityInStock
                };
            }
            catch
            {
                throw;
            }
        }

        private async Task editProductAsync(ProductDTO dto)
        {
            try
            {
                Product entity = await unitOfWork.Product.GetByIdAsync(dto.Id);

                if (!entity.Id.Equals(dto.Id))
                    entity.Id = dto.Id;

                entity.Name = dto.Name;
                entity.UnitPrice = dto.UnitPrice;
                entity.QuantityInStock = dto.QuantityInStock;

                await unitOfWork.Product.UpdateAsync(dto.Id, entity);
            }
            catch
            {
                throw;
            }
        }
    }
}
