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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Doctor_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";

        private void loadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Doctors";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        dgvDoctors.DataSource = dataTable;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void clearFields()
        {
            txtDoctorName.Text = "";
            txtSpecialization.Text = "";
            txtContact.Text = "";
            txtEmail.Text = "";
            for(int i= 0; i < chbAvailableDays.Items.Count; i++)
            {
                chbAvailableDays.SetItemChecked(i, false);
            }
            

        }
        public Doctor_Form()
        {
            InitializeComponent();
        }

        private void cmbAvailableDays_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                string doctorName = txtDoctorName.Text.Trim();
                string specialization = txtSpecialization.Text.Trim();
                string contact = txtContact.Text.Trim();
                string email = txtEmail.Text.Trim();
                string availableDays = string.Join(",", chbAvailableDays.CheckedItems.Cast<string>());


                if (chbAvailableDays.CheckedItems.Count==0)
                {
                    MessageBox.Show("Please Fill The Available Days Field With Correct Form");
                    return;
                }
                
                if (string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(specialization) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email) )
                {
                    MessageBox.Show("Please Fill All Fields");
                    return;
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Doctors(Name,Specialization,Contact,Email,AvailableDays) VALUES(@Name,@Specialization,@Contact,@Email,@AvailableDays)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Name", doctorName);
                        command.Parameters.AddWithValue("@Specialization", specialization);
                        command.Parameters.AddWithValue("@Contact", contact);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@AvailableDays", availableDays);
                        
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
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }
            





            string doctorName = txtDoctorName.Text.Trim();
            string specialization = txtSpecialization.Text.Trim();
            string contact = txtContact.Text.Trim();
            string email = txtEmail.Text.Trim();
            if (chbAvailableDays.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please Fill The Available Days Field With Correct Form");
                return;
            }
            string availableDays = string.Join(",", chbAvailableDays.CheckedItems.Cast<string>());


            int doctorID = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["DoctorID"].Value);
           

            if (string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(specialization) || string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please Fill All Fields");
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Doctors SET Name=@Name , Specialization = @Specialization , Contact = @Contact , Email = @Email , AvailableDays = @AvailableDays WHERE DoctorID =@DoctorID ";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Name", doctorName);
                        command.Parameters.AddWithValue("@Specialization", specialization);
                        command.Parameters.AddWithValue("@Contact", contact);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@AvailableDays", availableDays);
                        command.Parameters.AddWithValue("@DoctorID", doctorID);
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
            if (dgvDoctors.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }

            int id = Convert.ToInt32(dgvDoctors.CurrentRow.Cells["DoctorID"].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Doctors WHERE DoctorID=@DoctorID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DoctorID", id);
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

        private void Doctor_Form_Load(object sender, EventArgs e)
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
