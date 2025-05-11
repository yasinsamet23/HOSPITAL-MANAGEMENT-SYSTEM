using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HOSPITAL_MANAGEMENT_SYSTEM
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        

        private void btnPatientManagement_Click(object sender, EventArgs e)
        {
            Patient_Form patient_Form = new Patient_Form();
            patient_Form.Show();
        }

        private void btnDoctorManagement_Click(object sender, EventArgs e)
        {
            Doctor_Form doctor_Form = new Doctor_Form();
            doctor_Form.Show();
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            Appointment_Form appointment_Form = new Appointment_Form();
            appointment_Form.Show();

        }

        private void btnMedicalRecords_Click(object sender, EventArgs e)
        {
            Medical_Records_Form medical_Records_Form = new Medical_Records_Form();
            medical_Records_Form.Show();
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            Billing_Form billing_Form = new Billing_Form();
            billing_Form.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login_Form login_Form = new Login_Form();
            this.Hide();
            login_Form.Show();
        }
    }
}
