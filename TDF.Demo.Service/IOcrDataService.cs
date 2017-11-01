using System;
using TDF.Core.Models;
using TDF.Demo.Service.Dtos.OcrData;

namespace TDF.Demo.Service
{
    public interface IOcrDataService
    {
        IPagedList<OcrDataDto> GetOcrDataInfoPagedList(OcrDataCriteria criteria);
        bool RemoveOcrData(Guid Id);

        bool SaveOcrData(OcrDataDto model);

        OcrDataDto GetOcrDataById(Guid Id);
    }
}
