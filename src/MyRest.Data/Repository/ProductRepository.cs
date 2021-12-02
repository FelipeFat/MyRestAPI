using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRest.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MyRest.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MyDbContext context) : base(context) { }

        public async Task<Product> GetProductSupplier(Guid id)
        {
            return await Db.Products.AsNoTracking().Include(f => f.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsSupplieres()
        {
            return await Db.Products.AsNoTracking().Include(f => f.Supplier)
                .OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplier(Guid supplierId)
        {
            return await Get(p => p.SupplierId == supplierId);
        }
    }
}