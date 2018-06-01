using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MX.AIRobot.ApiModel;
using System.Collections;

namespace MX.AIRobot.Util
{
    public class ApiWxyHelper
    {
        /// <summary>
        /// 万象云专利评估
        /// </summary>
        /// <param name="type">0-申请号，1-公开号</param>
        /// <param name="an">号码（多个；隔开）</param>
        /// <param name="baseType">0-基础评价，1-分级评价</param>
        /// <param name="languageType">0-中文，1-英文</param>
        /// <returns>评估结果集合</returns>
        public static List<ApiWxyModelEvaluation> GetEPatentScoreList(string type, string an, string baseType, string languageType)
        {
            try
            {
                var url = "http://118.190.80.110:8080/sinofaith/PatentInfo/getEPatentScoreList";
                Hashtable ht = new Hashtable { { "type", type }, { "an", an }, { "baseType", baseType }, { "languageType", languageType } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<List<ApiWxyModelEvaluation>>(content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
