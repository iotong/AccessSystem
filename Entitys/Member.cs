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

    [Table("Member")]
    public class Member : Class.BaseClass
    {

        [Field("Member_ID", IsPrimaryKey = true)]
        public Guid Member_ID { get; set; }

        [CSetNumber(0)]
        [Field("���")]
        public string Member_Num { get; set; }

        [CRequired(ErrorMessage = "{name}����Ϊ��")]
        [Field("��Ա����")]
        public string Member_Name { get; set; }

        [Field("��Ա�绰")]
        public int? Member_Phone { get; set; }

        [Field("�Ա�")]
        public string Member_Sex { get; set; }

        [Field("����")]
        public DateTime? Member_Birthday { get; set; }

        [Field("ͷ��")]
        public string Member_Photo { get; set; }

        [Field("�ʻ�ID")]
        public Guid? Member_UserID { get; set; }

        [Field("����")]
        public string Member_Introduce { get; set; }

        [Field("�ļ�")]
        public string Member_FilePath { get; set; }

        [Field("����ʱ��", IsIgnore = true)]
        public DateTime? Member_CreateTime { get; set; }


    }
}
