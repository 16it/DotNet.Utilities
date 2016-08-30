using System;
using System.Net;

namespace YanZhiwei.DotNet3._5.Utilities.WebClient
{
    /// <summary>
    /// 中国网站请求
    /// </summary>
    /// <seealso cref="System.Net.WebClient" />
    public class CNNWebClient : System.Net.WebClient
    {
        private int timeOut = 200;
        
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeOut;
            }
            set
            {
                if(value <= 0)
                    timeOut = 200;
                    
                timeOut = value;
            }
        }
        
        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest _request = (HttpWebRequest)base.GetWebRequest(address);
            _request.Timeout = 1000 * Timeout;
            _request.ReadWriteTimeout = 1000 * Timeout;
            return _request;
        }
    }
}