using Sofos2toDatawarehouseSyncer.Infrastructure.Identity.Models;
using Sofos2toDatawarehouseSyncer.Web.Areas.Admin.Models;
using AutoMapper;

namespace Sofos2toDatawarehouseSyncer.Web.Areas.Admin.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}