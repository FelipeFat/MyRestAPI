using System;
using System.Threading.Tasks;
using MyRest.Business.Models;

namespace MyRest.Business.Intefaces
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<Address> GetAddressBySupplier(Guid supplierId);
    }
}