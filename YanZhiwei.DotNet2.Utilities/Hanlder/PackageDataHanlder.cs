using YanZhiwei.DotNet2.Utilities.Core;

namespace YanZhiwei.DotNet2.Utilities.Hanlder
{
    /// <summary>
    /// PackageData 派生类处理
    /// </summary>
    /// 时间：2016/8/30 17:24
    /// 备注：
    public abstract class PackageDataHanlder
    {
        /// <summary>
        /// 将BYTE数组转换为PackageData派生类
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="buffer">BYTE数组</param>
        /// <returns>PackageData派生类</returns>
        public abstract bool BuileObjFromBytes<T>(byte[] buffer) where T : PackageData;
    }
}