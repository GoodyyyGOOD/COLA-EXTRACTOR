﻿using Microsoft.AspNetCore.Mvc;

namespace Sofos2toDatawarehouseSyncer.Web.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}