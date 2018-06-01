using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace MX.AIRobot.Service
{
    public interface IService<T>
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        Boolean Add(T model);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="model">实体对象（需要指定主键id）</param>
        /// <returns></returns>
        Boolean Delete(T model);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns></returns>
        Boolean Delete(string id);

        /// <summary>
        /// 删除实体(多条）
        /// </summary>
        /// <param name="id">集合对象</param>
        /// <returns></returns>
        Boolean Delete(IEnumerable<T> list);

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        Boolean Update(T model);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <param name="pageCount">当前页</param>
        /// <param name="pageSize">页记录数</param>
        /// <returns>PetaPoco分页类</returns>
        Page<T> PageList(T model, int pageCount, int pageSize);

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <returns></returns>
        IEnumerable<T> List(T model);


        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T Single(T model);
    }
}
