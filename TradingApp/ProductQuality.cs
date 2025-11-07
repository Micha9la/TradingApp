using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp
{
    public class ProductQuality
    {
        public int Id { get; set; }

        //percentige 11,1% (drop down menu with suggestions)
        private float? _proteinPercentage;
		public float? Protein
		{
			get { return _proteinPercentage; }
			set { _proteinPercentage = value; }
		}

		//Unit: seconds
		private int? _fallingNumber;
		public int? FallingNumber
		{
			get { return _fallingNumber; }
			set { _fallingNumber = value; }
		}

		//kg/hl
		private int? _testWeight;
		public int? TestWeight
		{
			get { return _testWeight; }
			set { _testWeight = value; }
		}

		//%
		private int? _glassiness;
		public int? Glassiness
		{
			get { return _glassiness; }
			set { _glassiness = value; }
		}

        //%
        private int? _oilContent;
        public int? OilContent
        {
            get { return _oilContent; }
            set { _oilContent = value; }
        }

        //%
        private int? _damagedKernels;
        public int? DamagedKernels
        {
            get { return _damagedKernels; }
            set { _damagedKernels = value; }
        }

		//unit ppb
		private int? _don;
		public int? Don
		{
			get { return _don; }
			set { _don = value; }
		}

		//unti: ppb
		private int? _afla;
		public int? Afla
		{
			get { return _afla; }
			set { _afla = value; }
		}


	}
}
