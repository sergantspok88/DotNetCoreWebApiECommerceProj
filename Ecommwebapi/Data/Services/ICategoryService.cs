using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);
        Category GetCategoryByName(string categoryName);
        Category CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(string name);
    }
}
