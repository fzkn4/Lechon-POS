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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lechon_POS
{
    public partial class food_package_transaction : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public food_package_transaction()
        {
            InitializeComponent();
        }

        private string format_number(string number)
        {
            return number;
        }
        double total = 0;
        int option1 = 0;
        int option2 = 0;
        int option3 = 0;
        int option4 = 0;
        int option5 = 0;
        int option6 = 0;

        private void calculate(int op1, int op2, int op3, int op4, int op5, int op6)
        {
            total = (op1 * 2500) + (op2 * 3300) + (op3 * 4200) + (op4 * 4800) + (op5 * 5700) + (op6 * 6600);
            total_amount.Text = format_number(total.ToString());
        }

        private void seta_ValueChanged(object sender, EventArgs e)
        {
            option1 = (int)seta.Value;
            calculate(option1, option2, option3, option4, option5, option6);
        }

        private void setb_ValueChanged(object sender, EventArgs e)
        {
            option2 = (int)setb.Value;
            calculate(option1, option2, option3, option4, option5, option6);
        }

        private void setc_ValueChanged(object sender, EventArgs e)
        {
            option3 = (int)setc.Value;
            calculate(option1, option2, option3, option4, option5, option6);
        }

        private void setd_ValueChanged(object sender, EventArgs e)
        {
            option4 = (int)setd.Value;
            calculate(option1, option2, option3, option4, option5, option6);
        }

        private void sete_ValueChanged(object sender, EventArgs e)
        {
            option5 = (int)sete.Value;
            calculate(option1, option2, option3, option4, option5, option6);
        }

        private void setf_ValueChanged(object sender, EventArgs e)
        {
            option6 = (int)setf.Value;
            calculate(option1, option2, option3, option4, option5, option6);
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
                cmd.Parameters.Add("@product_name", MySqlDbType.VarChar, 255).Value = "Food Package";
                cmd.Parameters.Add("@payment_amount", MySqlDbType.Double).Value = Convert.ToDouble(payment.Text);
                cmd.Parameters.Add("@change", MySqlDbType.Double).Value = Convert.ToDouble(change.Text);

                addCustomer();
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

        private void addCustomer()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "INSERT INTO customers (customerName, numberOfOrders, totalOrderAmount, lastPurchaseDate) VALUES (@customerName, @numberOfOrders, @totalOrderAmount, @lastPurchaseDate) ON DUPLICATE KEY UPDATE numberOfOrders = numberOfOrders+@numberOfOrders, totalOrderAmount = totalOrderAmount+@totalOrderAmount, lastPurchaseDate = @lastPurchaseDate";
                cmd.Parameters.Add("@customerName", MySqlDbType.VarChar, 255).Value = first_letter_capital(customer_name.Text);
                cmd.Parameters.Add("@numberOfOrders", MySqlDbType.Int32).Value = 1;
                cmd.Parameters.Add("@totalOrderAmount", MySqlDbType.Double).Value = Convert.ToInt32(total_amount.Text);
                cmd.Parameters.Add("@lastPurchaseDate", MySqlDbType.DateTime).Value = DateTime.Now;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Transaction was successful.");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn1.Close();
        }
    }
}
