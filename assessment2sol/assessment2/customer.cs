using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Configuration;

/// Rory Miller 40296410
/// Assessment 2
/// Software Development 2

namespace assessment2
{
    
    
    class customer
      
    {
        

        private string fname;
        private string lname;
        private int passnum;
        private int cref;
        private int housenum;
        private string postcode;
        private int age;
        private DateTime startdate;
        
        public string Cfname
        {
            set { fname = value; }
            get { return fname; }



        }
        public string Clname
        {

            set { lname = value; }
            get { return lname; }



        }

        public int Cref
        {

            set { this.cref = value; }
            get { return this.cref; }

        }

        public int Passnum
        {

            set { this.passnum = value; }
            get { return this.passnum; }


        }

        public int Housenum
        {

            set { this.housenum = value; }
            get { return this.housenum; }

        }
        public string Postcode
        {

            set { postcode = value; }
            get { return postcode; }



        }
        public int Age
        {

            set { this.age = value; }
            get { return this.age; }

        }
        
        public string DT
        {
            get { return startdate.ToString(); }
            set { DateTime.TryParse(value, out startdate); }

        }
                
        
       
        
    }
    }


