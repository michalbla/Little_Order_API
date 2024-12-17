using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNAI.Dto.Products;

namespace TNAI.Dto.Orders
{
    public class OrdersInputDto
    {
        [Required]
        public List<int> ProductsId { get; set; }
        [Required]
        public string OrderName { get; set; }
        public List<ProductInputDto> Products { get; set; }
    }
}
