namespace YanZhiwei.DotNet.EntLib6.Utilities
{
    using DotNet4.Interfaces.Validations;
    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using System.Collections.Generic;
    
    /// <summary>
    /// 企业库验证操作
    /// </summary>
    public class EntLibValidateOperator : IValidation
    {
        #region Methods
        
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target">验证目标</param>
        public ValidationResultCollection Validate(object target)
        {
            var validator = ValidationFactory.CreateValidator(target.GetType());
            var results = validator.Validate(target);
            return GetResult(results);
        }
        
        /// <summary>
        /// 获取验证结果
        /// </summary>
        private ValidationResultCollection GetResult(IEnumerable<ValidationResult> results)
        {
            ValidationResultCollection _result = new ValidationResultCollection();
            
            foreach(ValidationResult item in results)
                _result.Add(new System.ComponentModel.DataAnnotations.ValidationResult(item.Message));
                
            return _result;
        }
        
        #endregion Methods
    }
}