using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyRest.Business.Intefaces;
using MyRest.Business.Models;
using MyRestAPI.Controllers;
using MyRestAPI.DTOs;

namespace MyRestAPI.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("v{version:apiVersion}/products")]
    public class ProductsController : MainController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;




        public ProductsController(INotifier notifier,
                                  IProductRepository productRepository,
                                  IProductService productService,
                                  IMapper mappe) : base(notifier)
        {
            _productRepository = productRepository;
            _productService = productService;
            _mapper = mappe;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetById(Guid id)
        {
            return CustomResponse("Version 2.0");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<SupplierViewModel>> Delete(Guid id)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var supplierViewModel = _mapper.Map<ProductViewModel>(await _productRepository.GetById(id));

            if (supplierViewModel == null) return NotFound();

            await _productService.Delete(id);

            return CustomResponse(supplierViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierViewModel>> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var imageName = Guid.NewGuid() + "_" + productViewModel.Image;

            if (!FileUpload(productViewModel.ImageUpload, imageName))
            {
                return CustomResponse(productViewModel);
            }

            productViewModel.Image = imageName;
            await _productService.Add(_mapper.Map<Product>(productViewModel));

            return CustomResponse(productViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Atualizar(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                NotifyError("The ids are not the same!");
                return CustomResponse();
            }

            var updateProduct = await _productRepository.GetById(id);

            if (string.IsNullOrEmpty(productViewModel.Image))
                productViewModel.Image = updateProduct.Image;

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            if (productViewModel.ImageUpload != null)
            {
                var imageName = Guid.NewGuid() + "_" + productViewModel.Image;
                if (!FileUpload(productViewModel.ImageUpload, imageName))
                {
                    return CustomResponse(ModelState);
                }

                updateProduct.Image = imageName;
            }

            updateProduct.SupplierId = productViewModel.SupplierId;
            updateProduct.Name = productViewModel.Name;
            updateProduct.Description = productViewModel.Description;
            updateProduct.ProductValue = productViewModel.ProductValue;
            updateProduct.Active = productViewModel.Active;

            await _productService.Update(_mapper.Map<Product>(updateProduct));

            return CustomResponse(productViewModel);
        }

        private bool FileUpload(string file, string fileName)
        {
            if (string.IsNullOrEmpty(file))
            {
                NotifyError("Please Select the Image!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(file);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

            if (System.IO.File.Exists(filePath))
            {
                NotifyError("There is already a file with the same name!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

            return true;
        }
    }
}
