using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;

namespace MX.AIRobot.Util
{
    public class JsonHelper
    {

        /// <summary> 
        /// 对象转Json 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>Json格式的字符串</returns> 
        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("JsonHelper.ObjectToJson(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 数据表转键值对集合 
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
            = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }
        /// <summary> 
        /// 数据集转键值对数组字典 
        /// </summary> 
        /// <param name="dataSet">数据集</param> 
        /// <returns>键值对数组字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));
            return result;
        }
        /// <summary> 
        /// 数据表转Json 
        /// </summary> 
        /// <param name="dataTable">数据表</param> 
        /// <returns>Json字符串</returns> 
        public static string DataTableToJson(DataTable dt)
        {
            return ObjectToJson(DataTableToList(dt));
        }
        /// <summary> 
        /// Json文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">Json文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JsonToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JsonHelper.JsonToObject(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 将Json文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">Json文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJson(string jsonText)
        {
            return JsonToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }
        /// <summary> 
        /// 将Json文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">Json文本</param> 
        /// <returns>数据行的字典</returns> 
        public static Dictionary<string, object> DataRowFromJson(string jsonText)
        {
            return JsonToObject<Dictionary<string, object>>(jsonText);
        }
        
        /// <summary>
        /// Json to List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string jsonString)
        {
            return JsonToObject<List<T>>(jsonString);
        }
    }
}
