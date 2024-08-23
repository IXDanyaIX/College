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
    public partial class Form4 : Form
    {
        Database Database = new Database();
        DataGridView DataGridView;
        //string value_cb1;
        actions Action;
        private string old_key;


        public Form4(DataGridView dataGrid, actions action)
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
                textBox1.ReadOnly = true;
                DataGridViewRow row = DataGridView.Rows[DataGridView.CurrentCell.RowIndex];
                old_key = Convert.ToString(row.Cells[0].Value);
                textBox1.Text = row.Cells[0].Value.ToString();
                comboBox1.Text = row.Cells[1].Value.ToString();
            }

            
           
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            string query = $"SELECT name FROM Specialty";

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
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dt = new DataTable();
                string query = $"SELECT number FROM Specialty WHERE name = '{comboBox1.Text}'";

                SqlCommand Command = new SqlCommand(query, Database.GetConnection());

                adapter.SelectCommand = Command;
                adapter.Fill(dt);

            
                var number_group = textBox1.Text;
                var number_specialty = dt.Rows[0].ItemArray[0].ToString();



                string add_query = "";


                if (Action == actions.insert)
                {
                    add_query = $"INSERT INTO Group_class VALUES ('{number_group}', '{number_specialty}')";
                }
                else if (Action == actions.update)
                {
                    add_query = $"UPDATE Group_class SET number = '{number_group}', number_specialty = '{number_specialty}' WHERE number = '{old_key}'";
                }
                

                SqlCommand sqlCommand = new SqlCommand(add_query, Database.GetConnection());
                sqlCommand.ExecuteNonQuery();
                

                Database.CloseConnection();

                DataGridView.Rows.Clear();

                string query2 = $"SELECT Group_class.number, Specialty.name FROM Group_class, Specialty WHERE Group_class.number_specialty = Specialty.number";

                SqlCommand command = new SqlCommand(query2, Database.GetConnection());
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //value
        }
    }
}
