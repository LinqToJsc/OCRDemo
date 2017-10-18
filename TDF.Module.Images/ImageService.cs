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
    public class ImageService : IImageService
    {
        public ImageDataDto GetImageDataDto(Guid imageId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<Image>>())
            {
                return repository.IQueryable().Where(x => x.Id == imageId)
                    .Select(x => new ImageDataDto()
                    {
                        Bytes = x.Bytes,
                        Format = x.Format
                    })
                    .FirstOrDefault();
            }
        }

        public string SaveImageAndReturnKey(BusinessType type, Image image)
        {
            image.Key = Image.GenerateKey(type, image.Id, image.Format);
            using (var repository = Ioc.Resolve<IRepositoryBase<Image>>())
            {
                repository.Insert(image);
                return image.Key;
            }
        }

        public List<string> GetImageKeysByTargetId(Guid id)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                return repository.IQueryable().Where(x => x.TargetId == id).Select(x => x.ImageKey).ToList();
            }
        }

        public Dictionary<Guid, List<string>> GetImageKeysByTargetId(List<Guid> ids)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                return
                    repository.IQueryable()
                        .Where(x => ids.Contains(x.TargetId))
                        .GroupBy(x => x.TargetId, x => x.ImageKey)
                        .ToDictionary(x => x.Key, x => x.ToList());
            }
        }

        public string GetImageKeyByTargetId(Guid id)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                return repository.IQueryable().Where(x => x.TargetId == id).Select(x => x.ImageKey).FirstOrDefault();
            }
        }

        public Dictionary<Guid, string> GetImageKeyByTargetId(List<Guid> ids)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                return
                    repository.IQueryable()
                        .Where(x => ids.Contains(x.TargetId))
                        .ToDictionary(x => x.TargetId, x => x.ImageKey);
            }
        }

        public void RelationImage(Guid targetId, string imageKey, bool isReplace = true)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                if (isReplace)
                {
                    var results = repository.IQueryable(x => x.TargetId == targetId).ToList();
                    foreach (var item in results)
                    {
                        repository.Delete(item);
                    }
                }
                repository.Insert(new ImageRelevance()
                {
                    Id = Guid.NewGuid(),
                    ImageKey = imageKey,
                    TargetId = targetId
                });
            }
        }

        public void RemoveRelationImage(Guid targetId)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ImageRelevance>>())
            {
                var results = repository.IQueryable(x => x.TargetId == targetId).ToList();
                foreach (var item in results)
                {
                    repository.Delete(item);
                }
            }
        }
    }
}
