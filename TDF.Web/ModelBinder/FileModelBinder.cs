using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TDF.Core.Models;

namespace TDF.Web.ModelBinder
{
    /// <summary>
    /// 文件模型绑定器
    /// </summary>
    public class FileModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var fileModel = bindingContext.Model as PostedFile ?? new PostedFile();
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var files = result.RawValue as System.Web.HttpPostedFileBase[];
            if (files != null && files.Any() && files[0] != null)
            {
                var file = files[0];
                fileModel.FileStream = file.InputStream;
                fileModel.ContentLength = file.ContentLength;
                fileModel.FileName = file.FileName;
                fileModel.ContentType = file.ContentType;
            }
            else
            {
                return null;
            }
            return fileModel;
        }
    }

    public class FilesModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var filesModel = bindingContext.Model as List<PostedFile> ?? new List<PostedFile>();
            var result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var files = result.RawValue as System.Web.HttpPostedFileBase[];
            if (files != null && files.Any())
            {
                filesModel.AddRange(files.Where(file => file != null).Select(file => new PostedFile()
                {
                    FileStream = file.InputStream,
                    FileName = file.FileName,
                    ContentLength = file.ContentLength,
                    ContentType = file.ContentType
                }));
            }
            return filesModel;
        }
    }
}
