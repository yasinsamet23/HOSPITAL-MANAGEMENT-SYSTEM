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
    public partial class Billing_Form : Form
    {
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=HospitalDB;Integrated Security=True;";
        public Billing_Form()
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
                    B.BillID AS BillID,
                    P.Name AS PatientName,
                    B.Amount AS Amount ,
                    B.Date AS Date,
                    B.Description AS Description
                    
                    
                FROM 
                    Billing B
                
                INNER JOIN Patients P ON B.PatientID = P.PatientID";

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        dgvBillRecords.DataSource = dataTable;
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
            txtAmount.Text = "";
            txtDescription.Text = "";
            


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
                if (cmbPatientName.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Fill The Patient Name Field With Correct Form");
                    return;
                }


                int patientID = Convert.ToInt32(cmbPatientName.SelectedValue);
                if (!decimal.TryParse(txtAmount.Text, out decimal amount))
                {
                    MessageBox.Show("Fill The Amount Field With Correct Form");
                    return;
                }
               

                 amount = decimal.Parse(txtAmount.Text);
                DateTime dateTime = dtpDate.Value.Date;
                string description = txtDescription.Text.Trim();
                
                

               




                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Billing(PatientID, Amount, Date, Description) VALUES (@PatientID, @Amount, @Date, @Description)";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", patientID);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Date", dateTime);
                        command.Parameters.AddWithValue("@Description", description);
                        

                        conn.Open();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Bill Record Added Successfully.");
                            loadData();
                            clearFields();
                        }
                        else
                        {
                            MessageBox.Show("Could Not Add BillRecord");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Billing_Form_Load(object sender, EventArgs e)
        {
            if (Login_Form.role == "Staff")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
            cmbPatientNames();
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd/MM/yyyy HH:mm";
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvBillRecords.CurrentRow == null)
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
            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Fill The Amount Field With Correct Form");
                return;
            }


            amount = decimal.Parse(txtAmount.Text);
            DateTime dateTime = dtpDate.Value.Date;
            string description = txtDescription.Text.Trim();
            

            





            int BillID = Convert.ToInt32(dgvBillRecords.CurrentRow.Cells["BillID"].Value);



            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Billing SET PatientID=@PatientID , Amount = @Amount , Date= @Date , Description = @Description  WHERE BillID =@BillID";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@PatientID", cmbPatientName.SelectedValue);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@Date", dateTime);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@BillID", BillID);
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
            if (dgvBillRecords.CurrentRow == null)
            {
                MessageBox.Show("Please Select A Row");
                return;
            }

            int id = Convert.ToInt32(dgvBillRecords.CurrentRow.Cells["BillID"].Value);
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Billing WHERE BillID=@BillID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BillID", id);
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
