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

    public class RecLogic : BaseLogic<Rec>
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
                .Query<Rec>()
                .WhereIF(!string.IsNullOrEmpty(Query["Name"].ToStr()), (a) => a.Name.Contains(Query["Name"].ToStr()));

            if (string.IsNullOrEmpty(Query["sortName"].ToStr()))
            {
                _Query.OrderBy((a) => new { a.CardNumber });
            }
            else
            {
                _Query.OrderBy((a) => Query["sortName"].ToStr() + " " + Query["sortOrder"].ToStr());//前端自动排序
            }

            var IQuery = _Query.Select(a => new { a.Name, a.CardNumber, a.EventType, a.Time,a.EquNo,a.EquName,_ukid = a.Id });

            return this.GetPagingEntity(IQuery, Page, Rows,new Rec(), new Sys_Role());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(Rec model)
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
                    db.DeleteById<Rec>(item);
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
            var _Rec = db.FindById<Rec>(Id);

            var di = this.EntityToDictionary(new Dictionary<string, object>()
            {
                {"_Rec",_Rec},
                {"status",1}
            });

            return di;
        }


        public string checkCardNumber(string CardNo)
        {
            string cardNumber = "null";
            string sql = "select CardNumber,Id from Rec where CardNumber ='" + CardNo + "'";
            DataTable dt = db.QueryDataTable(sql, "");
            if (dt.Rows.Count > 0)
            {
                cardNumber = dt.Rows[0]["Id"].ToStr();
            }
            else
            {
                cardNumber = "null";
            }
            return cardNumber;

        }



        #endregion


    }
}
