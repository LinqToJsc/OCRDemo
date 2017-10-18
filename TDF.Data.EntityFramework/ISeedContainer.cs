using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDF.Data.EntityFramework
{
    /// <summary>
    /// 数据种子容器接口
    /// </summary>
    public interface ISeedContainer
    {
        void Seed(System.Data.Entity.DbContext dbContext);
    }
}
