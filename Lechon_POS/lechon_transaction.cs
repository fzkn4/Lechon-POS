using Guna.UI2.WinForms;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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


        private void record_transaction()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "Insert INTO transaction set customerName=@customer_name, totalAmount=@total_amount, transactionDate=@transaction_date, productName=@product_name, paymentAmount=@payment_amount, paymentChange=@change;";
                cmd.Parameters.Add("@customer_name", MySqlDbType.VarChar, 255).Value = first_letter_capital(customer_name.Text);
                cmd.Parameters.Add("@total_amount", MySqlDbType.Double).Value = Convert.ToDouble(total_amount.Text);
                cmd.Parameters.Add("@transaction_date", MySqlDbType.Date).Value = DateTime.Now;
                cmd.Parameters.Add("@product_name", MySqlDbType.VarChar, 255).Value = "Whole Lechon";
                cmd.Parameters.Add("@payment_amount", MySqlDbType.Double).Value = Convert.ToDouble(payment.Text);
                cmd.Parameters.Add("@change", MySqlDbType.Double).Value = Convert.ToDouble(change.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Transaction was successful.");

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
                    total = Convert.ToDouble(kilo_amount.Text.ToString()) * 600;
                    total_amount.Text = format_number(total.ToString());


                }
                else if (kilo_amount.Text.Length == 0)
                {
                    total_amount.Text = "0.00";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void payment_TextChanged(object sender, EventArgs e)
        {

            if (payment.Text.Length > 0)
            {
               change.Text = format_number((Convert.ToDouble(payment.Text) - Convert.ToDouble(total_amount.Text)).ToString());

            }
            else
            {
                change.Text = "0.00";
            }

        }

        public static string format_number(string number)
        {
            return number;
        }

        private void confirm_transaction_Click(object sender, EventArgs e)
        {
            if (Convert.ToDouble(payment.Text) < Convert.ToDouble(total_amount.Text))
            {
                MessageBox.Show("Insufficient payment.");
            }
            else
            {
                record_transaction();
            }
        }

        private string first_letter_capital(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        private void kilo_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as Guna2TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
