using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    internal class Product
    {
		private string _productName;

		public string ProductName
		{
			get { return _productName; }
			set { _productName = value; }
		}

		private ProductQuality _productQuality;

		public ProductQuality ProductQuality
		{
			get { return _productQuality; }
			set { _productQuality = value; }
		}

	}
}
