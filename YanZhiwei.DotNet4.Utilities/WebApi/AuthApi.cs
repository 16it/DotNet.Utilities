using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.WebForm;
using YanZhiwei.DotNet4.Utilities.Model;

namespace YanZhiwei.DotNet4.Utilities.WebApi
{
    /// <summary>
    /// WEB API验证请求辅助类
    /// </summary>
    /// 时间：2016/10/19 14:01
    /// 备注：
    public class AuthApi
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webApi"></param>
        /// <param name="queryStr"></param>
        /// <param name="keyId"></param>
        /// <returns></returns>
        public static T Get<T>(string webApi, string query, string queryStr, int keyId, bool sign = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApi + "?" + queryStr);
            string nonce = RandomHelper.NextNumber(0, int.MaxValue).ToString(),
                   tiemStamp = DateHelper.GetTimeStamp().ToString();
            //加入头信息
            request.Headers.Add("staffid", keyId.ToString()); //当前请求用户StaffId
            request.Headers.Add("timestamp", tiemStamp);  //发起请求时的时间戳（单位：毫秒）
            request.Headers.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）
            
            if(sign)
                request.Headers.Add("signature", GetSignature(timeStamp, nonce, keyId, query)); //当前请求内容的数字签名
                
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 90000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();
            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();
            return JsonConvert.DeserializeObject<T>(strResult);
        }
        
        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="staffId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetSignature(string timeStamp, string nonce, int staffId, string data)
        {
            AuthApiToken token = null;
            var resultMsg = GetSignToken(staffId);
            
            if(resultMsg != null)
            {
                if(resultMsg.StatusCode == (int)StatusCodeEnum.Success)
                {
                    token = resultMsg.Result;
                }
                else
                {
                    throw new Exception(resultMsg.Data.ToString());
                }
            }
            else
            {
                throw new Exception("token为null，员工编号为：" + staffId);
            }
            
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = timeStamp + nonce + staffId + token.SignToken.ToString() + data;
            //将字符串中字符按升序排序
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            
            foreach(var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }
            
            return result.ToString().ToUpper();
        }
        
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public static AuthApiTokenResult GetSignToken(int staffId)
        {
            string tokenApi = AppSettingsConfig.GetTokenApi;
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("staffId", staffId.ToString());
            Tuple<string, string> parameters = WebRequestHelper.GetQueryString(parames);
            AuthApiTokenResult token = Get<AuthApiTokenResult>(tokenApi, parameters.Item1, parameters.Item2, staffId, false);
            return token;
        }
    }
}