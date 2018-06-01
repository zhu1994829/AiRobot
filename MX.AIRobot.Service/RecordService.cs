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
    public class RecordService : BaseService, IService<BizRecord>
    {
        public bool Add(BizRecord bizRecord)
        {
            object result = null;
            AspectF.Define
             .Log(log, "AnswerBankService-Add开始", "AnswerBankService-Add结束")
             .HowLong(log)
             .Do(() =>
             {
                 var model = db.Query<BizRecord>("select * from BizRecord where QuestionContent=@0", bizRecord.QuestionContent).FirstOrDefault();
                 if (model == null)
                 {
                     bizRecord.RecordID = Guid.NewGuid().ToString();
                     bizRecord.CreateTime = DateTime.Now;
                     bizRecord.UpdateTime = DateTime.Now;
                     bizRecord.IsDeleted = false;
                     bizRecord.RecordNum = 1;
                     result = db.Insert(bizRecord);
                 }
                 else
                 {
                     db.Execute("update BizRecord set RecordNum=RecordNum+1 where RecordID=@0 ", model.RecordID);
                 }
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


        #region 删除
        public bool Delete(BizRecord model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string[] RecordIDs)
        {
            int result = 0;
            AspectF.Define
              .Log(log, "RecordService-Delete开始", "RecordService-Delete结束")
              .HowLong(log)
              .Do(() =>
              {
                  db.BeginTransaction();
                  try
                  {
                      foreach (string RecordID in RecordIDs)
                      {
                          result = db.Execute("update BizRecord set IsDeleted=1 where RecordID=@0 ", RecordID);
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

        public bool Delete(IEnumerable<BizRecord> list)
        {
            var result = false;
            int result_a = 0;
            AspectF.Define
             .Log(log, "RecordService-Delete开始", "RecordService-Delete结束")
             .HowLong(log)
             .Retry(log)
             .Do(() =>
             {
                 db.BeginTransaction();
                 try
                 {
                     foreach (var item in list)
                     {
                         var sell = db.SingleOrDefault<BizRecord>("where relationid=@0", item.RecordID);
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

        public bool Update(BizRecord model)
        {
            throw new NotImplementedException();
        }

        #region 查询

        public Page<BizRecord> PageList(BizRecord bizRecord, int pageSize, int pageid = 1)
        {
            Page<BizRecord> result = new Page<BizRecord>();
            AspectF.Define
                .Log(log, "RecordService-PageList开始", "RecordService-PageList结束")
                .HowLong(log)
                .Retry(log)
                .Do(() =>
                {
                    var sql = Sql.Builder
                        .Select("brd.*")
                        .From("BizRecord brd");
                    if (!String.IsNullOrEmpty(bizRecord.Content))
                    {
                        sql.Where("brd.QuestionContent like @0", "%" + bizRecord.Content + "%");
                    }
                    if (!String.IsNullOrEmpty(bizRecord.QuestionContent))
                    {
                        sql.Where("brd.QuestionContent =@0", bizRecord.QuestionContent);
                    }

                    if (!string.IsNullOrEmpty(bizRecord.CreateUser))
                    {
                        sql.Where("brd.CreateUser =@0", bizRecord.CreateUser);
                    }
                    sql.Where("brd.IsDeleted=0");
                    sql.OrderBy("brd.RecordNum desc");
                    result = db.Page<BizRecord>(pageid, pageSize, sql);

                });
            return result;
        }

        public IEnumerable<BizRecord> List(BizRecord model)
        {
            throw new NotImplementedException();
        }

        public BizRecord Single(BizRecord model)
        {
            throw new NotImplementedException();
        }
        #endregion



        public void RecordNums(BizRecord bizRecord)
        {
            AspectF.Define
                   .Log(log, "RecordService-RecordNums开始", "RecordService-RecordNums结束")
                   .HowLong(log)
                   .Do(() =>
                   {
                       db.Execute("update BizRecord set RecordNum=RecordNum+1 where RecordID=@0 ", bizRecord.RecordID);
                   });
        }
    }
}
