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
    public partial class Form3 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private int old_key;
        public Form3(DataGridView dataGridView, actions action)
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
                textBox1.ReadOnly = true;
                DataGridViewRow row = DataGridView.Rows[DataGridView.CurrentCell.RowIndex];
                old_key = Convert.ToInt32(row.Cells[0].Value);
                textBox1.Text = row.Cells[0].Value.ToString();
                string[] name = row.Cells[1].Value.ToString().Split(' ');
                textBox2.Text = name[1];
                textBox3.Text = name[0];
                textBox4.Text = name[2];
                comboBox1.Text = row.Cells[2].Value.ToString();

            }
            


            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = $"SELECT number FROM Group_class";

            SqlCommand Command = new SqlCommand(query, Database.GetConnection());

            adapter.SelectCommand = Command;
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i].ItemArray[0].ToString());
            }
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();


                var number_card = textBox1.Text;
                var name = textBox2.Text;
                var surname = textBox3.Text;
                var patronymic = textBox4.Text;
                var number_group = comboBox1.Text;
                string add_query = "";


                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Student VALUES ('{number_card}', '{name}', '{surname}', '{patronymic}', '{number_group}')";
                }
                else if (Action == actions.update)
                { 
                    add_query = $"UPDATE Student SET card_number = '{number_card}', name = '{name}', surname = '{surname}', patronymic = '{patronymic}', number_group = '{number_group}' WHERE card_number = '{old_key}'";
                }

                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();
               

                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query = $"SELECT card_number, surname + ' ' + name + ' ' + patronymic as name, number FROM Student, Group_class WHERE Student.number_group = Group_class.number";

                SqlCommand command = new SqlCommand(query, Database.GetConnection());
                Database.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataGridView.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                }
                reader.Close();
                Database.CloseConnection();
                MessageBox.Show("Данные успешно добавлены!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                comboBox1.Text = "";
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
