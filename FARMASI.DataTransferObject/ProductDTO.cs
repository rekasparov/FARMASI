using System;
using System.ComponentModel.DataAnnotations;

namespace FARMASI.DataTransferObject
{
    public class ProductDTO
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
    }
}
