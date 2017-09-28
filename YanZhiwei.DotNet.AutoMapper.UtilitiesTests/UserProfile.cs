using AutoMapper;
using System;
using System.Globalization;
using YanZhiwei.DotNet.AutoMapper.Utilities.Tests;

namespace YanZhiwei.DotNet.AutoMapper.UtilitiesTests
{
    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserDto>()
       .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
       .ForMember(d => d.PassWord, opt => opt.MapFrom(s => s.PassWord));
        }
    }

    public class StringFromDateTimeTypeConverter : ITypeConverter<DateTime, String>
    {
        public string Convert(ResolutionContext context)
        {
            DateTime src = (DateTime)context.SourceValue;

            return src.ToString("dd/mm/yyyy", CultureInfo.InvariantCulture);
        }
    }
}