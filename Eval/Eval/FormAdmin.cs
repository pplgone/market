using System;
using Eval.AdminControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Eval
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        Connector kon = new Connector();

        private void AdminControls(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            LogActivity log = new LogActivity();
            AdminControls(log);
        }

        private void btnKelUser_Click(object sender, EventArgs e)
        {
            KelolaUser ku = new KelolaUser();
            AdminControls(ku);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Yakin untuk Logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SqlConnection Con = kon.getConn();

                DateTime now = DateTime.Now;

                Con.Open();

                SqlCommand cmd = new SqlCommand("insert into tbl_log (waktu, aktivitas, id_user) select @waktu, @akt, tbl_user.id_user from tbl_user where tbl_user.username = @uname", Con);
                cmd.Parameters.AddWithValue("@waktu", now);
                cmd.Parameters.AddWithValue("@akt", "Logout");
                cmd.Parameters.AddWithValue("@uname", FormLogin.Username);

                cmd.ExecuteNonQuery();

                Con.Close();

                Close();

                FormLogin login = new FormLogin();

                login.Show();
            }
        }

        private void btnKelLap_Click(object sender, EventArgs e)
        {
            KelolaLaporan kp = new KelolaLaporan();
            AdminControls(kp);
        }
    }
}
