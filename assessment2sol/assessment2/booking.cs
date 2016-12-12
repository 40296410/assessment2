using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// Rory Miller 40296410
/// Assessment 2
/// Software Development 2

namespace assessment2
{
    class booking
    {

        private DateTime arvdate;
        private DateTime depdate;
        private int bookref;
        private int nonights;
        private int extrabreak;
        private int extrameal;
        private int extracar;

        public string Adate
        {
            set { DateTime.TryParse(value, out arvdate); }
            get { return arvdate.ToString(); }



        }

        public string Depdate
        {
            set { DateTime.TryParse(value, out depdate); }
            get { return depdate.ToString(); }



        }

        public int Bookref
        {
            set { this.bookref = value; }
            get { return this.bookref; }



        }
        public int Nonights
        {
            set { this.nonights = value; }
            get { return this.nonights; }



        }
        public int ExtraBreak
        {
            set { this.extrabreak = value; }
            get { return this.extrabreak; }



        }
        public int ExtraMeal
        {
            set { this.extrameal = value; }
            get { return this.extrameal; }



        }
        public int ExtraCar
        {
            set { this.extracar = value; }
            get { return this.extracar; }



        }


    }
}
