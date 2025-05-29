using Sofos2toDatawarehouseSyncer.Application.Features.Products.Commands.Create;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Commands.Update;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Queries.GetAllCached;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Queries.GetById;
using Sofos2toDatawarehouseSyncer.Web.Areas.Catalog.Models;
using AutoMapper;

namespace Sofos2toDatawarehouseSyncer.Web.Areas.Catalog.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GetAllProductsCachedResponse, ProductViewModel>().ReverseMap();
            CreateMap<GetProductByIdResponse, ProductViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, ProductViewModel>().ReverseMap();
            CreateMap<UpdateProductCommand, ProductViewModel>().ReverseMap();
        }
    }
}