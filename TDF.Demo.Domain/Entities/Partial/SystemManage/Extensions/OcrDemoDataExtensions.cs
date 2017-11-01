using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core;
using TDF.Core.Ioc;
using TDF.Core.Operator;
using TDF.Core.Tools;

namespace TDF.Demo.Domain.Entities.Partial.SystemManage.Extensions
{
    public static class OcrDemoDataExtensions
    {
        public static IQueryable<OcrDemoData> WhereByKey(this IQueryable<OcrDemoData> query, string keyword)
        {
            var expression = Ext.True<OcrDemoData>();
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

        //public static IQueryable<SystemMember> WhereByEnabled(this IQueryable<SystemMember> query, bool? enabled)
        //{
        //    if (enabled == null)
        //    {
        //        return query;
        //    }
        //    if (!enabled.Value)
        //    {
        //        return query.Where(x => x.EnabledMark != null && !x.EnabledMark.Value);
        //    }
        //    return query.Where(x => x.EnabledMark == null || x.EnabledMark.Value);
        //}

        ///// <summary>
        ///// 过滤管理员数据权限
        ///// </summary>
        ///// <param name="query"></param>
        ///// <returns></returns>
        //public static IQueryable<SystemMember> WhereByCreatedId(this IQueryable<SystemMember> query)
        //{
        //    var oper = Ioc.Resolve<IOperatorProvider>().GetCurrent();
        //    // 如果是超级管理员或者是 一级管理员 则不作条件限制
        //    if (oper.IsSystem)
        //        return query;
        //    return query.Where(x => x.CreatorId == oper.Id);
        //}
    }
}
