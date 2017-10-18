using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Demo.Domain.Infrastructure
{
    public interface ICreationAudited : IEntity
    {
        DateTime CreatedTime { get; set; }

        string CreatorName { get; set; }

        Guid? CreatorId { get; set; }
    }
}
