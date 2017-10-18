using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Configuration;
using TDF.Core.Log;

namespace TDF.Module.Images
{
    public class ImageModuleInitializer : IModuleInitializer
    {
        public int Order => 1;
        public string ComponentName => "图片资源模块";
        public void Initialize()
        {
            LogFactory.GetLogger().Info("图片模块已加载");
        }
    }
}
