using Newtonsoft.Json.Linq;
using System;
using YanZhiwei.DotNet.AuthWebApi.Utilities.Model;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Encryptor;
using YanZhiwei.DotNet3._5.Utilities.Enum;
using YanZhiwei.DotNet3._5.Utilities.WebForm;

namespace YanZhiwei.DotNet.WebApiConsoleApp
{
    internal class Program
    {
        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="appSecret">接入秘钥</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        private static string SignatureString(string appSecret, string timestamp, string nonce)
        {
            string[] ArrTmp = { appSecret, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = MD5Encryptor.Encrypt(tmpStr);
            return tmpStr.ToLower();
        }
        
        private static void Main(string[] args)
        {
            try
            {
                string url = @"http://localhost:53879/";
                string timestamp = UnixEpochHelper.GetCurrentUnixTimestamp().TotalMilliseconds.ToString();
                string nonce = new Random().NextDouble().ToString();
                string signature = SignatureString("XXHHAREJDDF", timestamp, nonce);
                string appended = string.Format("&signature={0}&timestamp={1}&nonce={2}&appid={3}", signature, timestamp, nonce, "aabbcc");
                string queryUrl = url + "api/Auth?userId=test" + appended;
                TokenResult _tokenResult = NetHelper.HttpGet<TokenResult>(queryUrl, SerializationType.Json);
                Console.WriteLine(_tokenResult.Access_token);
                queryUrl = url + "api/Product/1?token=" + _tokenResult.Access_token;
                string jsonText = NetHelper.HttpGet(queryUrl);
                JObject jsonObj = JObject.Parse(jsonText);
                string aa = jsonObj["Data"].ToString();
                Console.WriteLine(jsonText);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}