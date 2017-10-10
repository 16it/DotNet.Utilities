namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    using System;
    
    using YanZhiwei.DotNet2.Utilities.Common;
    
    [Serializable]
    public sealed class UnPackageException : Exception
    {
        #region Fields
        
        public readonly string MethodName;
        public readonly string PackageHexString;
        
        #endregion Fields
        
        #region Constructors
        
        public UnPackageException(string methodName, string message, Exception inner, byte[] data)
        : base(message, inner)
        {
            MethodName = methodName;
            PackageHexString = data == null == true ? "NULL" : ByteHelper.ToHexStringWithBlank(data);
        }
        
        #endregion Constructors
    }
}