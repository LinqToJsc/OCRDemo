using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TDF.Core.Models;

namespace TDF.Web.ModelBinder
{
    /// <summary>
    /// 提供全局的文件绑定提供器
    /// </summary>
    public class FileModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(PostedFile))
            {
                return new FileModelBinder();
            }
            if (modelType == typeof(List<PostedFile>))
            {
                return new FilesModelBinder();
            }
            return null;
        }
    }
}
