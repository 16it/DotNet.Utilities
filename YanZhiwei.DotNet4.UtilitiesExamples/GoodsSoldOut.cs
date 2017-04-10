using System;
using YanZhiwei.DotNet4.Utilities.EventHandle.Events;

namespace YanZhiwei.DotNet4.UtilitiesExamples
{
    public class GoodsSoldOut : CustomizeEvent
    {
        /// <summary>
        /// 主商品ID
        /// </summary>
        public Guid GoodsId
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 子商品ID
        /// </summary>
        public Guid RealGoodsId
        {
            get;
            private set;
        }
        
        public GoodsSoldOut(Guid goodsId, Guid realGoodsId)
        {
            this.GoodsId = goodsId;
            this.RealGoodsId = realGoodsId;
        }
    }
}