using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIContainer.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ExportAttribute : Attribute
	{
		public ExportAttribute() { }

		public ExportAttribute(Type baseType)
		{
			this.BaseType = baseType;
		}

		public Type BaseType { get; private set; }
	}
}
