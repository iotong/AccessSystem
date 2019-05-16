using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFrame.CodeAnalysis
{
    //
    using DbFrame;
    using DbFrame.Class;
    using System.Linq.Expressions;

    public class DistinctAnalysis
    {

        public void Create(SQL _Sql)
        {
            if (!_Sql.Code_Column.ToString().ToLower().Contains("DISTINCT".ToLower()))
            {
                _Sql.Code_Column.Insert(0, " DISTINCT ");
            }

        }

    }
}
