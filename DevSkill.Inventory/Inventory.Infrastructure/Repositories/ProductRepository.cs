using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
     public class ProductRepository : Repository<Product, Guid>, IProductRepository
    {
        public ProductRepository(InventoryDbContext context) : base(context)
        {
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Title == title) > 0;
            }
            else
            {
                return GetCount(x => x.Title == title) > 0;
            }
        }

        public (IList<Product> data,int total, int totalDisplay) GetPagedProducts(int pageIndex,
            int pageSize, DataTablesSearch search, string? order)
        {
            if(string.IsNullOrWhiteSpace(search.Value))
                return GetDynamic(null, order, y => y.Include(z => z.Category),
                    pageIndex,pageSize,true);
            else 
            return GetDynamic(x => x.Title.Contains(search.Value), order,
                y => y.Include(z => z.Category), pageIndex, pageSize, true);
        }
        public async Task<Product> GetProductAsync(Guid id)
        {
            return (await GetAsync(x => x.Id == id, y => y.Include(z => z.Category))).FirstOrDefault();
        }

        
    }
}
 