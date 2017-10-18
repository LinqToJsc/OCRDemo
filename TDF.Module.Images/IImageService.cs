using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Module.Images.Dto;
using TDF.Module.Images.Entity;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images
{
    /// <summary>
    /// 图片资源服务
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// 获得图片数据
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        ImageDataDto GetImageDataDto(Guid imageId);

        /// <summary>
        /// 保存图片且返回图片Key
        /// </summary>
        /// <param name="type"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        string SaveImageAndReturnKey(BusinessType type, Image image);

        /// <summary>
        /// 根据关联的Id获取图片Key集合
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<string> GetImageKeysByTargetId(Guid id);

        /// <summary>
        /// 根据关联的Id获取图片Key集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Dictionary<Guid,List<string>> GetImageKeysByTargetId(List<Guid> ids);

        /// <summary>
        /// 根据关联的Id获取图片Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetImageKeyByTargetId(Guid id);


        /// <summary>
        /// 根据关联的Id获取图片Key
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Dictionary<Guid,string> GetImageKeyByTargetId(List<Guid> ids);

        /// <summary>
        /// 将图片Key和目标Id进行关联
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="imageKey"></param>
        /// <param name="isReplace">是否替换原来的关联</param>
        void RelationImage(Guid targetId,string imageKey,bool isReplace = true);

        /// <summary>
        /// 清除目标Id对应的所有关联图片
        /// </summary>
        /// <param name="targetId"></param>
        void RemoveRelationImage(Guid targetId);
    }
}
