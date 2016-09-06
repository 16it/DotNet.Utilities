namespace YanZhiwei.DotNet.Mvc.Utilities.Controls
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// RadioBoxList
    /// </summary>
    /// 时间：2016/9/6 17:36
    /// 备注：
    public static class RadioBoxListHelper
    {
        #region Methods

        /// <summary>
        /// Radioes the box list.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// 时间：2016/9/6 17:43
        /// 备注：
        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name)
        {
            return RadioBoxList(helper, name, helper.ViewData[name] as IEnumerable<SelectListItem>, new { });
        }

        /// <summary>
        /// Radioes the box list.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="selectList">The select list.</param>
        /// <returns></returns>
        /// 时间：2016/9/6 17:43
        /// 备注：
        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
        {
            return RadioBoxList(helper, name, selectList, new { });
        }

        /// <summary>
        /// RadioBoxList
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="name">Name</param>
        /// <param name="selectList">绑定集合</param>
        /// <param name="htmlAttributes">附加html属性</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString RadioBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            IDictionary<string, object> _htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            _htmlAttributes.Add("type", "radio");
            _htmlAttributes.Add("name", name);
            StringBuilder _builder = new StringBuilder();
            int i = 0;
            int j = 0;

            foreach(SelectListItem selectItem in selectList)
            {
                string _id = string.Format("{0}{1}", name, j++);
                IDictionary<string, object> _newHtmlAttributes = _htmlAttributes.DeepCopy();
                _newHtmlAttributes.Add("value", selectItem.Value);
                _newHtmlAttributes.Add("id", _id);
                var _selectedValue = (selectList as SelectList).SelectedValue;

                if(_selectedValue == null)
                {
                    if(i++ == 0)
                        _newHtmlAttributes.Add("checked", null);
                }
                else if(selectItem.Value == _selectedValue.ToString())
                {
                    _newHtmlAttributes.Add("checked", null);
                }

                TagBuilder _tagBuilder = new TagBuilder("input");
                _tagBuilder.MergeAttributes<string, object>(_newHtmlAttributes);
                string _inputAllHtml = _tagBuilder.ToString(TagRenderMode.SelfClosing);
                _builder.AppendFormat(@" {0}  <label for='{2}'>{1}</label>",
                                      _inputAllHtml, selectItem.Text, _id);
            }

            return MvcHtmlString.Create(_builder.ToString());
        }

        private static IDictionary<string, object> DeepCopy(this IDictionary<string, object> ht)
        {
            Dictionary<string, object> _ht = new Dictionary<string, object>();

            foreach(var p in ht)
            {
                _ht.Add(p.Key, p.Value);
            }

            return _ht;
        }

        #endregion Methods
    }
}