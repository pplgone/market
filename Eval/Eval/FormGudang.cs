using System;
using Eval.GudangControls;
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
    public partial class FormGudang : Form
    {
        public FormGudang()
        {
            InitializeComponent();
        }

        Connector Kon = new Connector();

        private void GudangControls(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(userControl);

            userControl.BringToFront();
        }

        private void FormGudang_Load(object sender, EventArgs e)
        {
            kelGudang kg = new kelGudang();
            GudangControls(kg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Yakin untuk Logout?", "Konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlConnection Con = Kon.getConn();

                DateTime now = DateTime.Now;

                Con.Open();

                SqlCommand cmd = new SqlCommand("insert into tbl_log (waktu, aktivitas, id_user) select @waktu, @akt, tbl_user.id_user from tbl_user where tbl_user.username = @uname", Con);
                cmd.Parameters.AddWithValue("@waktu", now);
                cmd.Parameters.AddWithValue("@akt", "Logout");
                cmd.Parameters.AddWithValue("@uname", FormLogin.Username);

                cmd.ExecuteNonQuery();

                Con.Close();
                FormLogin login = new FormLogin();

                this.Hide();
                login.Show();
            }
        }
    }
}
