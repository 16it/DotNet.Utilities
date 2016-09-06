namespace YanZhiwei.DotNet.Mvc.Utilities.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DotNet2.Interfaces;

    /// <summary>
    /// 分页控件专为SEO，显示为1-20, 21-40.....
    /// </summary>
    public static class PagerForSeo
    {
        #region Methods

        /// <summary>
        /// SEO优化版分页控件
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pagedList">分页集合</param>
        /// <param name="pageIndexParameterName">分页主键</param>
        /// <param name="sectionSize">分页大小</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString SeoPager(this HtmlHelper helper, IPagedList pagedList, string pageIndexParameterName = "id", int sectionSize = 20)
        {
            StringBuilder _builder = new StringBuilder();
            int pageCount = pagedList.TotalItemCount / pagedList.PageSize + (pagedList.TotalItemCount % pagedList.PageSize == 0 ? 0 : 1);

            if(pageCount > 1)
            {
                List<int> _pages = new List<int>();

                for(int i = 1; i <= pageCount; i++)
                    _pages.Add(i);

                var _sections = _pages.GroupBy(p => (p - 1) / sectionSize);
                var _currentSection = _sections.Single(s => s.Key == (pagedList.CurrentPageIndex - 1) / sectionSize);

                foreach(var item in _currentSection)
                {
                    if(item == pagedList.CurrentPageIndex)
                    {
                        _builder.AppendFormat("<span>{0}</span>", item);
                    }
                    else
                    {
                        _builder.AppendFormat("<a href=\"{1}\">{0}</a>", item, PrepearRouteUrl(helper, pageIndexParameterName, item));
                    }
                }

                if(_sections.Count() > 1)
                {
                    _builder.Append("<br/>");

                    foreach(var item in _sections)
                    {
                        if(item.Key == _currentSection.Key)
                        {
                            _builder.AppendFormat("<span>{0}-{1}</span>", item.First(), item.Last());
                        }
                        else
                        {
                            _builder.AppendFormat("<a href=\"{2}\">{0}-{1}</a>", item.First(), item.Last(), PrepearRouteUrl(helper, pageIndexParameterName, item.First()));
                        }
                    }
                }
            }

            return MvcHtmlString.Create(_builder.ToString());
        }

        private static string PrepearRouteUrl(HtmlHelper helper, string pageIndexParameterName, int pageIndex)
        {
            RouteValueDictionary _routeValues = new RouteValueDictionary();
            _routeValues["action"] = helper.ViewContext.RequestContext.RouteData.Values["action"];
            _routeValues["controller"] = helper.ViewContext.RequestContext.RouteData.Values["controller"];
            _routeValues[pageIndexParameterName] = pageIndex;
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            return urlHelper.RouteUrl(_routeValues);
        }

        #endregion Methods
    }
}