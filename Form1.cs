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

namespace Colledg
{
    public partial class Form1 : Form
    {
        Database database = new Database();
        public Form1()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
           


            InitializeComponent();
            pictureBox1.Image = Properties.Resources.eye;
            //pictureBox1.Size = new Size(20, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox2.Image = Properties.Resources.eye_hide;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 30;
            textBox2.MaxLength = 15;

            textBox2.UseSystemPasswordChar = true;

            pictureBox1.Visible = false;

            //Database database = new Database();
            //database.OpenConnection();
            ////SqlConnection SqlConnection = new SqlConnection(@"Data Source=LAPTOP-R80O94U8;Initial Catalog=college;Integrated Security=True");

            //if (database.SqlConnection.State == System.Data.ConnectionState.Open)
            //{
            //    MessageBox.Show("Подключение произошло удачно!");
            //}
            //else MessageBox.Show("Подключение произошло не удачно!");
            //Form2 form2 = new Form2();
            //form2.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.OpenConnection();
            var login = textBox1.Text;
            var password = textBox2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = $"SELECT Users_login.name, password, Roles.name FROM Users_login, Roles WHERE Users_login.name = '{login}' and password = '{password}' and Users_login.role = Roles.id";

            SqlCommand Command = new SqlCommand(query, database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);


            if (dt.Rows.Count == 1)
            {
                var user = new User(dt.Rows[0].ItemArray[0].ToString(), dt.Rows[0].ItemArray[1].ToString(), dt.Rows[0].ItemArray[2].ToString());
                MessageBox.Show($"Добро пожаловать, {login}");
                Form2 form2 = new Form2(user);
                this.Hide();
                form2.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Сообщение");
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
        }
    }
}
