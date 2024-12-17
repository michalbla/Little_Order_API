using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNAI.Dto.Categories
{
    public class CategoryInputDto
    {
        //public CategoryInputDto() { }

        [Required]
        [MaxLength(2000)]
        public string Name { get; set; }
    }
}
