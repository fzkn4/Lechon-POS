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
using static Guna.UI2.WinForms.Helpers.GraphicsHelper;

namespace Lechon_POS
{
    public partial class register : Form
    {
        static string con = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public register()
        {
            InitializeComponent();
            mismatch.Visible = false;
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

        private void back_Click(object sender, EventArgs e)
        {
            login window = new login();
            window.Show();
            this.Hide();
        }


        private void register_user()
        {
            MySqlConnection conn1 = new MySqlConnection(con);
            MySqlCommand cmd;
            conn1.Open();
            try
            {
                cmd = conn1.CreateCommand();
                cmd.CommandText = "Insert INTO users(username, password, fname, lname, position)VALUES( @username, @password, @fname, @lname, @position)";
                cmd.Parameters.Add("@fname", MySqlDbType.String).Value = first_letter_capital(fname.Text);
                cmd.Parameters.Add("@lname", MySqlDbType.String).Value = first_letter_capital(lname.Text);
                cmd.Parameters.Add("@position", MySqlDbType.String).Value = first_letter_capital(position.Text);
                cmd.Parameters.Add("@username", MySqlDbType.String).Value = username.Text;
                cmd.Parameters.Add("@password", MySqlDbType.String).Value = password.Text;
                cmd.ExecuteNonQuery();


                MessageBox.Show("Account registered successfuly.");
                clear();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn1.Close();

        }

        private string first_letter_capital(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        private void clear()
        {
            username.Clear();
            password.Clear();
            fname.Clear();
            lname.Clear();
            position.Clear();
            conf_pass.Clear();
        }

        private void conf_pass_TextChanged(object sender, EventArgs e)
        {
            if (conf_pass.Text.Length > 0 && password.Text.Length > 0)
            {
                if (password.Text != conf_pass.Text)
                {
                    mismatch.Visible = true;
                }
                else
                {
                    mismatch.Visible = false;
                }
            }
            else
            {
                mismatch.Visible = false;
            }
        }

        private void confirm_login_Click(object sender, EventArgs e)
        {
            if (!mismatch.Visible)
            {
                register_user();
                clear();

            }
        }
    }
}
