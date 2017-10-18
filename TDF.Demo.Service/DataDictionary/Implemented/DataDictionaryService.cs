using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Core.Models;
using TDF.Core.Tools;
using TDF.Data.Repository;
using TDF.Demo.Domain.Entities;
using TDF.Demo.Domain.Entities.Extensions;
using TDF.Demo.Domain.Entities.Extensions.DataDictionary;
using TDF.Demo.Domain.Infrastructure;
using TDF.Demo.Service.Dtos.DataDictionary;

namespace TDF.Demo.Service.DataDictionary
{
    public class DataDictionaryService : IDataDictionaryService
    {
        public List<DataDictionaryTypeBasicDto> GetDataDictionaryTypes()
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<DataDictionaryType>>())
            {
               return repository.IQueryable().WhereNotDelete().ToBasicDtos().ToList();
            }
        }

        public IPagedList<DataDictionaryBasicDto> GetDataDictionaryPagedList(DataDictionaryCriteria criteria)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<Domain.Entities.DataDictionary>>())
            {
                return repository.IQueryable()
                    .WhereNotDelete()
                    .WhereByEnabled(criteria.Enabled)
                    .WhereByTypeId(criteria.TypeId)
                    .WhereByKeyword(criteria.Keyword)
                    .OrderBy(x => x.ParentId).ThenBy(x => x.Sort)
                    .ThenByDescending(x => x.CreatedTime)
                    .ToBasicDtos()
                    .ToPageResult(criteria.PageIndex, criteria.PageSize);
            }
        }

        public void AddDataDictionary(DataDictionaryDto data)
        {
            if (string.IsNullOrEmpty(data.Key))
            {
                data.Key = Common.BuildKey();
            }
            var entity = AutoMapper.Mapper.Map<Domain.Entities.DataDictionary>(data);
            using (var repository = Ioc.Resolve<IRepositoryBase<Domain.Entities.DataDictionary>>())
            {
                //该字典类型下面的字典不能重复
                if (repository.IQueryable().Any(x => x.Name == entity.Name && x.ParentId == entity.ParentId))
                    throw new KnownException("创建失败，已存在相同的项");

                entity.CreateByOperator();
                repository.Insert(entity);
            }
        }

        public void AddDataDictionaryType(DataDictionaryTypeDto dataType)
        {
            var entity = AutoMapper.Mapper.Map<DataDictionaryType>(dataType);
            entity.Code = Common.BuildKey();
            entity.CreateByOperator();
            using (var repository = Ioc.Resolve<IRepositoryBase<DataDictionaryType>>())
            {
                repository.Insert(entity);
            }
        }
    }
}
