using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.DTOs;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    //everything gets derived from controller base
    {
        private readonly StoreContext _context;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IMapper _mapper;

        public ProductsController(StoreContext context, IGenericRepository<Product> productRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IGenericRepository<ProductBrand> productBrandRepo, 
        IMapper mapper)
        {
            
            //when a request comes in a new instance of our controller is created,
            //controller sees what its dependency is which is the store context, 
            // which we can work with
            //scoped means its available for the lifetime of the request
            //once the contect is other its then released from memory
            _context = context;
            _productsRepo = productRepo;
            _productTypesRepo = productTypeRepo;
            _productBrandsRepo = productBrandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts(
           [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productParams);
            var products = await _productsRepo.ListOnlyAsync(spec);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItem = await _productsRepo.CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
            //entity framework core
            return  Ok(new Pagination<ProductToReturnDTO>(productParams.PageIndex, productParams.PageSize,
            totalItem, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
           var spec = new ProductWithTypesAndBrandsSpecification(id);
            //dont want the request to wait around if it could be doing other things, passes
            //the request to av delegate
            //the request the thread is running on can go handle other things
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDTO>(product);
            //takes the first one in and converts it to the second one
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypesRepo.ListAllAsync();
            //entity framework core
            return Ok(productTypes);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandsRepo.ListAllAsync();
            //entity framework core
            return Ok(productBrands);
        }




    }
}