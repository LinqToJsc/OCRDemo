using System.ComponentModel.DataAnnotations;
using TDF.Core.Tools;

namespace TDF.Core.DataAnnotations
{
    /// <summary>
    /// 验证是否是Email
    /// </summary>
    public class EmailAttribute : ValidationAttribute
    {
        public bool Required { get; set; } = true;

        public override bool IsValid(object value)
        {
            if (!Required)
            {
                return true;
            }
            if (!(value is string)) return false;
            var passed = ValidateHelper.IsEmail(value.ToString());
            return passed;
        }
    }
}
