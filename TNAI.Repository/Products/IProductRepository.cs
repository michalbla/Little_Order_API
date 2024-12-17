using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNAI.Model.Entities;

namespace TNAI.Repository.Products
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetAllProductsAsync();
        Task<bool> SaveProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);

        Task<List<Product>> GetProductsByIdAsync(IEnumerable<int> productsId);
    }
}
