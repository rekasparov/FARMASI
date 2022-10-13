using FARMASI.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.BusinessLayer.Abstract
{
    public interface IShoppingCartBL
    {
        Task<IList<ShoppingCartDTO>> GetShoppingCarts(string id);
        Task AddProductToShoppingCartAsync(ShoppingCartDTO dto);
        Task DeleteProductFromShoppingCartAsync(ShoppingCartDTO dto);
    }
}
