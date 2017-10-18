using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Entity;

namespace TDF.Module.Images.Entity
{
    public class ImageRelevance : IEntity
    {
        public Guid Id { get; set; }

        public Guid TargetId { get; set; }

        public string ImageKey { get; set; }
    }
}
