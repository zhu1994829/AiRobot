using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MX.AIRobot.Model;
using MX.AIRobot.AOP;
using MX.AIRobot.Interface;
using MX.AIRobot.Log;
using PetaPoco;

namespace MX.AIRobot.Service
{
    public class RuleService:BaseService,IService<BizRule>
    {
        #region 添加

        public bool Add(BizRule bizRule)
        {
            object result = null;
            AspectF.Define
              .Log(log, "RuleService-Add开始", "RuleService-Add结束")
              .HowLong(log)
              .Do(() =>
              {
                  bizRule.RuleID = Guid.NewGuid().ToString();
                  bizRule.CreateTime = DateTime.Now;
                  bizRule.UpdateTime = DateTime.Now;
                  bizRule.IsDeleted = false;
                  result = db.Insert(bizRule);
              });
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


        #region 删除

        public bool Delete(BizRule model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string[] RuleIDs)
        {
            int result = 0;
            AspectF.Define
              .Log(log, "RuleService-Delete开始", "RuleService-Delete结束")
              .HowLong(log)
              .Do(() =>
              {
                  db.BeginTransaction();
                  try
                  {
                      foreach (string RuleID in RuleIDs)
                      {
                          result = db.Execute("update BizRule set IsDeleted=1 where RuleID=@0 ", RuleID);
                          if (result == 0)
                          {
                              break;
                          }
                      }

                      if (result == 0)
                      {
                          db.AbortTransaction();
                      }
                      else
                      {
                          db.CompleteTransaction();
                      }

                  }
                  catch
                  {
                      db.AbortTransaction();
                      throw;
                  }
              });
            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Delete(IEnumerable<BizRule> list)
        {
            var result = false;
            int result_a = 0;
            AspectF.Define
             .Log(log, "RuleService-Delete开始", "RuleService-Delete结束")
             .HowLong(log)
             .Retry(log)
             .Do(() =>
             {
                 db.BeginTransaction();
                 try
                 {
                     foreach (var item in list)
                     {
                         var sell = db.SingleOrDefault<BizRule>("where relationid=@0", item.RuleID);
                         if (sell != null)
                         {
                             db.Delete(sell);
                         }
                         result_a = db.Delete(item);
                     }
                     if (result_a > 0)
                     {
                         result = true;
                         db.CompleteTransaction();
                     }
                     else
                     {
                         result = false;
                         db.AbortTransaction();
                     }
                 }
                 catch (Exception ex)
                 {
                     db.AbortTransaction();
                     throw ex;
                 }
             });
            return result;
        }

        #endregion


        #region 编辑

        public bool Update(BizRule bizRule)
        {
            int result = 0;
            AspectF.Define
                 .Log(log, "RuleService-Update开始", "RuleService-Update结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     result = db.Update(bizRule, bizRule.RuleID, new List<string> { "KeyWordOne", "KeyWordTwo", "Answer", "AnswerTypes", "Website", "UpdateTime", "UpdateUser" });
                 });
            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion


        #region 查询

        public Page<BizRule> PageList(BizRule bizRule, int pageSize, int pageid = 1)
        {
            Page<BizRule> result = new Page<BizRule>();
            AspectF.Define
               .Log(log, "RuleService-PageList开始", "RuleService-PageList结束")
               .HowLong(log)
               .Retry(log)
               .Do(() =>
               {
                   var sql = Sql.Builder
                       .Select("br.*")
                       .From("BizRule br"); 
                   if(!string.IsNullOrEmpty(bizRule.KeyWord))
                   {
                       sql.Where("br.KeyWordOne like @0 or br.KeyWordTwo like @0", "%" + bizRule.KeyWord + "%");
                   }
                   if (!string.IsNullOrEmpty(bizRule.CreateUser))
                   {
                       sql.Where("br.CreateUser =@0", bizRule.CreateUser);
                   }
                   sql.Where("br.IsDeleted=0");
                   sql.OrderBy("br.CreateTime desc");
                   result = db.Page<BizRule>(pageid, pageSize, sql);

               });
            return result;
        }

        public IEnumerable<BizRule> List(BizRule bizRule)
        {

            List<BizRule> result = new List<BizRule>();
            AspectF.Define
               .Log(log, "RuleService-List开始", "RuleService-List结束")
               .HowLong(log)
               .Retry(log)
               .Do(() =>
               {
                   var sql = Sql.Builder
                       .Select("br.*")
                       .From("BizRule br");
                   sql.Where("br.IsDeleted=0");
                   sql.OrderBy("len(br.KeyWordOne) desc,len(br.KeyWordTwo) desc");
                   result = db.Query<BizRule>(sql).ToList();
               });
            return result;
        }

        public BizRule Single(BizRule bizRule)
        {
            BizRule result = new BizRule();
            AspectF.Define
                .Log(log, "RuleService-Single开始", "RuleService-Single结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    var sql = Sql.Builder
                        .Select("br.*")
                        .From("BizRule br")
                        .Where("br.RuleID=@0", bizRule.RuleID);
                    result = db.Query<BizRule>(sql).FirstOrDefault();
                });
            return result;
        }

        /// <summary>
        /// 关键字一为空
        /// </summary>
        /// <param name="keyValueTwo"></param>
        /// <returns></returns>
        public BizRule SingleTwo(string keyValueTwo)
        {
            BizRule result = new BizRule();
            AspectF.Define
                 .Log(log, "RuleService-Single开始", "RuleService-Single结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var sql = Sql.Builder
                         .Select("br.*")
                         .From("BizRule br")
                         .Where("br.IsDeleted=0");
                     sql.Where("br.KeyWordOne is null and br.KeyWordTwo=@0", keyValueTwo);
                     result = db.Query<BizRule>(sql).FirstOrDefault();
                 });
            return result;
        }

        /// <summary>
        /// 关键字二为空
        /// </summary>
        /// <param name="keyValueOne"></param>
        /// <returns></returns>
        public BizRule SingleOne(string keyValueOne)
        {
            BizRule result = new BizRule();
            AspectF.Define
                 .Log(log, "RuleService-Single开始", "RuleService-Single结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var sql = Sql.Builder
                         .Select("br.*")
                         .From("BizRule br")
                         .Where("br.IsDeleted=0");
                     sql.Where("br.KeyWordOne=@0 and br.KeyWordTwo is null", keyValueOne);
                     result = db.Query<BizRule>(sql).FirstOrDefault();
                 });
            return result;
        }

        public BizRule Single(string keyValueOne,string keyValueTwo)
        {
            BizRule result = new BizRule();
            AspectF.Define
                 .Log(log, "RuleService-Single开始", "RuleService-Single结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var sql = Sql.Builder
                         .Select("br.*")
                         .From("BizRule br")
                         .Where("br.IsDeleted=0");
                         sql.Where("br.KeyWordOne=@0 and br.KeyWordTwo=@1", keyValueOne, keyValueTwo);
                     result = db.Query<BizRule>(sql).FirstOrDefault();
                 });
            return result;
        }
        #endregion
    }
}
