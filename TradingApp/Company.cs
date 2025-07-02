using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class Company
    {
        public int Id { get; set; }
        private string _companyName;

		public string CompanyName
		{
			get { return _companyName; }
			set { _companyName = value; }
		}
		//contact person

		private string _contactPerson;

		public string ContactPerson
		{
			get { return _contactPerson; }
			set { _contactPerson = value; }
		}

	}
}
