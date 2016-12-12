using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assessment2
{
    class CarHire
    {
        private DateTime startdate;
        private DateTime enddate;
        private string drivername;
        
        public string Startdate
        {
            set { DateTime.TryParse(value, out startdate); }
            get { return startdate.ToString(); }



        }

        public string Enddate
        {
            set { DateTime.TryParse(value, out enddate); }
            get { return enddate.ToString(); }



        }

        public string DriverName
        {
            set { drivername = value; }
            get { return drivername; }



        }

    
    
    
    
    
    }
}
