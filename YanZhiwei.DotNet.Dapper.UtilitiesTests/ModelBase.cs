using System;

namespace YanZhiwei.DotNet.Dapper.UtilitiesTests
{
    public class ModelBase
    {
        public ModelBase()
        {
            CreateTime = DateTime.Now;
        }

        public int Id { get; protected set; }

        public DateTime CreateTime { get; protected set; }
    }
}