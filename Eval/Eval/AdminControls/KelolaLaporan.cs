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
using System.Windows.Forms.DataVisualization.Charting;
namespace Eval.AdminControls
{
    public partial class KelolaLaporan : UserControl
    {
        public KelolaLaporan()
        {
            InitializeComponent();
        }

        Connector kon = new Connector();

        private void munculData()
        {
            SqlConnection con = kon.getConn();

            DateTime tanggal1 = dateTimePicker1.Value;
            DateTime tanggal2 = dateTimePicker2.Value;

            string format1 = tanggal1.ToString("yyyy-MM-dd");
            string format2 = tanggal2.ToString("yyyy-MM-dd");

            con.Open();
            SqlCommand cmd = new SqlCommand("select tbl_transaksi.id_transaksi as 'ID Transaksi', tbl_transaksi.tgl_transaksi as 'Tanggal Transaksi', tbl_transaksi.total_bayar as 'Total Pembayaran', tbl_user.nama as 'Nama Kasir' " +
                "from tbl_transaksi join tbl_user on tbl_user.id_user = tbl_transaksi.id_user " +
                "where tbl_transaksi.tgl_transaksi >= @tanggal1 and tbl_transaksi.tgl_transaksi <= @tanggal2", con);
            cmd.Parameters.AddWithValue("@tanggal1", format1);
            cmd.Parameters.AddWithValue("@tanggal2", format2);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "tbl_transaksi");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tbl_transaksi";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dateTime1 = dateTimePicker1.Value;
            DateTime dateTime2 = dateTimePicker2.Value;

            int result = DateTime.Compare(dateTime1, dateTime2);
            if (result < 0 || result == 0)
            {
                munculData();
            }
        }

        private void KelolaLaporan_Load(object sender, EventArgs e)
        {
            munculData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tanggal1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string tanggal2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

            string query = "select tgl_transaksi, total_bayar as 'Total Pembayaran' " +
                "from tbl_transaksi " +
                "where tgl_transaksi >= @tanggal1 and tgl_transaksi <= @tanggal2";

            SqlConnection con = kon.getConn();
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@tanggal1", tanggal1);
            cmd.Parameters.AddWithValue("@tanggal2", tanggal2);

            // cari series omset

            Series series = chart1.Series.FirstOrDefault(s => s.Name == "Omset");

            if(series == null)
            {
                series = new Series();

                series.Name = "Omset";
                series.ChartType = SeriesChartType.Column;
                chart1.Series.Add(series);

            }
            else
            {
                series.Points.Clear();
            }

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DateTime tanggal = reader.GetDateTime(0);
                long bayar = reader.GetInt64(1);

                series.Points.AddXY(tanggal, bayar);
            }

        }
    }
}
