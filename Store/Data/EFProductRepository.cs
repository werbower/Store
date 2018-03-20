using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Data{
    public class EFProductRepository : IProductRepository {
        private ApplicationDbContext _db;

        public EFProductRepository(ApplicationDbContext dbContext) {
            _db = dbContext;
        }
        public IQueryable<Product> Products => _db.Products;
    }
}
