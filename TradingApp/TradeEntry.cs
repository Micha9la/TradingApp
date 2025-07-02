using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class TradeEntry
    {
        public int Id { get; set; }
        //catalog number can start from 1. every TradeEntry gets a number!!
        private int _catalogNumber;
        public int CatalogNumber
        {
            get { return _catalogNumber; }
            set { _catalogNumber = value; }
        }
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

		private ISCC _iscc;

		public ISCC ISCC
		{
			get { return _iscc; }
			set { _iscc = value; }
		}
		private GMP _gmp;

		public  GMP GMP
		{
			get { return _gmp; }
			set { _gmp = value; }
		}


		//not visible for customers
		private string _privateNotes;
		public string PrivateNotes
		{
			get { return _privateNotes; }
			set { _privateNotes = value; }
		}

		//visible to customer, part of message to customer. crop year
        private string _publicNotes;
        public string PublicNotes
        {
            get { return _publicNotes; }
            set { _publicNotes = value; }
        }
    }
}
