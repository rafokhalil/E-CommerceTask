using E_Commerce.Business.Abstract;
using E_Commerce.Entities.Concrete;
using E_Commerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.WebUI.Controllers
{
    public class ProductController : Controller
    {
        static List<Product> Products;
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(int page=1,int category=0)
        {
            int pageSize = 10;
            var products = _productService.GetByCategory(category);
            ProductListViewModel vm = null;
            if (Products != null)
            {
                vm = new ProductListViewModel()
                {
                    Products = Products,
                    PageCount = (int)Math.Ceiling(products.Count() / (double)pageSize),
                    PageSize = pageSize,
                    CurrentCategory = category,
                    CurrentPage = page
                };
            }
            else
            {
                vm = new ProductListViewModel()
                {
                    Products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                    PageCount = (int)Math.Ceiling(products.Count() / (double)pageSize),
                    PageSize = pageSize,
                    CurrentCategory = category,
                    CurrentPage = page
                };
            }
            return View(vm);
        }

        public IActionResult ShortList(string propName, string type, string jsonData)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonData);

            if (propName == "ProductName")
            {
                if (type == "A-z")
                    products = products.OrderBy(p => p.ProductName).ToList();
                else
                    products = products.OrderByDescending(p => p.ProductName).ToList();

            }
            else if (propName == "UnitPrice")
            {
                if (type == "1-9")
                    products = products.OrderBy(p => p.UnitPrice).ToList();
                else
                    products = products.OrderByDescending(p => p.UnitPrice).ToList();

            }
            else if (propName == "UnitsInStock")
            {
                if (type == "1-9")
                    products = products.OrderBy(p => p.UnitsInStock).ToList();
                else
                    products = products.OrderByDescending(p => p.UnitsInStock).ToList();

            }

            Products = products;
            return RedirectToAction("index");
        }
    }
}