using Microsoft.Practices.Unity;
using System;

namespace YanZhiwei.DotNet.Unity.Utilities
{
    /// <summary>
    /// Unity辅助类
    /// </summary>
    public static class UnityHelper
    {
        /// <summary>
        /// Tries the resolve.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static T TryResolve<T>(this IUnityContainer container)
        {
            bool _isRegistered = true;
            Type _typeToCheck = typeof(T);
            if (_typeToCheck.IsInterface || _typeToCheck.IsAbstract)
            {
                _isRegistered = container.IsRegistered(_typeToCheck);

                if (!_isRegistered && _typeToCheck.IsGenericType)
                {
                    var openGenericType = _typeToCheck.GetGenericTypeDefinition();
                    _isRegistered = container.IsRegistered(openGenericType);
                }
            }
            return _isRegistered ? container.Resolve<T>() : default(T);
        }
    }
}