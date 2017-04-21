using System;
using System.Threading;

namespace YanZhiwei.DotNet2.Utilities.Core
{
    /// <summary>
    /// 非交互重试机制
    /// </summary>
    public static class NonInteractRetry
    {
        /// <summary>
        /// 执行操作，若返回false则重试
        /// </summary>
        /// <param name="executeFacotry">执行的方法</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        public static void Execute(Func<bool> executeFacotry, TimeSpan retryInterval, ushort retryCount)
        {
            for(ushort retry = 0; retry < retryCount; retry++)
            {
                bool _result = executeFacotry();
                
                if(_result)
                {
                    break;
                }
                
                Thread.Sleep(retryInterval);
            }
        }
        
        /// <summary>
        /// 执行操作，若返回false则重试
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="executeFacotry">执行的方法</param>
        /// <param name="arg1">输入参数1</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// 时间：2016/12/13 16:41
        /// 备注：
        public static void Execute<T>(Func<T, bool> executeFacotry, T arg1, TimeSpan retryInterval, ushort retryCount)
        {
            for(ushort retry = 0; retry < retryCount; retry++)
            {
                bool _result = executeFacotry(arg1);
                
                if(_result)
                {
                    break;
                }
                
                Thread.Sleep(retryInterval);
            }
        }
        
        /// <summary>
        /// 执行操作，若返回false则重试
        /// </summary>
        /// <typeparam name="T1">泛型</typeparam>
        /// <typeparam name="T2">泛型</typeparam>
        /// <param name="executeFacotry">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// 时间：2016/12/13 16:42
        /// 备注：
        public static void Execute<T1, T2>(Func<T1, T2, bool> executeFacotry, T1 arg1, T2 arg2, TimeSpan retryInterval, ushort retryCount)
        {
            for(ushort retry = 0; retry < retryCount; retry++)
            {
                bool _result = executeFacotry(arg1, arg2);
                
                if(_result)
                {
                    break;
                }
                
                Thread.Sleep(retryInterval);
            }
        }
        
        /// <summary>
        /// 执行操作，若返回false则重试
        /// </summary>
        /// <typeparam name="T1">泛型</typeparam>
        /// <typeparam name="T2">泛型</typeparam>
        /// <typeparam name="T3">泛型</typeparam>
        /// <param name="executeFacotry">执行的方法</param>
        /// <param name="arg1">参数1</param>
        /// <param name="arg2">参数2</param>
        /// <param name="arg3">参数3</param>
        /// <param name="retryInterval">重试间隔</param>
        /// <param name="retryCount">重试次数</param>
        /// 时间：2016/12/13 16:42
        /// 备注：
        public static void Execute<T1, T2, T3>(Func<T1, T2, T3, bool> executeFacotry, T1 arg1, T2 arg2, T3 arg3, TimeSpan retryInterval, ushort retryCount)
        {
            for(ushort retry = 0; retry < retryCount; retry++)
            {
                bool _result = executeFacotry(arg1, arg2, arg3);
                
                if(_result)
                {
                    break;
                }
                
                Thread.Sleep(retryInterval);
            }
        }
        
        ///// <summary>
        ///// 执行操作，若返回false则重试
        ///// </summary>
        ///// <typeparam name="T1">泛型</typeparam>
        ///// <typeparam name="T2">泛型</typeparam>
        ///// <typeparam name="T3">泛型</typeparam>
        ///// <typeparam name="T4">泛型</typeparam>
        ///// <param name="executeFacotry">执行的方法</param>
        ///// <param name="arg1">参数1</param>
        ///// <param name="arg2">参数2</param>
        ///// <param name="arg3">参数3</param>
        ///// <param name="arg4">参数4</param>
        ///// <param name="retryInterval">重试间隔</param>
        ///// <param name="retryCount">重试次数</param>
        ///// 时间：2016/12/13 16:42
        ///// 备注：
        //public static void Execute<T1, T2, T3, T4>(Func<T1, T2, T3, T4, bool> executeFacotry, T1 arg1, T2 arg2, T3 arg3, T4 arg4, TimeSpan retryInterval, ushort retryCount)
        //{
        //    for(ushort retry = 0; retry < retryCount; retry++)
        //    {
        //        bool _result = executeFacotry(arg1, arg2, arg3, arg4);
                
        //        if(_result)
        //        {
        //            break;
        //        }
                
        //        Thread.Sleep(retryInterval);
        //    }
        //}
    }
}