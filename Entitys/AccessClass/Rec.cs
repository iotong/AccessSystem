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
		[Field("�¼�����")]
		public string EventType { get; set; }

		[Field("����")]
		public string Name { get; set; }

		[Field("����")]
		public string CardNumber { get; set; }

		[Field("����ʱ��")]
		public DateTime? Time { get; set; }

		[Field("�豸����")]
		public string EquName { get; set; }

        [Field("����ʱ��")]
        public DateTime? OperTime { get; set; }

        [Field("������")]
        public string Oper { get; set; }

        //[Field("ͼƬ")]
        //public Image Img {get;set;}

		[Field("�豸��")]
		public string EquNo { get; set; }


        [Field("��ע")]
		public string Memo { get; set; }

		


		[Field("ID", IsPrimaryKey = true)]
		public Guid Id { get; set; }


    }
}

