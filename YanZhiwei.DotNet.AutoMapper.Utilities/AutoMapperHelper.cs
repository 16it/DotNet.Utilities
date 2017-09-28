namespace YanZhiwei.DotNet.AutoMapper.Utilities
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using global::AutoMapper;

    /// <summary>
    /// AutoMapper扩展帮助类
    /// </summary>
    /// 时间：2015-12-07 15:49
    /// 备注：
    public static class AutoMapperHelper
    {
        #region Fields

        private static ConcurrentBag<string> customizedProfileList = null;

        #endregion Fields

        #region Constructors

        static AutoMapperHelper()
        {
            customizedProfileList = new ConcurrentBag<string>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// DataTable 映射
        /// </summary>
        /// <typeparam name="TDestination">泛型</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<TDestination> MapTo<TDestination>(this DataTable table)
        {
            if(!TypeExistMap<IDataReader, IEnumerable<TDestination>>())
                Mapper.CreateMap<IDataReader, IEnumerable<TDestination>>();

            IDataReader _dataReader = table.CreateDataReader();
            return Mapper.Map<IDataReader, IEnumerable<TDestination>>(_dataReader);
        }

        /// <summary>
        /// DataReader映射
        /// </summary>
        /// <typeparam name="TDestination">泛型</typeparam>
        /// <param name="reader">IDataReader</param>
        /// <returns>IEnumerable</returns>
        /// 时间：2015-12-07 15:45
        /// 备注：
        public static IEnumerable<TDestination> MapTo<TDestination>(this IDataReader reader)
        {
            if(!TypeExistMap<IDataReader, IEnumerable<TDestination>>())
                Mapper.CreateMap<IDataReader, IEnumerable<TDestination>>();

            return Mapper.Map<IDataReader, IEnumerable<TDestination>>(reader);
        }

        /// <summary>
        /// 类型映射
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">映射类型</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>TDestination</returns>
        /// 时间：2015-12-07 15:45
        /// 备注：
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
        {
            if(!TypeExistMap<TSource, TDestination>())
                Mapper.CreateMap<TSource, TDestination>();//配置 AutoMapper

            return Mapper.Map<TDestination>(source);  // 执行 mapping
        }

        /// <summary>
        /// 类型映射
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">映射类型</typeparam>
        /// <typeparam name="CustomizedProfile">Profile.</typeparam>
        /// <param name="source">数据源</param>
        /// <returns>TDestination</returns>
        public static TDestination MapTo<TSource, TDestination, CustomizedProfile>(this TSource source)
            where TSource : class
            where TDestination : class
            where CustomizedProfile : Profile, new()
        {
            var _customizedProfile = new CustomizedProfile();
            var _profileName = _customizedProfile.ProfileName;

            if(customizedProfileList.Count(ent => ent.Contains(_profileName)) == 0)
            {
                Mapper.Initialize(cfg => cfg.AddProfile(_customizedProfile));
                customizedProfileList.Add(_profileName);
            }

            return Mapper.Map<TDestination>(source);  // 执行 mapping
        }

        /// <summary>
        /// 类型映射是否已经存在
        /// </summary>
        /// <returns>是否已经存在</returns>
        public static bool TypeExistMap<TSource, TDestination>()
        {
            var _existMapType = Mapper.FindTypeMapFor(typeof(TSource), typeof(TDestination));
            return _existMapType != null;
        }

        #endregion Methods
    }
}