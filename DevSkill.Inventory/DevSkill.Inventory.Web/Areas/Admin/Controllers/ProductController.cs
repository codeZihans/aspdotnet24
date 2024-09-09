using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Web;
using DevSkill.Inventory.Infrastructure;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        

        public ProductController(ILogger<ProductController> logger,
            IProductManagementService productManagementService,
            ICategoryManagementService categoryManagementService,
            IMapper mapper)
        {
            _productManagementService = productManagementService;
            _categoryManagementService = categoryManagementService;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexSP()
        {
            var model = new ProductListModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View(model);
        }
        [HttpPost]
        public JsonResult GetProductJsonData([FromBody] ProductListModel model)
        {
            var result = _productManagementService.GetProducts(model.PageIndex, model.PageSize, model.Search,
                model.FormatSortExpression("Title", "Id"));
            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                            HttpUtility.HtmlEncode(record.Title),
                            HttpUtility.HtmlEncode(record.Body),
                            HttpUtility.HtmlEncode(record.Category?.Name),
                            record.PostDate.ToString(),
                            record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(productJsonData);
        }

        [HttpPost]
        public async Task <JsonResult> GetProductJsonDataSP([FromBody] ProductListModel model)
        {
            var result = await _productManagementService.GetProductsSP(model.PageIndex,
                model.PageSize, model.SearchItem,model.FormatSortExpression("Title", "Id"));
            var productJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = (from record in result.data
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Title),
                                HttpUtility.HtmlEncode(record.Body),
                                HttpUtility.HtmlEncode(record.CategoryName),
                                record.PostDate.ToString(),
                                record.Id.ToString()

                        }
                    ).ToArray()
            };

            return Json(productJsonData);
        }
        public IActionResult Create()
        {
            var model = new ProductCreateModel();
            model.SetCategoryValues(_categoryManagementService.GetCategories());    
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateModel model)
        {
            if (ModelState.IsValid)
            {
               var inventory = _mapper.Map<Product>(model);
                inventory.Id = Guid.NewGuid();
                inventory.PostDate = DateTime.Now;
                inventory.Category = _categoryManagementService.GetCategory(model.CategoryId);
              
                try
                {
                    _productManagementService.CreateProduct(inventory);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product created Successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product creation failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product creation failed");

                }
            }
            model.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(model);
        }

        public async Task <IActionResult> Update(Guid Id) 
        {
        Product product = await _productManagementService.GetProductAsync(Id);
            var model =_mapper.Map<ProductUpdateModel>(product);
            model.SetCategoryValues(_categoryManagementService.GetCategories());

            return View(model);
        
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(ProductUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var inventory = _mapper.Map<Product>(model);
                inventory.Category = _categoryManagementService.GetCategory(model.CategoryId);
               try
                {
                    _productManagementService.UpdateProduct(inventory);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product updated Successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index"); 
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product update failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product update failed");

                }
            }

            model.SetCategoryValues(_categoryManagementService.GetCategories());
            return View(model);
        
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
                try
                {
                    _productManagementService.DeleteProduct(id);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product deleted successfully",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Product delete failed",
                        Type = ResponseTypes.Danger
                    });
                    _logger.LogError(ex, "Product delete failed");

                }
            

            return View();

        }
    }
}



