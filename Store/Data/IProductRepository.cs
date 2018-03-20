using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Models;

namespace Store.Data{
    public interface IProductRepository    {
        IQueryable<Product> Products { get; }
    }
}
