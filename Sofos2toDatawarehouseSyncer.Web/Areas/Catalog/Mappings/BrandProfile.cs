using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Commands.Create;
using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Commands.Update;
using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Queries.GetAllCached;
using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Queries.GetById;
using Sofos2toDatawarehouseSyncer.Web.Areas.Catalog.Models;
using AutoMapper;

namespace Sofos2toDatawarehouseSyncer.Web.Areas.Catalog.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<GetAllBrandsCachedResponse, BrandViewModel>().ReverseMap();
            CreateMap<GetBrandByIdResponse, BrandViewModel>().ReverseMap();
            CreateMap<CreateBrandCommand, BrandViewModel>().ReverseMap();
            CreateMap<UpdateBrandCommand, BrandViewModel>().ReverseMap();
        }
    }
}