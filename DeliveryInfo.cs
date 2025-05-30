using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class DeliveryInfo
    {
		private ParityType _deliveryParity;

		public  ParityType DeliveryParity
		{
			get { return _deliveryParity; }
			set { _deliveryParity = value; }
		}

		//Townname +25km (it is always +25km)
		private string _locationDetail;
		public string LocationDetail
		{
			get { return _locationDetail; }
			set { _locationDetail = value; }
		}

	}
}
