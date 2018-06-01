using MX.AIRobot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MX.AIRobot.ApiModel
{
    //api返回的json实体类

    #region 企查查api实体类

    #region 公共类
    public abstract class _Root
    {
        /// <summary>
        /// 查询状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 查询消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    public class Page
    {
        /// <summary>
        /// 每页条数，默认10条，最大不超过50条
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 页码，默认第1页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalRecords { get; set; }
    }

    /// <summary>
    /// 结果类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Root<T> : _Root
    {
        /// <summary>
        /// 结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public Page Paging { get; set; }
    }
    #endregion

    #region 企业工商数据查询-精简版

    #region 企业关键字模糊查询
    public class ECISimpleSearch
    {
        /// <summary>
        /// 公司内部关联主键
        /// </summary>
        public string KeyNo { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 法定代表人
        /// </summary>
        public string OperName { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 登记状态（存续、在业、注销、迁入、吊销、迁出、停业、清算）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 注册号或统一社会信用代码，默认统一社会信用代码
        /// </summary>
        public string No { get; set; }
    }
    #endregion

    #region 根据id获取照面信息/企业关键字全名精确查询
    public class ECISimpleDetails
    {
        /// <summary>
        /// 公司内部关联主键
        /// </summary>
        public string KeyNo { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 注册号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 登记机关
        /// </summary>
        public string BelongOrg { get; set; }

        /// <summary>
        /// 法定代表人
        /// </summary>
        public string OperName { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 注销/吊销日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 登记状态（存续、在业、注销、迁入、吊销、迁出、停业、清算）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 所在省份缩写
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 记录更新时间
        /// </summary>
        public string UpdatedDate { get; set; }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string CreditCode { get; set; }

        /// <summary>
        /// 注册资本
        /// </summary>
        public string RegistCapi { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string EconKind { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 营业期限自
        /// </summary>
        public DateTime? TermStart { get; set; }

        /// <summary>
        /// 营业期限至
        /// </summary>
        public DateTime? TeamEnd { get; set; }

        /// <summary>
        /// 核准日期
        /// </summary>
        public DateTime? CheckDate { get; set; }
    }
    #endregion

    #endregion

    #region 企业经营异常信息

    #region 查询企业经营异常信息
    public class ECIException
    {
        /// <summary>
        /// 列入经营异常名录原因
        /// </summary>
        public string AddReason { get; set; }

        /// <summary>
        /// 列入日期
        /// </summary>
        public DateTime? AddDate { get; set; }

        /// <summary>
        /// 移出经营异常名录原因
        /// </summary>
        public string RomoveReason { get; set; }

        /// <summary>
        /// 移出日期
        /// </summary>
        public DateTime? RemoveDate { get; set; }

        /// <summary>
        /// 作出决定机关
        /// </summary>
        public string DecisionOffice { get; set; }

        /// <summary>
        /// 移除决定机关
        /// </summary>
        public string RemoveDecisionOffice { get; set; }
    }
    #endregion

    #endregion

    #region 著作权软著查询

    #region 作品著作权
    public class CopyRight
    {
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 作品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作品著作权人
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 登记号
        /// </summary>
        public string RegisterNo { get; set; }

        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime? RegisterDate { get; set; }

        /// <summary>
        /// 创作完成日期
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 首次发布日期
        /// </summary>
        public DateTime? PublishDate { get; set; }
    }
    #endregion

    #region 软件著作权
    public class SoftwareCr
    {
        /// <summary>
        /// 分类号
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 软件全称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 软件著作权人
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 登记号
        /// </summary>
        public string RegisterNo { get; set; }

        /// <summary>
        /// 软件简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// 登记批准日期
        /// </summary>
        public DateTime? RegisterAperDate { get; set; }
    }
    #endregion

    #endregion

    #region 接口剩余次数查询(通用)
    public class PubBalance
    {
        /// <summary>
        /// 购买类型，包月或计次
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 剩余情况，如果包月显示过期日期，如果计次显示剩余次数
        /// </summary>
        public int Balance { get; set; }
    }
    #endregion

    #endregion

    #region 专利接口api实体类
    /// <summary>
    /// 专利信息模型
    /// </summary>
    public class CniprResult
    {
        /// <summary>
        /// 状态码，0代表成功
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 响应的信息描述
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 检索结果总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 分段检索，开始值
        /// </summary>
        public int from { get; set; }

        /// <summary>
        /// 分段检索，结束值
        /// </summary>
        public int to { get; set; }

        /// <summary>
        /// 命中记录分布的数据库信息。两个字段：sectionName数据库名称和recordNum命中数量
        /// </summary>
        public List<SectionInfo> sectionInfos { get; set; }

        /// <summary>
        /// 检索结果集
        /// </summary>
        public List<Result> results { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public int Pages { get; set; }
    }

    /// <summary>
    /// 专利法律状态模型
    /// </summary>
    public class CniprFlztResult
    {
        /// <summary>
        /// 状态码，0代表成功
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 响应的信息描述
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 检索结果总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 分段检索，开始值
        /// </summary>
        public int from { get; set; }

        /// <summary>
        /// 分段检索，结束值
        /// </summary>
        public int to { get; set; }

        /// <summary>
        /// 命中记录分布的数据库信息。两个字段：sectionName数据库名称和recordNum命中数量
        /// </summary>
        public List<SectionInfo> sectionInfos { get; set; }

        /// <summary>
        /// 检索结果集
        /// </summary>
        public List<FlztResult> results { get; set; }

    }

    /// <summary>
    /// 检索数据库信息
    /// </summary>
    public class SectionInfo
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string sectionName { get; set; }

        /// <summary>
        /// 命中数量
        /// </summary>
        public string recordNum { get; set; }
    }

    /// <summary>
    /// 专利信息结果集
    /// </summary>
    public class Result
    {
        #region 接口属性
        /// <summary>
        /// 专利信息id
        /// </summary>
        public string pid { get; set; }

        /// <summary>
        /// sysid
        /// </summary>
        public string sysid { get; set; }

        /// <summary>
        /// 申请号
        /// </summary>
        public string appNumber { get; set; }

        /// <summary>
        /// 公开（公告）号
        /// </summary>
        public string pubNumber { get; set; }

        /// <summary>
        /// 申请日
        /// </summary>
        public DateTime? appDate { get; set; }

        /// <summary>
        /// 公开（公告）日
        /// </summary>
        public DateTime? pubDate { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 优先权
        /// </summary>
        public List<string> priority { get; set; }

        /// <summary>
        /// 专利代理机构
        /// </summary>
        public string agencyName { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string addrProvince { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string addrCity { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public string addrCounty { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 专利类型
        /// </summary>
        public int patType { get; set; }

        /// <summary>
        /// 国际申请
        /// </summary>
        public string iapp { get; set; }

        /// <summary>
        /// 国际公布
        /// </summary>
        public string ipub { get; set; }

        /// <summary>
        /// 进入国家日期
        /// </summary>
        public string den { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string abs { get; set; }

        /// <summary>
        /// 最新法律状态
        /// </summary>
        public string lprs { get; set; }

        /// <summary>
        /// 摘要附图存储路径
        /// </summary>
        public string draws { get; set; }

        /// <summary>
        /// 专利所属库名
        /// </summary>
        public string dbName { get; set; }

        /// <summary>
        /// 发布路径
        /// </summary>
        public string tifDistributePath { get; set; }

        /// <summary>
        /// 页数（对应tifDistributePath的图片张数）
        /// </summary>
        public int pages { get; set; }

        /// <summary>
        /// 相似度
        /// </summary>
        public string relevance { get; set; }

        /// <summary>
        /// 国省代码
        /// </summary>
        public string proCode { get; set; }

        /// <summary>
        /// 申请国代码
        /// </summary>
        public string appCoun { get; set; }

        /// <summary>
        /// 公报发布路径
        /// </summary>
        public string gazettePath { get; set; }

        /// <summary>
        /// 公报所在页（起始页）
        /// </summary>
        public string gazettePage { get; set; }

        /// <summary>
        /// 公报翻页信息 ：0代表只有一页，其他数字代表可再翻的页数（即总页数=gazetteCount+1）
        /// </summary>
        public string gazetteCount { get; set; }

        /// <summary>
        /// 专利状态码 ：10（有效），20（失效），21（专利权届满的专利），30（在审）
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// 同族号
        /// </summary>
        public string familyNo { get; set; }

        /// <summary>
        /// 分类号
        /// </summary>
        public List<string> ipc { get; set; }

        /// <summary>
        /// 申请（专利权）人
        /// </summary>
        public List<string> applicantName { get; set; }

        /// <summary>
        /// 发明（设计）人
        /// </summary>
        public List<string> inventroName { get; set; }

        /// <summary>
        /// 代理人
        /// </summary>
        public List<string> agentName { get; set; }

        /// <summary>
        /// 法律状态信息
        /// </summary>
        public CniprFlztResult flztResults { get; set; }

        /// <summary>
        /// 主权项
        /// </summary>
        public string cl { get; set; }

        /// <summary>
        /// 颁证日
        /// </summary>
        public string issueDate { get; set; }
        #endregion

        #region 自定义属性
        /// <summary>
        /// 对应的专利图片集（地址）
        /// </summary>
        public List<string> pics
        {
            get
            {
                var isWG = patType == 3 ? true : false;

                List<string> list = new List<string>();

                for (int i = 0; i < pages; i++)
                {
                    list.Add("http://pic.cnipr.com:8080/" + tifDistributePath.Replace("BOOKS", (isWG ? "BOOKS" : "ThumbnailImage")) + "/" + (i + 1).ToString().PadLeft(6, '0') + (isWG ? ".jpg" : ".gif"));
                }

                return list;
            }
        }

       
        /// <summary>
        /// 专利类型
        /// </summary>
        //public string PatentType
        //{
        //    get
        //    {
        //        return ConstClass.PatentTypeDic[this.patType];
        //    }
        //}

        /// <summary>
        /// 优先权号
        /// </summary>
        public string PriorityCode
        {
            get { if (priority != null && priority.Count > 0) { return string.Join(",", priority); } else { return string.Empty; } }
        }

        /// <summary>
        /// 发明人（字符串）
        /// </summary>
        public string inventroNameStr
        {
            get { if (inventroName != null && inventroName.Count > 0) { return string.Join(",", inventroName); } else { return string.Empty; } }
        }

        /// <summary>
        /// 权利人（字符串）
        /// </summary>
        public string applicantNameStr
        {
            get { if (applicantName != null && applicantName.Count > 0) { return string.Join(",", applicantName); } else { return string.Empty; } }
        }

        /// <summary>
        /// 分类号（字符串）
        /// </summary>
        public string ipcStr
        {
            get { if (ipc != null && ipc.Count > 0) { return string.Join(",", ipc); } else { return string.Empty; } }
        }
        #endregion
    }

    /// <summary>
    /// 专利法律状态结果集
    /// </summary>
    public class FlztResult
    {
        /// <summary>
        ///  法律状态id，申请号+@+法律状态公告日
        /// </summary>
        public string prsId { get; set; }

        /// <summary>
        ///   申请号
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        ///   法律状态
        /// </summary>
        public string prsCode { get; set; }

        /// <summary>
        ///   法律状态公告日
        /// </summary>
        public string prsDate { get; set; }

        /// <summary>
        ///   法律状态申请号
        /// </summary>
        public string prsNumber { get; set; }

        /// <summary>
        ///   法律状态信息
        /// </summary>
        public string codeExpl { get; set; }

    }

    /// <summary>
    /// 检索条件
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// 主要条件
        /// </summary>
        public string topCondition { get; set; }

        /// <summary>
        /// 专利数据库
        /// </summary>
        public List<string> ptDb { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string orderBy { get; set; }

        /// <summary>
        /// 法律状态字段
        /// </summary>
        public string lprState { get; set; }

    }
    #endregion

    #region 商标接口api实体类
    #region 商标检索模型
    /// <summary>
    /// 商标查询结果模型
    /// </summary>
    public class TrademarkRAResult
    {
        /// <summary>
        /// 返回码。0-成功  1-失败
        /// </summary>
        public int ret { get; set; }

        /// <summary>
        /// 如果ret=1，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 本天内，本接口的剩余允许调用次数
        /// </summary>
        public int remainCount { get; set; }

        /// <summary>
        /// 本接口调用后，共查询出多少条记录
        /// </summary>
        public int allRecords { get; set; }

        /// <summary>
        /// 商标列表
        /// </summary>
        public List<TrademarkRA> results { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Pages { get; set; }
    }

    /// <summary>
    /// 商标详细模型
    /// </summary>
    public class TrademarkRA
    {
        #region 接口属性
        /// <summary>
        /// 返回码。0-成功  1-失败
        /// </summary>
        public int ret { get; set; }

        /// <summary>
        /// 如果ret=1，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 本天内，本接口的剩余允许调用次数
        /// </summary>
        public int remainCount { get; set; }

        /// <summary>
        /// 商标图片名
        /// </summary>
        public string tmImg { get; set; }

        /// <summary>
        /// 商标小图(50*50)
        /// </summary>
        public string tmImgSmall { get { return "http://tmpic.tmkoo.com/" + this.tmImg + "-s"; } set { this.tmImgSmall = value; } }

        /// <summary>
        /// 商标中图(100*100)
        /// </summary>
        public string tmImgMiddle { get { return "http://tmpic.tmkoo.com/" + this.tmImg + "-m"; } set { this.tmImgMiddle = value; } }

        /// <summary>
        /// 商标大图
        /// </summary>
        public string tmImgBig { get { return "http://tmpic.tmkoo.com/" + this.tmImg; } set { this.tmImgBig = value; } }

        /// <summary>
        /// 注册号
        /// </summary>
        public string regNo { get; set; }

        /// <summary>
        /// 国际分类
        /// </summary>
        public string intCls { get; set; }

        /// <summary>
        /// 商标名
        /// </summary>
        public string tmName { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime? appDate { get; set; }

        /// <summary>
        /// 申请人中文
        /// </summary>
        public string applicantCn { get; set; }

        /// <summary>
        /// 申请人身份证号。如果是公司，此属性为空串，如果是个人，一般是身份证号，也有可能空串（此种情形是因为商标局没有录入身份证号）。
        /// </summary>
        public string idCardNo { get; set; }

        /// <summary>
        /// 申请人地址中文
        /// </summary>
        public string addressCn { get; set; }

        /// <summary>
        /// 共有申请人1---只要有内容，就说明此商标是共有商标
        /// </summary>
        public string applicantOther1 { get; set; }

        /// <summary>
        /// 共有申请人2---只要有内容，就说明此商标是共有商标
        /// </summary>
        public string applicantOther2 { get; set; }

        /// <summary>
        /// 申请人英文
        /// </summary>
        public string applicantEn { get; set; }

        /// <summary>
        /// 申请人地址英文
        /// </summary>
        public string addressEn { get; set; }

        /// <summary>
        /// 代理公司
        /// </summary>
        public string agent { get; set; }

        /// <summary>
        /// 初审公告期号
        /// </summary>
        public string announcementIssue { get; set; }

        /// <summary>
        /// 初审公告日期
        /// </summary>
        public DateTime? announcementDate { get; set; }

        /// <summary>
        /// 注册公告期号
        /// </summary>
        public string regIssue { get; set; }

        /// <summary>
        /// 注册公告日期
        /// </summary>
        public DateTime? regDate { get; set; }

        /// <summary>
        /// 专用权期限，它是一个时间范围，如：2001-02-12至2011-02-11
        /// </summary>
        public string privateDate { get; set; }

        /// <summary>
        /// 商标类型，一般、特殊、集体、证明
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 后期指定日期
        /// </summary>
        public DateTime? hqzdrq { get; set; }

        /// <summary>
        /// 国际注册日期
        /// </summary>
        public DateTime? gjzcrq { get; set; }

        /// <summary>
        /// 优先权日期
        /// </summary>
        public DateTime? yxqrq { get; set; }

        /// <summary>
        /// 指定颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 使用商品。它是列表对象，里面包含商品/服务信息，具体信息格式见下面示例
        /// </summary>
        public List<TrademarkGood> goods { get; set; }

        /// <summary>
        /// 商标流程。它是列表对象，里面包含状态信息，具体信息格式见下面示例。
        /// </summary>
        public List<TrademarkFlow> flow { get; set; }

        /// <summary>
        /// 商标公告结果
        /// </summary>
        public TrademarkNoticeResult noticeResult { get; set; }

        /// <summary>
        /// 当前流程状态
        /// </summary>
        public string currentStatus { get; set; }

     /// <summary>
    /// 请求状态
        /// </summary>
        public string state { get; set; }
        #endregion

        #region 自定义拓展属性



        /// <summary>
        /// 专用权期限开始
        /// </summary>
       public DateTime? RegistStartTime
        {
            get
            {
                if (!string.IsNullOrEmpty(privateDate) && !string.IsNullOrEmpty(privateDate.Split('至')[0]))
                {
                    return DateTime.Parse(privateDate.Split('至')[0]);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 专用权期限结束
        /// </summary>
        public DateTime? RegistEndTime
        {
            get
            {
                if (!string.IsNullOrEmpty(privateDate) && !string.IsNullOrEmpty(privateDate.Split('至')[1]))
                {
                    return DateTime.Parse(privateDate.Split('至')[1]);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 商品字符串
        /// </summary>
        public string GoodStrs
        {
            get
            {
                if (goods != null && goods.Count > 0)
                {
                    return string.Join("", goods.Select(s => s.goodsCode + "-" + s.goodsName + "\r\n"));
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 商标商品
    /// </summary>
    public class TrademarkGood
    {
        /// <summary>
        /// 群组号
        /// </summary>
        public string goodsCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string goodsName { get; set; }

        /// <summary>
        /// 是否被删除商品
        /// </summary>
        public bool beDeleted { get; set; }
    }

    /// <summary>
    /// 商标流程
    /// </summary>
    public class TrademarkFlow
    {
        /// <summary>
        /// 处理日期
        /// </summary>
        public string flowDate { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string flowName { get; set; }
    }

    /// <summary>
    /// 商标公告查询结果
    /// </summary>
    public class TrademarkNoticeResult
    {
        /// <summary>
        /// 返回码。0-成功  1-失败
        /// </summary>
        public int ret { get; set; }

        /// <summary>
        /// 如果ret=1，会有相应的错误信息提示，返回数据全部用UTF-8编码。
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 本天内，本接口的剩余允许调用次数
        /// </summary>
        public int remainCount { get; set; }

        /// <summary>
        /// 公告结果列表
        /// </summary>
        public List<TrademarkNotice> gonggaos { get; set; }
    }

    /// <summary>
    /// 商标公告信息
    /// </summary>
    public class TrademarkNotice
    {
        /// <summary>
        /// 公告日期
        /// </summary>
        public string ggDate { get; set; }

        /// <summary>
        /// 公告期号
        /// </summary>
        public string ggQihao { get; set; }

        /// <summary>
        /// 公告类型
        /// </summary>
        public string ggName { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public string ggPage { get; set; }

        /// <summary>
        /// 用于构造查看公告图片的url
        /// </summary>
        public string vcode { get; set; }

        /// <summary>
        /// 用于构造查看公告图片的url
        /// </summary>
        public string vcodeUri { get { return "http://www.tmkoo.com/sbgg-pic.php?qh=" + this.ggQihao + "&page=" + this.ggPage + "&vcode=" + this.vcode + "&isPng=true"; } set { this.vcodeUri = value; } }
    }

    /// <summary>
    /// 商标检索查询条件
    /// </summary>
    public class TrademarkRACondition
    {
        /// <summary>
        /// 待搜索的关键词
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 页面大小，即一次api调用最大获取多少条记录，取值范围：[0-50]。0-	不分页，一次性获得全部查询结果(但是，最多1万条记录)；1-	每次取1条记录，2-每次取2条记录 。。。。50-每次取50条记录
        /// </summary>
        public int? pageSize { get; set; }

        /// <summary>
        /// 当前页码数，即本次api调用是获得结果的第几页，从1开始计数。如果上面的pageSize参数为0，则此参数不起作用，可以不用传此参数。
        /// </summary>
        public int? pageNo { get; set; }

        /// <summary>
        /// 按什么来查，1: 商标名， 2：注册号， 3：申请人 4：商标名/注册号/申请人只要匹配到就算。
        /// </summary>
        public int? searchType { get; set; }

        /// <summary>
        /// 0：全部国际分类 非0：限定在指定类别，类别间用分号分割。如：4;12;34  表示在第4、12、34类内查询
        /// </summary>
        public string intCls { get; set; }

        /// <summary>
        /// 注册号/申请号
        /// </summary>
        public string regNo { get; set; }
    }

    /// <summary>
    /// 商标检索字典
    /// </summary>
    public class TrademarkRADic
    {
        /// <summary>
        /// 检索类别
        /// </summary>
        public static Dictionary<int, string> TypeDic = new Dictionary<int, string> { { 4, "全部" }, { 1, "商标名" }, { 2, "注册号" }, { 3, "申请人" } };

        /// <summary>
        /// 商标国际分类
        /// </summary>
        public static Dictionary<int, string> IntClsDic = new Dictionary<int, string> 
        { 
            { 0, "全部国际分类" },
            { 1, "第01类" }, 
            { 2, "第02类" }, 
            { 3, "第03类" }, 
            { 4, "第04类" }, 
            { 5, "第05类" }, 
            { 6, "第06类" }, 
            { 7, "第07类" }, 
            { 8, "第08类" }, 
            { 9, "第09类" },
            { 10, "第10类" },
            { 11, "第11类" }, 
            { 12, "第12类" }, 
            { 13, "第13类" }, 
            { 14, "第14类" }, 
            { 15, "第15类" }, 
            { 16, "第16类" }, 
            { 17, "第17类" }, 
            { 18, "第18类" }, 
            { 19, "第19类" },
            { 20, "第20类" },
            { 21, "第21类" }, 
            { 22, "第22类" }, 
            { 23, "第23类" }, 
            { 24, "第24类" }, 
            { 25, "第25类" }, 
            { 26, "第26类" }, 
            { 27, "第27类" }, 
            { 28, "第28类" }, 
            { 29, "第29类" },
            { 30, "第30类" },
            { 31, "第31类" }, 
            { 32, "第32类" }, 
            { 33, "第33类" }, 
            { 34, "第34类" }, 
            { 35, "第35类" }, 
            { 36, "第36类" }, 
            { 37, "第37类" }, 
            { 38, "第38类" }, 
            { 39, "第39类" },
            { 40, "第40类" },
            { 41, "第41类" }, 
            { 42, "第42类" }, 
            { 43, "第43类" }, 
            { 44, "第44类" }, 
            { 45, "第45类" }
        };
    }

    #endregion
    #endregion

    #region 万象云api实体类
    #region 专利评估api模型
    public class ApiWxyModelEvaluation
    {
        /// <summary>
        /// 专利号码
        /// </summary>
        public string applicationNumber { get; set; }

        /// <summary>
        /// 专利名称
        /// </summary>
        public string chineseName { get; set; }

        /// <summary>
        /// 专利类型
        /// </summary>
        public string patentType { get; set; }

        /// <summary>
        /// 专利权人
        /// </summary>
        public string patentee { get; set; }

        /// <summary>
        /// 专利申请日
        /// </summary>
        public string applyDay { get; set; }

        /// <summary>
        /// 专利评价得分
        /// </summary>
        public int patentScore { get; set; }

        /// <summary>
        /// 专利质量
        /// </summary>
        public List<string> patentQuality { get; set; }

        /// <summary>
        /// 专利布局
        /// </summary>
        public List<string> patentLayout { get; set; }

        /// <summary>
        /// 专利事实与应用
        /// </summary>
        public List<string> patentApplication { get; set; }
        /// <summary>
        /// 技术先进性
        /// </summary>
        public List<string> advancedTechnology { get; set; }
        /// <summary>
        /// 重点领域
        /// </summary>
        public List<string> keyAreas { get; set; }
    }
    #endregion
    #endregion

    #region 检索接口实体类
    /// <summary>
    /// 注册信息
    /// </summary>
    public class RegisterReuslt
    {
        /// <summary>
        /// 状态码，0代表成功
        /// </summary>
        public string rCode { get; set; }

        /// <summary>
        /// 响应的信息描述
        /// </summary>
        public string rmsg { get; set; }

        /// <summary>
        /// 检索结果总数
        /// </summary>
        public int 返回数据 { get; set; }

    }
    #endregion
}
