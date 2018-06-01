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
    public class AnswerBankService : BaseService, IService<BizAnswerBank>
    {
        #region 添加

        public bool Add(BizAnswerBank bizAnswerBank)
        {
            object result = null;
            AspectF.Define
              .Log(log, "AnswerBankService-Add开始", "AnswerBankService-Add结束")
              .HowLong(log)
              .Do(() =>
              {
                  bizAnswerBank.AnswerID = Guid.NewGuid().ToString();
                  bizAnswerBank.CreateTime = DateTime.Now;
                  bizAnswerBank.UpdateTime = DateTime.Now;
                  bizAnswerBank.IsDeleted = false;
                  result = db.Insert(bizAnswerBank);
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

        public bool Delete(BizAnswerBank model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string[] AnswerIDs)
        {
            int result = 0;
            AspectF.Define
              .Log(log, "AnswerBankService-Delete开始", "AnswerBankService-Delete结束")
              .HowLong(log)
              .Do(() =>
              {
                  db.BeginTransaction();
                  try
                  {
                      foreach (string AnswerID in AnswerIDs)
                      {
                          result = db.Execute("update BizAnswerBank set IsDeleted=1 where AnswerID=@0 ", AnswerID);
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

        public bool Delete(IEnumerable<BizAnswerBank> list)
        {
            var result = false;
            int result_a = 0;
            AspectF.Define
             .Log(log, "AnswerBankService-Delete开始", "AnswerBankService-Delete结束")
             .HowLong(log)
             .Retry(log)
             .Do(() =>
             {
                 db.BeginTransaction();
                 try
                 {
                     foreach (var item in list)
                     {
                         var sell = db.SingleOrDefault<BizAnswerBank>("where relationid=@0", item.AnswerID);
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

        public bool Update(BizAnswerBank bizAnswerBank)
        {
            int result = 0;
            AspectF.Define
                 .Log(log, "AnswerBankService-Update开始", "AnswerBankService-Update结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     result = db.Update(bizAnswerBank, bizAnswerBank.AnswerID, new List<string> { "QuestionTitle", "Answer", "AnswerType", "Website", "UpdateTime", "UpdateUser" });
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

        public Page<BizAnswerBank> PageList(BizAnswerBank bizAnswerBank, int pageSize, int pageid = 1)
        {
            Page<BizAnswerBank> result = new Page<BizAnswerBank>();
            AspectF.Define
                .Log(log, "AnswerBankService-PageList开始", "AnswerBankService-PageList结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    var sql = Sql.Builder
                        .Select("bab.*")
                        .From("BizAnswerBank bab");
                    if (!String.IsNullOrEmpty(bizAnswerBank.Title))
                    {
                        sql.Where("bab.QuestionTitle like @0 or bab.AnswerID like @0", "%" + bizAnswerBank.Title + "%");
                    }
                    sql.Where("bab.IsDeleted=0");
                    sql.OrderBy("bab.CreateTime desc");
                    result = db.Page<BizAnswerBank>(pageid, pageSize, sql);

                });
            return result;
        }

        public IEnumerable<BizAnswerBank> List(BizAnswerBank model)
        {
            throw new NotImplementedException();
        }

        public BizAnswerBank Single(BizAnswerBank bizAnswerBank)
        {
            BizAnswerBank result = new BizAnswerBank();
            AspectF.Define
                 .Log(log, "AnswerBankService-Single开始", "AnswerBankService-Single结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var sql = Sql.Builder
                         .Select("bab.*")
                         .From("BizAnswerBank bab")
                         .Where("bab.AnswerID=@0", bizAnswerBank.AnswerID);
                     sql.Where("bab.IsDeleted=0");
                     result = db.Query<BizAnswerBank>(sql).FirstOrDefault();
                 });
            return result;
        }

        /// <summary>
        /// 相同问题随机生成不同回答
        /// </summary>
        /// <param name="Question">问题</param>
        /// <returns>随机返回一条数据</returns>
        public BizAnswerBank Single(string Question)
        {
            BizAnswerBank result = new BizAnswerBank();
            AspectF.Define
                 .Log(log, "AnswerBankService-Single开始", "AnswerBankService-Single结束")
                 .HowLong(log)
                 .Retry(log)
                 .Do(() =>
                 {
                     var sql = Sql.Builder
                         .Select("bab.*")
                         .From("BizAnswerBank bab")
                         .Where("bab.QuestionTitle=@0 and bab.IsDeleted=0", Question)
                         .OrderBy("NEWID()");
                     result = db.Query<BizAnswerBank>(sql).FirstOrDefault();
                 });
            return result;
        }
        #endregion
    }
}
