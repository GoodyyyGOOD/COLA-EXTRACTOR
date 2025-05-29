using Microsoft.AspNetCore.Mvc;

namespace Sofos2toDatawarehouseSyncer.Web.Views.Shared.Components.Title
{
    public class TitleViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}