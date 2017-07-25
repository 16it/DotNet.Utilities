using AutoMapper;
using System;

namespace YanZhiwei.DotNet.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 映射配置接口
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        /// 获取映射配置
        /// </summary>
        /// <returns>IMapperConfigurationExpression</returns>
        Action<IMapperConfigurationExpression> GetConfiguration();

        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }
    }
}