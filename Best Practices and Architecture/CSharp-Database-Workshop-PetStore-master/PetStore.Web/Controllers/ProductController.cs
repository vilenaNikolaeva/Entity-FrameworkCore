using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services.Interfaces;
using PetStore.ViewModels.Product;
using PetStore.ViewModels.Product.InputModels;
using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;
using PetStore.ViewModels.Product.OutputModels;

namespace PetStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        public ProductController(IProductService productService,IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult All()
        {
            var allProducts = productService
                .GetAll()
                .ToList();

            ICollection<ListAllProductsViewModel> viewModels =
                this.mapper.Map<List<ListAllProductsViewModel>>(allProducts);
            
            return View(viewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }
            AddProductInputServiceModel serviceModel = this.mapper.Map<AddProductInputServiceModel>(model);

            this.productService.AddProduct(serviceModel);

            return this.RedirectToAction("All");
        }

        [HttpGet]

        public IActionResult Details(string id)
        {
            ProductDetailsServiceModel serviceModel = this.productService.GetById(id);

            ProductDetailsViewModel viewModel = this.mapper.Map<ProductDetailsViewModel>(serviceModel);
            return this.View(viewModel);
        }

        [HttpPost]

        public IActionResult Search(string word)
        {
            if (word == null)
            {
                return this.RedirectToAction("All");
            }

            ICollection<ListAllProductsByNameServiceModel> serviceModels =
                this.productService.SearchByName(word,false);

            ICollection<ListAllProductsViewModel> viewModel =
                this.mapper.Map<List<ListAllProductsViewModel>>
                (serviceModels);

            return this.View("All", viewModel);
        }

        [HttpPost]

        public IActionResult Edit(string productId, EditProductInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            EditProductInputServiceModel editViewModel = this.mapper.Map<EditProductInputServiceModel>(model);

            this.productService.EditProduct(productId, editViewModel);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var editViewModel = this.productService.GetById(id); 
            var editProductModel = this.mapper.Map<ProductDetailsViewModel>(editViewModel);
            this.ViewData["id"] = id;
            return this.View(editProductModel);
        }
    }
}