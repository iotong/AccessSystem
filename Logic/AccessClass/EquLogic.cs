using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Access
{
    using System.Data;
    using System.Collections;
    using Common;
	using Logic;
    using Logic.Class;
	using Entitys;
    using Entitys.SysClass;
	using DbFrame;
    using Entitys;

    public class EquLogic : BaseLogic<Equ>
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
                .Query<Equ>()
                .WhereIF(!string.IsNullOrEmpty(Query["Name"].ToStr()), (a) => a.Name.Contains(Query["Name"].ToStr()));

            if (string.IsNullOrEmpty(Query["sortName"].ToStr()))
            {
                _Query.OrderBy((a) => new { a.Id });
            }
            else
            {
                _Query.OrderBy((a) => Query["sortName"].ToStr() + " " + Query["sortOrder"].ToStr());//前端自动排序
            }

            var IQuery = _Query.Select(a => new { a.Name, a.Type, a.IpAddress,a.LoginName,a.Port,a.LoginPassword,a.Manufacturers,a.EquModel,a.SerialNumber , _ukid = a.Id });

            return this.GetPagingEntity(IQuery, Page, Rows,new Equ(), new Sys_Role());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(Equ model)
        {
			db.Commit(() =>
            {
                if (model.Id.ToGuid().Equals(Guid.Empty))
                {
                    model.Id = db.Insert(model).ToGuid();
                    if (model.Id == Guid.Empty)
                        throw new MessageBox(this.ErrorMessge);
                }
                else
                {
                    if (!db.UpdateById(model)) throw new MessageBox(this.ErrorMessge);
                }

            });

            return model.Id.ToGuidStr();
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
                    db.DeleteById<Equ>(item);
                    
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
            var _Equ = db.FindById<Equ>(Id);

            var di = this.EntityToDictionary(new Dictionary<string, object>()
            {
                {"_Equ",_Equ},
                {"status",1}
            });

            return di;
        }

        public DataTable getEquData(string Query)
        {
            return db.QueryDataTable(Query,"");
 
            //  DataSet ds = new DataSet();
            //  return  ;
        }

        #endregion


    }
}
