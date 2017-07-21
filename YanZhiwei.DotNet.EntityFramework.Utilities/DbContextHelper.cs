using System;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;

namespace YanZhiwei.DotNet.EntityFramework.Utilities
{
    /// <summary>
    /// DbContext 扩展辅助类
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <returns>数据库名称</returns>
        public static string DbName(this DbContext context)
        {
            var _connection = ((IObjectContextAdapter)context).ObjectContext.Connection as EntityConnection;
            if (_connection == null)
                return string.Empty;

            return _connection.StoreConnection.Database;
        }

        /// <summary>
        /// 获取DbEntityValidationException详细异常信息
        /// </summary>
        /// <param name="exc">DbEntityValidationException</param>
        /// <returns>DbEntityValidationException详细异常信息</returns>
        public static string GetFullErrorText(this DbEntityValidationException exc)
        {
            StringBuilder _fullError = new StringBuilder();
            foreach (var validationErrors in exc.EntityValidationErrors)
            {
                foreach (var error in validationErrors.ValidationErrors)
                {
                    _fullError.AppendFormat("Property: {0} Error: {1}{2}", error.PropertyName, error.ErrorMessage, Environment.NewLine);
                }
            }
            return _fullError.ToString();
        }

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