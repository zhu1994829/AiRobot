using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MX.AIRobot.Service;
using MX.AIRobot.Model;
using MX.AIRobot.Util;
using MX.AIRobot.ApiModel;
using MX.AIRobot.AOP;
using MX.AIRobot.Interface;
using PetaPoco;
using System.IO;
using Newtonsoft.Json;


namespace MX.AIRobot.Web.Controllers
{
    public class RecordController : BaseController
    {
        #region Service
        RecordService recordService = new RecordService();
        #endregion

        #region 列表

     

        [HttpGet]
        public ActionResult Index(BizRecord bizRecord, int page = 1)
        {
            Page<BizRecord> recordList = new Page<BizRecord>();
            Page<ViewRecord> viewlist = new Page<ViewRecord>();
            AspectF.Define
              .Log(log, "RecordController-Index[post]开始", "RecordController-Index[post]结束")
              .HowLong(log)
              .Retry(log)
              .Do(() =>
              {
                  recordList = recordService.PageList(bizRecord, PageHelper.PageSize, page);
                  ViewPageList(recordList, viewlist);
              });
            return new JsonResultPro(viewlist, JsonRequestBehavior.AllowGet, "yyyy-MM-dd HH:mm:ss");
        }

        #endregion

        #region 删除
        [HttpPost]
        public ActionResult Delete()
        {
            bool result = false;
            AspectF.Define
            .Log(log, "RecordController-Delete[post]开始", "RecordController-Delete[post]结束")
            .HowLong(log)
            .Retry(log)
            .Do(() =>
            {
                var reader = new StreamReader(Request.InputStream).ReadToEnd();
                var model = JsonConvert.DeserializeObject<string []>(reader);
                result = recordService.Delete(model);
            });
            if (result)
            {
                return Json(new { status = "y" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "n" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 添加
        [HttpPost]
        public ActionResult Create()
        {

            bool result = false;
            AspectF.Define
                 .Log(log, "RecordController-Create[post]开始", "RecordController-Create[post]结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var reader = new StreamReader(Request.InputStream).ReadToEnd();
                     var model = JsonConvert.DeserializeObject<ViewRecord>(reader);
                     BizRecord bizRecord = new BizRecord();                  
                     ViewToDBForModel(bizRecord, model);
                     result = recordService.Add(bizRecord);
                   
                 });
            if (result)
            {
                return Json(new { status = "y" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = "n" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion



    }
}
