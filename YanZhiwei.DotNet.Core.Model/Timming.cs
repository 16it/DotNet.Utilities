using System;

namespace YanZhiwei.DotNet.Core.Model
{
    [Flags]
    public enum Timming
    {
        /// <summary>
        /// 延时生成
        /// </summary>
        Lazy = 1,

        /// <summary>
        /// 上传后即时生成
        /// </summary>
        Immediate = 2,

        /// <summary>
        /// 请求图片时按需生成
        /// </summary>
        OnDemand = 4
    }
}