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

namespace Eval.GudangControls
{
    public partial class kelGudang : UserControl
    {
        public kelGudang()
        {
            InitializeComponent();
        }

        Connector kon = new Connector();

        void muncul()
        {
            SqlConnection Con = kon.getConn();

            Con.Open();
            SqlCommand cmd = new SqlCommand("select id_barang as 'ID Barang', kode_barang as 'Kode Barang', nama_barang as 'Nama Barang', expired_date as 'Expired Date', jumlah_barang as 'Jumlah Barang', satuan as Satuan, harga_satuan as 'Harga Satuan' " +
                "from tbl_barang ", Con);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "tbl_barang");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tbl_barang";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (textKode.Text != "" && textNama.Text != "" && textJumlah.Text != "" && textHarga.Text != "")
            {
                SqlConnection Con = kon.getConn();

                Con.Open();

                SqlCommand cmd = new SqlCommand("select count(*) from tbl_barang where kode_barang = @kode",Con);
                cmd.Parameters.AddWithValue("@kode", textKode.Text);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    cmd = new SqlCommand("update tbl_barang set jumlah_barang = jumlah_barang + @jumlah where kode_barang = @kode", Con);
                    cmd.Parameters.AddWithValue("@kode", textKode.Text);
                    cmd.Parameters.AddWithValue("@jumlah", int.Parse(textJumlah.Text));

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd = new SqlCommand("insert into tbl_barang (kode_barang, nama_barang, expired_date, jumlah_barang, satuan, harga_satuan) " +
                        "values (@kode, @nama, @exp, @jumlah, @satuan, @harga) ", Con);
                    cmd.Parameters.AddWithValue("@kode", textKode.Text);
                    cmd.Parameters.AddWithValue("@nama", textNama.Text);
                    cmd.Parameters.AddWithValue("@exp", dateExpired.Value);
                    cmd.Parameters.AddWithValue("@jumlah", int.Parse(textJumlah.Text));
                    cmd.Parameters.AddWithValue("@satuan", BoxSatuan.SelectedItem);
                    cmd.Parameters.AddWithValue("@harga", int.Parse(textHarga.Text));

                    cmd.ExecuteNonQuery();
                }

                muncul();
            }
            else
            {
                MessageBox.Show("Semua field harus di isi!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                textKode.Text = row.Cells[1].Value.ToString();
                textNama.Text = row.Cells[2].Value.ToString();
                textJumlah.Text = row.Cells[4].Value.ToString();
                BoxSatuan.SelectedItem = row.Cells[5].Value.ToString();
                textHarga.Text = row.Cells[6].Value.ToString();
            }
        }

        private void kelGudang_Load(object sender, EventArgs e)
        {
            muncul();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (textKode.Text != "" && textNama.Text != "" && textJumlah.Text != "" && textHarga.Text != "")
            {
                SqlConnection con = kon.getConn();

                con.Open();
                SqlCommand cmd = new SqlCommand("update tbl_barang " +
                    "set nama_barang = @nama, expired_date = @exp, jumlah_barang = @jumlah, satuan = @satuan, harga_satuan = @harga " +
                    "where kode_barang = @kode", con);
                cmd.Parameters.AddWithValue("@kode", textKode.Text);
                cmd.Parameters.AddWithValue("@nama", textNama.Text);
                cmd.Parameters.AddWithValue("@exp", dateExpired.Value);
                cmd.Parameters.AddWithValue("@jumlah", int.Parse(textJumlah.Text));
                cmd.Parameters.AddWithValue("@satuan", BoxSatuan.SelectedItem);
                cmd.Parameters.AddWithValue("@harga", int.Parse(textHarga.Text));

                cmd.ExecuteNonQuery();

                muncul();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if(textKode.Text != "")
            {
                if(MessageBox.Show("Yakin untuk menghapus data barang ?", "Konfirmasi",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlConnection con = kon.getConn();

                    con.Open();

                    SqlCommand cmd = new SqlCommand("delete from tbl_barang " +
                        "where kode_barang = @kode ", con);
                    cmd.Parameters.AddWithValue("@kode", textKode.Text);

                    cmd.ExecuteNonQuery();
                    muncul();

                }
            }
        }
    }
}
