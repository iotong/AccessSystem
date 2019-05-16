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

  
        [Field("�豸����")]
        [CRequired(ErrorMessage = "{Name}����Ϊ��")]
        public string Name { get; set; }

		[Field("�豸����")]
		public string Type { get; set; }

		[Field("�豸���")]
		public string SerialNumber { get; set; }

		[Field("IP��ַ")]
		public string IpAddress { get; set; }

		[Field("��¼����")]
		public string LoginName { get; set; }

		[Field("��¼����")]
		public string LoginPassword { get; set; }

		[Field("�˿ں�")]
		public int? Port { get; set; }

		[Field("����λ��")]
		public string Postion { get; set; }

		[Field("���쳧��")]
		public string Manufacturers { get; set; }

		[Field("�豸�ͺ�")]
		public string EquModel { get; set; }

		[Field("����ʱ��")]
		public DateTime? OperDateTime { get; set; }

		[Field("������")]
		public string Oper { get; set; }

		[Field("��ע")]
		public string Memo { get; set; }


    }
}
