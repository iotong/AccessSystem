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

	[Table("Equ")]
    public class Equ : Class.BaseClass
    {

        [Field("ID", IsPrimaryKey = true)]
		public Guid Id { get; set; }

  
        [Field("设备名称")]
        [CRequired(ErrorMessage = "{Name}不能为空")]
        public string Name { get; set; }

		[Field("设备类型")]
		public string Type { get; set; }

		[Field("设备序号")]
		public string SerialNumber { get; set; }

		[Field("IP地址")]
		public string IpAddress { get; set; }

		[Field("登录名称")]
		public string LoginName { get; set; }

		[Field("登录密码")]
		public string LoginPassword { get; set; }

		[Field("端口号")]
		public int? Port { get; set; }

		[Field("所在位置")]
		public string Postion { get; set; }

		[Field("制造厂家")]
		public string Manufacturers { get; set; }

		[Field("设备型号")]
		public string EquModel { get; set; }

		[Field("操作时间")]
		public DateTime? OperDateTime { get; set; }

		[Field("操作人")]
		public string Oper { get; set; }

		[Field("备注")]
		public string Memo { get; set; }


    }
}
