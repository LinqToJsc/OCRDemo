using System.Collections.Generic;
using System.Linq;

namespace TDF.Core.Configuration
{
    public abstract class AbstractInitializer : IInitializer
    {
        /// <summary>
        /// 初始化的组件名称
        /// </summary>
        public virtual string ComponentName => GetType().Name;

        public List<AbstractInitializer> Initializers { get; set; }

        public virtual int Order => 0;

        public virtual void Initialize()
        {
            if (Initializers == null)
            {
                return;
            }
            foreach (var initialzer in Initializers.OrderBy(x => x.Order))
            {
                initialzer.Initialize();
            }
        }
    }
}
