using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Eval.AdminControls
{
    public partial class LogActivity : UserControl
    {
        public LogActivity()
        {
            InitializeComponent();
        }

        Connector kon = new Connector();

        private void muncul()
        {
            SqlConnection Con = kon.getConn();

            Con.Open();

            SqlCommand cmd = new SqlCommand("select tbl_log.id_log as ID, tbl_user.username as Username, tbl_log.waktu as Waktu, tbl_log.aktivitas as Aktivitas from tbl_log join tbl_user on tbl_user.id_user = tbl_log.id_user", Con);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds, "tbl_log");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tbl_log";
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.Refresh();
        }
        private void LogActivity_Load(object sender, EventArgs e)
        {
            muncul();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            SqlConnection con = kon.getConn();

            con.Open();

            SqlCommand cmd = new SqlCommand("select tbl_log.id_log as ID, tbl_user.username as Username, tbl_log.waktu as Waktu, tbl_log.aktivitas as Aktivitas from tbl_log join tbl_user on tbl_user.id_user = tbl_log.id_user where convert(date, tbl_log.waktu) = convert(date, @waktu)", con);
            cmd.Parameters.AddWithValue("@waktu", DateTime.Parse(selectDate));

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "tbl_log2");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tbl_log2";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();
        }
    }
}
