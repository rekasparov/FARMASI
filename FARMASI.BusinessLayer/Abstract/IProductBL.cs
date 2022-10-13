using FARMASI.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.BusinessLayer.Abstract
{
    public interface IProductBL
    {
        IList<ProductDTO> GetProducts();
        Task<ProductDTO> GetProductByNameAsync(string name);
        Task AddProductAsync(ProductDTO dto);
        Task EditProductAsync(ProductDTO dto);
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task DeleteProductAsync(string id);
        Task<bool> IsStockAvailableAsync(string id, int quantity);
        Task DecreaseStockAsync(string id, int quantity);
    }
}
