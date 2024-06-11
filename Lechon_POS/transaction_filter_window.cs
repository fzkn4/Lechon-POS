using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lechon_POS
{
    public partial class transaction_filter_window : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public transaction_filter_window()
        {

            InitializeComponent();

            product_name.Items.Add("All");
            product_name.Items.Add("Whole Lechon");
            product_name.Items.Add("Lechon Belly");
            product_name.Items.Add("Food Package");
            product_name.Text = "All";

            customer.Items.Add("All");
            customer.Text = "All";

            from.Value = DateTime.Now;
            to.Value = DateTime.Now;
            get_customers();
        }

        private void get_customers()
        {
            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT customerName from customers";
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    customer.Items.Add(dr.GetString("customerName"));
                }
                con1.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            Form1.cancel_filter = true;
            this.Hide();
        }

        private void transaction_filter_Click(object sender, EventArgs e)
        {
            Form1.cancel_filter = false;
            Form1.datefrom = from.Value;
            Form1.dateto = to.Value;
            Form1.customername = customer.Text;
            Form1.prod = product_name.Text;
            this.Hide();
        }
        
    }
}
