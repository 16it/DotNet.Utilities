namespace YanZhiwei.DotNet.Core.Infrastructure.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;

    /// <summary>
    /// 网页类型TypeFinder
    /// </summary>
    /// <seealso cref="YanZhiwei.DotNet.Core.Infrastructure.AppDomainTypeFinder" />
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        private bool _binFolderAssembliesLoaded;
        private bool _ensureBinFolderAssembliesLoaded = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 设置或获取Bin目录程序集是否加载
        /// </summary>
        public bool EnsureBinFolderAssembliesLoaded
        {
            get
            {
                return _ensureBinFolderAssembliesLoaded;
            }

            set
            {
                _ensureBinFolderAssembliesLoaded = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns>
        /// 程序集
        /// </returns>
        public override IList<Assembly> GetAssemblies()
        {
            if(this.EnsureBinFolderAssembliesLoaded && !_binFolderAssembliesLoaded)
            {
                _binFolderAssembliesLoaded = true;
                string binPath = GetBinDirectory();
                LoadMatchingAssemblies(binPath);
            }

            return base.GetAssemblies();
        }

        /// <summary>
        /// 获取BIN目录路径
        /// </summary>
        /// <returns>BIN物理路径. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            if(HostingEnvironment.IsHosted)
            {
                return HttpRuntime.BinDirectory;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion Methods
    }
}