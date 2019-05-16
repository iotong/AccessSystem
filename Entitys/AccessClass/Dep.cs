using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entitys
{
   //
    using DbFrame.Class;
    using Entitys.Attributes;

	[Table("Dep")]
    public class Dep : Class.BaseClass
    {
		[Field("ID", IsPrimaryKey = true)]
		public Guid id { get; set; }

		[Field("部门名称")]
		public string Name { get; set; }

        [Field("部门类型 ")]
        public string Type { get; set; }


        [Field("上级部门ID")]
		public int? ParentId { get; set; }

		[Field("上级部门名称")]
		public string ParentName { get; set; }

		[Field("备注")]
		public string Memo { get; set; }

		[Field("操作时间")]
		public DateTime? OperTime { get; set; }

		[Field("操作人")]
		public string Oper { get; set; }

		

    }
}
