using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application
{
    public  interface IInventoryUnitOfWork :IUnitOfWork
    {
        IProductRepository ProductRepository { get;}
        ICategoryRepository CategoryRepository { get; }

        Task<(IList<ProductDto> data, int total, int totalDisplay) > GetPagedProductsUsingSPAsync(int pageIndex,
            int pageSize, ProductSearchDto search, string? order);
    }
}
