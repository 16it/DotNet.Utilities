using AutoMapper;
using System;
using System.Collections.Generic;

namespace YanZhiwei.DotNet.Core.Infrastructure.Mapper
{
    /// <summary>
    /// AutoMapper 配置
    /// </summary>
    public static class AutoMapperConfiguration
    {
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        /// <summary>
        /// 初始化映射
        /// </summary>
        /// <param name="configurationActions">AutoMapper configurationActions委托</param>
        public static void Init(List<Action<IMapperConfigurationExpression>> configurationActions)
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                foreach (var ca in configurationActions)
                    ca(cfg);
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }

        /// <summary>
        /// IMapper
        /// </summary>
        public static IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        /// <summary>
        ///MapperConfiguration
        /// </summary>
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }
    }
}