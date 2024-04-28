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

namespace Eval.KasirControls
{
    public partial class KasirMain : UserControl
    {
        public KasirMain()
        {
            InitializeComponent();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        Connector kon = new Connector();
        private void tambahKode()
        {
            SqlConnection con = kon.getConn();
            con.Open();
            SqlCommand cmd = new SqlCommand("select kode_barang, nama_barang from tbl_barang", con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0) + " - " + reader.GetString(1));
            }
        }

        private void munculHargaSatuan()
        {
            SqlConnection Con = kon.getConn();

            string[] parts = comboBox1.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None);
            Con.Open();
            SqlCommand cmd = new SqlCommand("select harga_satuan from tbl_barang where kode_barang = @kode", Con);
            cmd.Parameters.AddWithValue("@kode", parts[0]);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                textBox1.Text = reader["harga_satuan"].ToString();
            }
        }

        private void KasirMain_Load(object sender, EventArgs e)
        {
            tambahKode();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            munculHargaSatuan();
        }

        private void tambahKeranjang()
        {
            if(textBox3.Text != "")
            {
                DataTable table;
                if(dataGridView1.DataSource == null)
                {
                    table = new DataTable();

                    table.Columns.Add("ID Transaksi", typeof(string));
                    table.Columns.Add("Kode Barang", typeof(string));
                    table.Columns.Add("Nama Barang", typeof(string));
                    table.Columns.Add("Harga Satuan", typeof(int));
                    table.Columns.Add("Quantitas", typeof(int));
                    table.Columns.Add("Sub Total", typeof(int));

                    dataGridView1.DataSource = table;
                }
                else
                {
                    table = (DataTable)dataGridView1.DataSource;
                }

                string[] parts = comboBox1.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                bool cek = false;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataGridViewCell cell = row.Cells["Kode Barang"];
                        if (cell.Value != null && parts[0] == cell.Value.ToString())
                        {
                            cek = true;
                            break;
                        }
                    }
                }

                string idTransaksi, kodeBarang, namaBarang, hargaSatuan;
                int qty;
                string subTotal;

                string transaksiID = dataGridView1.Rows.Count.ToString();

                idTransaksi = "TR" + transaksiID.PadLeft(3, '0');
                hargaSatuan = textBox1.Text;
                kodeBarang = parts[0];
                namaBarang = parts[1];

                qty = int.Parse(textBox2.Text);

                subTotal = textBox3.Text;

                if (!cek)
                {
                    table.Rows.Add(idTransaksi, kodeBarang, namaBarang, hargaSatuan, qty, subTotal);
                }
                else
                {
                    int rowIndex = -1;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        DataGridViewCell cell = row.Cells["Kode Barang"];
                        if (cell.Value != null && cell.Value.ToString() == parts[0])
                        {
                            rowIndex = row.Index;
                            qty = qty + int.Parse(row.Cells[4].Value.ToString());
                            break;

                        }
                    }
                    subTotal = (int.Parse(textBox1.Text) * qty).ToString();

                    dataGridView1.Rows[rowIndex].Selected = true;
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                    table.Rows.Add(idTransaksi, kodeBarang, namaBarang, hargaSatuan, qty, subTotal);
                }
                dataGridView1.Refresh();
                int label = 0;
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCell cell = row.Cells["Sub Total"];
                    if(cell.Value != null)
                    {
                        label = label + int.Parse(row.Cells[5].Value.ToString());
                    }
                }

                label8.Text = string.Format("Rp. {0:#,##0}", label);
            }
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(int.TryParse(textBox2.Text, out _))
            {
                int harga = int.Parse(textBox1.Text);
                int qty = int.Parse(textBox2.Text);

                int total = harga * qty;

                textBox3.Text = total.ToString();
            }
            if(textBox2.Text == "")
            {
                textBox3.Text = "";
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            tambahKeranjang();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            if (dataGridView1.DataSource != null)
            {
                dataGridView1.DataSource = null;
            }
            else
            {
                dataGridView1.Rows.Clear();
            }
            dataGridView1.Refresh();
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
            if(label8.Text != "Rp." && textBox4.Text != "")
            {
                int harga = int.Parse(label8.Text.Replace("Rp. ", "").Replace(".", ""));
                int uang = int.Parse(textBox4.Text);

                int kembalian = 0;

                textBox4.Text = "";
                if (harga < uang)
                {
                    kembalian = uang - harga;
                    label10.Text = string.Format("Rp. {0:#,##0}", kembalian);
                }
                else
                {
                    kembalian = uang - harga;
                    MessageBox.Show("Uang pembeli kurang :" + string.Format("Rp. {0:#,##0}", kembalian));
                }
                
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if(label10.Text != "Rp.")
            {
                SqlConnection con = kon.getConn();
                con.Open();

                string[] parts = comboBox1.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                DateTime now = DateTime.Now;
                string nt = now.ToString("yyMMdd");

                string tt = now.ToString("yyyy-MM-dd");

                SqlCommand cmd = new SqlCommand("insert into tbl_transaksi (no_transaksi, tgl_transaksi, total_bayar, id_user, id_barang) " +
                    "select @nt, @tt, @tb, tbl_user.id_user, tbl_barang.id_barang " +
                    "from tbl_barang join tbl_user on tbl_user.username = @uname " +
                    "where tbl_barang.kode_barang = @kode ", con);
                cmd.Parameters.AddWithValue("@nt", nt);
                cmd.Parameters.AddWithValue("@tt", tt);
                cmd.Parameters.AddWithValue("@tb", int.Parse(label8.Text.Replace("Rp. ", "").Replace(".", "")));
                cmd.Parameters.AddWithValue("@kode", parts[0]);
                cmd.Parameters.AddWithValue("@uname", FormLogin.Username);

                cmd.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Bayar terlebih dahulu!");
            }
        }
    }
}
