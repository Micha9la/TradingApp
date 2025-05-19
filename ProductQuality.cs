using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class ProductQuality
    {
		private float _proteinPercentage;
		public float Protein
		{
			get { return _proteinPercentage; }
			set { _proteinPercentage = value; }
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
