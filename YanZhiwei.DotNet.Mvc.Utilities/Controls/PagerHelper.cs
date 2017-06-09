using System;
using System.Web.Mvc;
using YanZhiwei.DotNet2.Interfaces;

namespace YanZhiwei.DotNet.Mvc.Utilities.Controls
{
    /// <summary>
    /// 分页
    /// </summary>
    public static class PagerHelper
    {
        /// <summary>
        /// Bootstraps样式的分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageList">分页集合</param>
        /// <param name="getPagerUrlFactory">下一页请求URL</param>
        /// <param name="numberOfLinks">显示多少分页按钮</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapPager(this HtmlHelper helper, IPagedList pageList, Func<int, string> getPagerUrlFactory, int numberOfLinks = 5)
        {
            
            int _totalItems = pageList.TotalItemCount;
            int _pageSize = pageList.PageSize;
            int _currentPageIndex = pageList.CurrentPageIndex;
            if (_totalItems > 0)
            {
                var _totalPages = (int)Math.Ceiling(_totalItems / (double)_pageSize);
                var _lastPageNumber = (int)Math.Ceiling((double)_currentPageIndex / numberOfLinks) * numberOfLinks;
                var _firstPageNumber = _lastPageNumber - (numberOfLinks - 1);
                var _hasPreviousPage = _currentPageIndex > 1;
                var _hasNextPage = _currentPageIndex < _totalPages;
                if (_lastPageNumber > _totalPages)
                {
                    _lastPageNumber = _totalPages;
                }

                TagBuilder _ulBuilder = new TagBuilder("ul");
                _ulBuilder.AddCssClass("pagination");
                _ulBuilder.InnerHtml += AddLink(1, getPagerUrlFactory, _currentPageIndex == 1, "disabled", "<<", "First Page");
                _ulBuilder.InnerHtml += AddLink(_currentPageIndex - 1, getPagerUrlFactory, !_hasPreviousPage, "disabled", "<", "Previous Page");
                for (int i = _firstPageNumber; i <= _lastPageNumber; i++)
                {
                    _ulBuilder.InnerHtml += AddLink(i, getPagerUrlFactory, i == _currentPageIndex, "active", i.ToString(), i.ToString());
                }
                _ulBuilder.InnerHtml += AddLink(_currentPageIndex + 1, getPagerUrlFactory, !_hasNextPage, "disabled", ">", "Next Page");
                _ulBuilder.InnerHtml += AddLink(_totalPages, getPagerUrlFactory, _currentPageIndex == _totalPages, "disabled", ">>", "Last Page");
                return MvcHtmlString.Create(_ulBuilder.ToString());
            }
            return MvcHtmlString.Empty;
        }



        private static TagBuilder AddLink(int index, Func<int, string> getPagerUrlFactory, bool condition, string classToAdd, string linkText, string tooltip)
        {
            TagBuilder _liBuilder = new TagBuilder("li");
            _liBuilder.MergeAttribute("title", tooltip);
            if (condition)
            {
                _liBuilder.AddCssClass(classToAdd);
            }
            TagBuilder _aHtml = new TagBuilder("a");
            _aHtml.MergeAttribute("href", !condition ? getPagerUrlFactory(index) : "javascript:");
            _aHtml.SetInnerText(linkText);
            _liBuilder.InnerHtml = _aHtml.ToString();
            return _liBuilder;
        }
    }
}