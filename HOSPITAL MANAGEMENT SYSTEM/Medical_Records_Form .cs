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
    public partial class Medical_Records_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";
        public Medical_Records_Form()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    M.RecordID AS RecordID,
                    P.Name AS PatientName,
                    M.Diagnosis AS Diagnosis ,
                    M.Treatment AS Treatment,
                    M.Prescriptions AS Prescriptions,
                    M.Date AS Date
                    
                FROM 
                    MedicalRecords M
                
                INNER JOIN Patients P ON M.PatientID = P.PatientID";

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        dgvMedicalRecords.DataSource = dataTable;
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
            cmbPatientName.SelectedIndex = -1;
            txtDiagnosis.Text = "";
            txtTreatment.Text = "";
            txtPrescriptions.Text = "";

            
        }

        private void cmbPatientNames()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT PatientID, Name FROM Patients";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        cmbPatientName.DataSource = dataTable;
                        cmbPatientName.DisplayMember = "Name";
                        cmbPatientName.ValueMember = "PatientID";
                        cmbPatientName.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if ( cmbPatientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Fill The Patient Name Field With Correct Form");
                    return;
                }

                
                int patientID = Convert.ToInt32(cmbPatientName.SelectedValue);
                string diagnosis = txtDiagnosis.Text.Trim();
                string treatment = txtTreatment.Text.Trim();
                string prescriptions = txtPrescriptions.Text.Trim();
                DateTime date = dtpDate.Value.Date;

                if(string.IsNullOrEmpty(diagnosis) || string.IsNullOrEmpty(treatment) || string.IsNullOrEmpty(prescriptions))
                {
                    MessageBox.Show("Please Fill All Fields");
                    return;
                }


                

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO MedicalRecords(PatientID, Diagnosis, Treatment, Prescriptions, Date) VALUES (@PatientID, @Diagnosis, @Treatment, @Prescriptions, @Date)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", patientID);
                        command.Parameters.AddWithValue("@Diagnosis", diagnosis);
                        command.Parameters.AddWithValue("@Treatment", treatment);
                        command.Parameters.AddWithValue("@Prescriptions", prescriptions);
                        command.Parameters.AddWithValue("@Date", date);

                        conn.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Medical Record Added Successfully.");
                            loadData();
                            clearFields();
                        }
                        else
                        {
                            MessageBox.Show("Could Not Add Medical Record");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Medical_Records_Form_Load(object sender, EventArgs e)
        {
            if (Login_Form.role == "Staff")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
            cmbPatientNames();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvMedicalRecords.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }





            
            if (cmbPatientName.SelectedItem == null)
            {
                MessageBox.Show("Please Fill The Patient Name Field With Correct Form");
                return;
            }
            string PatientName = cmbPatientName.SelectedItem.ToString();
            string diagnosis = txtDiagnosis.Text.Trim();
            string treatment = txtTreatment.Text.Trim();
            string prescriptions = txtPrescriptions.Text.Trim();
            DateTime date = dtpDate.Value.Date;

            if (string.IsNullOrEmpty(diagnosis) || string.IsNullOrEmpty(treatment) || string.IsNullOrEmpty(prescriptions))
            {
                MessageBox.Show("Please Fill All Fields");
                return;
            }





            int AppointmentID = Convert.ToInt32(dgvMedicalRecords.CurrentRow.Cells["RecordID"].Value);



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE MedicalRecords SET PatientID=@PatientID , Diagnosis = @Diagnosis , Treatment= @Treatment , Prescriptions = @Prescriptions , Date= @Date WHERE RecordID =@RecordID";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", cmbPatientName.SelectedValue);
                        command.Parameters.AddWithValue("@Diagnosis", diagnosis);
                        command.Parameters.AddWithValue("@Treatment", treatment);
                        command.Parameters.AddWithValue("@Prescriptions", prescriptions);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@RecordID", AppointmentID);
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
            if (dgvMedicalRecords.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }

            int id = Convert.ToInt32(dgvMedicalRecords.CurrentRow.Cells["RecordID"].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM MedicalRecords WHERE RecordID=@RecordID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RecordID", id);
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
    }
}
