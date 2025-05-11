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

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Registration_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";
        public Registration_Form()
        {
            InitializeComponent();
        }

        private void clearFields()
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            cmbUserRole.SelectedIndex = -1;
            
        }

        

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {

                string userName = txtUserName.Text.Trim();
                string userPassword = txtPassword.Text.Trim();
                string userConfirmPassword = txtConfirmPassword.Text.Trim();
                if (cmbUserRole.SelectedItem == null)
                {
                    MessageBox.Show("Please Fill The Role Field With Correct Form");
                    return;
                }
                string userRole = cmbUserRole.SelectedItem.ToString();
                
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(userConfirmPassword) )
                {
                    MessageBox.Show("Please Fill All Fields");
                    return;
                }
                if(userPassword!= userConfirmPassword)
                {
                    MessageBox.Show("Password And Confirm Password should be same");
                    clearFields();
                    return;
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(Username) FROM Users WHERE Username=@Username";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Username", userName);
                        conn.Open();
                        int querycount = Convert.ToInt32(command.ExecuteScalar());
                        if (querycount > 0)
                        {
                            MessageBox.Show("A User With This Username Already Exists");
                            return;
                        }




                    }
                }



                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Users(Username,Password,Role) VALUES(@Username,@Password,@Role)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Username", userName);
                        command.Parameters.AddWithValue("@Password", userPassword);
                        command.Parameters.AddWithValue("@Role", userRole);
                        
                        conn.Open();
                        int couuntquery = command.ExecuteNonQuery();
                        
                        clearFields();



                        if (couuntquery > 0)
                        {
                            MessageBox.Show("Registration Operation Completed Successfully");
                            Login_Form login_Form = new Login_Form();
                            this.Hide();
                            login_Form.Show();

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

        private void chbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void chbConfirmPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chbConfirmPassword.Checked)
            {
                txtConfirmPassword.PasswordChar = '\0';
            }
            else
            {
                txtConfirmPassword.PasswordChar = '*';
            }
        }
    }
}


