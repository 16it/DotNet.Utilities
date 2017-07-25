using System;
using YanZhiwei.DotNet.Core.Infrastructure.DependencyManagement;

namespace YanZhiwei.DotNet.Core.Infrastructure
{
    /// <summary>
    /// 管理ContainerManager， 实例化服务，在程序启动时注册各种服务。
    /// </summary>
    public interface IEngine
    {
        //IEngion有个ContainerManager 类型的属性,
        //通过这个属性我们可以获得IContainer ，
        //看一下NopEngine的代码就会发现ContainerManager
        //是在执行Initialize方法的时候赋值的。
        //IEngion还提供了Resolve方法，其实就是调用ContainerManager 的Resolve方法,
        //向外界提供服务。
        /// <summary>
        /// 获取IOC容器
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 分解依赖
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>泛型类型</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        ///  分解依赖
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型</returns>
        object Resolve(Type type);

        /// <summary>
        /// 分解依赖
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>泛型类型</returns>
        T[] ResolveAll<T>();
    }
}