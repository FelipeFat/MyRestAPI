using System;
using System.Threading.Tasks;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRest.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace MyRest.Data.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        public AddressRepository(MyDbContext context) : base(context) { }

        public async Task<Address> GetAddressBySupplier(Guid supplierId)
        {
            return await Db.Addresss.AsNoTracking()
                .FirstOrDefaultAsync(f => f.SupplierId == supplierId);
        }
    }
}