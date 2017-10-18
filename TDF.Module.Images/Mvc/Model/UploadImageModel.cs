using System.Collections.Generic;
using TDF.Core.Models;
using TDF.Web.Attributes;

namespace TDF.Module.Images.Mvc.Model
{
    public class UploadImageModel
    {
        [FileValidate("jpg,png,gif", IsRequired = true, MaxContentLength = 1024*2)]
        public PostedFile FileModel { get; set; }

        //[FileValidate("jpg,png", IsRequired = true, MaxContentLength =200)]
        //public List<PostedFile> FileModels { get; set; }

        public string Name { get; set; }
    }
}
