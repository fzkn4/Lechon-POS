using Guna.Charts.WinForms;
using Guna.UI2.WinForms;

namespace Lechon_POS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            display_revenue_chart(revenue_chart);
            display_sales_cat_chart(sales_cat);
            hide_panels();
            dashboard_panel.Visible = true;

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
            selected_button(sales, customer, products, dashboard);

        }

        private void customer_Click(object sender, EventArgs e)
        {
            selected_button(customer, sales, products, dashboard);

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
        }

        private void food_package_img_Click(object sender, EventArgs e)
        {
            food_package_transaction window = new food_package_transaction();
            window.ShowDialog();
        }

        private void whole_lechon_img_Click(object sender, EventArgs e)
        {
            lechon_transaction window  = new lechon_transaction();
            window.ShowDialog();
        }
    }
}
