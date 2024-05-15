using MySqlConnector;
using System.Configuration;

namespace Lechon_POS
{
    public partial class login : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public login()
        {
            InitializeComponent();
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

        private void clear_Click(object sender, EventArgs e)
        {

        }

        private void confirm_transaction_Click(object sender, EventArgs e)
        {

        }

        public static string user_position = "";
        public static string name = "";
        private void validate_login()
        {
            MySqlConnection connauj = new MySqlConnection(con);
            connauj.Open();
            try
            {

                MySqlCommand Comauj = new MySqlCommand() { Connection = connauj, CommandText = "select * from users where username='" + username.Text + "' and password='" + password.Text + "'" };
                MySqlDataReader readerauj = Comauj.ExecuteReader();
                if (readerauj.Read())
                {
                    name = readerauj["lname"].ToString() +", "+ readerauj["fname"].ToString();
                    user_position = readerauj["position"].ToString();
                    Form1 mainpage = new Form1();
                    mainpage.Show();
                    this.Hide();
                    clear();

                }
                else
                {
                    MessageBox.Show("login failed.");

                }
                connauj.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void confirm_login_Click(object sender, EventArgs e)
        {
            validate_login();
        }

        private void register_Click(object sender, EventArgs e)
        {
            register window = new register();
            window.Show();
            this.Hide();
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                validate_login();
            }
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                validate_login();
            }
        }

        private void clear()
        {
            username.Clear();
            password.Clear();
        }
    }
}
