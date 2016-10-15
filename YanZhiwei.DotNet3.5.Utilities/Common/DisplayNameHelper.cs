namespace YanZhiwei.DotNet3._5.Utilities.Common
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 获取[DisplayName]辅助类
    /// </summary>
    /// 时间:2016/10/15 23:31
    /// 备注:
    public static class DisplayNameHelper
    {
        #region Methods

        /// <summary>
        /// 获取[DisplayName]名称
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="propertyName">需要获取[DisplayName]的属性</param>
        /// <returns>[DisplayName]</returns>
        /// 时间:2016/10/15 23:36
        /// 备注:
        public static string Get<T>(string propertyName)
            where T : class
        {
            return Get(typeof(T), propertyName);
        }

        /// <summary>
        /// 获取[DisplayName]名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="propertyName">需要获取[DisplayName]的属性</param>
        /// <returns>[DisplayName]</returns>
        /// 时间:2016/10/15 23:37
        /// 备注:
        public static string Get(Type type, string propertyName)
        {
            PropertyInfo _property = type.GetProperty(propertyName);

            if(_property == null) return null;

            return Get(_property);
        }

        /// <summary>
        /// 获取[DisplayName]名称
        /// </summary>
        /// <param name="property">PropertyInfo</param>
        /// <returns>[DisplayName]</returns>
        /// 时间:2016/10/15 23:37
        /// 备注:
        public static string Get(PropertyInfo property)
        {
            string _attrName = GetAttributeDisplayName(property);

            if(!string.IsNullOrEmpty(_attrName))
                return _attrName;

            var _metaName = GetMetaDisplayName(property);

            if(!string.IsNullOrEmpty(_metaName))
                return _metaName;

            return property.Name.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取[DisplayName]名称
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="keySelector">选择表达式</param>
        /// <returns>[DisplayName]</returns>
        /// 时间:2016/10/15 23:42
        /// 备注:
        public static string Get<T>(Expression<Func<T, object>> keySelector)
        {
            MemberInfo _memberInfo = GetPropertyInformation(keySelector.Body);

            if(_memberInfo == null)
            {
                return null;
            }

            DisplayNameAttribute _attr = _memberInfo.GetAttribute<DisplayNameAttribute>(false);
            return _attr == null ? _memberInfo.Name : _attr.DisplayName;
        }

        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            object[] _atts = property.GetCustomAttributes(
                                 typeof(DisplayNameAttribute), true);

            if(_atts.Length == 0)
                return null;

            DisplayNameAttribute _displayNameAttribute = _atts[0] as DisplayNameAttribute;
            return _displayNameAttribute != null ? _displayNameAttribute.DisplayName : null;
        }

        private static string GetMetaDisplayName(PropertyInfo property)
        {
            if(property.DeclaringType != null)
            {
                object[] _atts = property.DeclaringType.GetCustomAttributes(
                                     typeof(MetadataTypeAttribute), true);

                if(_atts.Length == 0)
                    return null;

                MetadataTypeAttribute _metaAttr = _atts[0] as MetadataTypeAttribute;

                if(_metaAttr != null)
                {
                    PropertyInfo _metaProperty =
                        _metaAttr.MetadataClassType.GetProperty(property.Name);
                    return _metaProperty == null ? null : GetAttributeDisplayName(_metaProperty);
                }
            }

            return null;
        }

        private static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            MemberExpression _memberExpr = propertyExpression as MemberExpression;

            if(_memberExpr == null)
            {
                UnaryExpression _unaryExpr = propertyExpression as UnaryExpression;

                if(_unaryExpr != null && _unaryExpr.NodeType == ExpressionType.Convert)
                {
                    _memberExpr = _unaryExpr.Operand as MemberExpression;
                }
            }

            if(_memberExpr != null && _memberExpr.Member.MemberType == MemberTypes.Property)
            {
                return _memberExpr.Member;
            }

            return null;
        }

        #endregion Methods
    }
}