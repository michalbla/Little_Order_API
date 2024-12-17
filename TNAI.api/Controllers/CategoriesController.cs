using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNAI.Dto.Categories;
using TNAI.Model.Entities;
using TNAI.Repository.Categories;

namespace TNAI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _categoryRepository.GetCategorytByIdAsync(id);
            if(category == null) return NotFound();

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if(!categories.Any()) return NotFound();

            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryInputDto category)
        {
            if (category == null) return BadRequest();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var newCategory = new Category()
            {
                Name = category.Name,
            };

            var result = await _categoryRepository.SaveCategoryAsync(newCategory);
            if (!result) throw new Exception("Error saving categories");
            return Ok(newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoryInputDto category)
        {
            if (category == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingCategory = await _categoryRepository.GetCategorytByIdAsync(id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = category.Name;

            var result = await _categoryRepository.SaveCategoryAsync(existingCategory);
            if (!result) throw new Exception("Error updating categories");
            return Ok(existingCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingCategory = await _categoryRepository.GetCategorytByIdAsync(id);
            if (existingCategory == null) return NotFound();

            var result = await _categoryRepository.DeleteCategoryAsync(id);
            if (!result) throw new Exception("Error deleting category");

            return Ok();
        }
    }
}
