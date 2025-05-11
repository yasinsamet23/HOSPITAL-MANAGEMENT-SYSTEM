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

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Login_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";
        public static string role;
        public Login_Form()
        {
            InitializeComponent();
        }
       

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                string userName = txtUsername.Text.Trim();
                string userPassword = txtPassword.Text.Trim();

                


                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword))
                    {
                        MessageBox.Show("Please Fill All Fields");
                        return;
                    }

               




                

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password ";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Username", userName);
                        command.Parameters.AddWithValue("@Password", userPassword);

                        conn.Open();
                        int couuntquery = (int)command.ExecuteScalar();

                       



                        if (couuntquery > 0)
                        {
                            string newquery = "SELECT Role FROM Users WHERE Username = @Username";
                            using (SqlCommand sqlCommand = new SqlCommand(newquery,conn))
                            {
                                sqlCommand.Parameters.AddWithValue("@Username",userName);
                                role =sqlCommand.ExecuteScalar().ToString();
                            }


                            Main main = new Main();
                            this.Hide();
                            main.Show();

                        }
                        else
                        {
                            MessageBox.Show("User that have this username and password wasn't found");
                            return;
                        }

                    }


                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            Registration_Form registration_Form = new Registration_Form();
            
            registration_Form.Show();
        }
    }
}
