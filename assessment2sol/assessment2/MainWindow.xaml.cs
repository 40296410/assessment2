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



namespace assessment2
{
    /// Rory Miller 40296410
    /// Assessment 2
    /// Software Development 2
    public partial class MainWindowForm : Window
    {

        //creating connection to my local database
        SqlConnection cn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Rory Miller\Desktop\soft dev\assessment2sol\assessment2\bookinglist.mdf;Integrated Security=True");
        
        //to use sql command
        SqlCommand cmd = new SqlCommand();

        
        //referencing other class
        customer cust = new customer();
        int extra_break;
        int extra_meal;
        int extra_car;
        
        int guest_no;
        public MainWindowForm()
        {
            InitializeComponent();
            cmd.Connection = cn;
            
            
            bcustref_txt.IsEnabled = false;
            DisGuest();

            
        }

        private void createcust()
        {
            //opeing local database connection
            cn.Open();
            
            //typing out sql command to use on databse
            cmd.CommandText = "insert into customer (First_name, Last_name, House_no, Postcode, Age, Passport_no) values ('" + fnametxt.Text + "','" + Lnametxt.Text + "','" + chname_txt.Text + "','" + cpcode_txt.Text + "','" + cage_txt.Text + "','" + cpnum_txt.Text + "')";

            //executing the command
            cmd.ExecuteNonQuery();

            cmd.CommandText = "select customerid from customer where customerid = (select Max(customerid) from customer)";
            cmd.ExecuteNonQuery();

            //reading the database and picking out information from it
            using (SqlDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    //assigning data to textboxes
                    custreftxt.Text = (read["customerid"].ToString());
                    bcustref_txt.Text =  (read["customerid"].ToString());
                }
                //stop reading database
                read.Close();

            }
                cmd.Clone();

