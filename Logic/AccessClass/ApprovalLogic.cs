using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    using System.Data;
    using System.Collections;
    using Common;
	using Logic;
    using Logic.Class;
	using Entitys;
    using Entitys.SysClass;
	using DbFrame;

    public class ApprovalLogic : BaseLogic<Ask>
    {
        #region  增、删、改、查

        /// <summary>
        /// 数据源
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <returns></returns>
        public PagingEntity GetDataSource(Hashtable Query, int Page, int Rows)
        {
            var _Query = db
                .Query<Ask>()
                //.WhereIF(!string.IsNullOrEmpty(Query["Name"].ToStr()), (a) => a.Name.Contains(Query["Name"].ToStr()))
                .WhereIF(!string.IsNullOrEmpty(Query["MangeDep"].ToStr()), (a) => a.MangeDep ==(Query["MangeDep"].ToStr()))
                .WhereIF(!string.IsNullOrEmpty(Query["sTime"].ToStr()), (a) => a.OperTime >= (Query["sTime"].ToDateTime()))
                .WhereIF(!string.IsNullOrEmpty(Query["eTime"].ToStr()), (a) => a.OperTime <= (Query["eTime"].ToDateTime()));

            if (string.IsNullOrEmpty(Query["sortName"].ToStr()))
            {
                _Query.OrderBy((a) => new { a.id });
            }
            else
            {
                _Query.OrderBy((a) => Query["sortName"].ToStr() + " " + Query["sortOrder"].ToStr());//前端自动排序
            }

            var IQuery = _Query.Select(a => new { a.Name, a.Depart, a.OutStartTime, a.OutEndTime,a.OutWhat,a.Auditor,a.SerialNumber, _ukid = a.id });

            return this.GetPagingEntity(IQuery, Page, Rows, new Ask());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(Ask model)
        {
			db.Commit(() =>
            {
                if (model.id.ToGuid().Equals(Guid.Empty))
                {
                    model.id = db.Insert(model).ToGuid();
                    if (model.id == Guid.Empty)
                        throw new MessageBox(this.ErrorMessge);
                }
                else
                {
                    if (!db.UpdateById(model)) throw new MessageBox(this.ErrorMessge);
                }

            });

            return model.Name.ToGuidStr();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        public void Delete(string Ids)
        {
			db.Commit(() =>
            {
                Ids.DeserializeObject<List<Guid>>().ForEach(item =>
                {
                    db.DeleteById<Ask>(item);
                });
            });
        }

        /// <summary>
        /// 表单数据加载
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Dictionary<string, object> LoadForm(Guid Id)
        {
            var _Ask = db.FindById<Ask>(Id);

            var di = this.EntityToDictionary(new Dictionary<string, object>()
            {
                {"_Ask",_Ask},
                {"status",1}
            });

            return di;
        }

        #endregion


    }
}
