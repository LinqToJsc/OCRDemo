using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Ioc;
using TDF.Core.Json;
using TDF.Demo.AdminWeb.Models.OCR;
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

        public ActionResult Add()
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


        public ActionResult OCRImg(string imgPath=null)
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
                physicalPath = physicalPath + pDate.Substring(pDate.Length - 2)+"/"+ imgKey.Substring(0, imgKey.IndexOf('.'))+"/"+"Full"+ imgKey.Substring(imgKey.IndexOf('.'));

                var imgDatas = OcrImgToDataServer.GetData(physicalPath);

                result.Succeed();
                result.Value = new {Data = imgDatas.Item1, ListData = imgDatas.Item2};
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
        public ActionResult SaveOcr()
        {
            
            return Success();
        }
    }
}