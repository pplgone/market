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
    public partial class KelolaUser : UserControl
    {
        public KelolaUser()
        {
            InitializeComponent();
        }

        void munculdata()
        {
            SqlConnection Con = Kon.getConn();

            SqlCommand cmd = new SqlCommand("select id_user as ID, tipe_user as 'Tipe User', nama as Nama, alamat as Alamat, telepon as Telepon from tbl_user", Con);

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds, "tbl_user");
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "tbl_user";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Refresh();
        }

        void awal()
        {
            tipeUser.SelectedIndex = 1;
            textNama.Text = "";
            textTelepon.Text = "";
            textAlamat.Text = "";
            textUsername.Text = "";
            textPassword.Text = "";
            munculdata();
        }

        private void KelolaUser_Load(object sender, EventArgs e)
        {
            
            awal();
            
        }

        Connector Kon = new Connector();

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textNama.Text) &&
                !string.IsNullOrEmpty(textAlamat.Text) && 
                !string.IsNullOrEmpty(textUsername.Text) &&
                !string.IsNullOrEmpty(textPassword.Text) && textTelepon.Text != "")
            {
                if(!int.TryParse(textTelepon.Text, out _))
                {
                    MessageBox.Show("Kolom 'Telepon' hanya boleh diisi oleh angka!");
                }
                else
                {
                    if(MessageBox.Show("yakin untuk menambah user?", "konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlConnection Con = Kon.getConn();
                        Con.Open();
                        SqlCommand cmd = new SqlCommand("insert into tbl_user (tipe_user, nama, alamat, telepon, username, password) " +
                            "values (@tipe, @nama, @alamat, @telepon, @username, @password)", Con);
                        cmd.Parameters.AddWithValue("@tipe", tipeUser.SelectedItem);
                        cmd.Parameters.AddWithValue("@nama", textNama.Text);
                        cmd.Parameters.AddWithValue("@alamat", textAlamat.Text);
                        cmd.Parameters.AddWithValue("@telepon", int.Parse(textTelepon.Text));
                        cmd.Parameters.AddWithValue("@username", textUsername.Text);
                        cmd.Parameters.AddWithValue("@password", textPassword.Text);

                        cmd.ExecuteNonQuery();
                        awal();

                    }
                }
            }
            else
            {
                MessageBox.Show("Tolong isi semua Form!");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                tipeUser.SelectedItem = row.Cells[1].Value.ToString();
                textNama.Text = row.Cells[2].Value.ToString();
                textAlamat.Text = row.Cells[3].Value.ToString();
                textTelepon.Text = row.Cells[4].Value.ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textNama.Text) &&
                !string.IsNullOrEmpty(textAlamat.Text) &&
                !string.IsNullOrEmpty(textUsername.Text) &&
                !string.IsNullOrEmpty(textPassword.Text) && textTelepon.Text != "")
            {
                if (!int.TryParse(textTelepon.Text, out _))
                {
                    MessageBox.Show("Kolom 'Telepon' hanya boleh diisi oleh angka dan maksimal 10 angka!");
                }
                else
                {
                    if (MessageBox.Show("yakin untuk mengedit user?", "konfirmasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlConnection Con = Kon.getConn();
                        Con.Open();

                        SqlCommand cmd = new SqlCommand("update tbl_user " +
                             "set tipe_user = @tipe, nama = @nama, alamat = @alamat, telepon = @telepon " +
                             "where username = @username and password = @password", Con);

                        cmd.Parameters.AddWithValue("@tipe", tipeUser.SelectedItem);
                        cmd.Parameters.AddWithValue("@nama", textNama.Text);
                        cmd.Parameters.AddWithValue("@alamat", textAlamat.Text);
                        cmd.Parameters.AddWithValue("@telepon", int.Parse(textTelepon.Text));
                        cmd.Parameters.AddWithValue("@username", textUsername.Text);
                        cmd.Parameters.AddWithValue("@password", textPassword.Text);

                        cmd.ExecuteNonQuery();

                        awal();

                    }
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textNama.Text))
            {
                if (MessageBox.Show("Yakin untuk menghapus user?", "konfirmaasi", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    SqlConnection Con = Kon.getConn();

                    Con.Open();

                    SqlCommand cmd = new SqlCommand("delete from tbl_user where nama = @nama", Con);
                    cmd.Parameters.AddWithValue("@nama", textNama.Text);

                    cmd.ExecuteNonQuery();

                    awal();
                }
            }
        }
    }
}
