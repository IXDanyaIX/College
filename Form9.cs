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
    public partial class Form9 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private string old_key;
        public Form9(DataGridView dataGridView, actions action)
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
                old_key = Convert.ToString(row.Cells[0].Value);
                textBox1.Text = row.Cells[0].Value.ToString();
                textBox2.Text = row.Cells[1].Value.ToString();
            }

           
           
        }

        private void Form9_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();

                var number_specialty = textBox1.Text;
                var name_specialty = textBox2.Text;


                string add_query = "";
                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Specialty VALUES ('{number_specialty}', '{name_specialty}')";
                }
                else if (Action == actions.update)
                {
                    add_query = $"UPDATE Specialty SET number = '{number_specialty}', name = '{name_specialty}' WHERE number = '{old_key}'";
                }
                
                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());

                sqlCommand.ExecuteNonQuery();
        


                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query = $"SELECT * FROM Specialty";

                SqlCommand command = new SqlCommand(query, Database.GetConnection());
                Database.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataGridView.Rows.Add(reader.GetString(0), reader.GetString(1));
                }
                reader.Close();
                Database.CloseConnection();
                MessageBox.Show("Данные успешно добавлены!");
                textBox1.Text = "";
                textBox2.Text = "";
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
