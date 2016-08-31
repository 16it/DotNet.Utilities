namespace YanZhiwei.DotNet3._5.Utilities.Lambda
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// 反射 帮助类
    /// </summary>
    /// 时间：2016/8/31 17:27
    /// 备注：
    public static class ReflectLambda
    {
        #region Methods

        /// <summary>
        /// 获取属性的DisplayName
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="keySelector">选择表达式</param>
        /// <returns>DisplayName名称</returns>
        /// 时间：2016/8/31 17:31
        /// 备注：
        public static string GetPropertyDisplayName<T>(this T model, Expression<Func<T, object>> keySelector) where T : class
        {
            var _memberInfo = GetPropertyInformation(keySelector.Body);
            var _attr = _memberInfo.GetAttribute<DisplayNameAttribute>(false);

            return _attr == null ? _memberInfo.Name : _attr.DisplayName;
        }

        private static T GetAttribute<T>(this MemberInfo member, bool isRequired)
        where T : Attribute
        {
            var _attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();
            return (T)_attribute;
        }

        private static MemberInfo GetPropertyInformation(Expression propertyExpression)
        {
            MemberExpression _memberExpr = propertyExpression as MemberExpression;

            if(_memberExpr == null)
            {
                UnaryExpression unaryExpr = propertyExpression as UnaryExpression;

                if(unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    _memberExpr = unaryExpr.Operand as MemberExpression;
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