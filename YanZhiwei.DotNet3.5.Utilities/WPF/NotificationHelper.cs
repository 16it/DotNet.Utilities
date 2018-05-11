namespace YanZhiwei.DotNet3._5.Utilities.WPF
{
    using System;
    using System.Linq.Expressions;

    public static class NotificationHelper
    {
        #region Methods

        public static void NotifyPropertyChanged<T, TProperty>(this T model, Expression<Func<T, TProperty>> keySelector)
            where T : NotificationObject
        {
            var _propertyName = string.Empty;
            if ((keySelector.Body as UnaryExpression) != null)
            {
                _propertyName = ((keySelector.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            else if ((keySelector.Body as MemberExpression) != null)
            {
                _propertyName = ((keySelector.Body as MemberExpression).Member.Name);
            }
            if (!string.IsNullOrEmpty(_propertyName))
            {
                model.NotifyChanges(_propertyName);
            }
        }

        #endregion Methods
    }
}