using Sofos2toDatawarehouseSyncer.Application.Features.Products.Commands.Create;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Queries.GetAllCached;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Queries.GetAllPaged;
using Sofos2toDatawarehouseSyncer.Application.Features.Products.Queries.GetById;
using Sofos2toDatawarehouseSyncer.Domain.Entities.Catalog;
using AutoMapper;

namespace Sofos2toDatawarehouseSyncer.Application.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>().ReverseMap();
            CreateMap<GetProductByIdResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsCachedResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsResponse, Product>().ReverseMap();
        }
    }
}