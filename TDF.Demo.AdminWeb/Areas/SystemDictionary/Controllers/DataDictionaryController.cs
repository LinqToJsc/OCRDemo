using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Demo.AdminWeb.Areas.SystemDictionary.Models;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.AdminWeb.Controllers;
using TDF.Demo.AdminWeb.Models;
using TDF.Demo.Service.DataDictionary;
using TDF.Demo.Service.Dtos.DataDictionary;
using TDF.Demo.Service.SystemManage;
using TDF.Web.Attributes.Mvc;
using TDF.Web.Models;

namespace TDF.Demo.AdminWeb.Areas.SystemDictionary.Controllers
{
    public class DataDictionaryController : AdminControllerBase
    {
        // GET: SystemDictionary/DataDictionary
        //-------------------

        //字典列表页面
        public ActionResult List(Guid typeid)
        {
            return View();
        }

        /// <summary>
        /// 字典类型列表数据加载
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [TdfHandlerAuthorize(true)]
        public ActionResult GetDataDictionaryTypePagedList(PagedCriteria criteria)
        {
            return Ioc.Resolve<IDataDictionaryService>().GetDataDictionaryTypes().ToJqueryDataTableModel(criteria.draw);
        }

        /// <summary>
        /// 字典新增类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult AddDataType(DataTypeModel model)
        {
            var dataType = new DataDictionaryTypeDto()
            {
                Name = model.Name,
                Remark = model.Remark
            };
            Ioc.Resolve<IDataDictionaryService>().AddDataDictionaryType(dataType);
            return Success();
        }

    }
}