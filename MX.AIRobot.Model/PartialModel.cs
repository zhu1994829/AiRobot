using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.Text.RegularExpressions;

namespace MX.AIRobot.Model
{
    //模型拓展类

    public partial class BizRule
    {
        public string KeyWord { get;set; }
    }

    public partial class BizAnswerBank
    {
        public string Title { get; set; }
    }

    public partial class BizRecord
    {
        public string Content { get; set; }
    }
    #region 专利
    public partial class BizPatent
    {

        /// <summary>
        /// 总量
        /// </summary>
        public int Count { get; set; }


        /// <summary>
        /// 专利出版社数据库属性
        /// </summary>
        public string CniprDbs
        {
            get;
            set;
        }

        /// <summary>
        /// 检索专利出版社的查询条件
        /// </summary>
        public string SearchCondition
        { get; set; }

        /// <summary>
        /// 缴费状态
        /// </summary>
        public string PaidState
        {
            get;
            set;
        }

        /// <summary>
        /// 距离缴费天数
        /// </summary>
        [ResultColumn]
        public int Diffday { get; set; }


        /// <summary>
        /// 专利附件路径
        /// </summary>
        public string AttachmentPath { get; set; }

        /// <summary>
        /// 是否挂牌交易（用于托管列表显示字段）
        /// </summary>
        public string IsSell
        {
            get;
            set;
        }

    }

    #endregion


    public partial class RuleResult
    {

        public string text { get; set; }

        public string url { get; set; }

        public string type { get; set; }

        public string keyword { get; set; }

        public List<BizRule> bizRules { get; set; }
    }


    public partial class TuLingResult
    {
        public int code { get; set; }

        public string text { get; set; }
    }


}
