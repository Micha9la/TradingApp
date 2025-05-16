using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class Company
    {
		private string _companyName;

		public string CompanyName
		{
			get { return _companyName; }
			set { _companyName = value; }
		}

		private CompanyType _buyerOrSeller;

		public  CompanyType BuyerOrSeller
		{
			get { return _buyerOrSeller; }
			set { _buyerOrSeller = value; }
		}


	}
}
