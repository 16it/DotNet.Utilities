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
        /// <param name="isSuppressException">
        /// Suppress exception is true / false</param>
        /// <returns>返回值</returns>
        public static TResult Execute<TResult>(
            Func<TResult> action,
            TimeSpan retryInterval,
            int retryCount,
            TResult expectedResult,
            bool isExpectedResultEqual = true,
            bool isSuppressException = true
        )
        {
            TResult result = default(TResult);
            bool succeeded = false;
            var exceptions = new List<Exception>();
            
            for(int retry = 0; retry < retryCount; retry++)
            {
                try
                {
                    if(retry > 0)
                        Thread.Sleep(retryInterval);
                        
                    // Execute method
                    result = action();
                    
                    if(isExpectedResultEqual)
                        succeeded = result.Equals(expectedResult);
                        
                    else
                        succeeded = !result.Equals(expectedResult);
                }
                
                catch(Exception ex)
                {
                    exceptions.Add(ex);
                }
                
                if(succeeded)
                    return result;
            }
            
            if(!isSuppressException)
                throw new AggregateException(exceptions);
                
            else
                return result;
        }
        
        #endregion Methods
    }
}