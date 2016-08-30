using System;
using System.Threading;
using YanZhiwei.DotNet2.Utilities.Enum;

namespace YanZhiwei.DotNet2.Utilities.Operator
{
    /// <summary>
    /// 文件升级抽象类，适用于串口，Socket应答模式
    /// </summary>
    /// 时间：2016/8/30 13:35
    /// 备注：
    public abstract class FileUpgradeOperator
    {
        /// <summary>
        /// 文件升级信号量
        /// </summary>
        public ManualResetEvent MREFileUpgrade = null;

        /// <summary>
        /// 失败重试次数
        /// </summary>
        private readonly int fileUpgradeRetryCount = 5;

        /// <summary>
        /// 文件升级超时时间
        /// </summary>
        private readonly TimeSpan fileUpgradeTimeOut = new TimeSpan(0, 0, 5);

        /// <summary>
        /// 升级状态，用于是否取消升级
        /// </summary>
        private bool fileUpgradeCancel = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="upgradeTimeOut">文件升级超时时间【秒】</param>
        /// <param name="upgradeRetryCount">失败重试次数</param>
        public FileUpgradeOperator(int upgradeTimeOut, int upgradeRetryCount)
        {
            MREFileUpgrade = new ManualResetEvent(false);
            fileUpgradeTimeOut = new TimeSpan(0, 0, upgradeTimeOut);
            fileUpgradeRetryCount = upgradeRetryCount;
        }

        /// <summary>
        /// 默认构造函数
        /// <para>失败重试次数：5</para>
        /// <para>文件升级超时时间:5(秒)</para>
        /// </summary>
        public FileUpgradeOperator() : this(5, 5)
        {
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// 时间：2016/8/30 14:07
        /// 备注：
        public virtual void ReplySuccess()
        {
            if(MREFileUpgrade != null)
                MREFileUpgrade.Set();
        }

        /// <summary>
        /// 发送升级请求
        /// </summary>
        /// <param name="requestFactory">委托</param>
        /// <returns>发送状态</returns>
        /// 时间：2016/8/30 13:59
        /// 备注：
        public virtual FileUpgradeStatus RequestUpgradePackage_TimeOut(Func<bool> requestFactory)
        {
            FileUpgradeStatus _upgradeStatus = FileUpgradeStatus.Success;
            int _retryCount = 0;

            for(int i = 0; i < fileUpgradeRetryCount; i++)
            {
                if(fileUpgradeCancel)                           //取消升级
                {
                    _upgradeStatus = FileUpgradeStatus.Cancel;
                    break;
                }

                MREFileUpgrade.Reset();

                if(requestFactory())
                {
                    if(!MREFileUpgrade.WaitOne(fileUpgradeTimeOut, false))
                    {
                        _retryCount++;//超时累加
                    }

                    if(_retryCount == fileUpgradeRetryCount)              //超时
                    {
                        _upgradeStatus = FileUpgradeStatus.Timeout;
                        break;
                    }
                }
                else
                {
                    _upgradeStatus = FileUpgradeStatus.RequestError;//发送错误
                    break;
                }
            }

            return _upgradeStatus;
        }
    }
}