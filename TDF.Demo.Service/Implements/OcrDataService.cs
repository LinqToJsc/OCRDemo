using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Demo.Service.Dtos.OcrData;
using TDF.Core.Ioc;
using TDF.Data.EntityFramework.Repository;
using TDF.Data.Repository;
using TDF.Demo.Domain.Entities;
using TDF.Core.Operator;
using TDF.Demo.Domain.Entities.Extensions;
namespace TDF.Demo.Service.Implements
{
    public class OcrDataService : IOcrDataService
    {
        public IPagedList<OcrDataDto> GetOcrDataInfoPagedList(OcrDataCriteria criteria)
        {
            //OcrDataDto dto = new OcrDataDto();
            //dto.Id = Guid.NewGuid().ToString();
            //dto.Company = "1111";

            //OcrDataDto dto1 = new OcrDataDto();
            //dto1.Id = Guid.NewGuid().ToString();
            //dto1.Company = "1111";

            //PagedList<OcrDataDto> list = new PagedList<OcrDataDto>();
            //list.Rows.Add(dto);
            //list.Rows.Add(dto1);
            //list.PageSize = 10;
            //list.PageIndex = 1;

            //return list;


            using (var repository = Ioc.Resolve<IRepositoryBase<ocr_data>>())
            {
                var OcrDataConfigInfos1 = repository.IQueryable()
                    //.WhereByKeyword(criteria.Keyword)
                    .ToOcrDataDto()
                 .OrderByDescending(x => x.CreatedTime);
                var OcrDataConfigInfos=  OcrDataConfigInfos1.ToPageResult(criteria.PageIndex, criteria.PageSize);

                //var productTypeIds = productConfigInfos.Rows.Select(x => x.ProductTypeId).Distinct().ToList();

                //var areas = GetAreaList(repository, productTypeIds);

                //Level 排序
                //productConfigInfos.Rows.ForEach(x => { OrderProductConfigInfoDto(x, areas); });

                return OcrDataConfigInfos;
            }
        }

        public bool RemoveOcrData(Guid Id)
        {
            return false;
        }

        public bool SaveOcrData(OcrDataDto model)
        {
            try
            {
                using (var repository = Ioc.Resolve<IRepositoryBase<ocr_data>>())
                {
                    var entity = AutoMapper.Mapper.Map<ocr_data>(model);
                    repository.Insert(entity);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public OcrDataDto GetOcrDataById(Guid Id)
        {
            using (var repository = Ioc.Resolve<IRepositoryBase<ocr_data>>())
            {
                var entity = repository.FindEntity(Id);
                if (entity != null)
                {
                    OcrDataDto dto = new OcrDataDto();
                    dto.Company = entity.Company;
                    dto.CreatedTime = entity.CreatedTime;
                    dto.DeliveryAddress = entity.DeliveryAddress;
                    dto.Id = entity.Id;
                    dto.InvoiceDate = entity.InvoiceDate;
                    dto.InvoiceNumber = entity.InvoiceNumber;
                    dto.ListData = entity.ListData;
                    dto.TotalAmount = entity.TotalAmount;
                    return dto;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
