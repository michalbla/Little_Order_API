using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNAI.Model.Entities;

namespace TNAI.Repository.Categories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategorytByIdAsync(int id);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> SaveCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
