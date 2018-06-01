using MX.AIRobot.ApiModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MX.AIRobot.Util
{
    /// <summary>
    /// 依赖于标库网http://api.tmkoo.com/的api帮助类
    /// </summary>
    public class TrademarkHelper
    {
        /// <summary>
        /// 标库网apikey
        /// </summary>
        public static string apiKey = "JINGYING_50591549";//4399320012393234

        /// <summary>
        /// 标库网apiPassword
        /// </summary>
        public static string apiPassword = "sJ9u4RTpn";//331nd3342d

        /// <summary>
        /// 给定关键字，查询商标名/注册号/申请人中含有该关键字的商标列表
        /// </summary>
        public static string searchUrl = "http://api.tmkoo.com/search.php";

        /// <summary>
        /// 给定申请人，查询商标列表
        /// </summary>
        public static string tmsearchUrl = "http://api.tmkoo.com/sqr-tm-list.php";

        /// <summary>
        /// 获取指定商标的详细信息
        /// </summary>
        public static string infoUrl = "http://api.tmkoo.com/info.php";

        /// <summary>
        /// 给定一个商标注册号，获得它的商标公告情况。
        /// </summary>
        public static string noticUrl = "http://api.tmkoo.com/tm-gonggao-list.php";

        /// <summary>
        /// 列表查询
        /// </summary>
        public static TrademarkRAResult ApiTrademarkList(TrademarkRACondition condition)
        {
            try
            {
                var param = ClassToHashtable<TrademarkRACondition>(condition);
                param.Add("apiKey", apiKey);
                param.Add("apiPassword", apiPassword);
                string jsonResult = HttpClientHelper.QueryGetWebService(searchUrl, param);
                var result = JsonHelper.JsonToObject<TrademarkRAResult>(jsonResult);
                result.Pages =
                    result.allRecords % condition.pageSize.Value == 0 ? result.allRecords / condition.pageSize.Value : (result.allRecords / condition.pageSize.Value) + 1;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 给定申请人名，获得该申请人名下的全部商标列表
        /// </summary>
        public static TrademarkRAResult ApiTrademarkList(string name)
        {
            try
            {
                var param = new Hashtable();
                param.Add("applicantCn", name);
                param.Add("apiKey", apiKey);
                param.Add("apiPassword", apiPassword);
                string jsonResult = HttpClientHelper.QueryGetWebService(tmsearchUrl, param);
                var result = JsonHelper.JsonToObject<TrademarkRAResult>(jsonResult);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 详情查询
        /// </summary>
        public static TrademarkRA ApiTrademarkInfo(TrademarkRACondition condition)
        {
            try
            {
                var param = ClassToHashtable<TrademarkRACondition>(condition, new string[] { "intCls", "regNo" });
                param.Add("apiKey", apiKey);
                param.Add("apiPassword", apiPassword);
                string jsonResult = HttpClientHelper.QueryGetWebService(infoUrl, param);
                return JsonHelper.JsonToObject<TrademarkRA>(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 商标公告查询
        /// </summary>D:\WorkSpace\EIPMP\EIPMP\Views\Error\
        /// <param name="regNo">商标注册号</param>
        /// <returns></returns>
        public static TrademarkNoticeResult ApiTrademarkNotice(string regNo)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("apiKey", apiKey);
                param.Add("apiPassword", apiPassword);
                param.Add("regNo", regNo);
                string jsonResult = HttpClientHelper.QueryGetWebService(noticUrl, param);
                return JsonHelper.JsonToObject<TrademarkNoticeResult>(jsonResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 类转Hashtable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Hashtable ClassToHashtable<T>(T t, params string[] showperty)
        {
            Hashtable param = new Hashtable();
            var type = t.GetType();
            foreach (var p in type.GetProperties())
            {
                if (showperty != null && showperty.Length > 0)
                {
                    if (showperty.Contains(p.Name))
                    {
                        param.Add(p.Name, p.GetValue(t));
                    }
                }
                else
                {
                    param.Add(p.Name, p.GetValue(t));
                }
            }
            return param;
        }

    }
}
