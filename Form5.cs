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
    public partial class Form5 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private int old_key;

        public Form5(DataGridView dataGrid, actions action)
        {
            
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            DataGridView = dataGrid;


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
                string[] name = row.Cells[1].Value.ToString().Split(' ');
                textBox2.Text = name[1];
                textBox3.Text = name[0];
                textBox4.Text = name[2];
                textBox5.Text = row.Cells[2].Value.ToString();
            }


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();

               
                var name = textBox2.Text;
                var surname = textBox3.Text;
                var patronymic = textBox4.Text;
                var post = textBox5.Text;

                string add_query = "";
                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Teacher VALUES ('{name}', '{surname}', '{patronymic}', '{post}')";
                }
                else if (Action == actions.update)
                {
                    add_query = $"UPDATE Teacher SET name = '{name}', surname = '{surname}', patronymic = '{patronymic}', post = '{post}' WHERE id = '{old_key}'";
                }

                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();
                

                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query = $"SELECT id, surname + ' ' + name + ' ' + patronymic as name, post FROM Teacher";

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
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
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

        private void Form5_Load(object sender, EventArgs e)
        {

        }
    }
}
