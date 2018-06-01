using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MX.AIRobot.ApiModel;
using System.Text.RegularExpressions;
using System.Configuration;
using MX.AIRobot.Log;
using System.Net;
using System.IO;
using MX.AIRobot.Model;

namespace MX.AIRobot.Util
{
    /// <summary>
    /// 专利出版社数据接口类
    /// </summary>
    public class CheckPatentHelper
    {
        #region 公共关键参数
        /// <summary>
        /// 授权客户端编号
        /// </summary>
        private static string client_id = "8DF25E0C04B76951F5D869EBF03B7EFC";
        private static string client_secret = "7A6585978AA282B0376555227784F777";
        private static string redirect_uri = "http://www.hndky.com.cn";
        /// <summary>
        /// 用户统一标识（需平台授权后，开启第一步请求获取）
        /// </summary>
        private const string openid = "891599e03b848e597d941438f1c6b5b0";
        /// <summary>
        /// 令牌刷新
        /// </summary>
        private static string refresh_token = "3d6b6362bf3950d6fc7f2a5001b2a890";
        /// <summary>
        /// 令牌
        /// </summary>
        private static string access_token = GetToken();
        /// <summary>
        /// sf1，专利信息概览检索接口
        /// </summary>
        private static string patentInfoUrl = string.Format("http://open.cnipr.com/cnipr-api/rs/api/search/sf1/{0}", client_id);

        /// <summary>
        /// sf3，专利的法律状态概览检索接口
        /// </summary>
        private static string patentFlztUrl = string.Format("http://open.cnipr.com/cnipr-api/rs/api/search/sf3/{0}", client_id);
        #endregion

        #region 缓存token
        private static string token = string.Empty;
        #endregion

        #region 外部调用
        /// <summary>
        /// 检索专利法律状态
        /// </summary>
        /// <param name="number">专利申请号</param>
        /// <returns></returns>
        public static CniprFlztResult CheckPatentFlztSingle(string number)
        {
            var result = new CniprFlztResult();
            Hashtable ht = new Hashtable();     //请求参数
            ht.Add("exp", string.Format("申请号={0}", number));
            ht.Add("order", "申请日");
            ht.Add("option", "1");
            ht.Add("from", "0");
            ht.Add("to", "50");
            ht.Add("access_token", access_token);
            ht.Add("openid", openid);
            var doc = HttpClientHelper.QueryPostWebService(patentFlztUrl, ht);
            result = JsonHelper.JsonToObject<CniprFlztResult>(doc);
            if (result.status == "0")
            {
                var flztresult = result.results;
                for (var i = 0; i < flztresult.Count; i++)
                {
                    var first = flztresult[i];
                    for (var j = i + 1; j < flztresult.Count; j++)
                    {
                        var secd = flztresult[j];

                        if (DateTime.Compare(DateTime.Parse(secd.prsDate.Replace(".", "-")), DateTime.Parse(first.prsDate.Replace(".", "-"))) > 0)
                        {
                            var temp = flztresult[j];
                            flztresult[j] = flztresult[i];
                            flztresult[i] = temp;
                        }
                    }
                }
                result.results = flztresult;
            }
            return result;
        }

