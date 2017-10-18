using System;
using TDF.Core.Models.Enum;

namespace TDF.Web.Models
{
    public class BaseSearchCriteria : PagedCriteria
    {
        public string Keyword { get; set; }

        public DateTime? CreatedTimeFrom { get; set; }

        public DateTime? CreatedTimeTo { get; set; }

        public string Property { get; set; }

        public OrderBy OrderBy { get; set; }
    }
}
