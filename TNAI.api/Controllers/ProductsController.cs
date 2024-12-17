using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNAI.Dto.Products;
using TNAI.Model.Entities;
using TNAI.Repository.Products;

namespace TNAI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new { Message = "Product not found" });

            return Ok(new
            {
                productName = product.Name,
                categoryName = product.Category?.Name ?? "No Category",
                productPrice = product.Price,
                categoryId = product.Category?.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllProductsAsync();
            if(!products.Any()) return NotFound();
            var result = products.Select(product => new
            {
                productName = product.Name,
                categoryName = product.Category?.Name ?? "No Category",
                productPrice = product.Price,
                categoryId = product.Category?.Id,
                productId = product.Id,
            });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductInputDto product) 
        {
            if (product == null) return BadRequest();

            if(!ModelState.IsValid) return BadRequest();

            var newProduct = new Product()
            {
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.ProductPrice
            };

            var result = await _productRepository.SaveProductAsync(newProduct);
            if (!result) throw new Exception("Error saving product");

            var savedProduct = await _productRepository.GetProductByIdAsync(newProduct.Id);

            var newProductDto = new ProductDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                CategoryId = newProduct.CategoryId,
                ProductPrice = (int)newProduct.Price,
                CategoryName = newProduct.Category?.Name
            };

            /*var resultDto = await _productRepository.
            GetProductByIdAsync(newProductDto.Id);*/

            return Ok(newProductDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductInputDto product)
        {
            if(product == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if(existingProduct == null) return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Price = product.ProductPrice;

            var result = await _productRepository.SaveProductAsync(existingProduct);
            if (!result) throw new Exception("Error updating product");

            var updatedProductDto = new ProductDto
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                CategoryId = existingProduct.CategoryId,
                ProductPrice = (int)existingProduct.Price,
                CategoryName = existingProduct.Category?.Name
            };

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null) return NotFound();

            var result = await _productRepository.DeleteProductAsync(id);
            if (!result) throw new Exception("Error deleting product");

            return Ok();
        }
    }
}
