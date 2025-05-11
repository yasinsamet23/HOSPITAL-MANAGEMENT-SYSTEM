using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Patient_Form : Form
    {

        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";

        public Patient_Form()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Patients";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        dgvPatients.DataSource = dataTable;
                    }
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void clearFields()
        {
            txtPatientName.Text = "";
            txtPatientAge.Text = "";
            cmbPatientGender.SelectedIndex = -1;
            txtPatientContact.Text = "";
            txtPatientEmail.Text = "";
            txtPatientAddress.Text = "";
            
        }
        

    

        

        private void btnView_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            try
            {
                
                string patientName = txtPatientName.Text.Trim();
                if(!int.TryParse(txtPatientAge.Text,out int patientAge))
                {
                    MessageBox.Show("Please Fill The Patient Age Field With Correct Form");
                    return;
                }
                 patientAge = Convert.ToInt32(txtPatientAge.Text);
                if(cmbPatientGender.SelectedItem==null)
                {
                    MessageBox.Show("Please Fill The Gender Field With Correct Form");
                    return;
                }
                string patientGender = cmbPatientGender.SelectedItem.ToString();
                string patientContact = txtPatientContact.Text.Trim();
                string patientEmail = txtPatientEmail.Text.Trim();
                string patientAddress = txtPatientAddress.Text.Trim();
                if (string.IsNullOrEmpty(patientName) || string.IsNullOrEmpty(patientAge.ToString())  || string.IsNullOrEmpty(patientContact) || string.IsNullOrEmpty(patientEmail) || string.IsNullOrEmpty(patientAddress))
                {
                    MessageBox.Show("Please Fill All Fields");
                    return;
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Patients(Name,Gender,Age,Contact,Address,Email) VALUES(@Name,@Gender,@Age,@Contact,@Address,@Email)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Name", patientName);
                        command.Parameters.AddWithValue("@Gender", patientGender);
                        command.Parameters.AddWithValue("@Age", patientAge);
                        command.Parameters.AddWithValue("@Contact", patientContact);
                        command.Parameters.AddWithValue("@Address", patientAddress);
                        command.Parameters.AddWithValue("@Email", patientEmail);
                        conn.Open();
                        int couuntquery = command.ExecuteNonQuery();
                        loadData();
                        clearFields();



                        if (couuntquery > 0)
                        {
                            MessageBox.Show("Adding Operation Completed Successfully");
                        }

                    }


                }
            }
            catch(SqlException ex )
            {
                MessageBox.Show(ex.Message);
            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

            


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(dgvPatients.CurrentRow==null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }
            


            


            string patientName = txtPatientName.Text.Trim();
            if(!int.TryParse(txtPatientAge.Text,out int patientAge))
            {
                MessageBox.Show("Please Fill The Patient Age Field With Correct Form");
                return;
            }
             patientAge = Convert.ToInt32(txtPatientAge.Text);
            if (cmbPatientGender.SelectedItem == null)
            {
                MessageBox.Show("Please Fill The Gender Field With Correct Form");
                return;
            }
            string patientGender = cmbPatientGender.SelectedItem.ToString();
            string patientContact = txtPatientContact.Text.Trim();
            string patientEmail = txtPatientEmail.Text.Trim();
            string patientAddress = txtPatientAddress.Text.Trim();


            int patientID = Convert.ToInt32(dgvPatients.CurrentRow.Cells["PatientID"].Value);
            if (string.IsNullOrEmpty(patientName) || string.IsNullOrEmpty(txtPatientAge.Text) ||   string.IsNullOrEmpty(patientContact) || string.IsNullOrEmpty(patientEmail) || string.IsNullOrEmpty(patientAddress))
            {
                MessageBox.Show("Please Fill All Fields");
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Patients SET Name=@Name , Gender = @Gender , Age = @Age , Contact = @Contact ,Address = @Address,Email=@Email WHERE PatientID =@PatientID ";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Name", patientName);
                        command.Parameters.AddWithValue("@Gender", patientGender);
                        command.Parameters.AddWithValue("@Age", patientAge);
                        command.Parameters.AddWithValue("@Contact", patientContact);
                        command.Parameters.AddWithValue("@Address", patientAddress);
                        command.Parameters.AddWithValue("@Email", patientEmail);
                        command.Parameters.AddWithValue("@PatientID", patientID);
                        conn.Open();
                        int couuntquery = command.ExecuteNonQuery();
                        loadData();
                        clearFields();



                        if (couuntquery > 0)
                        {
                            MessageBox.Show("Updating Operation Completed Successfully");
                        }

                    }


                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);

            }



        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPatients.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }

            int id = Convert.ToInt32(dgvPatients.CurrentRow.Cells["PatientID"].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Patients WHERE PatientID=@PatientID";
                    using (SqlCommand cmd = new SqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientID",id);
                        conn.Open();
                        int couuntquery = cmd.ExecuteNonQuery();
                        loadData();
                        



                        if (couuntquery > 0)
                        {
                            MessageBox.Show("Delete Operation Completed Successfully");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void Patient_Form_Load(object sender, EventArgs e)
        {
            if(Login_Form.role=="Staff")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }
    }
}
