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
    public class AnswerBankController : BaseController
    {
        #region Service
        AnswerBankService answerBankService = new AnswerBankService();
        #endregion

        #region 列表

        [HttpGet]
        public ActionResult Index(BizAnswerBank bizAnswerBank, int page = 1)
        {
            Page<BizAnswerBank> answerBankList = new Page<BizAnswerBank>();
            Page<ViewAnswerBank> viewlist = new Page<ViewAnswerBank>();
            AspectF.Define
             .Log(log, "AnswerBankController-Index[get]开始", "AnswerBankController-Index[get]结束")
             .HowLong(log)
             .Retry(log)
             .Do(() =>
             {
                 answerBankList = answerBankService.PageList(bizAnswerBank, PageHelper.PageSize, page);
                 ViewPageList(answerBankList, viewlist);

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
            .Log(log, "AnswerBankController-Delete[post]开始", "AnswerBankController-Delete[post]结束")
            .HowLong(log)
            .Retry(log)
            .Do(() =>
            {
                var reader = new StreamReader(Request.InputStream).ReadToEnd();
                var model = JsonConvert.DeserializeObject<string[]>(reader);
                result = answerBankService.Delete(model);
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



        #region 编辑

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewAnswerBank viewmodel = new ViewAnswerBank();
            AspectF.Define
               .Log(log, "AnswerBankController-Edit[get]开始", "AnswerBankController-Edit[get]结束")
               .HowLong(log)
               .Retry(log)
               .Do(() =>
               {
                   BizAnswerBank bizAnswerBank = new BizAnswerBank();
                   bizAnswerBank.AnswerID = id;
                   bizAnswerBank = answerBankService.Single(bizAnswerBank);
                   DBToViewForModel(bizAnswerBank, viewmodel);
               });

            return new JsonResultPro(viewmodel, JsonRequestBehavior.AllowGet, "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        public ActionResult Edit()
        {
            BizAnswerBank bizAnswerBank = new BizAnswerBank();
            bool result = false;
            AspectF.Define
                .Log(log, "AnswerBankController-Edit[post]开始", "AnswerBankController-Edit[post]结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    var reader = new StreamReader(Request.InputStream).ReadToEnd();
                    var model = JsonConvert.DeserializeObject<ViewAnswerBank>(reader);
                    ViewToDBForModel(bizAnswerBank, model);
                    if (string.IsNullOrEmpty(model.id))
                    {
                        result = answerBankService.Add(bizAnswerBank);
                        if (!string.IsNullOrEmpty(model.countID))
                        {
                            BizRecord bizRecord = new BizRecord();
                            bizRecord.RecordID = model.countID;
                            RecordService recordService = new RecordService();
                            recordService.Delete(new string[] { bizRecord.RecordID });
                        }
                    }
                    else
                    {
                        bizAnswerBank.UpdateTime = DateTime.Now;
                        result = answerBankService.Update(bizAnswerBank);
                    }

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

        #region 详情
        [HttpGet]
        public ActionResult Details(string text)
        {
            BizAnswerBank bizAnswerBank = new BizAnswerBank();
            AspectF.Define
           .Log(log, "AnswerBankController-Details[Get]开始", "AnswerBankController-Details[Get]结束")
           .HowLong(log)
           .Retry(log)
           .Do(() =>
           {
               bizAnswerBank = answerBankService.Single(text);
           });
            return new JsonResultPro(bizAnswerBank,JsonRequestBehavior.AllowGet, "yyyy-MM-dd");
        }
        #endregion
    }
}
