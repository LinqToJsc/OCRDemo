using TDF.Web.Models;

namespace TDF.Demo.Service.Dtos.OcrData
{
    public class OcrDataCriteria : PagedCriteria
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyword { get; set; }
    }
}
