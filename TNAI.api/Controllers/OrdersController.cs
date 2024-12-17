using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNAI.Dto.Orders;
using TNAI.Dto.Products;
using TNAI.Model.Entities;
using TNAI.Repository.Orders;
using TNAI.Repository.Products;

namespace TNAI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);

            if (order == null) return NotFound();
            var orderDto = new OrdersDto
            {
                Id = order.Id,
                Name = order.Name,
                Products = order.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name,
                    ProductPrice = p.Price ?? 0
                }).ToList()
            };
            return Ok(orderDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            if (orders == null || !orders.Any())
                return NotFound("No orders found.");

            var ordersDto = orders.Select(order => new OrdersDto
            {
                Id = order.Id,
                Name = order.Name,
                Products = order.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name,
                    ProductPrice = p.Price ?? 0
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrdersInputDto order)
        {
            if (order == null) return BadRequest();

            if(!ModelState.IsValid) return BadRequest();

            var newOrder = new Order()
            {
                Name = order.OrderName,
            };

            if (order.ProductsId != null && order.ProductsId.Any())
            {
                var products = await _productRepository.GetProductsByIdAsync(order.ProductsId);
                if (products == null || !products.Any())
                    return BadRequest("Invalid product IDs provided.");

                foreach (var product in products)
                {
                    newOrder.Products.Add(product);
                }
            }

            var results = await _orderRepository.SaveOrderAsync(newOrder);

            if (!results) throw new Exception("Error saving Order");

            var orderToUpdate = await _orderRepository.GetOrderByIdAsync(newOrder.Id);

            var resultDto = new OrdersDto
            {
                Id = orderToUpdate.Id,
                Name = orderToUpdate.Name,
                Products = orderToUpdate.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    ProductPrice = (int)p.Price,
                    CategoryName = p.Category.Name,
                }).ToList(),
            };
            return Ok(resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] OrdersInputDto order)
        {
            //TODO: AKTUALIZACJA CENY
            if (order == null) return BadRequest();

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);

            if(existingOrder == null) return NotFound();

            existingOrder.Name = order.OrderName;
            
            foreach(var product in order.Products)
            {
                var existingProduct = existingOrder.Products.FirstOrDefault(p => p.Id == product.ProductId);
                if(existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.Price = existingProduct.Price;
                }
                else
                    return Ok(existingOrder);

            }
            var savedOrder = _orderRepository.SaveOrderAsync(existingOrder);
            return Ok(savedOrder);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingOrder = await _orderRepository.GetOrderByIdAsync(id);
            if (existingOrder == null) return NotFound();

            var result = await _orderRepository.DeleteOrderAsync(id);
            if (!result) throw new Exception("Error deleting order");

            return Ok();
        }
    }
}
