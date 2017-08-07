namespace YanZhiwei.DotNet4.Utilities.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// 重试机制
    /// </summary>
    public class Retry
    {
        #region Methods

        /// <summary>
        /// 重试机制
        /// </summary>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="action">执行的方法委托</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="expectedResult">期待的结果</param>
        /// <param name="isExpectedResultEqual">是否期待结果一致</param>
        /// <param name="isSuppressException">是否不处理异常</param>
        /// <returns>返回值</returns>
        public static TResult Execute<TResult>(Func<TResult> action, TimeSpan retryInterval, int retryCount, TResult expectedResult, bool isExpectedResultEqual = true, bool isSuppressException = true)
        {
            TResult _result = default(TResult);
            bool _succeeded = false;
            List<Exception> _exceptions = new List<Exception>();

            for (int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    _result = action();

                    if (isExpectedResultEqual)
                        _succeeded = _result.Equals(expectedResult);
                    else
                        _succeeded = !_result.Equals(expectedResult);

                    if (!_succeeded && (retry + 1) < retryCount)
                    {
                        Thread.Sleep(retryInterval);
                    }
                }
                catch (Exception ex)
                {
                    _exceptions.Add(ex);
                }

                if (_succeeded)
                    return _result;
            }

            if (!isSuppressException && _exceptions.Count > 0)
                throw new AggregateException(_exceptions);
            else
                return _result;
        }

        #endregion Methods
    }
}