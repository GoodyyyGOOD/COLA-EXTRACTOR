﻿using Microsoft.AspNetCore.Mvc;

namespace Sofos2toDatawarehouseSyncer.Web.Views.Shared.Components.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}