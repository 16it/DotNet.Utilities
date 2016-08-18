namespace YanZhiwei.DotNet.Core.Config
{
    using DotNet3._5.Utilities.Common;
    using Model;
    using System;

    /// <summary>
    /// 配置上下文
    /// </summary>
    public class ConfigContext
    {
        #region Constructors

        /// <summary>
        /// 默认以文件形式存取配置
        /// </summary>
        public ConfigContext() : this(new FileConfigService())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageContentConfigService">IConfigService</param>
        public ConfigContext(IConfigService pageContentConfigService)
        {
            ConfigService = pageContentConfigService;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// IConfigService
        /// </summary>
        public IConfigService ConfigService
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 根据分区索引获取配置对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="index">分区索引</param>
        /// <returns>配置对象</returns>
        public virtual T Get<T>(string index) where T : ConfigFileBase, new()
        {
            T _result = new T();
            CheckedClusteredByIndex(_result, index);
            _result = this.GetConfigFile<T>(index);
            return _result;
        }

        /// <summary>
        /// 根据分区索引获取配置对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>配置对象</returns>
        public virtual T Get<T>() where T : ConfigFileBase, new()
        {
            return Get<T>(null);
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="configFile">配置文件类型</param>
        /// <param name="index">分区索引</param>
        public void Save<T>(T configFile, string index) where T : ConfigFileBase
        {
            CheckedClusteredByIndex(configFile, index);
            configFile.Save();
            string _fileName = GetConfigFileName<T>(index);
            ConfigService.SaveConfig(_fileName, SerializeHelper.XmlSerialize(configFile));
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="configFile">配置文件类型</param>
        public void Save<T>(T configFile) where T : ConfigFileBase
        {
            Save<T>(configFile, null);
        }

        /// <summary>
        /// 获取配置文件名称，非全路径
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="index">分区索引</param>
        /// <returns>配置文件名称</returns>
        protected string GetConfigFileName<T>(string index)
        {
            string _fileName = typeof(T).Name;

            if(!string.IsNullOrEmpty(index))
                _fileName = string.Format("{0}_{1}", _fileName, index);

            return _fileName;
        }

        /// <summary>
        /// 获取配置文件名称，非全路径
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <returns>配置文件名称</returns>
        protected string GetConfigFileName<T>()
        {
            return GetConfigFileName<T>(null);
        }

        /// <summary>
        /// 验证配置文件分区索引是否正确
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="configFile">ConfigFileBase</param>
        /// <param name="index">分区索引</param>
        private void CheckedClusteredByIndex<T>(T configFile, string index) where T : ConfigFileBase
        {
            if(configFile.ClusteredByIndex && string.IsNullOrEmpty(index))
                throw new Exception("未能提供配置文件的分区索引！");
        }

        private T GetConfigFile<T>(string index = null) where T : ConfigFileBase, new()
        {
            T _result = new T();
            string _fileName = this.GetConfigFileName<T>(index);
            string _content = this.ConfigService.GetConfig(_fileName);

            if(_content == null)
            {
                this.ConfigService.SaveConfig(_fileName, string.Empty);
            }
            else if(!string.IsNullOrEmpty(_content))
            {
                try
                {
                    _result = SerializeHelper.XmlDeserialize<T>(_content);
                }
                catch
                {
                    _result = new T();
                }
            }

            return _result;
        }

        private T GetConfigFile<T>() where T : ConfigFileBase, new()
        {
            return GetConfigFile<T>(null);
        }

        #endregion Methods
    }
}