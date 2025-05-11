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
    public partial class Appointment_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";

        private void loadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    A.AppointmentID,
                    D.Name AS DoctorName,
                    P.Name AS PatientName,
                    A.Date,
                    A.Time,
                    A.Notes
                FROM 
                    Appointments A
                INNER JOIN Doctors D ON A.DoctorID = D.DoctorID
                INNER JOIN Patients P ON A.PatientID = P.PatientID";

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        dgvAppointments.DataSource = dataTable;
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
            cmbDoctorName.SelectedIndex = -1;
            cmbPatientName.SelectedIndex = -1;
            txtNotes.Text = "";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            loadData();
        }




        private void cmbDoctorNames()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT DoctorID, Name FROM Doctors";
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        cmbDoctorName.DataSource = dataTable;
                        cmbDoctorName.DisplayMember = "Name";
                        cmbDoctorName.ValueMember = "DoctorID";
                        cmbDoctorName.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        public Appointment_Form()
        {
            InitializeComponent();
        }

        private void Appointment_Form_Load(object sender, EventArgs e)
        {
            if (Login_Form.role == "Staff")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
            cmbDoctorNames();
            cmbPatientNames();

            dtpDateAndTime.Format = DateTimePickerFormat.Custom;
            dtpDateAndTime.CustomFormat = "dd/MM/yyyy HH:mm";
            // dtpDateAndTime.ShowUpDown = true;

        }

        



        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbDoctorName.SelectedIndex == -1 || cmbPatientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select both a doctor and a patient.");
                    return;
                }

                int doctorID = Convert.ToInt32(cmbDoctorName.SelectedValue);
                int patientID = Convert.ToInt32(cmbPatientName.SelectedValue);
                DateTime selectedDateTime = dtpDateAndTime.Value;
                DateTime dateOnly = selectedDateTime.Date;
                TimeSpan timeOnly = selectedDateTime.TimeOfDay;
                string notes = txtNotes.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Appointments (PatientID, DoctorID, Date, Time, Notes) VALUES (@PatientID, @DoctorID, @Date, @Time, @Notes)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", patientID);
                        command.Parameters.AddWithValue("@DoctorID", doctorID);
                        command.Parameters.AddWithValue("@Date", dateOnly);
                        command.Parameters.AddWithValue("@Time", timeOnly);
                        command.Parameters.AddWithValue("@Notes", notes);

                        conn.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Appointment added successfully.");
                            loadData();
                            clearFields();
                        }
                        else
                        {
                            MessageBox.Show("Could not add appointment.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }

            int id = Convert.ToInt32(dgvAppointments.CurrentRow.Cells["AppointmentID"].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Appointments WHERE AppointmentID=@AppointmentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentID", id);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }





            if(cmbDoctorName.SelectedItem==null)
            {
                MessageBox.Show("Please Fill The Doctor Name Field With Correct Form");
                return;
            }
            string DoctortName = cmbDoctorName.SelectedItem.ToString();
            if (cmbPatientName.SelectedItem==null)
            {
                MessageBox.Show("Please Fill The Patient Name Field With Correct Form");
                return;
            }
            string PatientName = cmbPatientName.SelectedItem.ToString();
            string notes = txtNotes.Text;

            
            


            int AppointmentID = Convert.ToInt32(dgvAppointments.CurrentRow.Cells["AppointmentID"].Value);


            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Appointments SET PatientID=@PatientID , DoctorID = @DoctorID , Date= @Date , Time = @Time , Notes= @Notes WHERE AppointmentID =@AppointmentID";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", cmbPatientName.SelectedValue);
                        command.Parameters.AddWithValue("@DoctorID", cmbDoctorName.SelectedValue);
                        command.Parameters.AddWithValue("@Date", dtpDateAndTime.Value.Date);
                        command.Parameters.AddWithValue("@Time", dtpDateAndTime.Value.TimeOfDay);
                        command.Parameters.AddWithValue("@Notes", notes);
                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
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
    }
}
