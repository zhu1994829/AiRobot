using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MX.AIRobot.Model
{
    /// <summary>
    /// 视图展现模型
    /// </summary>
    public class ViewModel
    {

    }

    class DBColumn : Attribute
    {
        string dbname { get; set; }
        public DBColumn(string dbname)
        {
            this.dbname = dbname;
        }
    }

    public class ViewAnswerBank
    {
        [DBColumn("AnswerID")]
        public string id { get; set; }

        [DBColumn("QuestionTitle")]
        public string title { get; set; }

        [DBColumn("Answer")]
        public string reply { get; set; }

        [DBColumn("AnswerTypes")]
        public string type { get; set; }

        [DBColumn("Website")]
        public string url { get; set; }

        [DBColumn("CreateTime")]
        public DateTime? createdate { get; set; }

        public string countID { get; set; }
    }

    public class ViewRule
    {
        [DBColumn("RuleID")]
        public string id { get; set; }

        [DBColumn("KeyWordOne")]
        public string keyone { get; set; }

        [DBColumn("KeyWordTwo")]
        public string keytwo { get; set; }

        [DBColumn("Answer")]
        public string reply { get; set; }

        [DBColumn("AnswerTypes")]
        public string type { get; set; }

        [DBColumn("Website")]
        public string url { get; set; }

        [DBColumn("CreateTime")]
        public DateTime? createdate { get; set; }
    }

    public class ViewRecord
    {

        [DBColumn("RecordID")]
        public string id { get; set; }

        [DBColumn("OpenID")]
        public string opid { get; set; }

        [DBColumn("QuestionContent")]
        public string content { get; set; }

        [DBColumn("CreateTime")]
        public DateTime? createdate { get; set; }
    }
}
