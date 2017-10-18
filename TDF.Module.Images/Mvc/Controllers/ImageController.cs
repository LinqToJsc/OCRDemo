using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Module.Images.Enums;
using TDF.Module.Images.Extensions;
using TDF.Module.Images.Mvc.Model;
using TDF.Web.Attributes.Mvc;
using TDF.Web.Models;

namespace TDF.Module.Images.Mvc.Controllers
{
    public class ImageController : Controller
    {
        public FilePathResult Index(ImageParameter parameter = null)
        {
            parameter = parameter.GetFixed(new DefaultImageParameterFixer());
            var physicalPath = Server.MapPath(parameter.GetRelativePath());
            var contentType = "image/" + parameter.Format;
            if (System.IO.File.Exists(physicalPath))
            {
                return File(physicalPath, contentType);
            }
            lock (physicalPath)
            {
                var physicalFolder = Path.GetDirectoryName(physicalPath);
                if (!Directory.Exists(physicalFolder))
                {
                    Directory.CreateDirectory(physicalFolder);
                }
                var data = Resolver.GetService<IImageService>().GetImageDataDto(parameter.Guid);
                if (data == null)
                {
                    throw new KnownException("图片已经被删除了");
                }
                using (var stream = new MemoryStream(data.Bytes))
                {
                    using (var sysImage = Image.FromStream(stream))
                    {
                        if (data.Format == ImageFormat.Gif)
                        {
                            sysImage.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Gif);
                        }
                        else
                        {
                            using (var sizedImage = sysImage.ToSize(parameter.Size.GetSize()))
                            {
                                sizedImage.SaveToFileInQuality(physicalPath, parameter.ImageFormat.GetSystemImageFormat());
                            }
                        }
                    }
                }
            }
            return File(physicalPath, contentType);
        }

        public ActionResult Upload()
        {
            return View("Upload");
        }

        [HttpPost]
        [ModelValidation]
        public ActionResult Upload(UploadImageModel model)
        {
            var imageKey = Ioc.Resolve<IImageService>().SaveImageAndReturnKey(BusinessType.Default, model.FileModel.ToImage());
            var result = new JsonResult<object>();
            result.Succeed();
            result.Value = Tuple.Create(imageKey, ImageHelper.BuildSrc(imageKey, ImageSize.Full));
            return result;
        }
    }
}