            //validation the method worked
            MessageBox.Show("Customer Reference: "+custreftxt.Text+ " added");
            //close connection
            cn.Close();
            //emptying all the textboxes
            fnametxt.Text = "";
            Lnametxt.Text = "";
            chname_txt.Text = "";
            cpcode_txt.Text = "";
            cage_txt.Text = "";
            cpnum_txt.Text = "";
            
            

        }
        private void custsearch()
        {
            //open connection
            cn.Open();
            //sql query
            cmd.CommandText = "select * from [customer] where customerid='" + custreftxt.Text + "'";
            
            
            //reading database and executing my query
            using (SqlDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    //assigning textboxes values from database
                   fnametxt.Text = (read["First_name"].ToString());
                   Lnametxt.Text = (read["Last_name"].ToString());
                   chname_txt.Text = (read["House_no"].ToString());
                   cpcode_txt.Text = (read["Postcode"].ToString());
                   cage_txt.Text = (read["Age"].ToString());
                   cpnum_txt.Text = (read["Passport_no"].ToString());
                   bcustref_txt.Text = (read["customerid"].ToString());


                }
                read.Close();


                //close connection
                cn.Close();
            }



        }
        private void custdelete()
        {
            //reading from database
            using (SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) from booking3 where cust_ref like @cust_ref ", cn))
            {
                //open connection    
                cn.Open();
                //sql command to count cust_ref, dont delete customers with bookings   
                sqlCommand.Parameters.AddWithValue("@cust_ref", custreftxt.Text);                    
                int userCount = (int) sqlCommand.ExecuteScalar();
                            //if user has booking don't delete            
                            if(userCount > 0 )
                            {
                                MessageBox.Show("Record not deleted");
                            ClearCustomer();
                            }
                            //if user doesnt then delete customer            
                            else
                            {
                                cmd.CommandText = "delete from customer where customerid='" + custreftxt.Text + "'";
                                cmd.ExecuteNonQuery();
                                
            
                                 MessageBox.Show("Record deleted");

                            }
            }
            
            //close connection
            cn.Close();

        }
        private void custedit()
        {
            //open connection
            cn.Open();
            //sql query
            cmd.CommandText = "update customer set First_name ='" + fnametxt.Text + "', Last_name = '" + Lnametxt.Text + "', House_no = '"+chname_txt.Text+"', Postcode = '"+cpcode_txt.Text+ "', Age='"+cage_txt.Text+"', Passport_no='"+cpnum_txt.Text+"' where customerid='" + custreftxt.Text + "'";
            //execute query
            cmd.ExecuteNonQuery();
            //close connection
            cn.Close();
            MessageBox.Show("record updated");
            //clear all textboxes
            fnametxt.Text = "";
            Lnametxt.Text = "";
            chname_txt.Text = "";
            cpcode_txt.Text = "";
            cage_txt.Text = "";
            cpnum_txt.Text = "";

            
        }
        private void makebook()
        {

            //validation for date box
            DateTime datevalue = default(DateTime);
            if (DateTime.TryParse(barrivdate_txt.Text, out datevalue))
            {
                cn.Open();
                //sql query executed
                cmd.CommandText = "insert into booking3 (arr_date, No_nights, notes, cust_ref,extra_meal,extra_break, extra_car) values ('" + barrivdate_txt.Text + "','" + bnonights_txt.Text + "','" + bdiet_txt.Text + "','" + bcustref_txt.Text + "','" + extra_meal + "','" + extra_break + "','" + extra_car + "')";
                cmd.ExecuteNonQuery();
                //sql quey executed
                cmd.CommandText = "select booking_id from booking3 where booking_id = (select Max(booking_id) from booking3)";
                cmd.ExecuteNonQuery();
                //reading from databse while using sql query
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {

                        bbookref_txt.Text = (read["booking_id"].ToString());

                    }
                    read.Close();

                }

                cmd.Clone();
                MessageBox.Show("Booking Reference: " + bbookref_txt.Text + " added");
                cn.Close();
            }
            else
            {

                MessageBox.Show("Incorrect date");
            }

        }

        
        private void editbook()
        {
            //conneciton opened
            cn.Open();
            //sql query wrote and executed
            cmd.CommandText = "update booking3 set arr_date ='" + barrivdate_txt.Text + "', no_nights ='"+bnonights_txt.Text+"', notes='" +bdiet_txt.Text+"' where booking_id='" + bbookref_txt.Text + "'";
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Record updated");
            
            

        }
        private void searchbook()
        {
            //validation book ref must be there
            if (bbookref_txt.Text != "")
            {
                cn.Open();
                
                //sql query
                cmd.CommandText = "select * from [booking3] where booking_id='" + bbookref_txt.Text + "'";
                
                // valus from switch statements
                int brek = 0;
                int meal = 0;
                int car = 0;


                //reading from database applying sql query
                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {

                        bcustref_txt.Text = (read["cust_ref"].ToString());
                        barrivdate_txt.Text = (read["arr_date"].ToString());
                        bnonights_txt.Text = (read["no_nights"].ToString());
                        bdiet_txt.Text = (read["notes"].ToString());
                        brek = Convert.ToInt32(read["extra_break"]);
                        meal = Convert.ToInt32(read["extra_meal"]);
                        car = Convert.ToInt32(read["extra_car"]);
                        custreftxt.Text = (read["cust_ref"].ToString());

                    }
                    read.Close();
                    //switch statment for checkboxes depending on how many are checked
                    switch (brek)
                    {
                        case 1:
                            Break_box.IsChecked = true;
                            break;
                        case 2:
                            Break_box.IsChecked = true;
                            Break_box_g1.IsChecked = true;
                            break;
                        case 3:
                            Break_box.IsChecked = true;
                            Break_box_g1.IsChecked = true;
                            Break_box_g2.IsChecked = true;
                            break;
                        case 4:
                            Break_box.IsChecked = true;
                            Break_box_g1.IsChecked = true;
                            Break_box_g2.IsChecked = true;
                            Break_box_g3.IsChecked = true;
                            break;
                        case 5:
                            Break_box.IsChecked = true;
                            Break_box_g1.IsChecked = true;
                            Break_box_g2.IsChecked = true;
                            Break_box_g3.IsChecked = true;
                            Break_box_g4.IsChecked = true;
                            break;


                    }
                    switch (meal)
                    {
                        case 1:
                            Meal_box.IsChecked = true;
                            break;
                        case 2:
                            Meal_box.IsChecked = true;
                            Meal_box_g1.IsChecked = true;
                            break;
                        case 3:
                            Meal_box.IsChecked = true;
                            Meal_box_g1.IsChecked = true;
                            Break_box_g2.IsChecked = true;
                            break;
                        case 4:
                            Meal_box.IsChecked = true;
                            Meal_box_g1.IsChecked = true;
                            Meal_box_g2.IsChecked = true;
                            Meal_box_g3.IsChecked = true;
                            break;
                        case 5:
                            Meal_box.IsChecked = true;
                            Meal_box_g1.IsChecked = true;
                            Meal_box_g2.IsChecked = true;
                            Meal_box_g3.IsChecked = true;
                            Meal_box_g4.IsChecked = true;
                            break;


                    }
                    switch (car)
                    {
                        case 50:
                            Car_box.IsChecked = true;
                            break;
                    }

                }
                cn.Close();
            }
            else 
            {
                MessageBox.Show("Enter Reference number ");
            }
            
            

        

        }

        private void deletebook()
        {
            //validation
            if (bbookref_txt.Text != "")
            {
                //opening connection and applying sql queries which search with booking reference
                cn.Open();
                cmd.CommandText = "delete from booking3 where booking_id='" + bbookref_txt.Text + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from guest where booking_id='" + bbookref_txt.Text + "'";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "delete from Car_hire where booking_id='" + bbookref_txt.Text + "'";
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Booking Reference: " + bbookref_txt.Text + " deleted");

            }
            
            




        }
        private void addguests()
        {

            //switch statmemt for which guest has been ticked. adds only the guests that have been ticked
            
            cn.Open();
            switch(guest_no)
            {
                case 0: cmd.CommandText = "insert into guest DEFAULT VALUES";

                    break;
                
                case 1:
                    if (bg1fname_txt.Text != "" && bg1age_txt.Text != "" && bg1passnum_txt.Text != "")
                    {
                        cmd.CommandText = "insert into guest (guest1_name, guest1_age, guest1_pass ) values ('" + bg1fname_txt.Text + "','" + bg1age_txt.Text + "','" + bg1passnum_txt.Text + "')";
                    }
                    else 
                    {
                        MessageBox.Show("Insert valid information for guest 1");
                    }
                    break;
                case 2:
                    if (bg1fname_txt.Text != "" && bg1age_txt.Text != "" && bg1passnum_txt.Text != "" && bg2fname_txt.Text != "" && bg2age_txt.Text != "" && bg2passnum_txt.Text != "" )
                    {
                        cmd.CommandText = "insert into guest (guest1_name, guest1_age, guest1_pass, guest2_name, guest2_age, guest2_pass ) values ('" + bg1fname_txt.Text + "','" + bg1age_txt.Text + "','" + bg1passnum_txt.Text + "','" + bg2fname_txt.Text + "','" + bg2age_txt.Text + "','" + bg2passnum_txt.Text + "')";
                    }
                    break;

                case 3:
                    if (bg1fname_txt.Text != "" && bg1age_txt.Text != "" && bg1passnum_txt.Text != "" && bg2fname_txt.Text != "" && bg2age_txt.Text != "" && bg2passnum_txt.Text != "" && bg3fname_txt.Text != "" && bg3age_txt.Text != "" && bg3passnum_txt.Text != "")
                    {
                        cmd.CommandText = "insert into guest (guest1_name, guest1_age, guest1_pass, guest2_name, guest2_age, guest2_pass, guest3_name, guest3_age, guest3_pass ) values ('" + bg1fname_txt.Text + "','" + bg1age_txt.Text + "','" + bg1passnum_txt.Text + "','" + bg2fname_txt.Text + "','" + bg2age_txt.Text + "','" + bg2passnum_txt.Text + "','" + bg3fname_txt.Text + "','" + bg3age_txt.Text + "','" + bg3passnum_txt.Text + "')";
                    }
                    break;

                case 4:
                    if (bg1fname_txt.Text != "" && bg1age_txt.Text != "" && bg1passnum_txt.Text != "" && bg2fname_txt.Text != "" && bg2age_txt.Text != "" && bg2passnum_txt.Text != "" && bg3fname_txt.Text != "" && bg3age_txt.Text != "" && bg3passnum_txt.Text != "" && bg4fname_txt.Text != "" && bg4age_txt1.Text != "" && bg4passnum_txt.Text != "")
                    {
                        cmd.CommandText = "insert into guest (guest1_name, guest1_age, guest1_pass, guest2_name, guest2_age, guest2_pass, guest3_name, guest3_age, guest3_pass, guest4_name, guest4_age, guest4_pass ) values ('" + bg1fname_txt.Text + "','" + bg1age_txt.Text + "','" + bg1passnum_txt.Text + "','" + bg2fname_txt.Text + "','" + bg2age_txt.Text + "','" + bg2passnum_txt.Text + "','" + bg3fname_txt.Text + "','" + bg3age_txt.Text + "','" + bg3passnum_txt.Text + "','" + bg4fname_txt.Text + "','" + bg4age_txt1.Text + "','" + bg4passnum_txt.Text + "')";
                    }
                    break;
            }
            
            
            cmd.ExecuteNonQuery();
            cmd.Clone();
            
            
            
            
            cn.Close();
            

        }
        private void editguest()
        {
            cn.Open();

            //switch statment for edit, only edits the ones which have been checked then the sql querys are applied
            switch (guest_no)
            {
                                            //searches using booking reference then updates the new values
                case 1: cmd.CommandText = "update guest set guest1_name ='" + bg1fname_txt.Text + "', guest1_age ='" + bg1age_txt.Text + "', guest1_pass='" + bg1passnum_txt.Text + "' where booking_id='" + bbookref_txt.Text + "'";

                    break;
                case 2: cmd.CommandText = "update guest set guest1_name ='" + bg1fname_txt.Text + "', guest1_age ='" + bg1age_txt.Text + "', guest1_pass='" + bg1passnum_txt.Text + "', guest2_name ='" + bg2fname_txt.Text + "', guest2_age ='" + bg2age_txt.Text + "', guest2_pass='" + bg2passnum_txt.Text + "' where booking_id='" + bbookref_txt.Text + "'";

                    break;

                case 3: cmd.CommandText = "update guest set guest1_name ='" + bg1fname_txt.Text + "', guest1_age ='" + bg1age_txt.Text + "', guest1_pass='" + bg1passnum_txt.Text + "', guest2_name ='" + bg2fname_txt.Text + "', guest2_age ='" + bg2age_txt.Text + "', guest2_pass='" + bg2passnum_txt.Text + "', guest3_name ='" + bg3fname_txt.Text + "', guest3_age ='" + bg3age_txt.Text + "', guest3_pass='" + bg3passnum_txt.Text + "' where booking_id='" + bbookref_txt.Text + "'";

                    break;

                case 4: cmd.CommandText = "update guest set guest1_name ='" + bg1fname_txt.Text + "', guest1_age ='" + bg1age_txt.Text + "', guest1_pass='" + bg1passnum_txt.Text + "', guest2_name ='" + bg2fname_txt.Text + "', guest2_age ='" + bg2age_txt.Text + "', guest2_pass='" + bg2passnum_txt.Text + "', guest3_name ='" + bg3fname_txt.Text + "', guest3_age ='" + bg3age_txt.Text + "', guest3_pass='" + bg3passnum_txt.Text + "', guest4_name ='" + bg4fname_txt.Text + "', guest4_age ='" + bg4age_txt1.Text + "', guest4_pass='" + bg4passnum_txt.Text + "' where booking_id='" + bbookref_txt.Text + "'";

                    break;
            }
            
            
            cmd.ExecuteNonQuery();
            cn.Close();
            
            
            

        }

        private void searchguest()
        {
            if (bbookref_txt.Text != "")
            {

                cn.Open();
               //reads then searches the databse for bookig reference then puts the data in apporopiate textboxes
                cmd.CommandText = "select * from [guest] where booking_id='" + bbookref_txt.Text + "'";
                
                


                using (SqlDataReader read = cmd.ExecuteReader())
                {
                    while (read.Read())
                    {
                        
                        //validation for the searched items then adds data to the textboxes
                        if (read["guest1_name"].ToString() != "")
                        {
                            bg1fname_txt.Text = (read["guest1_name"].ToString());
                            bg1age_txt.Text = (read["guest1_age"].ToString());
                            bg1passnum_txt.Text = (read["guest1_pass"].ToString());
                            guest1_check.IsChecked = true;
                        }

                        if ((read["guest2_name"].ToString()) != "")
                        {
                            bg2fname_txt.Text = (read["guest2_name"].ToString());
                            bg2age_txt.Text = (read["guest2_age"].ToString());
                            bg2passnum_txt.Text = (read["guest2_pass"].ToString());
                            guest2_check.IsChecked = true;

                        }
                        if ((read["guest3_name"].ToString()) != "")
                        {
                            bg3fname_txt.Text = (read["guest3_name"].ToString());
                            bg3age_txt.Text = (read["guest3_age"].ToString());
                            bg3passnum_txt.Text = (read["guest3_pass"].ToString());
                            guest3_check.IsChecked = true;

                        }

                        if ((read["guest4_name"].ToString()) != "")
                        {
                            bg4fname_txt.Text = (read["guest4_name"].ToString());
                            bg4age_txt1.Text = (read["guest4_age"].ToString());
                            bg4passnum_txt.Text = (read["guest4_pass"].ToString());
                            guest4_check.IsChecked = true;
                        }

                    }
                    read.Close();



                    cn.Close();
                }
                
            }

        }
        private void adddriver()
        {
            //date validation
            DateTime dateValue = default(DateTime);
            //validation for car_hire, only works when checked
            cn.Open();
            if(Car_box.IsChecked == true)
            {
                if (DateTime.TryParse(strt_date.Text, out dateValue))
                {
                    
                        cmd.CommandText = "insert into Car_hire ( named_driver, start_date, nights) values ('" + dr_namebox.Text + "','" + strt_date.Text + "','" + end_date.Text + "')";
                        cmd.ExecuteNonQuery();
                        cmd.Clone();
                    
                }
                else
                {
                    MessageBox.Show("StartDate incorrect");
                }
             } 
            else
            {
                //inserts null into table
                cmd.CommandText = "insert into Car_hire DEFAULT VALUES";
                cmd.ExecuteNonQuery();
                cmd.Clone(); 

            }
        cn.Close();  
        }
        private void searchdriver()
        {
            cn.Open();
            //sql query which searches for providedm booking reference
            cmd.CommandText = "select * from [Car_hire] where booking_id='" + bbookref_txt.Text + "'";
           
            

            //reads and executes the database
            using (SqlDataReader read = cmd.ExecuteReader())
            {
                while (read.Read())
                {
                    //fills in textboxes with found results
                    dr_namebox.Text = (read["named_driver"].ToString());
                    strt_date.Text = (read["Start_date"].ToString());
                    end_date.Text = (read["nights"].ToString());



                }
                read.Close();


            }
                cn.Close();


        }
        private void editdriver()
        {
            //sql statment which searches the booking reference then updates the found data with ones in the textboxes
            cn.Open();
            cmd.CommandText = "update Car_hire set booking_id ='"+bbookref_txt.Text+"'named_driver ='" + dr_namebox.Text + "', Start_date ='" + strt_date.Text + "', nights ='" + end_date.Text + "' where booking_id='" + bbookref_txt.Text + "'";
            cmd.ExecuteNonQuery();
            cn.Close();
            

        }

        private void InvCal()
        {
            //booking reference validation
            if (bbookref_txt.Text != "")
            {
                //values to work out the costs
                int nights;
                int total_cost = 0;
                int cost_night = 0;
                int cost_extra = 0;
                searchbook();
                searchguest();
                searchdriver();
                countextras();
                custsearch();
                
                //method to work out room cost depending on age and how many guests
                if (0 < int.Parse(cage_txt.Text) && 18 <= int.Parse(cage_txt.Text) )
                {
                    cost_night += 50;

                }
                else if (0 < int.Parse(cage_txt.Text) && 18 > int.Parse(cage_txt.Text) )
                {
                    cost_night += 30;

                }
                
                if (!string.IsNullOrEmpty(bg1age_txt.Text))
                {
                    if (0 < int.Parse(bg1age_txt.Text) && 18 <= int.Parse(bg1age_txt.Text))
                    {
                        cost_night += 50;

                    }
                    else if (0 < int.Parse(bg1age_txt.Text) && 18 > int.Parse(bg1age_txt.Text) && bg1fname_txt.Text != "")
                    {
                        cost_night += 30;

                    }
                }
                
                if (!string.IsNullOrEmpty(bg2age_txt.Text))
                {
                    if (0 < int.Parse(bg2age_txt.Text) && 18 <= int.Parse(bg2age_txt.Text))
                    {
                        cost_night += 50;

                    }
                    else if (0 < int.Parse(bg2age_txt.Text) && 18 > int.Parse(bg2age_txt.Text) && bg2fname_txt.Text.Length == 0)
                    {
                        cost_night += 30;

                    }
                }
               
                if (!string.IsNullOrEmpty(bg3age_txt.Text))
                {
                    if (0 < int.Parse(bg3age_txt.Text) && 18 <= int.Parse(bg3age_txt.Text) && !string.IsNullOrWhiteSpace(bg3age_txt.Text))
                    {
                        cost_night += 50;

                    }
                    else if (0 < int.Parse(bg3age_txt.Text) && 18 > int.Parse(bg3age_txt.Text) && !string.IsNullOrWhiteSpace(bg3age_txt.Text))
                    {
                        cost_night += 30;

                    }
                }
                
                if (!string.IsNullOrEmpty(bg4age_txt1.Text))
                {
                    if (0 < int.Parse(bg4age_txt1.Text) && 18 <= int.Parse(bg4age_txt1.Text) && bg4fname_txt != null)
                    {
                        cost_night += 50;

                    }
                    if (0 < int.Parse(bg4age_txt1.Text) && 18 > int.Parse(bg4age_txt1.Text) && bg4fname_txt != null)
                    {
                        cost_night += 30;

                    }
                }
                    nights = Convert.ToInt32(end_date.Text);
                    //method for working out room cost extra cost and total cost
                    cost_extra = (5 * extra_break) + (15 * extra_meal) + (nights*50);
                    cost_night = cost_night * int.Parse(bnonights_txt.Text);
                    total_cost = cost_night + cost_extra;    
               //displaying the newly calculated costs in a new window in labels 
                InvoiceWindow popup = new InvoiceWindow();              
                popup.Inv_roomcost.Content = (cost_night.ToString());
                popup.Inv_extracost.Content = cost_extra.ToString();
                popup.Inv_totalcost.Content = total_cost.ToString();
                popup.ShowDialog();
            }
            
        }
 
       
        //click event
        private void createcust_click(object sender, RoutedEventArgs e)
        {
            //validation and counting extras i.e how many meals or breakfast are ticked
            countextras();
            if (fnametxt.Text != "" && Lnametxt.Text != "" && chname_txt.Text != "" && cpcode_txt.Text != "" && cage_txt.Text != "" && cpnum_txt.Text != "")
            {
                if(cpnum_txt.Text.Length <= 10 )
                {
                    createcust();
                    ClearCustomer();
            
                }
                else
                {
                    MessageBox.Show("Passport Number invalid");
                }
           }
            else
            {
                MessageBox.Show("information invalid");
            }
        }
        //click event
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            custsearch();                      
        }
        //clcik event
        private void Editcust_click(object sender, RoutedEventArgs e)
        {
            //call method
            custedit();
            ClearCustomer();
        }
        //click event
        private void Deletecust_click(object sender, RoutedEventArgs e)
        {
            custdelete();
            ClearCustomer();
        }
        //click event
        private void makebookbutt_click(object sender, RoutedEventArgs e)
        {

            //null validation
            if (bcustref_txt.Text != "" && barrivdate_txt.Text !="" && bnonights_txt.Text != "")
            {
                //date validation
                DateTime datevalue = default(DateTime);
                if (DateTime.TryParse(barrivdate_txt.Text, out datevalue))
                {
                    //call methods facade design pattern
                    countextras();
                    makebook();
                    addguests();
                    adddriver();
                    ClearBoxes_Book();
                    DisGuest();
                    MessageBox.Show("Booking: " + bbookref_txt.Text + " added");
                }
                else
                {
                    MessageBox.Show("Invalid date given");

                }
            }
            else 
            {
                MessageBox.Show("Invalid Information given");
            }
        }

        

        
        //click event
        private void Deletebooking_click(object sender, RoutedEventArgs e)
        {
            if (bbookref_txt.Text != "")
            {
                //call methods
                deletebook();
                ClearBoxes_Book();
                DisGuest();
            }
        }

        private void Editbook_click(object sender, RoutedEventArgs e)
        {
            //null validation
            if (bbookref_txt.Text != "")
            {
                //call methods
                editbook();
                editguest();
                editdriver();
                ClearBoxes_Book();
                DisGuest();
                MessageBox.Show("Booking: " + bbookref_txt.Text + " updated");
            }
            else
            {
                MessageBox.Show("Invalid Booking Reference");

            }
        }

        

        private void searchbutt_Click(object sender, RoutedEventArgs e)
        { 
           //call methods
            searchbook();
           searchguest();
           searchdriver();
           
        }

        

        private void countextras()
        {
            //counts checked checkboxes for meals and breakfast
            if(Break_box.IsChecked == true)
            {
                extra_break = extra_break + 1;
                

            }
            if ( Break_box_g1.IsChecked == true)
            {
                extra_break = extra_break + 1;


            }
            if (Break_box_g2.IsChecked == true)
            {
                extra_break = extra_break + 1;


            }
            if (Break_box_g3.IsChecked == true)
            {
                extra_break = extra_break + 1;


            }
            if (Break_box_g4.IsChecked == true)
            {
                extra_break = extra_break + 1;
            }

            if(Car_box.IsChecked ==true)
           {
                extra_car = 50;
           }
            if(Meal_box.IsChecked ==true)
            {
                extra_meal = extra_meal + 1;

            }
            if (Meal_box_g1.IsChecked == true)
            {
                extra_meal = extra_meal + 1;

            }
                           
            if (Meal_box_g2.IsChecked == true)
            {
                extra_meal = extra_meal + 1;

            }
                
            if (Meal_box_g3.IsChecked == true)
            {
                extra_meal = extra_meal + 1;

            }
         
                
            if (Meal_box_g4.IsChecked == true)
            {
                extra_meal = extra_meal + 1;
                

            }
        
        }

        private void ClearCustomer()
        {
            //clear textboxes
            fnametxt.Text = string.Empty;
            Lnametxt.Text = string.Empty;
            cpcode_txt.Text = string.Empty;
            chname_txt.Text = string.Empty;
            cpnum_txt.Text = string.Empty;
            custreftxt.Text = string.Empty;
            cage_txt.Text = string.Empty;

        }

        private void ClearBoxes_Book()
        {
            //clear textboxes
            bcustref_txt.Text = string.Empty;
            bbookref_txt.Text = string.Empty;
            barrivdate_txt.Text = string.Empty;
            bnonights_txt.Text = string.Empty;
            bdiet_txt.Text = string.Empty;
            bg1fname_txt.Text = string.Empty;
            bg1age_txt.Text = string.Empty;
            bg1passnum_txt.Text = string.Empty;
            bg2fname_txt.Text = string.Empty;
            bg2age_txt.Text = string.Empty;
            bg2passnum_txt.Text = string.Empty;
            bg3fname_txt.Text = string.Empty;
            bg3age_txt.Text = string.Empty;
            bg3passnum_txt.Text = string.Empty;
            bg4fname_txt.Text = string.Empty;
            bg4age_txt1.Text = string.Empty;
            bg4passnum_txt.Text = string.Empty;
            dr_namebox.Text = string.Empty;
            strt_date.Text = string.Empty;
            end_date.Text = string.Empty;
            Meal_box.IsChecked = false;
            Break_box.IsChecked = false;
            Car_box.IsChecked = false;


        }

        private void DisGuest()
        {

            //disable guest options
            guest1_check.IsChecked = false;
            bg1fname_txt.IsEnabled = false;
            bg1age_txt.IsEnabled = false;
            bg1passnum_txt.IsEnabled = false;
            Meal_box_g1.IsEnabled = false;
            Break_box_g1.IsEnabled = false;
            bg2fname_txt.IsEnabled = false;
            bg2age_txt.IsEnabled = false;
            bg2passnum_txt.IsEnabled = false;
            Meal_box_g2.IsEnabled = false;
            Break_box_g2.IsEnabled = false;
            bg3fname_txt.IsEnabled = false;
            bg3age_txt.IsEnabled = false;
            bg3passnum_txt.IsEnabled = false;
            Meal_box_g3.IsEnabled = false;
            Break_box_g3.IsEnabled = false;
            bg4fname_txt.IsEnabled = false;
            bg4age_txt1.IsEnabled = false;
            bg4passnum_txt.IsEnabled = false;
            Meal_box_g4.IsEnabled = false;
            Break_box_g4.IsEnabled = false;
            guest2_check.IsEnabled = false;
            guest3_check.IsEnabled = false;
            guest4_check.IsEnabled = false;
            dr_namebox.IsEnabled = false;
            strt_date.IsEnabled = false;
            end_date.IsEnabled = false;
        }

        private void InvButton_Click(object sender, RoutedEventArgs e)
        {
            InvCal();
        }

        private void Guest1_Check_Checked(object sender, RoutedEventArgs e)
        {
            //validation for guests, cant create guest 3 without guest 2
                guest2_check.IsEnabled = true;
                bg1fname_txt.IsEnabled = true;
                bg1age_txt.IsEnabled = true;
                bg1passnum_txt.IsEnabled = true;
                Meal_box_g1.IsEnabled = true;
                Break_box_g1.IsEnabled = true;
                guest_no += 1;
            
            
        }

        private void Guest1_Uncheckclick(object sender, RoutedEventArgs e)
        {

                guest2_check.IsEnabled = true;
                bg1fname_txt.IsEnabled = false;
                bg1age_txt.IsEnabled = false;
                bg1passnum_txt.IsEnabled = false;
                Meal_box_g1.IsEnabled = false;
                Break_box_g1.IsEnabled = false;
                guest_no -= 1;
            
        }

        private void guest2_unchecked(object sender, RoutedEventArgs e)
        {
            guest3_check.IsEnabled = false;
            bg2fname_txt.IsEnabled = false;
            bg2age_txt.IsEnabled = false;
            bg2passnum_txt.IsEnabled = false;
            Meal_box_g2.IsEnabled = false;
            Break_box_g2.IsEnabled = false;
            guest_no -= 1;
        }

        private void guest2_checked(object sender, RoutedEventArgs e)
        {
            guest3_check.IsEnabled = true;
            bg2fname_txt.IsEnabled = true;
            bg2age_txt.IsEnabled = true;
            bg2passnum_txt.IsEnabled = true;
            Meal_box_g2.IsEnabled = true;
            Break_box_g2.IsEnabled = true;
            guest_no+=1;
        }

        private void guest3_checked(object sender, RoutedEventArgs e)
        {
            guest4_check.IsEnabled = true;
            bg3fname_txt.IsEnabled = true;
            bg3age_txt.IsEnabled = true;
            bg3passnum_txt.IsEnabled = true;
            Meal_box_g3.IsEnabled = true;
            Break_box_g3.IsEnabled = true;
            guest_no += 1;
        }

        private void guest3_unchecked(object sender, RoutedEventArgs e)
        {
            guest4_check.IsEnabled = false;
            bg3fname_txt.IsEnabled = false;
            bg3age_txt.IsEnabled = false;
            bg3passnum_txt.IsEnabled = false;
            Meal_box_g3.IsEnabled = false;
            Break_box_g3.IsEnabled = false;
            guest_no -= 1;
        }

        private void guest4_checked(object sender, RoutedEventArgs e)
        {
            bg4fname_txt.IsEnabled = true;
            bg4age_txt1.IsEnabled = true;
            bg4passnum_txt.IsEnabled = true;
            Meal_box_g4.IsEnabled = true;
            Break_box_g4.IsEnabled = true;
            guest_no += 1;
            
        }

        private void guest4_unchecked(object sender, RoutedEventArgs e)
        {
            bg4fname_txt.IsEnabled = false;
            bg4age_txt1.IsEnabled = false;
            bg4passnum_txt.IsEnabled = false;
            Meal_box_g4.IsEnabled = false;
            Break_box_g4.IsEnabled = false;
            guest_no -= 1;
        }

        private void IsNumber(TextCompositionEventArgs e)
        {
            //only numerical values allowed in textbox
            int result;

            if(!(int.TryParse(e.Text, out result) || e.Text == "."))
            {
                e.Handled = true;

            }

        }

        

        private void cage_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gage1_Ptxtinput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gage2_Ptxtinput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gage3_Ptxtinput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gage4_Ptxtinput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gpass1_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gpass2_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gpass3_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void gpass4_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void Bbook_ref_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void Nonights_PTI(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        private void car_check(object sender, RoutedEventArgs e)
        {
            dr_namebox.IsEnabled = true;
            strt_date.IsEnabled = true;
            end_date.IsEnabled = true;
        }

        private void car_uncheck(object sender, RoutedEventArgs e)
        {
            dr_namebox.IsEnabled = false;
            dr_namebox.Text = string.Empty;
            strt_date.IsEnabled = false;
            strt_date.Text = string.Empty;
            end_date.IsEnabled = false;
            end_date.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClearCustomer();
        }

        private void bclear_butt_Click(object sender, RoutedEventArgs e)
        {
            ClearBoxes_Book();
            DisGuest();
        }

        private void end_date_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumber(e);
        }

        

        
        

       
        

        
    }
}
 