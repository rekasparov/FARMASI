using FARMASI.BusinessLayer.Abstract;
using FARMASI.Common.BaseReturnTypes.Abstract;
using FARMASI.Common.BaseReturnTypes.Concrete;
using FARMASI.DataTransferObject;
using FARMASI.WebAPI.Extensions;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductBL productBL;

        public ProductsController(IProductBL productBL)
        {
            this.productBL = productBL;
        }

        [HttpGet]
        public IActionResult Get()
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                IList<ProductDTO> products = productBL.GetProducts();

                baseReturnType.Data = products;
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
        public async Task<IActionResult> Post([FromBody] ProductDTO model)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                if (await isUnique(model.Name))
                {
                    await productBL.AddProductAsync(model);

                    baseReturnType.SuccessMessage = "OK";
                }
                else
                {
                    baseReturnType.ErrorOccurred = true;
                    baseReturnType.ErrorMessage = "Bu isimde bir ürün zaten var";
                }
            }
            catch (Exception ex)
            {
                ex = ex.GetInnerException();
                baseReturnType.ErrorOccurred = true;
                baseReturnType.ErrorMessage = ex.Message;
            }

            return Ok(baseReturnType);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductDTO model)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                bool isAnyResult = await isAny(model.Id);
                bool isUniqueResult = await isUnique(model);

                if (isAnyResult && isUniqueResult)
                {
                    await productBL.EditProductAsync(model);

                    baseReturnType.SuccessMessage = "OK";
                }
                else
                {
                    baseReturnType.ErrorOccurred = true;
                    if (!isAnyResult)
                        baseReturnType.ErrorMessage = "Böyle bir kayıt bulunamadı";
                    else if (!isUniqueResult)
                        baseReturnType.ErrorMessage = "Bu bir ürün zaten var";
                }
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
        public async Task<IActionResult> Delete([FromBody] ProductDTO model)
        {
            using IBaseReturnType baseReturnType = new BaseReturnType();

            try
            {
                if (await isAny(model.Id))
                {
                    await productBL.DeleteProductAsync(model.Id);

                    baseReturnType.SuccessMessage = "OK";
                }
                else
                {
                    baseReturnType.ErrorOccurred = true;
                    baseReturnType.ErrorMessage = "Böyle bir kayıt bulunamadı";
                }
            }
            catch (Exception ex)
            {
                ex = ex.GetInnerException();
                baseReturnType.ErrorOccurred = true;
                baseReturnType.ErrorMessage = ex.Message;
            }

            return Ok(baseReturnType);
        }

        private async Task<bool> isUnique(string name)
        {
            try
            {
                return await productBL.GetProductByNameAsync(name) == null
                    ? true
                    : false;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> isUnique(ProductDTO model)
        {
            try
            {
                ProductDTO savedModel = await productBL.GetProductByIdAsync(model.Id);

                if (model.Id.Equals(savedModel.Id) && model.Name.Equals(savedModel.Name) && model.UnitPrice == savedModel.UnitPrice && model.QuantityInStock == savedModel.QuantityInStock)
                    return false;

                return true;
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> isAny(string id)
        {
            try
            {
                return await productBL.GetProductByIdAsync(id) != null
                    ? true
                    : false;
            }
            catch
            {
                throw;
            }
        }
    }
}
