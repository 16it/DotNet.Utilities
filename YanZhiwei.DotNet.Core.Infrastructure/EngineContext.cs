namespace YanZhiwei.DotNet.Core.Infrastructure
{
    using System.Runtime.CompilerServices;

    using YanZhiwei.DotNet2.Utilities.DesignPattern;

    /// <summary>
    /// Engine上下文单例的访问
    /// </summary>
    public class EngineContext
    {
        #region Properties

        /// <summary>
        /// 获取IEngine单例实例.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 初始化工厂IEngine的静态实例
        /// </summary>
        /// <param name="forceRecreate">创建新工厂实例，即使工厂已初始化。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || Singleton<IEngine>.Instance.ContainerManager == null || forceRecreate)
            {
                Singleton<IEngine>.Instance.Initialize();
            }

            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 将静态引擎实例设置为所提供的引擎。使用此方法来提供自己的引擎实现。
        /// </summary>
        /// <param name="engine">IEngine</param>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion Methods
    }
}