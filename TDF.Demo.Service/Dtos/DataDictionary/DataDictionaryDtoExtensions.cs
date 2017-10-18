using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Service.Dtos.DataDictionary
{
    public static class DataDictionaryDtoExtensions
    {
        public static IQueryable<DataDictionaryTypeDto> ToDtos(this IQueryable<DataDictionaryType> query)
        {
            return query.Select(x => new DataDictionaryTypeDto()
            {
                Id = x.Id,
                CreatedTime = x.CreatedTime,
                CreatorId = x.CreatorId,
                CreatorName = x.CreatorName,
                ModifiedTime = x.ModifiedTime,
                Name = x.Name,
                Remark = x.Remark,
                Deleted = x.Deleted,
                DeletedTime = x.DeletedTime,
                ModifierName = x.ModifierName
            });
        }

        public static IQueryable<DataDictionaryTypeBasicDto> ToBasicDtos(this IQueryable<DataDictionaryType> query)
        {
            return query.Select(x => new DataDictionaryTypeBasicDto()
            {
                Id = x.Id,
                Name = x.Name,
                Remark = x.Remark,
                Code = x.Code
            });
        }

        public static IQueryable<DataDictionaryBasicDto> ToBasicDtos(this IQueryable<Domain.Entities.DataDictionary> query)
        {
            return query.Select(x => new DataDictionaryBasicDto()
            {
                Id = x.Id,
                CreatedTime = x.CreatedTime,
                ParentId = x.ParentId,
                IsSystem = x.IsSystem,
                Disabled = x.Disabled,
                Sort = x.Sort,
                TypeName = x.DataDictionaryType.Name,
                Key = x.Key,
                Name = x.Name
            });
        }
    }
}
