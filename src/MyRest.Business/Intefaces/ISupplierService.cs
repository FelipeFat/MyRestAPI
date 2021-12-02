using System;
using System.Threading.Tasks;
using MyRest.Business.Models;

namespace MyRest.Business.Intefaces
{
    public interface ISupplierService : IDisposable
    {
        Task<bool> Add(Supplier supplier);
        Task<bool> Update(Supplier supplier);
        Task<bool> Delete(Guid id);

        Task UpdateAddress(Address address);
    }
}