        /// <summary>
        /// 专利信息概览模糊检索(分页)
        /// </summary>
        public static CniprResult CheckPatentInfoLike(Condition condition, int count = 1, int size = 10)
        {
            CniprResult results = null;
            try
            {
                if (!string.IsNullOrEmpty(condition.topCondition))
                {
                    Hashtable ht = new Hashtable();     //请求参数

                    ht.Add("exp", ConditionAnalysis(condition));
                    if (condition.ptDb != null)
                    {
                        ht.Add("dbs", string.Join(",", condition.ptDb));
                    }
                    else
                    {
                        ht.Add("dbs", "FMZL,FMSQ,WGZL,SYXX");
                    }

                    if (!string.IsNullOrEmpty(condition.orderBy))
                    {
                        ht.Add("order", condition.orderBy);

                    }
                    else
                    {
                        ht.Add("order", "RELEVANCE");
                    }
                    ht.Add("option", "1");
                    ht.Add("from", (count - 1) * size);
                    ht.Add("to", size * count);
                    ht.Add("access_token", access_token);
                    ht.Add("openid", openid);
                    var doc = HttpClientHelper.QueryPostWebService(patentInfoUrl, ht);
                    results = JsonHelper.JsonToObject<CniprResult>(doc);
                    if (results.status == "0")
                    {
                        results.Pages = results.total % size == 0 ? results.total / size : (results.total / size) + 1;
                    }
                    else
                    {
                        results = new CniprResult { results = new List<Result>(), sectionInfos = new List<SectionInfo>() };
                    }
                }
                else
                {
                    results = new CniprResult { results = new List<Result>(), sectionInfos = new List<SectionInfo>() };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return results;
        }

        /// <summary>
        /// 专利信息概览模糊检索(通过专利号检索)
        /// </summary>
        public static CniprResult CheckPatentSingle(string number)
        {
            CniprResult result = null;
            try
            {
                Hashtable ht = new Hashtable();     //请求参数
                ht.Add("exp", string.Format("申请号=CN{0}", number));
                ht.Add("dbs", "FMZL,FMSQ,WGZL,SYXX");
                ht.Add("option", "1");
                ht.Add("from", 0);
                ht.Add("to", 1);
                ht.Add("access_token", access_token);
                ht.Add("openid", openid);
                var doc = HttpClientHelper.QueryPostWebService(patentInfoUrl, ht);
                result = JsonHelper.JsonToObject<CniprResult>(doc);
                if (result.status == "0")
                {
                    result.results[0].flztResults = CheckPatentFlztSingle(result.results[0].appNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 专利信息概览模糊检索(通过专利号检索)
        /// </summary>
        public static CniprResult CheckPatentSingleForNum(string number, string type)
        {
            CniprResult result = null;
            try
            {
                Hashtable ht = new Hashtable();     //请求参数
                if (type == "1")
                {
                    ht.Add("exp", string.Format("申请号=CN{0}", number));
                }
                else
                {
                    ht.Add("exp", string.Format("公开（公告）号={0}", number));
                }
                ht.Add("dbs", "FMZL,FMSQ,WGZL,SYXX");
                ht.Add("option", "1");
                ht.Add("from", 0);
                ht.Add("to", 1);
                ht.Add("access_token", access_token);
                ht.Add("openid", openid);
                var doc = HttpClientHelper.QueryPostWebService(patentInfoUrl, ht);
                result = JsonHelper.JsonToObject<CniprResult>(doc);
                if (result.status == "0")
                {
                    result.results[0].flztResults = CheckPatentFlztSingle(result.results[0].appNumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 根据权利人检索专利（分页）
        /// </summary>
        /// <param name="app">权利人</param>
        /// <returns></returns>
        public static CniprResult CheckPatentListByApplication(string app, int count = 1, int size = 10)
        {
            CniprResult results = null;
            try
            {
                if (!string.IsNullOrEmpty(app))
                {
                    Hashtable ht = new Hashtable();     //请求参数
                    ht.Add("exp", "申请（专利权）人=" + app);
                    ht.Add("dbs", "FMZL,FMSQ,WGZL,SYXX");
                    ht.Add("option", "1");
                    ht.Add("from", (count - 1) * size);
                    ht.Add("to", size * count);
                    ht.Add("access_token", access_token);
                    ht.Add("openid", openid);
                    var doc = HttpClientHelper.QueryPostWebService(patentInfoUrl, ht);
                    results = JsonHelper.JsonToObject<CniprResult>(doc);
                    if (results.status == "0")
                    {
                        results.Pages = results.total % size == 0 ? results.total / size : (results.total / size) + 1;
                    }
                    else
                    {
                        results = new CniprResult { results = new List<Result>(), sectionInfos = new List<SectionInfo>() };
                    }
                }
                else
                {
                    results = new CniprResult { results = new List<Result>(), sectionInfos = new List<SectionInfo>() };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return results;
        }

        public static byte[] GetPatentPDFStream(string pid)
        {
            string url = string.Format("http://open.cnipr.com/cnipr-api/rs/api/fulltext/ft1/{0}?openid={1}&access_token={2}&pid={3}", client_id, openid, access_token, pid);
            return HttpClientHelper.GetResponseStream(url);
        }
        #endregion

        #region 内部方法
        public static string GetToken()
        {
            var validate = false;
            if (!string.IsNullOrEmpty(token))
            {
                var patent = new BizPatent { SearchCondition = "申请号=CN200710092420.7", CniprDbs = "FMZL" };
                Hashtable ht = new Hashtable();
                ht.Add("exp", patent.SearchCondition);
                ht.Add("dbs", patent.CniprDbs);
                ht.Add("order", "申请日");
                ht.Add("option", "1");
                ht.Add("from", "0");
                ht.Add("to", "5");
                ht.Add("access_token", token);
                ht.Add("openid", openid);
                var doc = HttpClientHelper.QueryPostWebService(string.Format("http://open.cnipr.com/cnipr-api/rs/api/search/sf1/{0}", client_id), ht);
                Dictionary<string, object> results = JsonHelper.DataRowFromJson(doc);
                if (results["message"].ToString().ToUpper() != "SUCCESS")
                {
                    validate = true;
                }
            }
            else
            {
                validate = true;
            }

            if (validate)
            {
                var content = HttpClientHelper.QueryGetWebService("http://www.easy-ip.com.cn:8770/api/GetPatentToken", new Hashtable { });
                var result = JsonHelper.JsonToObject<object>(content.ToString());
                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    token = result.ToString();
                }
                else
                {
                    var log = new Logger();
                    log.WriteLog(MX.AIRobot.Interface.LogLevel.Error, new Exception("获取Token出现错误"));
                    throw new Exception("出现错误");
                }
            }

            //return "f50cb51f5c4435b607a88fda5485cbe7";
            return token;

        }

        /// <summary>
        /// 条件分析
        /// </summary>
        /// <returns>检索表达式</returns>
        protected static string ConditionAnalysis(Condition conditionObj)
        {
            var inCondition = Regex.Replace(conditionObj.topCondition.Trim().Replace(" ", "+"), "[+]+", "+");
            conditionObj.topCondition = inCondition.Replace("+", " ");
            var conditions = inCondition.Split('+');

            Dictionary<int, string> dic = new Dictionary<int, string>();
            foreach (var item in conditions)
            {
                var condition = item;
                if (!string.IsNullOrEmpty(Regex.Match(condition.ToUpper(), "CN[0-9]+").Value) || !string.IsNullOrEmpty(Regex.Match(condition.ToUpper(), "ZL[0-9]+").Value) || !string.IsNullOrEmpty(Regex.Match(condition.ToUpper(), "^[0-9]+.[0-9X]+$").Value))
                {
                    SetKeyCode(dic, Regex.Match(condition.ToUpper(), "[0-9]+.[0-9X]+").Value, 2);
                }
                else
                {
                    SetKeyCode(dic, condition, 3);
                }
            }

            var andList = new List<string>();
            string codeStr = string.Join(" or ", dic.Where(w => w.Key % 2 == 0).Select(s => "'%" + s.Value + "%'"));
            string nameStr = string.Join(" or ", dic.Where(w => w.Key % 3 == 0).Select(s => "'%" + s.Value + "%'"));

            if (!string.IsNullOrEmpty(codeStr))
            {
                andList.Add(string.Format("申请号,公开（公告）号+=({0})", codeStr));
            }

            if (!string.IsNullOrEmpty(nameStr))
            {
                andList.Add(string.Format("名称,申请（专利权）人,发明（设计）人,优先权号,专利代理机构,代理人,地址,申请国代码,国省代码,摘要,主权项,分类号 +=({0})", nameStr));
            }

            if (!string.IsNullOrEmpty(conditionObj.lprState))
            {
                andList.Add(string.Format("专利权状态 =('%{0}%')", conditionObj.lprState));
            }


            return string.Join(" and ", andList);
        }

        /// <summary>
        /// 检索词条设置
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="code"></param>
        /// <param name="type">2 专利号检索 3 名词检索</param>
        /// <returns></returns>
        public static Dictionary<int, string> SetKeyCode(Dictionary<int, string> dic, string code, int type)
        {
            var key = new StringBuilder();
            var list = dic.Select(s => s.Key).Where(w => w % type == 0).ToList();
            for (int i = 0; i < list.Count + 1; i++)
            {
                key.Append(type.ToString());
            }
            dic.Add(Convert.ToInt32(key.ToString()), code);
            return dic;
        }
        #endregion
    }
}
