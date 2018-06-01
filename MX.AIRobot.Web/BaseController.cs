using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MX.AIRobot.AOP;
using MX.AIRobot.ApiModel;
using MX.AIRobot.Interface;
using MX.AIRobot.Log;
using MX.AIRobot.Model;
using MX.AIRobot.Service;
using MX.AIRobot.Util;
using PetaPoco;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace MX.AIRobot.Web
{
    public class BaseController : Controller
    {
        protected ILogger log = new Logger();

        public class JsonResultPro : JsonResult
        {
            public JsonResultPro() { }
            /// <summary>
            /// JsonResult拓展类（针对日期类型序列化）
            /// </summary>
            /// <param name="data">实体模型</param>
            /// <param name="behavior">是否允许客户端get请求</param>
            /// <param name="dateTimeFormat">日期序列化字符串</param>
            public JsonResultPro(object data, JsonRequestBehavior behavior, String dateTimeFormat = "yyyy-MM-dd")
            {
                base.Data = data;
                base.JsonRequestBehavior = behavior;
                this.DateTimeFormat = dateTimeFormat;
            }

            public string DateTimeFormat { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("MvcResources.JsonRequest_GetNotAllowed");
                }
                HttpResponseBase mybase = context.HttpContext.Response;
                if (!string.IsNullOrEmpty(this.ContentType))
                {
                    mybase.ContentType = this.ContentType;
                }
                else
                {
                    mybase.ContentType = "application/json";
                }
                if (this.ContentEncoding != null)
                {
                    mybase.ContentEncoding = this.ContentEncoding;
                }
                if (this.Data != null)
                {
                    //转换System.DateTime的日期格式到 ISO 8601日期格式
                    //ISO 8601 (如2008-04-12T12:53Z)
                    IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
                    //设置日期格式
                    isoDateTimeConverter.DateTimeFormat = DateTimeFormat;
                    //序列化
                    String jsonResult = JsonConvert.SerializeObject(this.Data, isoDateTimeConverter);
                    //相应结果
                    mybase.Write(jsonResult);
                }

            }
        }

        #region dbmodel转viewmodel
        /// <summary>
        /// page对象dbmodel转成viewmodel
        /// </summary>
        /// <typeparam name="T1">dbmodel</typeparam>
        /// <typeparam name="T2">viewmodel</typeparam>
        /// <param name="page">dbmodel的page泛型</param>
        /// <param name="pageResult">viewmodel的page泛型</param>
        public void ViewPageList<T1, T2>(Page<T1> page, Page<T2> pageResult) where T2 : new()  
        {
            pageResult.CurrentPage = page.CurrentPage;
            pageResult.TotalItems = page.TotalItems;
            pageResult.TotalPages = page.TotalPages;
            pageResult.ItemsPerPage = page.ItemsPerPage;
            if (pageResult.Items == null)
            {
                pageResult.Items = new List<T2>();
            }
            ViewList(page.Items, pageResult.Items);
        }

        /// <summary>
        /// List对象dbmodel转成viewmodel
        /// </summary>
        /// <typeparam name="T1">dbmodel</typeparam>
        /// <typeparam name="T2">viewmodel</typeparam>
        /// <param name="list">dbmodel的list泛型</param>
        /// <param name="listResult">viewmodel的list泛型</param>
        public void ViewList<T1, T2>(List<T1> list, List<T2> listResult) where T2 : new()  
        {
            
            for (var i = 0; i < list.Count; i++)
            {
                T2 model = new T2();
                DBToViewForModel(list[i], model);
                listResult.Add(model);
            }
        }

        /// <summary>
        /// 普通对象dbmodel转成viewmodel
        /// </summary>
        /// <typeparam name="T1">dbmodel</typeparam>
        /// <typeparam name="T2">viewmodel</typeparam>
        /// <param name="model">dbmodel对象</param>
        /// <param name="modelResult">viewmodel对象</param>
        public void DBToViewForModel<T1, T2>(T1 model, T2 modelResult)
        {
            if (modelResult != null)
            {
                Type type = modelResult.GetType();
                string attr = string.Empty;

                foreach (var p in type.GetProperties())
                {
                    foreach (var a in p.CustomAttributes)
                    {
                        if (a.AttributeType.Name == "DBColumn")
                        {
                            var colname = a.ConstructorArguments[0].Value.ToString();
                            var temp = model.GetType().GetProperties().Where(w => w.Name == colname).FirstOrDefault();
                            var val = temp.GetValue(model);
                            p.SetValue(modelResult, val);
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region viewmodel转成dbmodel
        /// <summary>
        /// viewmodel转成dbmodel
        /// </summary>
        /// <typeparam name="T1">dbmodel</typeparam>
        /// <typeparam name="T2">viewmodel</typeparam>
        /// <param name="model">dbmodel对象</param>
        /// <param name="modelResult">viewmodel对象</param>
        public void ViewToDBForModel<T1, T2>(T1 model, T2 modelResult)
        {
            if (modelResult != null)
            {
                Type type = modelResult.GetType();
                string attr = string.Empty;

                foreach (var p in type.GetProperties())
                {
                    foreach (var a in p.CustomAttributes)
                    {
                        if (a.AttributeType.Name == "DBColumn")
                        {
                            var colname = a.ConstructorArguments[0].Value.ToString();
                            var temp = model.GetType().GetProperties().Where(w => w.Name == colname).FirstOrDefault();
                            var val = p.GetValue(modelResult);
                            temp.SetValue(model, val);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 正则去标点符号
        /// </summary>
        /// <param name="text">要除去标点的字符串</param>
        /// <returns>无标点符号的字符串</returns>
        public string RegularRemoveSign(string text)
        {
            string Unsignedtext = Regex.Replace(text, @"\W+", "");
            return Unsignedtext;
        }

        /// <summary>
        /// 图灵机器人识别接口
        /// </summary>
        /// <param name="ans">与图灵对话的字符串</param>
        /// <returns>图灵回复</returns>
        public static TuLingResult ConnectTu(string ans)
        {
            TuLingResult tuLingResult = new TuLingResult();
            string res = string.Empty;
            string APIKEY = "32b17e3aa352423dbb6dfbc93b90f657";
            if (!string.IsNullOrEmpty(ans))
            {
                WebRequest wq = WebRequest.Create("http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + ans);
                WebResponse wp = wq.GetResponse();
                res = new StreamReader(wp.GetResponseStream()).ReadToEnd();
                tuLingResult = JsonConvert.DeserializeObject<TuLingResult>(res);
            }
            return tuLingResult;
        }
    }
}
