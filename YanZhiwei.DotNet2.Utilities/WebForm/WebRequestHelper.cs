namespace YanZhiwei.DotNet2.Utilities.WebForm
{
    using System.Collections.Generic;
    using System.Text;
    
    using Operator;
    
    /// <summary>
    /// HttpRquest 辅助类
    /// </summary>
    /// 时间：2016/10/19 14:26
    /// 备注：
    public class WebRequestHelper
    {
        #region Methods
        
        /// <summary>
        /// 拼接Get参数
        /// </summary>
        /// <param name="paramter"></param>
        /// <returns>get参数拼接字符串</returns>
        public static string GetQueryString(IDictionary<string, string> paramter)
        {
            ValidateOperator.Begin().NotNull(paramter, "需要拼接GET请求参数");
            IDictionary<string, string> _sortedParamter = new SortedDictionary<string, string>(paramter);
            IEnumerator<KeyValuePair<string, string>> _paramter = _sortedParamter.GetEnumerator();
            StringBuilder _builder = new StringBuilder();
            
            while(_paramter.MoveNext())
            {
                string _key = _paramter.Current.Key;
                string _value = _paramter.Current.Value;
                
                if(!string.IsNullOrEmpty(_key))
                {
                    _builder.Append("&").Append(_key).Append("=").Append(_value);
                }
            }
            
            return _builder.ToString().Substring(1, _builder.Length - 1);
        }
        
        #endregion Methods
    }
}