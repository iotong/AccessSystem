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

    public class DepLogic : BaseLogic<Dep>
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
                .Query<Sys_Role>()
                .WhereIF(!string.IsNullOrEmpty(Query["Role_Name"].ToStr()), (a) => a.Role_Name.Contains(Query["Role_Name"].ToStr()));

            if (string.IsNullOrEmpty(Query["sortName"].ToStr()))
            {
                _Query.OrderBy((a) => new { a.Role_Num });
            }
            else
            {
                _Query.OrderBy((a) => Query["sortName"].ToStr() + " " + Query["sortOrder"].ToStr());//前端自动排序
            }

            var IQuery = _Query.Select(a => new { a.Role_Num, a.Role_Name, a.Role_Remark, a.Role_CreateTime, _ukid = a.Role_ID });

            return this.GetPagingEntity(IQuery, Page, Rows, new Sys_Role());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(Dep model)
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

            return model.id.ToGuidStr();
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
                    db.DeleteById<Dep>(item);
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
            var _Dep = db.FindById<Dep>(Id);

            var di = this.EntityToDictionary(new Dictionary<string, object>()
            {
                {"_Dep",_Dep},
                {"status",1}
            });

            return di;
        }

        #endregion


    }
}
