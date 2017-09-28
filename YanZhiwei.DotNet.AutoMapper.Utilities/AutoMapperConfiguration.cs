using AutoMapper;
using System;
using System.Linq;

namespace YanZhiwei.DotNet.AutoMapper.Utilities
{
    /// <summary>
    /// 自动配置Profile
    /// </summary>
    /// <typeparam name="CustomizedProfile">The type of the ustomized profile.</typeparam>
    public static class AutoMapperConfiguration<CustomizedProfile>
     where CustomizedProfile : Profile, new()
    {
        /// <summary>
        /// 配置
        /// </summary>
        public static void Configure()
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
        }

        private static void GetConfiguration(IConfiguration configuration)
        {
            var _profiles = typeof(CustomizedProfile).Assembly.GetTypes().Where(x => typeof(Profile).IsAssignableFrom(x));
            foreach (var profile in _profiles)
            {
                configuration.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }
    }
}