using System;
using System.Collections.Generic;
using System.Reflection;

namespace YanZhiwei.DotNet.Core.Infrastructure
{
    /// <summary>
    /// 通过反射，找到某个Type的类，子类或实现类
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <param name="assignTypeFrom">指定类型.</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类.</param>
        /// <returns>IEnumerable</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <param name="assignTypeFrom">指定类型.</param>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>IEnumerable</returns>
        IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>IEnumerable</returns>
        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true);

        /// <summary>
        /// 查找类的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblies">程序集</param>
        /// <param name="onlyConcreteClasses">是否只创建具体的类</param>
        /// <returns>IEnumerable</returns>
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true);

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns>程序集</returns>
        IList<Assembly> GetAssemblies();
    }
}