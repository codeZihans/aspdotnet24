using DevSkill.Inventory.Application;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;

namespace DevSkill.Inventory.Infrastructure.UnitOfWorks
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }




        public InventoryUnitOfWork(InventoryDbContext dbContext,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository) : base(dbContext)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
        }

        public async Task<(IList<ProductDto> data, int total, int totalDisplay)> GetPagedProductsUsingSPAsync(int pageIndex,
            int pageSize, ProductSearchDto search, string? order)
        {
            var procedureName = "GetProducts";

            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductDto>(procedureName,
                   new Dictionary<string, object>
                   {
                    {"PageIndex", pageIndex},
                    {"PageSize", pageSize},
                    {"OrderBy", order},
                    {"CreateDateFrom",DateTime.Parse(search.CreateDateFrom) },
                    {"CreateDateTo",DateTime.Parse(search.CreateDateTo) },
                    { "Title",search.Title ==string.Empty ? null : search.Title },
                    {"CategoryId",search.CategoryId == Guid.Empty ? null : search.CategoryId},
                   
                },


               new Dictionary<string, Type>
               {
                {"Total", typeof(int) },
                {"TotalDisplay", typeof (int) },

               });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

      
    }
}