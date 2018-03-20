using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Data;
using Store.ViewModels;

namespace Store.Controllers{
    public class ProductController:Controller    {
        private IProductRepository _repo;
        public int PageSize = 4;

        public ProductController(IProductRepository repo) {
            _repo = repo;
        }

        public ViewResult List(string category,int page = 1) {
            var model = new ProductsListViewModel {
                Products = _repo.Products
                .Where(p => category == null || p.Category == category)
               .OrderBy(p => p.ProductId)
               .Skip((page - 1) * PageSize)
               .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repo.Products.Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}
