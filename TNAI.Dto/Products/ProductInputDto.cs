using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNAI.Dto.Products
{
    public class ProductInputDto
    {
        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int? ProductPrice { get; set; }
        //public string? CategoryName { get; set; }
    }
}
