namespace YanZhiwei.DotNet.Core.WebApi
{
    using System;
    using System.Web.Http;

    using YanZhiwei.DotNet.WebApi.Utilities;
    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet4.Utilities.Common;

    /// <summary>
    /// WebApi 基类
    /// </summary>
    public class ApiBaseController : ApiController
    {
        #region Properties

        /// <summary>
        /// 当前通道ID
        /// </summary>
        public Guid CurrentAppId
        {
            get
            {
                return Request.GetUriOrHeaderValue("Access_appId").ToGuidOrDefault(Guid.Empty);
            }
        }

        /// <summary>
        /// 当前令牌
        /// </summary>
        public string CurrentToken
        {
            get
            {
                return Request.GetUriOrHeaderValue("Access_token").ToStringOrDefault(string.Empty);
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid CurrentUserId
        {
            get
            {
                return Request.GetUriOrHeaderValue("Access_userId").ToGuidOrDefault(Guid.Empty);
            }
        }

        #endregion Properties
    }
}