using System;

namespace YanZhiwei.DotNet.Core.Model
{
    [Flags]
    public enum Timming
    {
        //延迟用工具生成
        Lazy = 1,
        
        //上传后即时生成
        Immediate = 2,
        
        //请求图片时按需生成
        OnDemand = 4
    }
}