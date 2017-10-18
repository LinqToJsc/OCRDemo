using System.Collections.Generic;
using System.Linq;

namespace TDF.Core.Configuration
{
    /// <summary>
    /// 框架配置上下文
    /// </summary>
    public class InitializerContext
    {
        public List<IInitializer> Initialzer { get; set; }

        public static InitializerContext Instance => new InitializerContext();

        private InitializerContext()
        {
            Initialzer = new List<IInitializer>();
        }

        /// <summary>
        /// 应用配置
        /// </summary>
        public void ExecuteInit()
        {
            Initialzer.OrderBy(x => x.Order).ToList().ForEach(x=> x.Initialize());
        }
    }
}
