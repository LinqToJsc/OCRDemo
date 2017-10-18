using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Demo.Domain.Infrastructure
{
    public interface IModificationAudited : IEntity
    {
        DateTime? ModifiedTime { get; set; }

        Guid? ModifierId { get; set; }

        string ModifierName { get; set; }
    }
}
