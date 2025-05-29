using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Commands.Create;
using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Queries.GetAllCached;
using Sofos2toDatawarehouseSyncer.Application.Features.Brands.Queries.GetById;
using Sofos2toDatawarehouseSyncer.Domain.Entities.Catalog;
using AutoMapper;

namespace Sofos2toDatawarehouseSyncer.Application.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsCachedResponse, Brand>().ReverseMap();
        }
    }
}