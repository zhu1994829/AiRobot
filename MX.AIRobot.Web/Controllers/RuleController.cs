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
using System.Text.RegularExpressions;

namespace MX.AIRobot.Web.Controllers
{
    public class RuleController : BaseController
    {
        #region Service
        RuleService ruleService = new RuleService();
        RecordService recordService = new RecordService();
        #endregion

        #region 列表

        [HttpGet]
        public ActionResult Index(BizRule bizRule, int page = 1)
        {
            Page<BizRule> ruleList = new Page<BizRule>();
            Page<ViewRule> viewlist = new Page<ViewRule>();
            AspectF.Define
             .Log(log, "RuleController-Index[post]开始", "RuleController-Index[post]结束")
             .HowLong(log)
             .Retry(log)
             .Do(() =>
             {
                 ruleList = ruleService.PageList(bizRule, PageHelper.PageSize, page);
                 ViewPageList(ruleList, viewlist);
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
            .Log(log, "RuleController-Delete[post]开始", "RuleController-Delete[post]结束")
            .HowLong(log)
            .Retry(log)
            .Do(() =>
            {
                var reader = new StreamReader(Request.InputStream).ReadToEnd();
                var model = JsonConvert.DeserializeObject<string[]>(reader);
                result = ruleService.Delete(model);
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
            ViewRule viewmodel = new ViewRule();

            AspectF.Define
               .Log(log, "RuleController-Edit[get]开始", "RuleController-Edit[get]结束")
               .HowLong(log)
               .Retry(log)
               .Do(() =>
               {
                   BizRule bizRule = new BizRule();
                   bizRule.RuleID = id;
                   bizRule = ruleService.Single(bizRule);
                   DBToViewForModel(bizRule, viewmodel);
                  
               });
            return new JsonResultPro(viewmodel, JsonRequestBehavior.AllowGet, "yyyy-MM-dd HH:mm:ss");
        }

        [HttpPost]
        public ActionResult Edit()
        {
            BizRule bizRule = new BizRule();
            bool result = false;
            AspectF.Define
                .Log(log, "RuleController-Edit[post]开始", "RuleController-Edit[post]结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    var reader = new StreamReader(Request.InputStream).ReadToEnd();
                    var model = JsonConvert.DeserializeObject<ViewRule>(reader);
                    ViewToDBForModel(bizRule, model);
                    if (string.IsNullOrEmpty(model.id))
                    {
                        result = ruleService.Add(bizRule);
                    }
                    else
                    {
                        bizRule.UpdateTime = DateTime.Now;
                        result = ruleService.Update(bizRule);
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

        #region 获取回复
        public ActionResult GetAnswer(BizRule bizRule,string text)
        {
            RuleResult ruleResult = new RuleResult();
            TuLingResult tuLingResult = new TuLingResult();
            BizRecord bizRecord = new BizRecord();
            List<BizRule> bizRules = new List<BizRule>();
            AspectF.Define
                .Log(log, "RuleController-QueryRule[post]开始", "RuleController-QueryRule[post]结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    text = RegularRemoveSign(text);
                    bizRules = ruleService.List(bizRule).ToList();
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    foreach (var item in bizRules)
                    {
                        //关键字一为Null
                        if (item.KeyWordOne == null)
                        {
                            if (text.Contains(item.KeyWordTwo))
                            {
                                text = text.Substring(0, text.IndexOf(item.KeyWordTwo) + item.KeyWordTwo.Length);
                                string keyWordTwo = item.KeyWordTwo;
                                text = text.Replace(keyWordTwo, "");
                                bizRule = ruleService.SingleTwo(keyWordTwo);
                            }
                        }
                        //关键字二为Null
                        else if (item.KeyWordTwo == null)
                        {
                            if (text.Contains(item.KeyWordOne))
                            {
                                text = text.Substring(text.IndexOf(item.KeyWordOne));
                                text = text.Replace(item.KeyWordOne, "");
                                bizRule = ruleService.SingleOne(item.KeyWordOne);
                            }
                        }
                        //关键字均不为空
                        else if (text.Contains(item.KeyWordOne))
                        {
                            if (text.Contains(item.KeyWordTwo))
                            {
                                text = text.Substring(text.IndexOf(item.KeyWordOne));
                                text = text.Substring(0,text.IndexOf(item.KeyWordTwo)+ item.KeyWordTwo.Length);
                                text = text.Replace(item.KeyWordOne, "");
                                text = text.Replace(item.KeyWordTwo, "");
                                keyValues.Add(item.KeyWordOne, item.KeyWordTwo);
                                bizRule = ruleService.Single(keyValues.Keys.SingleOrDefault(), keyValues.Values.SingleOrDefault());
                            }
                        }
                    }
                });
            if (bizRule.RuleID==null)
            {
                //图灵机器人回复
                ruleResult.text = ConnectTu(text).text;
                bizRecord.QuestionContent = text;
                recordService.Add(bizRecord);
            }
            else
            {
                ruleResult.text = bizRule.Answer;
                ruleResult.url = bizRule.Website;
                ruleResult.type = bizRule.AnswerTypes;
                ruleResult.keyword = text;
            }
            return new JsonResultPro(ruleResult, JsonRequestBehavior.AllowGet, "yyyy-MM-dd");
        }
        #endregion
    }
}
