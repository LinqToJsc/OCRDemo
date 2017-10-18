using System.ComponentModel.DataAnnotations;
using TDF.Core.Tools;

namespace TDF.Core.DataAnnotations
{
    /// <summary>
    /// 验证是否是固定电话或者是手机
    /// </summary>
    public class PhoneAttribute : ValidationAttribute
    {
        public bool Required { get; set; } = true;

        public override bool IsValid(object value)
        {
            if (!Required)
            {
                return true;
            }
            if (!(value is string)) return false;
            var passed = ValidateHelper.IsValidPhone(value.ToString()) ||
                         ValidateHelper.IsValidMobile(value.ToString());
            return passed;
        }
    }
}
