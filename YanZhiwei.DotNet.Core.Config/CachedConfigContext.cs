namespace YanZhiwei.DotNet.Core.Config
{
    using System.Web.Caching;

    using YanZhiwei.DotNet.Core.Model;
    using YanZhiwei.DotNet2.Utilities.DesignPattern;
    using YanZhiwei.DotNet2.Utilities.WebForm.Core;

    /// <summary>
    /// CachedConfigContext
    /// </summary>
    public sealed class CachedConfigContext : ConfigContext
    {
        #region Properties

        /// <summary>
        /// 单例对象
        /// </summary>
        public static CachedConfigContext Instance
        {
            get
            {
                return Singleton<CachedConfigContext>.GetInstance();
            }
        }

        /// <summary>
        /// WEB API 用户令牌验证配置项
        /// </summary>
        public AuthWebApiConfig AuthWebApiConfig
        {
            get
            {
                return this.Get<AuthWebApiConfig>();
            }
        }

        /// <summary>
        /// 缓存配置项
        /// </summary>
        public CacheConfig CacheConfig
        {
            get
            {
                return this.Get<CacheConfig>();
            }
        }

        /// <summary>
        /// 文件下载配置项
        /// </summary>
        public DownloadConfig DownloadConfig
        {
            get
            {
                return this.Get<DownloadConfig>();
            }
        }

        /// <summary>
        /// 文件上传配置项
        /// </summary>
        public UploadConfig UploadConfig
        {
            get
            {
                return this.Get<UploadConfig>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 重写基类的取配置，加入缓存机制
        /// </summary>
        public override T Get<T>(string index = null)
        {
            string _fileName = this.GetConfigFileName<T>(index),
                   _key = "ConfigFile_" + _fileName;
            object _content = CacheManger.Get(_key);

            if (_content != null)
            {
                return (T)_content;
            }
            else
            {
                T _value = base.Get<T>(index);
                CacheManger.Set(_key, _value, new CacheDependency(ConfigService.GetFilePath(_fileName)));
                return _value;
            }
        }

        #endregion Methods
    }
}