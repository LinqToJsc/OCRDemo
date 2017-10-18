using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Json;
using TDF.Core.Models;

namespace TDF.Demo.AdminWeb.Models
{
    /// <summary>
    /// 前端BootstrapTable数据模型
    /// </summary>
    public class BootstrapTableModel : ActionResult
    {
        public long total { get; set; }

        public object rows { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            WriteToResponse(response);
        }

        public void WriteToResponse(HttpResponseBase response)
        {
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.Write(this.ToJson());
        }
    }

    public class JqueryDataTableModel : ActionResult
    {
        public long draw { get; set; }

        public object data { get; set; }

        public long recordsTotal { get; set; }

        public long recordsFiltered { get; set; }

        public string error { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            WriteToResponse(response);
        }

        public void WriteToResponse(HttpResponseBase response)
        {
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.Write(this.ToJson());
        }
    }

    public static class BootstrapTableModelExtensions
    {
        public static BootstrapTableModel ToBootstrapTableModel<T>(this IPagedList<T> pagedList) where T : IDto
        {
            return new BootstrapTableModel()
            {
                rows = pagedList.Rows ?? new List<T>(),
                total = pagedList.TotalCount
            };
        }

        public static BootstrapTableModel ToBootstrapTableModel<T>(this IList<T> list) where T : IDto
        {
            return new BootstrapTableModel()
            {
                rows = list ?? new List<T>(),
                total = list.Count
            };
        }
    }

    public static class JqueryDataTableModelExtensions
    {
        public static JqueryDataTableModel ToJqueryDataTableModel<T>(this IPagedList<T> pagedList) where T : IDto
        {
            return new JqueryDataTableModel()
            {
                draw = pagedList.PageIndex,
                data = pagedList.Rows ?? new List<T>(),
                recordsFiltered = pagedList.TotalCount,
                recordsTotal = pagedList.TotalCount
            };
        }

        public static JqueryDataTableModel ToJqueryDataTableModel<T>(this IList<T> list, long? draw) where T : IDto
        {
            return new JqueryDataTableModel()
            {
                draw = draw ?? 0,
                data = list ?? new List<T>(),
                recordsFiltered = list.Count,
                recordsTotal = list.Count
            };
        }
    }
}