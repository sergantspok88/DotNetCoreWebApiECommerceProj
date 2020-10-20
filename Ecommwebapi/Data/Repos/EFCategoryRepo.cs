using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public class EFCategoryRepo : ICategoryRepo
    {
        private IDataContext ctx;

        public EFCategoryRepo(IDataContext ctx)
        {
            this.ctx = ctx;
        }

        public IQueryable<Category> Categories => ctx.Categories;

        public void CreateCategory(Category c)
        {
            ctx.Categories.Add(c);
        }

        public void DeleteCategory(Category c)
        {
            ctx.Categories.Remove(c);
        }

        public void SaveCategory(Category c)
        {
            ctx.SaveChanges();
        }
    }
}
