using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Demo.Service.Dtos.DataDictionary;

namespace TDF.Demo.Service.DataDictionary
{
    public interface IDataDictionaryService
    {
        List<DataDictionaryTypeBasicDto> GetDataDictionaryTypes();

        IPagedList<DataDictionaryBasicDto> GetDataDictionaryPagedList(DataDictionaryCriteria criteria);

        void AddDataDictionary(DataDictionaryDto data);

        void AddDataDictionaryType(DataDictionaryTypeDto dataType);
    }
}
