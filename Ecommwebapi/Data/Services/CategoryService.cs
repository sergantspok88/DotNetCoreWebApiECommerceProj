using Ecommwebapi.Data.Models;
using Ecommwebapi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {

            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return unitOfWork.CategoryRepo.Categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            return unitOfWork.CategoryRepo.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public Category GetCategoryByName(string categoryName)
        {
            return unitOfWork.CategoryRepo.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
        }

        public Category CreateCategory(Category category)
        {
            if (unitOfWork.CategoryRepo.Categories.Any(c => c.Name == category.Name))
            {
                throw new AppException("Category name " + category.Name + " is already taken");
            }

            unitOfWork.CategoryRepo.CreateCategory(category);
            unitOfWork.Complete();

            return category;
        }

        public void UpdateCategory(Category categoryParam)
        {
            var category = unitOfWork.CategoryRepo.Categories.SingleOrDefault(c => c.Id == categoryParam.Id);

            if (category == null)
            {
                throw new AppException("Category not found");
            }

            if (!string.IsNullOrWhiteSpace(categoryParam.Name))
            {
                category.Name = categoryParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(categoryParam.Description))
            {
                category.Description = categoryParam.Description;
            }

            unitOfWork.Complete();
        }

        public void DeleteCategory(string name)
        {
            var category = unitOfWork.CategoryRepo.Categories.SingleOrDefault(c => c.Name == name);

            if (category != null)
            {
                unitOfWork.CategoryRepo.DeleteCategory(category);
                unitOfWork.Complete();
            }
        }
    }
}
