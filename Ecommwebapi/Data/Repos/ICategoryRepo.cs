using Ecommwebapi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data.Repos
{
    public interface ICategoryRepo
    {
        public IQueryable<Category> Categories
        {
            get;
        }

        void CreateCategory(Category c);
        void DeleteCategory(Category c);
        void SaveCategory(Category c);
    }
}
