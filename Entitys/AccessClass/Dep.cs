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

		[Field("��������")]
		public string Name { get; set; }

        [Field("�������� ")]
        public string Type { get; set; }


        [Field("�ϼ�����ID")]
		public int? ParentId { get; set; }

		[Field("�ϼ���������")]
		public string ParentName { get; set; }

		[Field("��ע")]
		public string Memo { get; set; }

		[Field("����ʱ��")]
		public DateTime? OperTime { get; set; }

		[Field("������")]
		public string Oper { get; set; }

		

    }
}
