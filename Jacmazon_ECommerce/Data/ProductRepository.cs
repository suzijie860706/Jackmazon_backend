﻿using Jacmazon_ECommerce.Interfaces;
using Jacmazon_ECommerce.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Jacmazon_ECommerce.Data
{
    public class ProductRepository : ICRUDRepository<Product>
    {
        protected readonly AdventureWorksLt2019Context context;
        protected readonly IConfiguration configuration;

        public ProductRepository(AdventureWorksLt2019Context context,
            IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public List<Product> GetAll(int currentPage)
        {
            int page_size = Convert.ToInt16(configuration["PageSize"]);
            int skip_count = 0;
            //第一頁不skip資料
            if (currentPage != 1) skip_count = (currentPage - 1) * page_size;

            IQueryable<Product> productCategories = context.Products.Skip(skip_count).Take(page_size);
            return productCategories.ToList();
        }
        

        public Product? Detail(int id)
        {
            Product? Product = context.Products.Find(id);
            return Product;
        }

        public List<Product> GetCategoryList(int? id)
        {

            IQueryable<Product>? products = from product in context.Products
                                      where product.ProductCategoryId == id
                                      select product;
            return products.ToList();
        }
    }
}
