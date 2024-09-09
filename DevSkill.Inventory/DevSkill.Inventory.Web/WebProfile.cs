using AutoMapper;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Web.Areas.Admin.Models;

namespace DevSkill.Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile() 
        {
            CreateMap<ProductCreateModel, Product>().ReverseMap();
            CreateMap<ProductUpdateModel, Product>().ReverseMap();
        } 
    }
}
