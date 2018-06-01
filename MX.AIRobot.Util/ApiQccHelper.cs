using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MX.AIRobot.ApiModel;

namespace MX.AIRobot.Util
{
    /// <summary>
    /// 企查查接口api实现类，参照网址：http://www.yjapi.cn/
    /// </summary>
    public class ApiQccHelper
    {
        #region 公共变量
        /// <summary>
        /// 接口ApiKey
        /// </summary>
        protected static string key = "070ccd45a58948cea44036befe2a1046";
        #endregion

        #region 企业工商数据查询-精简版 http://www.yjapi.cn/DataApi/Api?apiCode=200

        #region 企业关键字模糊查询
        /// <summary>
        /// 企业关键字模糊查询
        /// </summary>
        /// <param name="keyword">公司名称、注册号或统一信用代码</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<ECISimpleSearch>> ECISimpleSearch(string keyword, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECISimple/Search";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "keyword", keyword }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<ECISimpleSearch>>>(content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 根据id获取照面信息
        /// <summary>
        /// 根据id获取照面信息
        /// </summary>
        /// <param name="keyno">公司内部关联主键KeyNo</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<ECISimpleDetails> ECISimpleGetDetails(string keyno, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECISimple/GetDetails";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "keyno", keyno }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<ECISimpleDetails>>(content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 企业关键字全名精确查询
        /// <summary>
        /// 企业关键字全名精确查询
        /// </summary>
        /// <param name="keyword">公司名称、注册号或统一信用代码</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<ECISimpleDetails> ECISimpleGetDetailsByName(string keyword, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECISimple/GetDetailsByName";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "keyword", keyword }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<ECISimpleDetails>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region 接口剩余次数查询
        /// <summary>
        /// 接口剩余次数查询
        /// </summary>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<PubBalance> ECISimpleBalance(bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECISimple/Balance";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<PubBalance>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #endregion

        #region 企业经营异常信息 http://www.yjapi.cn/DataApi/Api?apiCode=214

        #region 查询企业经营异常信息
        /// <summary>
        /// 查询企业经营异常信息
        /// </summary>
        /// <param name="keyNo">公司KeyNo</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<ECIException>> GetOpException(string keyNo, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECIException/GetOpException";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "keyNo", keyNo }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<ECIException>>>(content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 接口剩余次数查询
        /// <summary>
        /// 接口剩余次数查询
        /// </summary>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<PubBalance> ECIExceptionBalance(bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/ECIException/Balance";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<PubBalance>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #endregion

        #region 著作权软著查询 http://www.yjapi.cn/DataApi/Api?apiCode=233

        #region 著作权多重查询
        /// <summary>
        /// 著作权多重查询
        /// </summary>
        /// <param name="personName">作品著作权人(必填：否)</param>
        /// <param name="productName">作品名称(必填：否)</param>
        /// <param name="registeNo">登记号(必填：否)</param>
        /// <param name="pageSize">每页条数，默认10条，最大不超过50条(必填：否)</param>
        /// <param name="pageIndex">页码，默认第1页(必填：否)</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<CopyRight>> GetCopyRight(string registeNo = "", string personName = "", string productName = "", int pageSize = 10, int pageIndex = 1, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/CopyRight/GetCopyRight";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "personName", personName }, { "productName", productName }, { "registeNo", registeNo }, { "pageSize", pageSize }, { "pageIndex", pageIndex }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<CopyRight>>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region 软件著作权多重查询
        /// <summary>
        /// 软件著作权多重查询
        /// </summary>
        /// <param name="personName">软件著作权人(必填：否)</param>
        /// <param name="fullName">软件全称(必填：否)</param>
        /// <param name="shortName">软件简称(必填：否)</param>
        /// <param name="registeNo">登记号(必填：否)</param>
        /// <param name="pageSize">每页条数，默认10条，最大不超过50条(必填：否)</param>
        /// <param name="pageIndex">页码，默认第1页(必填：否)</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<SoftwareCr>> GetSoftwareCr(string registeNo = "", string personName = "", string fullName = "", string shortName = "", int pageSize = 10, int pageIndex = 1, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/CopyRight/GetSoftwareCr";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "personName", personName }, { "fullName", fullName }, { "shortName", shortName }, { "registeNo", registeNo }, { "pageSize", pageSize }, { "pageIndex", pageIndex }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<SoftwareCr>>>(content);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 著作权查询
        /// <summary>
        /// 著作权查询
        /// </summary>
        /// <param name="searchKey">关键字(公司名或keyNo)</param>
        /// <param name="pageSize">每页条数，默认10条，最大不超过50条</param>
        /// <param name="pageIndex">页码，默认第1页</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<CopyRight>> SearchCopyRight(string searchKey, int pageSize = 10, int pageIndex = 1, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/CopyRight/SearchCopyRight";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "searchKey", searchKey }, { "pageSize", pageSize }, { "pageIndex", pageIndex }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<CopyRight>>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region 软件著作权查询
        /// <summary>
        /// 软件著作权查询
        /// </summary>
        /// <param name="searchKey">关键字(公司名或keyNo)</param>
        /// <param name="pageSize">每页条数，默认10条，最大不超过50条</param>
        /// <param name="pageIndex">页码，默认第1页</param>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<List<SoftwareCr>> SearchSoftwareCr(string searchKey, int pageSize = 10, int pageIndex = 1, bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/CopyRight/SearchSoftwareCr";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "searchKey", searchKey }, { "pageSize", pageSize }, { "pageIndex", pageIndex }, { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<List<SoftwareCr>>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #region 接口剩余次数查询
        /// <summary>
        /// 接口剩余次数查询
        /// </summary>
        /// <param name="test">是否调用测试接口，默认false</param>
        /// <returns>属性status为200才算成功</returns>
        public static Root<PubBalance> CopyRightBalance(bool test = false)
        {
            try
            {
                var url = "http://{0}i.yjapi.com/CopyRight/Balance";
                url = string.Format(url, test ? "dev." : string.Empty);
                Hashtable ht = new Hashtable { { "key", key } };
                var content = HttpClientHelper.QueryGetWebService(url, ht);
                return JsonHelper.JsonToObject<Root<PubBalance>>(content);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        #endregion
    }
}
