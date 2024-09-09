using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Application.Services
{
    public interface IProductManagementService
    {
        void CreateProduct(Product product);
        void DeleteProduct(Guid id);
        Task<Product> GetProductAsync(Guid id);
        (IList<Product>data, int total,int totalDisplay)GetProducts(int pageIndex, int pageSize,
            DataTablesSearch search, string ? order);
       Task<(IList<ProductDto> data, int total, int totalDisplay)> GetProductsSP(int pageIndex, int pageSize,
            ProductSearchDto search, string? order);
        
        void UpdateProduct(Product inventory);
    }
}