using System.Collections.Generic;
using Ecommwebapi.Data.Models;
using System.Linq;
using Ecommwebapi.Helpers;
using System;
using Microsoft.EntityFrameworkCore;

namespace Ecommwebapi.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Product CreateProduct(Product product)
        {
            //Realistically need to check also category etc
            if (unitOfWork.ProductRepo.Products.Any(p => p.Name == product.Name))
            {
                throw new AppException("Product name " + product.Name + " is already taken");
            }

            unitOfWork.ProductRepo.CreateProduct(product);
            unitOfWork.Complete();

            return product;
        }

        public void DeleteProduct(int id)
        {
            var product = unitOfWork.ProductRepo.Products.SingleOrDefault(p => p.Id == id);

            if (product != null)
            {
                unitOfWork.ProductRepo.DeleteProduct(product);
                unitOfWork.Complete();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return unitOfWork.ProductRepo.Products.Include(p => p.Category);
        }

        public IEnumerable<Product> GetAllProductsNameContains(string nameContains)
        {
            return unitOfWork.ProductRepo.Products.Include(p => p.Category).Where(p => EF.Functions.Like(p.Name, $"%{nameContains}%"));
        }

        public IEnumerable<Product> GetProductsNameLikeSkipAndTakeNumber(string nameContains, int skip, int number)
        {
            //return repo.Products.Where(p => p.Name.Contains(nameContains)).Skip(0).Take(number);
            return unitOfWork.ProductRepo.Products.Where(p => EF.Functions.Like(p.Name, $"%{nameContains}%")).Skip(0).Take(number);
        }

        public Product GetProductById(int id)
        {
            return unitOfWork.ProductRepo.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsByCategoryName(string categoryName)
        {
            return unitOfWork.ProductRepo.Products.Include(p => p.Category).Where(p => p.Category.Name == categoryName);
        }

        public IEnumerable<Product> GetProductsSkipAndTakeNumber(int skip, int number)
        {
            return unitOfWork.ProductRepo.Products.Include(p => p.Category).Skip(skip).Take(number);
        }

        public IEnumerable<Product> GetProductsByCategoryNameSkipAndTakeNumber(string categoryName, int skip, int number)
        {
            return unitOfWork.ProductRepo.Products.Where(p => p.Category.Name == categoryName).Include(p => p.Category).Skip(skip).Take(number);
        }

        public void UpdateProduct(Product productParam)
        {
            var product = unitOfWork.ProductRepo.Products.SingleOrDefault(p => p.Id == productParam.Id);

            if (product == null)
            {
                throw new AppException("Product not found");
            }

            if (!string.IsNullOrWhiteSpace(productParam.Name))
            {
                product.Name = productParam.Name;
            }

            if (!string.IsNullOrWhiteSpace(productParam.Description))
            {
                product.Description = productParam.Description;
            }

            if (productParam.Price > 0)
            {
                product.Price = productParam.Price;
            }

            {
                product.Category = productParam.Category;
            }

            unitOfWork.Complete();
        }
    }
}