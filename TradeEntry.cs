using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class TradeEntry
    {
		private TradeDirectionType _tradeDirection;
		public  TradeDirectionType TradeDirection
		{
			get { return _tradeDirection; }
			set { _tradeDirection = value; }
		}
		//not visible to customer
		private Company _company;
		public Company Company
		{
			get { return _company; }
			set { _company = value; }
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
			get { return _price; }
			set { _price = value; }
		}

		private string _currency;
		public string Currency
		{
			get { return _currency; }
			set { _currency = value; }
		}
		//not visible to customers
		private DateTime _date;
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}
		//not visible for customers
		private string _privateNotes;
		public string PrivateNotes
		{
			get { return _publicNotes; }
			set { _publicNotes = value; }
		}

        private string _publicNotes;
        public string Notes
        {
            get { return _publicNotes; }
            set { _publicNotes = value; }
        }
    }
}
