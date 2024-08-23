using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Colledg
{
    public partial class Form7 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private int old_key;
        public Form7(DataGridView dataGridView, actions action)
        {
            
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            DataGridView = dataGridView;

            Action = action;
            if (Action == actions.insert)
            {
                this.Text = "Add an entry";
            }
            else if (Action == actions.update)
            {
                this.Text = "Edit an entry";
                DataGridViewRow row = DataGridView.Rows[DataGridView.CurrentCell.RowIndex];
                old_key = Convert.ToInt32(row.Cells[0].Value);
                comboBox4.Text = row.Cells[1].Value.ToString();
                comboBox3.Text = row.Cells[2].Value.ToString();
                comboBox2.Text = row.Cells[3].Value.ToString();
                comboBox1.Text = row.Cells[4].Value.ToString();
            }
            
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "5", "4", "3", "2", "н/а"});
            comboBox4.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = $"SELECT name FROM Object";

            SqlCommand Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox2.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }

            dt.Clear();
            dt.Columns.Clear();

            query = $"SELECT surname + ' ' + name + ' ' + patronymic FROM Student";

            Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox3.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dt = new DataTable();
                string query = $"SELECT id FROM Object WHERE name = '{comboBox2.Text}'";

                SqlCommand Command = new SqlCommand(query, Database.GetConnection());

                adapter.SelectCommand = Command;
                adapter.Fill(dt);


                var subject = dt.Rows[0].ItemArray[0].ToString();


                dt.Clear();
                dt.Columns.Clear();
                query = $"SELECT card_number FROM Student WHERE surname + ' ' + name + ' ' + patronymic = '{comboBox3.Text}'";

                 Command = new SqlCommand(query, Database.GetConnection());

                adapter.SelectCommand = Command;
                adapter.Fill(dt);



                var semestr = comboBox4.Text;
                var student = dt.Rows[0].ItemArray[0].ToString();

                var mark = comboBox1.Text;
                string add_query = "";
                if(Action == actions.insert)
                {
                    add_query = $"INSERT INTO Session VALUES ('{semestr}', '{student}', '{subject}', '{mark}')";
                }
                else if (Action == actions.update)
                {
                    add_query = $"UPDATE Session SET semester = '{semestr}', number_student = '{student}', number_object = '{subject}', mark = '{mark}' WHERE id = '{old_key}'";
                }
                

                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();
                

                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query2 = $"SELECT Session.id, semester, Student.surname + ' ' + Student.name + ' ' + Student.patronymic as Student, Object.name, mark FROM Session, Object, Student WHERE Session.number_object = Object.id and Session.number_student = Student.card_number";

                SqlCommand command = new SqlCommand(query2, Database.GetConnection());
                Database.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataGridView.Rows.Add(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                }
                reader.Close();
                Database.CloseConnection();
                MessageBox.Show("Данные успешно добавлены!");
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
                if (Action == actions.update)
                {
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Данные не были добавлены!", "Ошибка");
            }
        }
    }
}
