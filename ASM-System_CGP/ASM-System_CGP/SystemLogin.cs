using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class SystemLogin : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public SystemLogin()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            txtuser.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void txtuser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtpassword.Focus();
            }
        }

        private void txtpassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                comboBox1.Focus();
            }
        }

        private void butlogin_Click(object sender, EventArgs e)
        {
            try
            {

                conn.Open();


                if (txtuser.Text == " " || txtpassword.Text == "")
                {
                    MessageBox.Show("Fill Username Password", "Empty Username Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (comboBox1.Text == "Administrator")
                    {
                        string query = "SELECT * FROM [users] WHERE Username = @username AND Password = @password AND account_type='Administrator'";
                        SqlCommand cmd = new SqlCommand(query, conn);


                        cmd.Parameters.AddWithValue("@username", txtuser.Text);
                        cmd.Parameters.AddWithValue("@password", txtpassword.Text);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            using (SqlConnection conn = new SqlConnection(connstring))
                            {

                                SystemDashboard system = new SystemDashboard();
                                system.Show();
                                this.Hide();

                            }
                        }
                        else
                        {

                            MessageBox.Show("Wrong username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        reader.Close();
                    }
                    else
                    {
                        string query = "SELECT * FROM [users] WHERE Username = @username AND Password = @password ";
                        SqlCommand cmd = new SqlCommand(query, conn);


                        cmd.Parameters.AddWithValue("@username", txtuser.Text);
                        cmd.Parameters.AddWithValue("@password", txtpassword.Text);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            using (SqlConnection conn = new SqlConnection(connstring))
                            {
                                saller_dashboard salleer = new saller_dashboard();
                                salleer.Show();
                                this.Hide();

                            }
                        }
                        else
                        {

                            MessageBox.Show("Wrong username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        reader.Close();
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtpassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtpassword.UseSystemPasswordChar = true;
            }
        }

        private void butclear_Click(object sender, EventArgs e)
        {
            txtuser.Clear();
            txtpassword.Clear();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                butlogin.PerformClick();
            }
        }
    }
}
