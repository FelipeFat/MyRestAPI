using System;
using System.Threading.Tasks;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRest.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MyRest.Data.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<Supplier> GetSupplierAddress(Guid id)
        {
            return await Db.Supplieres.AsNoTracking()
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Supplier> GetSupplierProductsAddress(Guid id)
        {
            return await Db.Supplieres.AsNoTracking()
                .Include(c => c.Products)
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}