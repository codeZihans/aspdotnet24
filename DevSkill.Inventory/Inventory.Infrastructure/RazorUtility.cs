using Microsoft.AspNetCore.Mvc.Rendering;
using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Infrastructure
{
    public class RazorUtility
    {
        public static IList<SelectListItem>ConvertCategories(IList<Category>categories)
        {
            var items = (from c in categories
                         select new SelectListItem(c.Name, c.Id.ToString())).ToList();
            items.Insert(0, new SelectListItem("All", Guid.Empty.ToString()));
            return items;

        }
    }
}
