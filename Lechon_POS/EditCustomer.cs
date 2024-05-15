using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lechon_POS
{
    public partial class EditCustomer : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public EditCustomer()
        {
            InitializeComponent();
            get_resident_details();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void kilo_amount_TextChanged(object sender, EventArgs e)
        {

        }

        private void edit_Click(object sender, EventArgs e)
        {
            update_customer_name();
        }


        string customerName = "";
        private void get_resident_details()
        {
            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT customerName from customers where customerName='" + Form1.customer_name + "'";
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    customerName = (dr["customerName"].ToString());
                }
                con1.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void update_customer_name()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "update customers set customerName=@customerName where customerName='" + customerName+ "'";
                cmd.Parameters.Add("@customerName", MySqlDbType.String).Value = first_letter_capital(name.Text);
                

                cmd.ExecuteNonQuery();

                MessageBox.Show("Updated Successfully.");
                Form1 main_page = new Form1();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn1.Close();
            this.Close();
        }

        private string first_letter_capital(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

    }
}
