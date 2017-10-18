using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;
using TDF.Demo.Domain.Infrastructure;

namespace TDF.Demo.Domain.Entities
{
    public abstract partial class EntityBase : IEntity, IBetweenCreatedTime
    {

    }
}
