using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    /// <summary>
    /// PropertyGrid 辅助类
    /// </summary>
    public static class PropertyGridHelper
    {
        /// <summary>
        /// 加载显示配置文件
        /// </summary>
        /// <param name="propertyGrid">PropertyGrid</param>
        /// <param name="configurationFile">config文件路径</param>
        public static void LoadConfiguration(this PropertyGrid propertyGrid, string configurationFile)
        {
            PropertyGridSource _ppgSource = new PropertyGridSource();

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(configurationFile);
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

                    _ppgSource.AddProperty(_atrrKey.Value.Trim(), _atrrValue.Value.Trim(),
                        _atrrDesText, _sectionList[j].Name, _propType, false, false);
                }
            }

            propertyGrid.SelectedObject = _ppgSource;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="propertyGrid">PropertyGrid</param>
        /// <param name="configurationFile">config文件路径</param>
        public static void SaveConfiguration(this PropertyGrid propertyGrid, string configurationFile)
        {
            if (!(propertyGrid.SelectedObject is PropertyGridSource))
                return;

            PropertyGridSource _ppgSource = (PropertyGridSource)propertyGrid.SelectedObject;

            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(configurationFile);
            _xmlDoc.Save(configurationFile + "_bak");
            PropertyDescriptorCollection _props = _ppgSource.GetProperties();
            SaveSection("ApplicationConfiguration", _xmlDoc, _props);
            SaveSection("CommonConfiguration", _xmlDoc, _props);
            SaveSection("appSettings", _xmlDoc, _props);
            SaveConnectionSection(_xmlDoc, _props);

            _xmlDoc.Save(configurationFile);
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
    }
}