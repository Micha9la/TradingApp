using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class Product
    {
		private string _productName;

		public string ProductName
		{
			get { return _productName; }
			set { _productName = value; }
		}
		//quality is 13/78/250/75 protein/hectoliter weight/falling number/glassiness(durum) no units given when sending to customers!
		private ProductQuality _productQuality;

		public ProductQuality ProductQuality
		{
			get { return _productQuality; }
			set { _productQuality = value; }
		}

		//tons
		private int _quantity;
		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}


		//crop year (not always important- goes to notes)
		private int _catalogNumber;

		public int CatalogNumber
		{
			get { return _catalogNumber; }
			set { _catalogNumbe = value; }
		}

		//catalog number can start from 1 again
	}
}
