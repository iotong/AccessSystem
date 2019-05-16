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
    public class Dep : Models.Class.BaseClass
    {
		[Field("ID", IsPrimaryKey = true)]
		public Guid id { get; set; }

		[Field("Name")]
		public string Name { get; set; }

		[Field("ParentId")]
		public int? ParentId { get; set; }

		[Field("ParentName")]
		public string ParentName { get; set; }

		[Field("Memo")]
		public string Memo { get; set; }

		[Field("OperTime")]
		public DateTime? OperTime { get; set; }

		[Field("Oper")]
		public string Oper { get; set; }

		[Field("Type")]
		public string Type { get; set; }


    }
}
