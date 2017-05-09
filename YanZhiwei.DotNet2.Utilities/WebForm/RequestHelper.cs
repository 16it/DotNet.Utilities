namespace YanZhiwei.DotNet2.Utilities.WebForm
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Web;

    using Common;

    using Model;

    using Operator;

    /// <summary>
    /// Request 帮助类
    /// </summary>
    /// 时间：2016/7/27 14:28
    /// 备注：
    public static class RequestHelper
    {
        #region Methods

        /// <summary>
        /// 检查HttpContext.Current.Request.Form是否包含指定参数
        /// </summary>
        /// <param name="keys">键</param>
        /// <returns>是否包含指定的参数键</returns>
        public static bool CheckedRequestBody(params string[] keys)
        {
            bool _result = HttpContext.Current.Request != null;

            if(!_result) return _result;

            if(keys != null && keys.Length > 0)
            {
                _result = HttpContext.Current.Request.Form.AllKeys.Length >= keys.Length;

                if(!_result) return _result;

                foreach(string key in keys)
                {
                    string[] _keyValue = HttpContext.Current.Request.Form.GetValues(key);

                    if(_keyValue == null || _keyValue.Length == 0)
                    {
                        _result = false;
                        break;
                    }
                }
            }

            return _result;
        }

        /// <summary>
        /// 创建请求URL
        /// <para>使用http请求的queryString然后加上时间戳还有随机数来作为签名的参数。</para>
        /// </summary>
        /// <param name="queryParamter">请求参数</param>
        /// <returns>签名参数字符串</returns>
        public static string CreateQueryParamter(SortedDictionary<string, string> queryParamter)
        {
            return CreateQueryParamter(queryParamter, null, null);
        }

        /// <summary>
        /// 创建请求URL
        /// <para>使用http请求的queryString然后加上时间戳还有随机数来作为签名的参数。</para>
        /// </summary>
        /// <param name="queryParamter">请求参数</param>
        /// <param name="dateTime">时间戳</param>
        /// <param name="rumdon">随机数</param>
        /// <returns>签名参数字符串</returns>
        /// 时间：2016/9/22 10:16
        /// 备注：
        public static string CreateQueryParamter(SortedDictionary<string, string> queryParamter, string dateTime, string rumdon)
        {
            ValidateOperator.Begin().NotNull(queryParamter, "请求参数");

            if(string.IsNullOrEmpty(dateTime))
                dateTime = DateTime.Now.FormatDate(12);

            if(string.IsNullOrEmpty(rumdon))
                rumdon = RandomHelper.NextString(8, true);

            StringBuilder _builder = new StringBuilder();

            foreach(var keyValue in queryParamter)
            {
                _builder.AppendFormat("{0}={1}&", keyValue.Key, keyValue.Value);
            }

            if(_builder.Length > 1)
            {
                _builder.Remove(_builder.Length - 1, 1);
            }

            _builder.Append(dateTime);
            _builder.Append(rumdon);
            return _builder.ToString().ToUpper();
        }

        /// <summary>
        /// 获取请求客户端信息
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>RequestClientInfo</returns>
        /// 时间：2016/7/27 14:31
        /// 备注：
        public static RequestClientInfo GetClientInfo(HttpRequest request)
        {
            ValidateOperator.Begin().NotNull(request, "HttpRequest");
            RequestClientInfo _clientInfo = new RequestClientInfo();
            _clientInfo.OSVersion = GetOsVersion(request);
            _clientInfo.BrowserVersion = GetBrowserVersion(request);
            _clientInfo.ComputerName = GetComputerName(request);
            _clientInfo.Ip4Address = GetIP4Address(request);
            return _clientInfo;
        }

        private static string GetBrowserVersion(HttpRequest request)
        {
            HttpBrowserCapabilities _hbc = request.Browser;
            return string.Format("{0} {1}", _hbc.Browser, _hbc.Version);
        }

        private static string GetComputerName(HttpRequest request)
        {
            string _clientIP = request.UserHostAddress;
            IPHostEntry _hostEntry = Dns.GetHostEntry(_clientIP);
            return _hostEntry.HostName;
        }

        private static string GetIP4Address(HttpRequest request)
        {
            string _userHostAddress = request.UserHostAddress;

            if(string.IsNullOrEmpty(_userHostAddress))
            {
                _userHostAddress = request.ServerVariables["REMOTE_ADDR"];
            }

            if(!string.IsNullOrEmpty(_userHostAddress) && CheckHelper.IsIp4Address(_userHostAddress))
            {
                return _userHostAddress;
            }

            return "127.0.0.1";
        }

        private static string GetOsVersion(HttpRequest request)
        {
            string _agent = request.ServerVariables["HTTP_USER_AGENT"];

            if(_agent.IndexOf("NT 4.0", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows NT ";

            else if(_agent.IndexOf("NT 5.0", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 2000";

            else if(_agent.IndexOf("NT 5.1", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows XP";

            else if(_agent.IndexOf("NT 5.2", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 2003";

            else if(_agent.IndexOf("NT 6.0", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows Vista";

            else if(_agent.IndexOf("NT 6.1", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 7";

            else if(_agent.IndexOf("NT 6.2", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 8";

            else if(_agent.IndexOf("NT 6.3", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 8.1";

            else if(_agent.IndexOf("NT 10.0", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 10";

            else if(_agent.IndexOf("WindowsCE", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows CE";

            else if(_agent.IndexOf("NT", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows NT ";

            else if(_agent.IndexOf("9x", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows ME";

            else if(_agent.IndexOf("98", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 98";

            else if(_agent.IndexOf("95", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows 95";

            else if(_agent.IndexOf("Win32", StringComparison.OrdinalIgnoreCase) > 0)
                return "Win32";

            else if(_agent.IndexOf("Linux", StringComparison.OrdinalIgnoreCase) > 0)
                return "Linux";

            else if(_agent.IndexOf("SunOS", StringComparison.OrdinalIgnoreCase) > 0)
                return "SunOS";

            else if(_agent.IndexOf("Mac", StringComparison.OrdinalIgnoreCase) > 0)
                return "Mac";

            else if(_agent.IndexOf("Linux", StringComparison.OrdinalIgnoreCase) > 0)
                return "Linux";

            else if(_agent.IndexOf("Windows", StringComparison.OrdinalIgnoreCase) > 0)
                return "Windows";

            return _agent;
        }

        #endregion Methods
    }
}