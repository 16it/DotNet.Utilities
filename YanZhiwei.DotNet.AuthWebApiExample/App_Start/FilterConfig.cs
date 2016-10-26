using System.Web;
using System.Web.Mvc;

namespace YanZhiwei.DotNet.AuthWebApiExample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
