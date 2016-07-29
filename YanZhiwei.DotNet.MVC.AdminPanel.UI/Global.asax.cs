using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using YanZhiwei.DotNet.Core.Cache;
using YanZhiwei.DotNet.MVC.AdminPanel.Config;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CacheConfigContext.SetCacheConfig(CachedConfigContext.Current.CacheConfig);
        }
    }
}
