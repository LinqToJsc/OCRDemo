using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TDF.Demo.AdminWeb.Areas.SystemDictionary.Models
{
    public class DataTypeModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "名称不能为空"), MaxLength(20, ErrorMessage = "最多20个字符")]
        public string Name { get; set; }

        [Required(ErrorMessage = "备注不能为空"), MaxLength(200, ErrorMessage = "不能超过2000个字符")]
        public string Remark { get; set; }
    }
}