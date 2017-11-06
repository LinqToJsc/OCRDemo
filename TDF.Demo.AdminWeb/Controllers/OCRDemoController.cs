using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Configuration;
using TDF.Core.Ioc;
using TDF.Core.Json;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.AdminWeb.Models;
using TDF.Demo.AdminWeb.Models.OCR;
using TDF.Demo.Service;
using TDF.Demo.Service.Dtos.OcrData;
using TDF.Demo.Service.Implements;
using TDF.Module.Images;
using TDF.Module.Images.Enums;
using TDF.Module.Images.Extensions;
using TDF.Module.Images.Mvc.Model;
using TDF.Web.Attributes.Mvc;
using TDF.Web.Models;

namespace TDF.Demo.AdminWeb.Controllers
{
    /// <summary>
    /// 图片识别Demo
    /// </summary>
    public class OCRDemoController : AdminControllerBase
    {
        // GET: OCRDemo
        public override ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Add(Guid? Id)
        {
            ViewBag.ListGo = Id != null;
            if (Id == null)
            {
                OcrDataDto dto = new OcrDataDto();
                return View(dto);
            }
            else
            {
                
                OcrDataService ocrDataService = new OcrDataService();
                var model = ocrDataService.GetOcrDataById(Id.Value);
                return View(model);
            }
        }
        public ActionResult List()
        {
            return View();
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


        public ActionResult OCRImg(string imgPath = null)
        {
            //"img/{size}/t{imageType}t{yearMonth}-{id}.{format}"
            var result = new JsonResult<object>();
            if (string.IsNullOrEmpty(imgPath))
                result.Fail();
            else
            {
                var physicalPath = Server.MapPath("~/storage/images/2017/");
                var pj = imgPath.Split('/');
                var p3 = pj[3];
                var pDate = p3.Split('-')[0];
                var imgKey = p3.Substring(p3.IndexOf('-') + 1);
                var imgFix = imgKey.Substring(imgKey.IndexOf('.') + 1).ToLower();
                ImageFormat imageFormat = imgFix == "jpg" ? ImageFormat.Jpeg : ImageFormat.Png;
                physicalPath = physicalPath + pDate.Substring(pDate.Length - 2) + "/" + imgKey.Substring(0, imgKey.IndexOf('.')) + "/" + "Full" + imgKey.Substring(imgKey.IndexOf('.'));

                var imgDatas = OcrImgToDataServer.GetData(physicalPath);

                result.Succeed();
                result.Value = new { Data = imgDatas.Item1, ListData = imgDatas.Item2 };
            }

            return result;
        }

        ///// <summary>
        ///// 将数据录入到数据库
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult SaveOcr(OcrDataModel model)
        //{

        //    return Success();
        //}


        /// <summary>
        /// 将数据录入到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult SaveOcr(OcrDataModel model)
        {
            OcrDataService ocrDataService = new OcrDataService();
            OcrDataDto dto = new OcrDataDto();
            dto.Company = model.Company;
            dto.DeliveryAddress = model.DeliveryAddress;
            dto.InvoiceDate = model.InvoiceDate;
            dto.InvoiceNumber = model.InvoiceNumber;
            dto.TotalAmount = model.TotalAmount;
            dto.ListData = model.ListData;
            if (!string.IsNullOrEmpty(model.Id))
            {
                dto.Id = Guid.Parse(model.Id);
            }
            try
            {
                SaveDataOcr(dto);
            }
            catch (Exception ex)
            {
            }

            return Success();
        }
        [TdfHandlerAuthorize(true)]
        public ActionResult GetPagedList(OcrDataCriteria criteria)
        {
            OcrDataService ocrDataService = new OcrDataService();
            var pagedList = ocrDataService.GetOcrDataInfoPagedList(criteria);
            return pagedList.ToJqueryDataTableModel();
        }

        private void SaveDataOcr(OcrDataDto dto)
        {
            //string sql = string.Format("SET SQL_SAFE_UPDATES = 0;insert INTO ocr_data(Id, InvoiceNumber, InvoiceDate, DeliveryAddress, TotalAmount, Company, ListData, CreatedTime) VALUES(uuid(), @InvoiceNumber, @InvoiceDate, @DeliveryAddress, @TotalAmount, @Company, @ListData, NOW());SET SQL_SAFE_UPDATES = 1;", dto.InvoiceNumber, dto.InvoiceDate, dto.DeliveryAddress, dto.TotalAmount, dto.Company, dto.ListData);

            if (dto.Id==Guid.Empty)
            {
                string sql = string.Format("SET SQL_SAFE_UPDATES = 0;insert INTO ocr_data(Id, InvoiceNumber, InvoiceDate, DeliveryAddress, TotalAmount, Company, ListData, CreatedTime) VALUES(uuid(),'{0}','{1}','{2}','{3}','{4}','{5}', NOW());SET SQL_SAFE_UPDATES = 1;", dto.InvoiceNumber, dto.InvoiceDate, dto.DeliveryAddress, dto.TotalAmount, dto.Company, dto.ListData);
                MySqlConnection myCon = new MySqlConnection(ConfigurationManager.ConnectionStrings[Configs.Instance.ConnectionString].ToString());
                myCon.Open();
                MySqlCommand mysqlcom = new MySqlCommand(sql, myCon);
                mysqlcom.ExecuteNonQuery();
                mysqlcom.Dispose();
                myCon.Close();
                myCon.Dispose();
            }
            else
            {
                string sql = string.Format("SET SQL_SAFE_UPDATES = 0;update ocr_data set InvoiceNumber='{0}', InvoiceDate='{1}', DeliveryAddress='{2}', TotalAmount='{3}', Company='{4}', ListData='{5}' where Id='{6}';SET SQL_SAFE_UPDATES = 1;", dto.InvoiceNumber, dto.InvoiceDate, dto.DeliveryAddress, dto.TotalAmount, dto.Company, dto.ListData,dto.Id);
                MySqlConnection myCon = new MySqlConnection(ConfigurationManager.ConnectionStrings[Configs.Instance.ConnectionString].ToString());
                myCon.Open();
                MySqlCommand mysqlcom = new MySqlCommand(sql, myCon);
                mysqlcom.ExecuteNonQuery();
                mysqlcom.Dispose();
                myCon.Close();
                myCon.Dispose();
            }
        }
    }
}