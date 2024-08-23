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
    public partial class Form6 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        actions Action;
        private int old_key;
        public Form6(DataGridView dataGrid, actions action)
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
                textBox1.Text = row.Cells[1].Value.ToString();
               
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Database.OpenConnection();

       
                var name = textBox1.Text;

                string add_query = "";
                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Object VALUES ('{name}')";
                }
                else if(Action == actions.update)
                {
                    add_query = $"UPDATE Object SET name = '{name}' WHERE id = '{old_key}'";
                }

                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();
                

                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query = $"SELECT * FROM Object";
               // string query2 = $"SELECT name FROM Object WHERE id = {DataGridView.}"

                SqlCommand command = new SqlCommand(query, Database.GetConnection());
                Database.OpenConnection();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DataGridView.Rows.Add(reader.GetInt32(0), reader.GetString(1));
                }
                reader.Close();
                Database.CloseConnection();
                MessageBox.Show("Данные успешно добавлены!");
                textBox1.Text = "";
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
