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

        [Field("����")]
		public string Name { get; set; }

		[Field("�����ʼʱ��")]
		public DateTime? OutStartTime { get; set; }

		[Field("�����ֹʱ��")]
		public DateTime? OutEndTime { get; set; }

		[Field("�������")]
		public string OutWhat { get; set; }

		[Field("��������")]
		public string Depart { get; set; }

        [Field("������")]
        public string MangeDep { get; set; }

        [Field("�豸ID")]
        public string SerialNumber { get; set; }


		[Field("��׼��")]
		public string Auditor { get; set; }

		[Field("��׼�����")]
		public string Opinion { get; set; }

		[Field("����ʱ��")]
		public DateTime? AuditorTime { get; set; }

		[Field("��ԱID")]
		public int? NameId { get; set; }

		[Field("����ʱ��")]
		public DateTime? OperTime { get; set; }

		[Field("������")]
		public string Oper { get; set; }


        [Field("��ע")]
        public string Memo { get; set; }

       


    }
}
