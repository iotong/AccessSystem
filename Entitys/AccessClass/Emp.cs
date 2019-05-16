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

	[Table("Emp")]
    public class Emp : Class.BaseClass
    {
		[Field("ID", IsPrimaryKey = true)]
		public Guid Id { get; set; }

		
		[Field("人员编号")]
		public int? Number { get; set; }

        [CRequired(ErrorMessage = "{Name}不能为空")]
        [Field("姓名")]
		public string Name { get; set; }

		[Field("性别")]
		public string Gender { get; set; }

		[Field("电话号码")]
		public string Tel { get; set; }

		[Field("手机号码")]
		public string MobliePhone { get; set; }

		[Field("籍贯")]
		public string Origin { get; set; }
		
		[Field("部门名称")]
		public string Department { get; set; }

		[Field("管理部门")]
		public string ManagementDept { get; set; }

		[Field("卡号")]
		public string CardNumber { get; set; }

		[Field("卡类型")]
		public string CardType { get; set; }

		[Field("卡密码")]
		public string CardPassword { get; set; }

		[Field("卡标识")]
		public string CardIdentity { get; set; }

		[Field("开始时间")]
		public DateTime? CardStartTime { get; set; }

		[Field("结束时间")]
		public DateTime? CardEndTime { get; set; }

		[Field("备注")]
		public string Memo { get; set; }


		[Field("操作时间")]
		public DateTime? OperTime { get; set; }

		[Field("操作人")]
		public string Oper { get; set; }


    }
}
