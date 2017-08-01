namespace YanZhiwei.DotNet4.Utilities.WebForm
{
    using System.Web;

    /// <summary>
    /// Request辅助类
    /// </summary>
    public static class RequestHelper
    {
        #region Methods

        /// <summary>
        /// 请求是否可用
        /// </summary>
        /// <param name="httpContext">HttpContextBase</param>
        /// <returns>
        ///  请求是否可用
        /// </returns>
        public static bool IsRequestAvailable(this HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            try
            {
                if (httpContext.Request == null)
                    return false;
            }
            catch (HttpException)
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}