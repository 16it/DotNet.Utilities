namespace YanZhiwei.DotNet3._5.Utilities.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// 程序集反射辅助类
    /// </summary>
    public static class AssemblyHelper
    {
        #region Methods

        /// <summary>
        /// 扫描程序集找到带有某个Attribute的所有PropertyInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>字典</returns>
        public static Dictionary<PropertyInfo, T> FindAllPropertyByAttribute<T>()
            where T : Attribute
        {
            return FindAllPropertyByAttribute<T>("*.dll");
        }

        /// <summary>
        /// 扫描程序集找到带有某个Attribute的所有PropertyInfo
        /// </summary>
        /// <typeparam name="T">反省</typeparam>
        /// <param name="searchpattern">文件名过滤</param>
        /// <returns>字典</returns>
        public static Dictionary<PropertyInfo, T> FindAllPropertyByAttribute<T>(string searchpattern)
            where T : Attribute
        {
            Dictionary<PropertyInfo, T> _result = new Dictionary<PropertyInfo, T>();
            Type _attr = typeof(T);

            string _domain = GetBaseDirectory();
            string[] _dllFiles = Directory.GetFiles(_domain, searchpattern, SearchOption.TopDirectoryOnly);

            foreach (string dllFileName in _dllFiles)
            {
                foreach (Type type in Assembly.LoadFrom(dllFileName).GetLoadableTypes())
                {
                    foreach (var property in type.GetProperties())
                    {
                        object[] _attrs = property.GetCustomAttributes(_attr, true);

                        if (_attrs.Length == 0)
                            continue;

                        _result.Add(property, (T)_attrs.First());
                    }
                }
            }

            return _result;
        }

        /// <summary>
        /// 扫描程序集找到类型上带有某个Attribute的所有类型
        /// </summary>
        /// <typeparam name="T">反省</typeparam>
        /// <returns>字典</returns>
        public static Dictionary<string, List<T>> FindAllTypeByAttribute<T>()
            where T : Attribute
        {
            return FindAllTypeByAttribute<T>("*.dll");
        }

        /// <summary>
        /// 扫描程序集找到类型上带有某个Attribute的所有类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchpattern">文件名过滤</param>
        /// <returns>字典</returns>
        public static Dictionary<string, List<T>> FindAllTypeByAttribute<T>(string searchpattern)
            where T : Attribute
        {
            Dictionary<string, List<T>> _result = new Dictionary<string, List<T>>();
            Type _attr = typeof(T);

            string _domain = GetBaseDirectory();
            string[] _dllFiles = Directory.GetFiles(_domain, searchpattern, SearchOption.TopDirectoryOnly);

            foreach (string dllFileName in _dllFiles)
            {
                foreach (Type type in Assembly.LoadFrom(dllFileName).GetLoadableTypes())
                {
                    string _typeName = type.AssemblyQualifiedName;

                    object[] _attrs = type.GetCustomAttributes(_attr, true);
                    if (_attrs.Length == 0)
                        continue;

                    _result.Add(_typeName, new List<T>());

                    foreach (T a in _attrs)
                        _result[_typeName].Add(a);
                }
            }

            return _result;
        }

        /// <summary>
        /// 扫描程序集找到继承了某基类的所有子类
        /// </summary>
        /// <param name="inheritType">基类</param>
        /// <returns>子类集合</returns>
        public static List<Type> FindTypeByInheritType(Type inheritType)
        {
            return FindTypeByInheritType(inheritType, "*.dll");
        }

        /// <summary>
        /// 扫描程序集找到继承了某基类的所有子类
        /// </summary>
        /// <param name="inheritType">基类</param>
        /// <param name="searchpattern">文件名过滤</param>
        /// <returns>子类集合</returns>
        public static List<Type> FindTypeByInheritType(Type inheritType, string searchpattern)
        {
            List<Type> _result = new List<Type>();
            Type _attr = inheritType;

            string _domain = GetBaseDirectory();
            string[] _dllFiles = Directory.GetFiles(_domain, searchpattern, SearchOption.TopDirectoryOnly);

            foreach (string dllFileName in _dllFiles)
            {
                foreach (Type type in Assembly.LoadFrom(dllFileName).GetLoadableTypes())
                {
                    if (type.BaseType == inheritType)
                    {
                        _result.Add(type);
                    }
                }
            }

            return _result;
        }

        /// <summary>
        /// 扫描程序集找到实现了某个接口的第一个实例
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>类型</returns>
        public static T FindTypeByInterface<T>()
            where T : class
        {
            return FindTypeByInterface<T>("*.dll");
        }

        /// <summary>
        /// 扫描程序集找到实现了某个接口的第一个实例
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="searchpattern">文件名过滤</param>
        /// <returns>类型</returns>
        public static T FindTypeByInterface<T>(string searchpattern)
            where T : class
        {
            Type _interfaceType = typeof(T);

            string _domain = GetBaseDirectory();
            string[] _dllFiles = Directory.GetFiles(_domain, searchpattern, SearchOption.TopDirectoryOnly);

            foreach (string dllFileName in _dllFiles)
            {
                foreach (Type type in Assembly.LoadFrom(dllFileName).GetLoadableTypes())
                {
                    if (_interfaceType != type && _interfaceType.IsAssignableFrom(type))
                    {
                        T _instance = Activator.CreateInstance(type) as T;
                        return _instance;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 得到当前应用程序的根目录
        /// </summary>
        /// <returns>根目录</returns>
        public static string GetBaseDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;

            if (AppDomain.CurrentDomain.SetupInformation.PrivateBinPath == null)
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return baseDirectory;
        }

        /// <summary>
        /// 得到入口程序集，兼容Web和Winform
        /// </summary>
        /// <returns></returns>
        public static Assembly GetEntryAssembly()
        {
            var _entryAssembly = Assembly.GetEntryAssembly();
            if (_entryAssembly != null)
                return _entryAssembly;

            if (HttpContext.Current == null ||
                HttpContext.Current.ApplicationInstance == null)
                return Assembly.GetExecutingAssembly();

            Type _type = HttpContext.Current.ApplicationInstance.GetType();
            while (_type != null && _type.Namespace == "ASP")
            {
                _type = _type.BaseType;
            }

            return _type == null ? null : _type.Assembly;
        }

        /// <summary>
        /// 获取Assembly类型集合
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>类型集合</returns>
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        #endregion Methods
    }
}