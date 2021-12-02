using System;
using System.Linq;
using System.Threading.Tasks;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRest.Business.Models.Validations;

namespace MyRest.Business.Services
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IAddressRepository _addressRepository;

        public SupplierService(ISupplierRepository supplierRepository, 
                                 IAddressRepository addressRepository,
                                 INotifier notificador) : base(notificador)
        {
            _supplierRepository = supplierRepository;
            _addressRepository = addressRepository;
        }

        public async Task<bool> Add(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier) 
                || !ExecuteValidation(new AddressValidation(), supplier.Address)) return false;

            if (_supplierRepository.Get(f => f.Document == supplier.Document).Result.Any())
            {
                Notify("There is already a supplier with the same document.");
                return false;
            }

            await _supplierRepository.Add(supplier);
            return true;
        }

        public async Task<bool> Update(Supplier supplier)
        {
            if (!ExecuteValidation(new SupplierValidation(), supplier)) return false;

            if (_supplierRepository.Get(f => f.Document == supplier.Document && f.Id != supplier.Id).Result.Any())
            {
                Notify("There is already a supplier with this document informed.");
                return false;
            }

            await _supplierRepository.Update(supplier);
            return true;
        }

        public async Task UpdateAddress(Address address)
        {
            if (!ExecuteValidation(new AddressValidation(), address)) return;

            await _addressRepository.Update(address);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (_supplierRepository.GetSupplierProductsAddress(id).Result.Products.Any())
            {
                Notify("The supplier has registered products!");
                return false;
            }

            var address = await _addressRepository.GetAddressBySupplier(id);

            if (address != null)
            {
                await _addressRepository.Delete(address.Id);
            }

            await _supplierRepository.Delete(id);
            return true;
        }

        public void Dispose()
        {
            _supplierRepository?.Dispose();
            _addressRepository?.Dispose();
        }
    }
}