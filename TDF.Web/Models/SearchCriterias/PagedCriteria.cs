using System.Collections.Generic;
using TDF.Core.Configuration;

namespace TDF.Web.Models
{
    public class PagedCriteria
    {
        public PagedCriteria()
        {
            PageSize = Configs.PageSize;
        }

        /// <summary>
        /// 从0开始的页数
        /// </summary>
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long draw { get; set; }

        public int PageNumber
        {
            get { return PageIndex + 1; }
            //set { PageIndex = value - 1; }//为了防止WebApi的歧义，只保留PageIndex这一个输入参数
        }

        public List<PageOrders>  OrderSorts { get; set; }
    }

    public class PageOrders
    {
        public string OrderName { get; set; }

        public PageOrderType OrderType { get; set; }
    }

    public enum PageOrderType
    {
        /// <summary>
        /// 正序
        /// </summary>
        asc,
        
        /// <summary>
        /// 倒序
        /// </summary>
        desc
    }
}
