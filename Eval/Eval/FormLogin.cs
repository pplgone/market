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

namespace Eval
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public static string Username { get; private set; }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if(textBox2.Text == "Username")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        Connector Kon = new Connector();

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Username";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Password")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Password";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Password";
            textBox2.Text = "Username";

            textBox1.ForeColor = Color.Gray;
            textBox2.ForeColor = Color.Gray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox1.Text))
            {
                Username = textBox2.Text;
                string password = textBox1.Text;

                SqlConnection Con = Kon.getConn();

                Con.Open();
                SqlCommand cmd = new SqlCommand("Select * from tbl_user where username = @user and password = @pass", Con);
                cmd.Parameters.AddWithValue("@user", Username);
                cmd.Parameters.AddWithValue("@pass", password);

                SqlDataReader reader = cmd.ExecuteReader();

                reader.Read();
                if (reader.HasRows)
                {
                    MessageBox.Show("Berhasil Login!");
                    if(reader["tipe_user"].ToString().ToLower() == "admin")
                    {
                        FormAdmin admin = new FormAdmin();
                        Hide();
                        admin.Show();
                    }else if(reader["tipe_user"].ToString().ToLower() == "gudang")
                    {
                        FormGudang gudang = new FormGudang();
                        Hide();
                        gudang.Show();
                    }else if(reader["tipe_user"].ToString().ToLower() == "kasir")
                    {
                        formKasir kasir = new formKasir();
                        Hide();
                        kasir.Show();
                    }

                    Con.Close();

                    DateTime now = DateTime.Now;

                    Con.Open();

                    cmd = new SqlCommand("insert into tbl_log (waktu, aktivitas, id_user) select @waktu, @akt, tbl_user.id_user from tbl_user where tbl_user.username = @uname", Con);
                    cmd.Parameters.AddWithValue("@waktu", now);
                    cmd.Parameters.AddWithValue("@akt", "Login");
                    cmd.Parameters.AddWithValue("@uname", Username);

                    cmd.ExecuteNonQuery();

                    Con.Close();

                }
                else
                {
                    MessageBox.Show("tidak ada data!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
