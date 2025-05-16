using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class TradeEntry
    {
		private TradeType _offerOrDemand;
		public  TradeType OfferOrDemand
		{
			get { return _offerOrDemand; }
			set { _offerOrDemand = value; }
		}

		private Company _buyerOrSeller;
		public Company BuyerOrSeller
		{
			get { return _buyerOrSeller; }
			set { _buyerOrSeller = value; }
		}

		private Product _product;
		public Product Product
		{
			get { return _product; }
			set { _product = value; }
		}

		private DeliveryInfo _deliveryInfo;
		public DeliveryInfo DeliveryInfo
		{
			get { return _deliveryInfo; }
			set { _deliveryInfo = value; }
		}
        
		private decimal _price;
        public decimal Price
		{
			get { return _Price; }
			set { _Price = value; }
		}

		private string _Currency;
		public string Currency
		{
			get { return _Currency; }
			set { _Currency = value; }
		}

		private DateTime _date;
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

		private string _notes;
		public string Notes
		{
			get { return _notes; }
			set { _notes = value; }
		}

	}
}
