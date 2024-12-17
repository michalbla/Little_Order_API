using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNAI.Model;
using TNAI.Model.Entities;

namespace TNAI.Repository.Categories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Category?> GetCategorytByIdAsync(int id)
        {
            var category = await DbContext.Categories.SingleOrDefaultAsync(x => x.Id == id);

            return category;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await DbContext.Categories.ToListAsync();

            return categories;
        }
        public async Task<bool> SaveCategoryAsync(Category category)
        {
            if (category == null) return false;

            DbContext.Entry(category).State = category.Id == default(int) ? EntityState.Added : EntityState.Modified;

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch(Exception)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetCategorytByIdAsync(id);
            if(category == null) return true;

            DbContext.Categories.Remove(category);

            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
