namespace YanZhiwei.DotNet2.Utilities.WebForm
{
    using Common;
    using Model;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Request 帮助类
    /// </summary>
    /// 时间：2016/7/27 14:28
    /// 备注：
    public static class RequestHelper
    {
        #region Methods

        /// <summary>
        /// 获取请求客户端信息
        /// </summary>
        /// <param name="request">HttpRequest</param>
        /// <returns>RequestClientInfo</returns>
        /// 时间：2016/7/27 14:31
        /// 备注：
        public static RequestClientInfo GetClientInfo(HttpRequest request)
        {
            ValidateHelper.Begin().NotNull(request, "HttpRequest");
            RequestClientInfo _clientInfo = new RequestClientInfo();
            //  _clientInfo.OSVersion = GetOsVersion(request);
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
            if (string.IsNullOrEmpty(_userHostAddress))
            {
                _userHostAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            if (!string.IsNullOrEmpty(_userHostAddress) && CheckHelper.IsIp4Address(_userHostAddress))
            {
                return _userHostAddress;
            }
            return "127.0.0.1";
        }

        private static string GetOsVersion(HttpRequest request)
        {
            string _agent = request.ServerVariables["HTTP_USER_AGENT"];
            if (_agent.IndexOf("NT 4.0") > 0)
                return "Windows NT ";
            else if (_agent.IndexOf("NT 5.0") > 0)
                return "Windows 2000";
            else if (_agent.IndexOf("NT 5.1") > 0)
                return "Windows XP";
            else if (_agent.IndexOf("NT 5.2") > 0)
                return "Windows 2003";
            else if (_agent.IndexOf("NT 6.0") > 0)
                return "Windows Vista";
            else if (_agent.IndexOf("NT 6.1") > 0)
                return "Windows 7";
            else if (_agent.IndexOf("NT 6.2") > 0)
                return "Windows 8";
            else if (_agent.IndexOf("NT 6.3") > 0)
                return "Windows 8.1";
            else if (_agent.IndexOf("NT 10.0") > 0)
                return "Windows 10";
            else if (_agent.IndexOf("WindowsCE") > 0)
                return "Windows CE";
            else if (_agent.IndexOf("NT") > 0)
                return "Windows NT ";
            else if (_agent.IndexOf("9x") > 0)
                return "Windows ME";
            else if (_agent.IndexOf("98") > 0)
                return "Windows 98";
            else if (_agent.IndexOf("95") > 0)
                return "Windows 95";
            else if (_agent.IndexOf("Win32") > 0)
                return "Win32";
            else if (_agent.IndexOf("Linux") > 0)
                return "Linux";
            else if (_agent.IndexOf("SunOS") > 0)
                return "SunOS";
            else if (_agent.IndexOf("Mac") > 0)
                return "Mac";
            else if (_agent.IndexOf("Linux") > 0)
                return "Linux";
            else if (_agent.IndexOf("Windows") > 0)
                return "Windows";
            return _agent;
        }

        #endregion Methods
    }
}