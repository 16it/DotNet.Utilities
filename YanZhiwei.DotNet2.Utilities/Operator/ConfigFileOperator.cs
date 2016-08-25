namespace YanZhiwei.DotNet2.Utilities.Operator
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web.Configuration;

    using Common;
    using Enum;

    /// <summary>
    /// Config文件操作读取
    /// </summary>
    /// 时间：2016/8/25 14:52
    /// 备注：
    public class ConfigFileOperator
    {
        #region Fields

        /// <summary>
        /// Configuration对象
        /// </summary>
        private Configuration config = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mode">程序模式</param>
        public ConfigFileOperator(ProgramMode mode)
        {
            switch(mode)
            {
                case ProgramMode.WebForm:
                    config = WebConfigurationManager.OpenWebConfiguration("~/");
                    break;

                case ProgramMode.WinForm:
                    config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    break;
            }

            CheckedConfigFile();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mode">程序模式</param>
        /// <param name="filePath">config文件路径</param>
        public ConfigFileOperator(ProgramMode mode, string filePath)
        {
            switch(mode)
            {
                case ProgramMode.WinForm:
                    ExeConfigurationFileMap _configFileMap = new ExeConfigurationFileMap();
                    _configFileMap.ExeConfigFilename = filePath;

                    if(File.Exists(filePath))
                    {
                        config = ConfigurationManager.OpenMappedExeConfiguration(_configFileMap, ConfigurationUserLevel.None);
                    }

                    break;

                case ProgramMode.WebForm:
                    config = WebConfigurationManager.OpenWebConfiguration(filePath);
                    break;
            }

            CheckedConfigFile();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// 添加或修改appSettings
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void AddOrUpdateSetting(string key, string value)
        {
            KeyValueConfigurationElement _key = config.AppSettings.Settings[key];

            if(_key == null)
            {
                config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                config.AppSettings.Settings[key].Value = value;
            }

            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sctionKey">节点键</param>
        /// <returns>数值</returns>
        public T GetSection<T>(string sctionKey)
        where T : ConfigurationSection
        {
            return config.Sections[sctionKey] as T;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>获取的值</returns>
        public string GetSetting(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            return config.AppSettings.Settings[key].Value;
        }

        /// <summary>
        /// 移除Setting
        /// </summary>
        /// <param name="key">键</param>
        public void RemoveSetting(string key)
        {
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="setion">节点值</param>
        /// <param name="sectionKey">节点键</param>
        public void SaveSection<T>(T setion, string sectionKey)
        where T : ConfigurationSection
        {
            config.Sections.Remove(sectionKey);
            config.Sections.Add(sectionKey, setion);
            config.Save();
        }

        private void CheckedConfigFile()
        {
            ValidateHelper.Begin().Check<ArgumentNullException>(() => config.HasFile, "config文件不存在。");
        }

        #endregion Methods
    }
}