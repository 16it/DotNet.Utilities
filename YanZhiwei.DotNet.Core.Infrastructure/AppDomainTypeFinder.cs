namespace YanZhiwei.DotNet.Core.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 应用程序域TypeFinder
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.ITypeFinder" />
    public class AppDomainTypeFinder : ITypeFinder
    {
        #region Fields

        private IList<string> assemblyNames = new List<string>();
        private string assemblyRestrictToLoadingPattern = ".*";
        private string assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";
        private bool ignoreReflectionErrors = true;
        private bool loadAppDomainAssemblies = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 要查找类型的应用程序域
        /// </summary>
        public virtual AppDomain App
        {
            get
            {
                return AppDomain.CurrentDomain;
            }
        }

        /// <summary>
        /// 除了在appdomain中加载的程序集之外，获取或设置程序集加载启动
        /// </summary>
        public IList<string> AssemblyNames
        {
            get
            {
                return assemblyNames;
            }

            set
            {
                assemblyNames = value;
            }
        }

        /// <summary>
        /// 程序集对加载模式进行限制
        /// </summary>
        public string AssemblyRestrictToLoadingPattern
        {
            get
            {
                return assemblyRestrictToLoadingPattern;
            }

            set
            {
                assemblyRestrictToLoadingPattern = value;
            }
        }

        /// <summary>
        /// 设置或获取需要跳过的程序集
        /// </summary>
        public string AssemblySkipLoadingPattern
        {
            get
            {
                return assemblySkipLoadingPattern;
            }

            set
            {
                assemblySkipLoadingPattern = value;
            }
        }

        /// <summary>
        /// 是否加载应用程序域程序集
        /// </summary>
        public bool LoadAppDomainAssemblies
        {
            get
            {
                return loadAppDomainAssemblies;
            }

            set
            {
                loadAppDomainAssemblies = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>
        /// IEnumerable
        /// </returns>
        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <param name="assignTypeFrom">指定类型.</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类.</param>
        /// <returns>
        /// IEnumerable
        /// </returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>
        /// IEnumerable
        /// </returns>
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            return FindClassesOfType(typeof(T), assemblies, onlyConcreteClasses);
        }

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <param name="assignTypeFrom">指定类型.</param>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>
        /// IEnumerable
        /// </returns>
        public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
        {
            var result = new List<Type>();

            try
            {
                foreach (var a in assemblies)
                {
                    Type[] types = null;

                    try
                    {
                        types = a.GetTypes();
                    }

                    catch
                    {
                        //Entity Framework 6 doesn't allow getting types (throws an exception)
                        if (!ignoreReflectionErrors)
                        {
                            throw;
                        }
                    }

                    if (types != null)
                    {
                        foreach (var t in types)
                        {
                            if (assignTypeFrom.IsAssignableFrom(t) || (assignTypeFrom.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                            {
                                if (!t.IsInterface)
                                {
                                    if (onlyConcreteClasses)
                                    {
                                        if (t.IsClass && !t.IsAbstract)
                                        {
                                            result.Add(t);
                                        }
                                    }

                                    else
                                    {
                                        result.Add(t);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;

                foreach (var e in ex.LoaderExceptions)
                    msg += e.Message + Environment.NewLine;

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);
                throw fail;
            }

            return result;
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns>
        /// 程序集
        /// </returns>
        public virtual IList<Assembly> GetAssemblies()
        {
            var addedAssemblyNames = new List<string>();
            var assemblies = new List<Assembly>();

            if (LoadAppDomainAssemblies)
                AddAssembliesInAppDomain(addedAssemblyNames, assemblies);

            AddConfiguredAssemblies(addedAssemblyNames, assemblies);
            return assemblies;
        }

        /// <summary>
        /// 程序集是否符合正则表达式匹配
        /// </summary>
        /// <param name="assemblyFullName">程序集名称.</param>
        /// <returns>是否匹配</returns>
        public virtual bool Matches(string assemblyFullName)
        {
            return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
                   && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
        }

        /// <summary>
        /// 添加已配置的程序集
        /// </summary>
        /// <param name="addedAssemblyNames">需要添加的程序集名称.</param>
        /// <param name="assemblies">程序集集合</param>
        protected virtual void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (string assemblyName in AssemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);

                if (!addedAssemblyNames.Contains(assembly.FullName))
                {
                    assemblies.Add(assembly);
                    addedAssemblyNames.Add(assembly.FullName);
                }
            }
        }

        /// <summary>
        /// 是否是派生类
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="openGeneric">需要判定的类型</param>
        /// <returns>是否是派生类</returns>
        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            try
            {
                var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();

                foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
                {
                    if (!implementedInterface.IsGenericType)
                        continue;

                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
                    return isMatch;
                }

                return false;
            }

            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 加载特定目录下的程序集
        /// </summary>
        /// <param name="directoryPath">特定的目录</param>
        protected virtual void LoadMatchingAssemblies(string directoryPath)
        {
            var loadedAssemblyNames = new List<string>();

            foreach (Assembly a in GetAssemblies())
            {
                loadedAssemblyNames.Add(a.FullName);
            }

            if (!Directory.Exists(directoryPath))
            {
                return;
            }

            foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
            {
                try
                {
                    var an = AssemblyName.GetAssemblyName(dllPath);

                    if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                    {
                        App.Load(an);
                    }
                }

                catch (BadImageFormatException ex)
                {
                    Trace.TraceError(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 程序集名称是否符合正则表达式匹配
        /// </summary>
        /// <param name="assemblyFullName">程序集名称</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>是否匹配</returns>
        protected virtual bool Matches(string assemblyFullName, string pattern)
        {
            return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Matches(assembly.FullName))
                {
                    if (!addedAssemblyNames.Contains(assembly.FullName))
                    {
                        assemblies.Add(assembly);
                        addedAssemblyNames.Add(assembly.FullName);
                    }
                }
            }
        }

        #endregion Methods
    }
}