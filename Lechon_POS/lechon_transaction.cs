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
    public partial class lechon_transaction : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public lechon_transaction()
        {
            InitializeComponent();
        }

        private void lechon_transaction_Load(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }


        private void add_blotter()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "Insert INTO blotter(customer_name, total_amount, transaction_date, product_name)VALUES(@customer_name, @total_amount, @transaction_date, @product_name)";
                //cmd.Parameters.Add("@caseID", MySqlDbType.Int32).Value = set_blotter_id();

                cmd.ExecuteNonQuery();


                MessageBox.Show("Blotter recorded.");
                Form1 main_page = new Form1();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn1.Close();
            this.Close();
        }


        double total = 0;
        private void kilo_amount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (kilo_amount.Text.Length > 0)
                {
                    total =  Convert.ToDouble(kilo_amount.Text.ToString()) * 600;
                    total_amount.Text = total.ToString();
                }else if (kilo_amount.Text.Length == 0)
                {
                    total_amount.Text = "0.00";
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
