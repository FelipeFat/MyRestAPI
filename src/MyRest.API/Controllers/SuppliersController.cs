using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRestAPI.DTOs;

namespace MyRestAPI.Controllers
{
    [Route("supplieres")]
    public class SuppliersController : MainController
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(INotifier notifier,
                                    ISupplierRepository supplierRepository,
                                    ISupplierService supplierService,
                                    IMapper mapper) : base(notifier)
        {
            _supplierRepository = supplierRepository;
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<SupplierViewModel>> GetAll()
        {
            var supplier = _mapper.Map<IEnumerable<SupplierViewModel>>(await _supplierRepository.GetAlls());

            return supplier;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> GetById(Guid id)
        {
            var supplier = _mapper.Map<SupplierViewModel>(await _supplierRepository.GetSupplierProductsAddress(id));

            if(supplier == null) return NotFound();

            return supplier;
        }

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Create(SupplierViewModel supplierViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Add(supplier);

            return CustomResponse(supplierViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Update(Guid id, SupplierViewModel supplierViewModel)
        {
            if(id != supplierViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(supplierViewModel);
            await _supplierService.Update(supplier);

            return CustomResponse(supplierViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var supplier = _mapper.Map<Supplier>(await _supplierRepository.GetById(id));

            if (supplier == null) return NotFound();

            await _supplierService.Delete(id);

            return CustomResponse();
        }
    }
}
