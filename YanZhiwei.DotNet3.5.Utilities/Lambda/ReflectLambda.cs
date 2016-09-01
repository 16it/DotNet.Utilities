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
            MemberInfo _memberInfo = GetPropertyInformation(keySelector.Body);
            var _attr = _memberInfo.GetAttribute<DisplayNameAttribute>(false);

            return _attr == null ? _memberInfo.Name : _attr.DisplayName;
        }

        /// <summary>
        /// 获取属性的数值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="keySelector">选择表达式</param>
        /// <returns>属性的数值</returns>
        /// 时间：2016/9/1 9:10
        /// 备注：
        public static object GetPropertyValue<T>(this T model, Expression<Func<T, object>> keySelector) where T : class
        {
            MemberInfo _memberInfo = GetPropertyInformation(keySelector.Body);

            if(_memberInfo is PropertyInfo)
            {
                return ((PropertyInfo)_memberInfo).GetValue(model, null);
            }
            else if(_memberInfo is FieldInfo)
            {
                return ((FieldInfo)_memberInfo).GetValue(model);
            }

            return null;
        }

        /// <summary>
        /// 设置属性的数值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="F">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="value">设置的数值</param>
        /// <param name="keySelector">选择表达式</param>
        /// 时间：2016/9/1 9:20
        /// 备注：
        public static void SetPropertyValue<T, F>(this T model, F value, Expression<Func<T, object>> keySelector) where T : class
        {
            MemberInfo _memberInfo = GetPropertyInformation(keySelector.Body);

            if(_memberInfo is PropertyInfo)
            {
                ((PropertyInfo)_memberInfo).SetValue(model, value, null);
            }
            else if(_memberInfo is FieldInfo)
            {
                ((FieldInfo)_memberInfo).SetValue(model, value);
            }
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