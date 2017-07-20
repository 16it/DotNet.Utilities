using System.Data.Entity;

namespace YanZhiwei.DotNet.EntityFramework.Utilities
{
    /// <summary>
    /// DbContext 扩展辅助类
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        /// 保存更改
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        /// <param name="validateOnSaveEnabled">保存时验证实体有效性，涉及到按需更新</param>
        /// <returns>影响行数</returns>
        public static int SaveChanges(this DbContext dbContext, bool validateOnSaveEnabled)
        {
            bool _originalValidateOnSaveEnabled = dbContext.Configuration.ValidateOnSaveEnabled != validateOnSaveEnabled;
            try
            {
                dbContext.Configuration.ValidateOnSaveEnabled = validateOnSaveEnabled;
                return dbContext.SaveChanges();
            }
            finally
            {
                if (_originalValidateOnSaveEnabled)
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = !validateOnSaveEnabled;
                }
            }
        }
    }
}