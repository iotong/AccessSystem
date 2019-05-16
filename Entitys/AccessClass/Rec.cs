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
    using static System.Net.Mime.MediaTypeNames;

    [Table("Rec")]
    public class Rec : Class.BaseClass
    {
		[Field("事件类型")]
		public string EventType { get; set; }

		[Field("姓名")]
		public string Name { get; set; }

		[Field("卡号")]
		public string CardNumber { get; set; }

		[Field("发生时间")]
		public DateTime? Time { get; set; }

		[Field("设备名称")]
		public string EquName { get; set; }

        [Field("操作时间")]
        public DateTime? OperTime { get; set; }

        [Field("操作人")]
        public string Oper { get; set; }

        //[Field("图片")]
        //public Image Img {get;set;}

		[Field("设备号")]
		public string EquNo { get; set; }


        [Field("备注")]
		public string Memo { get; set; }

		


		[Field("ID", IsPrimaryKey = true)]
		public Guid Id { get; set; }


    }
}

