﻿using System;
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

    public class CountAnalysis
    {
        public SQL Create(SQL _Sql)
        {
            var _New_Sql = new SQL();
            _New_Sql.Parameter = _Sql.Parameter;

            _New_Sql.Code_Column.Clear().Append(" COUNT(1) ");
            _New_Sql.Code_FromTab = _Sql.Code_FromTab;
            _New_Sql.Code_GroupBy = _Sql.Code_GroupBy;
            _New_Sql.Code_Join = _Sql.Code_Join;
            //_New_Sql.Code_OrderBy = _Sql.Code_OrderBy;
            _New_Sql.Code_Where = _Sql.Code_Where;
            return _New_Sql;
        }
    }
}
