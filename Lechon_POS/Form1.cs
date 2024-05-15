using Guna.Charts.WinForms;
using Guna.UI2.WinForms;
using MySqlConnector;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace Lechon_POS
{
    public partial class Form1 : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            display_revenue_chart(revenue_chart);
            display_sales_cat_chart(sales_cat);
            hide_panels();
            dashboard_panel.Visible = true;
            updateTable();
            get_total_sales();
            user_name.Text = login.name;
            user_position.Text = login.user_position;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }


        private void selected_button(Guna2Button selected, Guna2Button button2, Guna2Button button3, Guna2Button button4)
        {
            selected.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void hide_panels()
        {
            dashboard_panel.Visible = false;
            product_panel.Visible = false;
            sales_panel.Visible = false;
            customer_panel.Visible = false;
        }

        private void dashboard_Click(object sender, EventArgs e)
        {
            selected_button(dashboard, customer, sales, products);
            hide_panels();
            dashboard_panel.Visible = true;
            revenue_chart.Update();
            sales_cat.Update();

        }

        private void products_Click(object sender, EventArgs e)
        {
            selected_button(products, customer, sales, dashboard);
            hide_panels();
            product_panel.Visible = true;

        }

        private void sales_Click(object sender, EventArgs e)
        {
            hide_panels();
            selected_button(sales, customer, products, dashboard);
            sales_panel.Visible = true;
            transaction_table.ClearSelection();
        }

        private void customer_Click(object sender, EventArgs e)
        {
            selected_button(customer, sales, products, dashboard);
            hide_panels();
            customer_panel.Visible = true;
        }

        private void display_revenue_chart(Guna.Charts.WinForms.GunaChart chart)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec" };

            //Chart configuration 
            chart.XAxes.GridLines.Display = false;
            var r = new Random();
            for (int i = 0; i < months.Length; i++)
            {
                //random number
                int num = r.Next(10, 100);

                revenue_dataset.DataPoints.Add(months[i], num);
            }

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(revenue_dataset);

            //An update was made to re-render the chart
            chart.Update();
        }

        private void display_sales_cat_chart(Guna.Charts.WinForms.GunaChart chart)
        {
            chart.Update();
        }

        private void lechon_belly_img_Click(object sender, EventArgs e)
        {
            lechon_belly_transaction window = new lechon_belly_transaction();
            window.ShowDialog();
            updateTable();
            get_total_sales();
        }

        private void food_package_img_Click(object sender, EventArgs e)
        {
            food_package_transaction window = new food_package_transaction();
            window.ShowDialog();
            updateTable();
            get_total_sales();

        }

        private void whole_lechon_img_Click(object sender, EventArgs e)
        {
            lechon_transaction window = new lechon_transaction();
            window.ShowDialog();
            updateTable();
            get_total_sales();

        }


        private void updateTable()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT transactionID as 'ID', customerName as 'Customer', productName as 'Type of Product' , paymentAmount as 'Payment Amount' , paymentChange as 'Change' , transactionDate as 'Date' from transaction", con1);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                transaction_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();

                update_customer_table();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void update_customer_table()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT customerName as 'Customer', numberOfOrders as 'Number of Orders' , totalOrderAmount as 'Total Purchases' , lastPurchaseDate as 'Last Purchase' from customers", con1);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                customer_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void transaction_table_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            transaction_table.Rows[0].Selected = false;
        }

        private void get_total_sales()
        {
            double total = 0;
            int orders = 0;
            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT totalAmount from transaction ";
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    orders++;
                    total += dr.GetDouble("totalAmount");
                }
                display_total_sales.Text = format_number(total.ToString());
                display_total_orders.Text = format_number(orders.ToString());
                con1.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string format_number(string number)
        {
            // Remove any non-numeric characters and commas from the input
            string input = Regex.Replace(number, "[^0-9]", "");

            // Format the input by inserting commas every 3 digits from the right
            string formattedInput = "";
            for (int i = input.Length - 1, j = 1; i >= 0; i--, j++)
            {
                formattedInput = input[i] + formattedInput;
                if (j % 3 == 0 && i != 0)
                {
                    formattedInput = "," + formattedInput;
                }
            }

            return formattedInput;
        }

        int id = 0;
        private void transaction_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (transaction_table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                try
                {

                    id = Convert.ToInt32(transaction_table.Rows[e.RowIndex].Cells[0].Value.ToString());

                }
                catch (Exception ex)
                {
                }
            }
        }

        private void delete_transaction()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "delete from transaction where transactionID=@id";
                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted Successfully");
                updateTable();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn1.Close();
        }

        private void confirm_transaction_Click(object sender, EventArgs e)
        {
            delete_transaction();
            updateTable();
            get_total_sales();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void logout_Click(object sender, EventArgs e)
        {
            this.Hide();
            login window = new login();
            window.Show();
        }

        private void customer_table_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            customer_table.Rows[0].Selected = false;

        }
        public static string customer_name = "";
        private void customer_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customer_table.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                try
                {

                    customer_name = (customer_table.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
           EditCustomer window = new EditCustomer();
            window.ShowDialog();

        }
    }
}
