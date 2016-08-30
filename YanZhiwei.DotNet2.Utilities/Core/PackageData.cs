namespace YanZhiwei.DotNet2.Utilities.Core
{
    /// <summary>
    /// 适用于串口，Socket数据协议组包，拆包
    /// </summary>
    /// 时间：2016/8/30 16:22
    /// 备注：
    public abstract class PackageData
    {
        /// <summary>
        /// 同步字1
        /// </summary>
        public abstract byte SynWord1
        {
            get;
        }

        /// <summary>
        /// 同步字2
        /// </summary>
        public abstract byte SyncWord2
        {
            get;
        }

        /// <summary>
        /// 长度域
        /// </summary>
        public byte DataLength
        {
            get;
            set;
        }

        /// <summary>
        /// 长度域数组
        /// </summary>
        public byte[] DataLengthArray
        {
            get;
            set;
        }

        /// <summary>
        /// 命令字
        /// </summary>
        public byte CmdWord
        {
            get;
            set;
        }

        /// <summary>
        /// 命令字
        /// </summary>
        public byte[] CmdWordArray
        {
            get;
            set;
        }

        /// <summary>
        /// 命令字的参数
        /// </summary>
        public byte[] CmdParmArray
        {
            get;
            set;
        }

        /// <summary>
        /// 校验码
        /// </summary>
        public byte CRC
        {
            get;
            set;
        }

        /// <summary>
        /// 校验码
        /// </summary>
        public byte CRCArray
        {
            get;
            set;
        }

        /// <summary>
        ///  通过构造函数初始化参数，来将对象转换为BYTE数组
        /// </summary>
        /// <returns>BYTE数组</returns>
        public abstract byte[] ToBytes();
    }
}