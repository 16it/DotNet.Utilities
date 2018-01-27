namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Xml;

    /// <summary>
    /// PropertyGrid 辅助类
    /// </summary>
    public static class PropertyGridHelper
    {
        #region Methods

        /// <summary>
        /// 加载Xml Node节点
        /// </summary>
        /// <param name="propertyGrid">PropertyGrid</param>
        /// <param name="configureFile">xml文件路径</param>
        public static void LoadNodeConfigure(this PropertyGrid propertyGrid, string configureFile)
        {
            XmlDocument _xmlDoc = LoadXmlFile(configureFile);
            propertyGrid.SelectedObject = new XmlNodeWrapper(_xmlDoc.DocumentElement);
        }

        /// <summary>
        /// 加载Config Section节点
        /// </summary>
        /// <param name="propertyGrid">PropertyGrid</param>
        /// <param name="configureFile">config文件路径</param>
        public static void LoadSectionConfigure(this PropertyGrid propertyGrid, string configureFile)
        {
            XmlSectionWrapper _xmlWrapper = new XmlSectionWrapper();

            XmlDocument _xmlDoc = LoadXmlFile(configureFile);
            XmlNode _configuration = _xmlDoc.SelectSingleNode("configuration");

            XmlNodeList _sectionList = _configuration.ChildNodes;
            for (int j = 0; j < _sectionList.Count; j++)
            {
                XmlNodeList _settingList = _xmlDoc.SelectNodes("configuration/" + _sectionList[j].Name + "/add");
                if (_settingList == null || _settingList.Count == 0)
                    continue;

                for (int i = 0; i < _settingList.Count; i++)
                {
                    XmlNode _cfgSettode = _settingList[i];
                    XmlAttribute _atrrKey = _cfgSettode.Attributes["key"] != null ? _cfgSettode.Attributes["key"] : _settingList[i].Attributes["name"];
                    XmlAttribute _atrrValue = _cfgSettode.Attributes["value"] != null ? _cfgSettode.Attributes["value"] : _settingList[i].Attributes["connectionString"];
                    XmlAttribute _atrrDes = _cfgSettode.Attributes["description"];

                    if (_atrrKey == null || _atrrValue == null) continue;

                    string _atrrDesText = string.Empty;
                    if (_atrrDes == null)
                    {
                        _atrrDesText = _cfgSettode.PreviousSibling == null ? string.Empty : _cfgSettode.PreviousSibling.Value;
                        _atrrDesText = string.IsNullOrEmpty(_atrrDesText) == true ? _atrrKey.Value.Trim() : _atrrDesText;
                    }
                    else
                    {
                        _atrrDesText = _atrrDes.Value.Trim();
                    }

                    bool _isBoolType = (string.Compare(_atrrValue.Value, "true", true) == 0 || string.Compare(_atrrValue.Value, "false", true) == 0);
                    Type _propType = _isBoolType == true ? typeof(Boolean) : typeof(String);

                    _xmlWrapper.AddProperty(_atrrKey.Value.Trim(), _atrrValue.Value.Trim(),
                        _atrrDesText, _sectionList[j].Name, _propType, false, false);
                }
            }

            propertyGrid.SelectedObject = _xmlWrapper;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="propertyGrid">PropertyGrid</param>
        /// <param name="configureFile">config文件路径</param>
        public static void SaveConfigure(this PropertyGrid propertyGrid, string configureFile)
        {
            SaveSectionConfigure(propertyGrid, configureFile);
            SaveNodeConfigure(propertyGrid, configureFile);
        }

        private static XmlDocument LoadXmlFile(string configureFile)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(configureFile);
            return _xmlDoc;
        }

        private static void SaveConnectionSection(XmlDocument xmlDoc, PropertyDescriptorCollection props)
        {
            XmlNodeList _xmlNodes = xmlDoc.SelectNodes("configuration/connectionStrings/add");

            for (int i = 0; i < _xmlNodes.Count; i++)
            {
                DynamicProperty _property = (DynamicProperty)props[_xmlNodes[i].Attributes["name"].Value];

                if (_property != null)
                {
                    _xmlNodes[i].Attributes["connectionString"].Value = _property.GetValue(null).ToString();

                    if (_property.Description != _property.Name)
                    {
                        if (_xmlNodes[i].Attributes["description"] != null)
                        {
                            _xmlNodes[i].Attributes["description"].Value = _property.Description;
                        }
                    }
                }
            }
        }

        private static void SaveNodeConfigure(PropertyGrid propertyGrid, string configureFile)
        {
            if (propertyGrid.SelectedObject is XmlNodeWrapper)
            {
                XmlDocument _xmlDoc = new XmlDocument();
                XmlNodeWrapper _nodeWrapper = (XmlNodeWrapper)propertyGrid.SelectedObject;

                _xmlDoc.LoadXml(_nodeWrapper.GetXmlNode().OuterXml);
                _xmlDoc.Save(configureFile);
                _xmlDoc.Save(configureFile + "_bak");
            }
        }

        private static void SaveSection(string sectionName, XmlDocument xmlDoc, PropertyDescriptorCollection props)
        {
            XmlNodeList _xmlNodes = xmlDoc.SelectNodes("configuration/" + sectionName + "/add");

            for (int i = 0; i < _xmlNodes.Count; i++)
            {
                DynamicProperty _property = (DynamicProperty)props[_xmlNodes[i].Attributes["key"].Value];

                if (_property != null)
                {
                    _xmlNodes[i].Attributes["value"].Value = _property.GetValue(null).ToString();

                    if (_property.Description != _property.Name)
                    {
                        if (_xmlNodes[i].Attributes["description"] != null)
                        {
                            _xmlNodes[i].Attributes["description"].Value = _property.Description;
                        }
                    }
                }
            }
        }

        private static void SaveSectionConfigure(PropertyGrid propertyGrid, string configureFile)
        {
            if (propertyGrid.SelectedObject is XmlSectionWrapper)
            {
                XmlSectionWrapper _xmlWrapper = (XmlSectionWrapper)propertyGrid.SelectedObject;

                XmlDocument _xmlDoc = new XmlDocument();
                _xmlDoc.Load(configureFile);
                _xmlDoc.Save(configureFile + "_bak");
                PropertyDescriptorCollection _props = _xmlWrapper.GetProperties();
                SaveSection("ApplicationConfiguration", _xmlDoc, _props);
                SaveSection("CommonConfiguration", _xmlDoc, _props);
                SaveSection("appSettings", _xmlDoc, _props);
                SaveConnectionSection(_xmlDoc, _props);

                _xmlDoc.Save(configureFile);
            }
        }

        #endregion Methods
    }
}