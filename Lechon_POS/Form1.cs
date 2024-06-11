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
        static Dictionary<string, int> map = new Dictionary<string, int>();
        static Dictionary<string, int> category_map = new Dictionary<string, int>();
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
            hide_panels();
            dashboard_panel.Visible = true;
            updateTable();
            get_total_sales();
            user_name.Text = login.name;
            user_position.Text = login.user_position;

            //dropdown option year
            set_revenue_years();
            year_option.Text = DateTime.Now.Year.ToString();
        }

        //proper program termination 
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

            //total sales 
            get_total_sales();

            //dropdown option year
            set_revenue_years();
            year_option.Text = DateTime.Now.Year.ToString();

            //update charts
            init_map();
            display_revenue_chart(revenue_chart, year_option.Text);
            init_cat_map();
            display_sales_cat_chart(sales_cat, year_option.Text);

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
            updateTable();
            sumtable_sales();
        }

        private void customer_Click(object sender, EventArgs e)
        {
            selected_button(customer, sales, products, dashboard);
            hide_panels();
            customer_panel.Visible = true;
            update_customer_table();
        }

        private void init_map()
        {
            try
            {
                //init map
                map.Add("January", 0);
                map.Add("February", 0);
                map.Add("March", 0);
                map.Add("April", 0);
                map.Add("May", 0);
                map.Add("June", 0);
                map.Add("July", 0);
                map.Add("August", 0);
                map.Add("September", 0);
                map.Add("October", 0);
                map.Add("November", 0);
                map.Add("December", 0);

            }
            catch (Exception ex)
            {

            }
        }

        private void init_cat_map()
        {
            try
            {
                //init category map
                category_map.Add("Whole Lechon", 0);
                category_map.Add("Lechon Belly", 0);
                category_map.Add("Food Package", 0);
            }
            catch (Exception ex) { }
        }

        private void reset_cat_map()
        {
            foreach (string key in category_map.Keys)
            {
                category_map[key] = 0;
            }
        }

        private void set_revenue_years()
        {
            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT year(transactionDate) as 'year' from transaction";
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                    if (!year_option.Items.Contains(dr["year"].ToString()))
                    {
                        year_option.Items.Add(dr["year"].ToString());
                    }
                }
                con1.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void reset_map()
        {
            foreach (string key in map.Keys)
            {
                map[key] = 0;
            }
        }
        private string month_abr(string month)
        {
            if (month == "January")
            {
                return "Jan";
            }
            else if (month == "February")
            {
                return "Feb";
            }
            else if (month == "March")
            {
                return "Mar";
            }
            else if (month == "April")
            {
                return "Apr";
            }
            else if (month == "May")
            {
                return "May";
            }
            else if (month == "June")
            {
                return "Jun";
            }
            else if (month == "July")
            {
                return "Jul";
            }
            else if (month == "August")
            {
                return "Aug";
            }
            else if (month == "September")
            {
                return "Sept";
            }
            else if (month == "October")
            {
                return "Oct";
            }
            else if (month == "November")
            {
                return "Nov";
            }
            else if (month == "December")
            {
                return "Dec";
            }
            return "";
        }

        private void display_revenue_chart(Guna.Charts.WinForms.GunaChart chart, string year)
        {
            //reset everything before filling with values
            revenue_dataset.DataPoints.Clear();
            reset_map();
            chart.XAxes.GridLines.Display = false;


            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT date_format(transactionDate,'%M') as 'Month',  sum(totalAmount) as 'revenue' FROM transaction WHERE year(transactionDate)=year(@year) GROUP BY year(transactionDate), month(transactionDate), date_format(transactionDate,'%M') ORDER BY year(transactionDate), month(transactionDate)";
                cmd.Parameters.Add("@year", MySqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(year), 1, 1);
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    map[dr["Month"].ToString()] = Convert.ToInt32(dr["revenue"].ToString());
                }
                con1.Close();
                foreach (string key in map.Keys)
                {
                    revenue_dataset.DataPoints.Add(month_abr(key), map[key]);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(revenue_dataset);

            //An update was made to re-render the chart
            chart.Update();
        }



        private void display_sales_cat_chart(Guna.Charts.WinForms.GunaChart chart, string year)
        {
            double lechon = 0;
            double belly = 0;
            double pack = 0;
            cat_chart.DataPoints.Clear();
            reset_cat_map();

            //Chart configuration
            chart.XAxes.Display = false;
            chart.YAxes.Display = false;

            MySqlConnection con1 = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand();
            con1.Open();
            cmd.Connection = con1;
            try
            {
                cmd.CommandText = "SELECT SUM(totalAmount) AS 'sales', productName FROM transaction WHERE YEAR(transactionDate)=year(@year) GROUP BY productName;";
                cmd.Parameters.Add("@year", MySqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(year), 1, 1);
                cmd.CommandTimeout = 3600;
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    category_map[dr["productName"].ToString()] = Convert.ToInt32(dr["sales"].ToString());
                }
                con1.Close();
                foreach (string key in category_map.Keys)
                {
                    cat_chart.DataPoints.Add(key, category_map[key]);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Add a new dataset to a chart.Datasets
            chart.Datasets.Add(cat_chart);

            //An update was made to re-render the chart
            chart.Update();
            lechon = category_map["Whole Lechon"];
            belly = category_map["Lechon Belly"];
            pack = category_map["Food Package"];
            double total = belly + lechon + pack;

            //percentage display
            lechonP.Text = (String.Format("{0:0.##}", (lechon / total * 100)))+ "%";
            bellyP.Text = (String.Format("{0:0.##}", (belly / total * 100))) + "%";
            packP.Text = (String.Format("{0:0.##}", (pack / total * 100))) + "%";
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

        private void year_option_SelectedIndexChanged(object sender, EventArgs e)
        {
            init_map();
            display_revenue_chart(revenue_chart, year_option.Text);
            init_cat_map();
            display_sales_cat_chart(sales_cat, year_option.Text);
        }

        private void dashboard_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sales_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void transaction_filter_Click(object sender, EventArgs e)
        {
            transaction_filter_window window = new transaction_filter_window();
            window.ShowDialog();
            if (cancel_filter) return;
            if (prod == "All" && customername == "All")
            {
                filter_just_date();
            }
            else if (prod == "All" && customername != "All")
            {

                filter_customer();
            }
            else if (customername == "All" && prod != "All")
            {
                filter_product();
            }
            else if (prod != "All" && customername != "All")
            {

                filter_all();
            }
            sumtable_sales();

        }

        public static bool cancel_filter = false;
        public static string customername = "";
        public static string prod = "";
        public static DateTime datefrom = new DateTime();
        public static DateTime dateto = new DateTime();
        private void filter_just_date()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT transactionID as 'ID', customerName as 'Customer', productName as 'Type of Product' , paymentAmount as 'Payment Amount' , paymentChange as 'Change' , transactionDate as 'Date' from transaction where transactionDate>= @datefrom and transactionDate<=@dateto", con1);
                cmd.Parameters.Add("@datefrom", MySqlDbType.Date).Value = datefrom.Date;
                cmd.Parameters.Add("@dateto", MySqlDbType.Date).Value = dateto.Date;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                transaction_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void filter_customer()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT transactionID as 'ID', customerName as 'Customer', productName as 'Type of Product' , paymentAmount as 'Payment Amount' , paymentChange as 'Change' , transactionDate as 'Date' from transaction where transactionDate >= @datefrom and transactionDate<=@dateto and customerName=@customer", con1);
                cmd.Parameters.Add("@datefrom", MySqlDbType.Date).Value = datefrom.Date;
                cmd.Parameters.Add("@dateto", MySqlDbType.Date).Value = dateto.Date;
                cmd.Parameters.Add("@customer", MySqlDbType.String).Value = customername;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                transaction_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void filter_product()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT transactionID as 'ID', customerName as 'Customer', productName as 'Type of Product' , paymentAmount as 'Payment Amount' , paymentChange as 'Change' , transactionDate as 'Date' from transaction where productName=@product and transactionDate >= @datefrom and transactionDate<=@dateto", con1);
                cmd.Parameters.Add("@datefrom", MySqlDbType.Date).Value = datefrom.Date;
                cmd.Parameters.Add("@dateto", MySqlDbType.Date).Value = dateto.Date;
                cmd.Parameters.Add("@product", MySqlDbType.String).Value = prod;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                transaction_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void filter_all()
        {
            try
            {
                MySqlConnection con1 = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand("SELECT transactionID as 'ID', customerName as 'Customer', productName as 'Type of Product' , paymentAmount as 'Payment Amount' , paymentChange as 'Change' , transactionDate as 'Date' from transaction where transactionDate >= @datefrom and transactionDate<=@dateto and productName=@product and customerName=@customer", con1);
                cmd.Parameters.Add("@datefrom", MySqlDbType.Date).Value = datefrom.Date;
                cmd.Parameters.Add("@dateto", MySqlDbType.Date).Value = dateto.Date;
                cmd.Parameters.Add("@product", MySqlDbType.String).Value = prod;
                cmd.Parameters.Add("@customer", MySqlDbType.String).Value = customername;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                con1.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "transaction");
                transaction_table.DataSource = ds.Tables["transaction"].DefaultView;
                con1.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void search_customers()
        {
            DataTable dt = new DataTable();
            try
            {
                string query = "SELECT customerName as 'Customer', numberOfOrders as 'Number of Orders' , totalOrderAmount as 'Total Purchases' , lastPurchaseDate as 'Last Purchase' from customers where customerName LIKE CONCAT('%', @input, '%')";
                MySqlConnection conn1 = new MySqlConnection(con);
                MySqlCommand cmd;
                conn1.Open();
                cmd = conn1.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add("@input", MySqlDbType.String).Value = search_customer.Text;
                cmd.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                customer_table.DataSource = dt;
                conn1.Close();
            }
            catch (Exception e)
            {

            }


        }

        private void search_customer_TextChanged(object sender, EventArgs e)
        {
            search_customers();
        }


        //total sales summary for sales table
        int total_Sales = 0;
        private void sumtable_sales()
        {
            try
            {
                total_Sales = 0;
                foreach(DataGridViewRow row in transaction_table.Rows)
                {
                    total_Sales += Convert.ToInt32(row.Cells["transactionTable_Col4"].Value) - Convert.ToInt32(row.Cells["transactionTable_Col5"].Value);
                }
                total_sales_sumtable.Text = format_number(total_Sales.ToString());
            }catch(Exception ex){}
            
        }
    }
}
