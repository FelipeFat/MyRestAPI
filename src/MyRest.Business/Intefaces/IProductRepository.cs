using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRest.Business.Models;

namespace MyRest.Business.Intefaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsBySupplier(Guid supplierId);
        Task<IEnumerable<Product>> GetProductsSupplieres();
        Task<Product> GetProductSupplier(Guid id);
    }
}