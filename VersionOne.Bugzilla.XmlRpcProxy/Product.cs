using System;
using System.Collections.Generic;
using System.Text;

namespace VersionOne.Bugzilla.XmlRpcProxy
{
	public class Product
	{
		public readonly int ID;
		public readonly string Name;
		public readonly string Description;

		private Product(GetProductResult result)
		{
			ID = result.id;
			Name = result.name;
			Description = result.description;
		}

		public static Product Create(GetProductResult result)
		{
			return new Product(result);
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", ID, Name);
		}
	}
}
