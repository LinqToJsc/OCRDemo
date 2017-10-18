using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Web.Models;

namespace TDF.Demo.Service.Dtos.SystemManage
{
    public class ModuleCriteria : PagedCriteria
    {
        public bool? Enabled { get; set; }
    }
}
