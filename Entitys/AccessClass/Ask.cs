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

	[Table("Ask")]
    public class Ask : Class.BaseClass
    {
        [Field("Id", IsPrimaryKey = true)]
        public Guid id { get; set; }

        [Field("姓名")]
		public string Name { get; set; }

		[Field("外出开始时间")]
		public DateTime? OutStartTime { get; set; }

		[Field("外出结止时间")]
		public DateTime? OutEndTime { get; set; }

		[Field("外出事由")]
		public string OutWhat { get; set; }

		[Field("部门名称")]
		public string Depart { get; set; }

        [Field("管理部门")]
        public string MangeDep { get; set; }

        [Field("设备ID")]
        public string SerialNumber { get; set; }


		[Field("批准人")]
		public string Auditor { get; set; }

		[Field("批准决意见")]
		public string Opinion { get; set; }

		[Field("批推时间")]
		public DateTime? AuditorTime { get; set; }

		[Field("人员ID")]
		public int? NameId { get; set; }

		[Field("操作时间")]
		public DateTime? OperTime { get; set; }

		[Field("操作人")]
		public string Oper { get; set; }


        [Field("备注")]
        public string Memo { get; set; }

       


    }
}
