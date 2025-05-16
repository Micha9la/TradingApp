using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    internal class ProductQuality
    {
		private string _protein;
		public string Protein
		{
			get { return _protein; }
			set { _protein = value; }
		}

		private int _fallingNumber;
		public int FallingNumber
		{
			get { return _fallingNumber; }
			set { _fallingNumber = value; }
		}

		private int _weight;

		public int Weight
		{
			get { return _weight; }
			set { _weight = value; }
		}

	}
}
