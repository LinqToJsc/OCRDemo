using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Ioc;
using TDF.Data.Repository;
using TDF.Module.Images.Dto;
using TDF.Module.Images.Entity;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images
{
    public static class ImageHelper
    {
        /// <summary>
        /// 返回的格式: "img/{size}/default-{name}.{format}", "img/{size}/t{imageType}t{yearMonth}-{id}.{format}"
        /// </summary>
        /// <param name="key">
        /// t10t201704-6C294E0437664E3FA99C0CBAA4E8CCD1.png
        /// default-websitelogo.png
        /// </param>
        /// <param name="size"></param>
        /// <returns>
        /// 例如 /img/full/t20t201410-6C294E0437664E3FA99C0CBAA4E8CCD1.png
        /// 例如 /img/s80x80/t20t201410-6C294E0437664E3FA99C0CBAA4E8CCD1.png
        /// </returns>
        public static string BuildSrc(string key, ImageSize size)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return $"/img/{(int)size}/{key}";
        }

        public static ImageDataDto GetImageDataDto(Guid imageId)
        {
            return Ioc.Resolve<IImageService>().GetImageDataDto(imageId);
        }

        public static string SaveImageAndReturnKey(BusinessType type, Image image)
        {
            return Ioc.Resolve<IImageService>().SaveImageAndReturnKey(type, image);
        }

        public static List<string> GetImageKeysByTargetId(Guid id)
        {
            return Ioc.Resolve<IImageService>().GetImageKeysByTargetId(id);
        }

        public static Dictionary<Guid, List<string>> GetImageKeysByTargetId(List<Guid> ids)
        {
            return Ioc.Resolve<IImageService>().GetImageKeysByTargetId(ids);
        }

        public static string GetImageKeyByTargetId(Guid id)
        {
            return Ioc.Resolve<IImageService>().GetImageKeyByTargetId(id);
        }

        public static Dictionary<Guid, string> GetImageKeyByTargetId(List<Guid> ids)
        {
            return Ioc.Resolve<IImageService>().GetImageKeyByTargetId(ids);
        }

        public static void RelationImage(Guid targetId, string imageKey, bool isReplace = true)
        {
            Ioc.Resolve<IImageService>().RelationImage(targetId, imageKey, isReplace);
        }

        public static void RemoveRelationImage(Guid targetId)
        {
            Ioc.Resolve<IImageService>().RemoveRelationImage(targetId);
        }
    }
}
