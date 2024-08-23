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
    public partial class Form8 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private int old_key;
        public Form8(DataGridView dataGridView, actions action)
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
                comboBox1.Text = row.Cells[1].Value.ToString();
                comboBox2.Text = row.Cells[2].Value.ToString();
                comboBox3.Text = row.Cells[3].Value.ToString();
                comboBox4.Text = row.Cells[4].Value.ToString();
                numericUpDown1.Text = row.Cells[5].Value.ToString();
                numericUpDown2.Text = row.Cells[6].Value.ToString();
            }
            
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = $"SELECT number FROM Group_class";

            SqlCommand Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox2.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            dt.Clear();
            dt.Columns.Clear();

           ////
            
            query = $"SELECT name FROM Object";

            Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox3.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }
            dt.Clear();
            dt.Columns.Clear();
            ///////////////
            query = $"SELECT surname + ' ' + name + ' ' + patronymic FROM Teacher";

            Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox4.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dt = new DataTable();
                string query = $"SELECT id FROM Object WHERE name = '{comboBox3.Text}'";

                SqlCommand Command = new SqlCommand(query, Database.GetConnection());

                adapter.SelectCommand = Command;
                adapter.Fill(dt);
                var subject = dt.Rows[0].ItemArray[0].ToString();
                dt.Clear();
                dt.Columns.Clear();
                //////////////////////////
                query = $"SELECT id FROM Teacher WHERE surname + ' ' + name + ' ' + patronymic = '{comboBox4.Text}'";

                Command = new SqlCommand(query, Database.GetConnection());

                adapter.SelectCommand = Command;
                adapter.Fill(dt);
                var name_teacher = dt.Rows[0].ItemArray[0].ToString();
                dt.Clear();
                dt.Columns.Clear();
                //////////////////////////
                ///

                var semestr = comboBox1.Text;
                var number_group = comboBox2.Text;
                
               
                var hours_lecture = numericUpDown1.Value.ToString();
                var hours_practice = numericUpDown2.Value.ToString();

                string add_query = "";
                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Workplan VALUES ('{semestr}', '{number_group}', '{subject}', '{name_teacher}', '{hours_lecture}', '{hours_practice}')";
                }
                else if (Action == actions.update)
                {
                    add_query = $"UPDATE Workplan SET semester = '{semestr}', number_group = '{number_group}', number_object = '{subject}', id_teacher = '{name_teacher}', hours_lecture = '{hours_lecture}', hours_practice = '{hours_practice}' WHERE id = '{old_key}'";
                }
                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();


                Database.CloseConnection();

                DataGridView.Rows.Clear();

                query = $"select  Workplan.id, Workplan.semester, Group_class.number, Object.name, Teacher.surname + ' ' + Teacher.name + ' ' + Teacher.patronymic as name , hours_lecture, hours_practice from Workplan, Group_class, Teacher, Object where Workplan.number_group = Group_class.number and Workplan.number_object = Object.id and Workplan.id_teacher = Teacher.id";

                SqlCommand command = new SqlCommand(query, Database.GetConnection());
                Database.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataGridView.Rows.Add(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), reader.GetInt32(6));
                }
                reader.Close();
                Database.CloseConnection();
                MessageBox.Show("Данные успешно добавлены!");
                comboBox1.Text = "";
                comboBox2.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
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
