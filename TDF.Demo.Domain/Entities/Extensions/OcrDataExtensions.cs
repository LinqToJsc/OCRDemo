using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core;

namespace TDF.Demo.Domain.Entities.Extensions
{
    public static class OcrDataExtensions
    {
        public static IQueryable<OcrData> WhereByKey(this IQueryable<OcrData> query, string keyword)
        {
            var expression = Ext.True<OcrData>();
            if (string.IsNullOrEmpty(keyword))
            {
                return query;
            }
            //expression = expression.And(x => x.RealName.Contains(keyword) || x.Account.Contains(keyword));
            //if (keyword == "男" || keyword == "女")
            //{
            //    var isMan = keyword == "男";
            //    expression = expression.Or(x => x.Gender != null && x.Gender.Value == isMan);
            //}
            //if (ValidateHelper.IsValidMobile(keyword))
            //{
            //    expression = expression.Or(x => x.MobilePhone == keyword);
            //}
            return query.Where(expression);
        }
    }
}
