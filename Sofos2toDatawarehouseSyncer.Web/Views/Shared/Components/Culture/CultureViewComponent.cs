using Microsoft.AspNetCore.Mvc;

namespace Sofos2toDatawarehouseSyncer.Web.Views.Shared.Components.Culture
{
    public class CultureViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}