using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Web.Models;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public class DataDictionaryCriteria : BaseSearchCriteria
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// 字典分类Id
        /// </summary>
        public Guid? TypeId { get; set; }
    }
}
