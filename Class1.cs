using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using System.Data.Sql;
using System.Data.SQLite;

namespace Colledg
{
    internal class Database
    {
        SqlConnection SqlConnection = new SqlConnection(@"Data Source=LAPTOP-R80O94U8;Initial Catalog=college;User ID=daniil;Password=ADCLG1");
                                                                    
        public void OpenConnection()
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed)
            {
                SqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if(SqlConnection.State == System.Data.ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }

        public SqlConnection GetConnection()
        {
            return SqlConnection;
        }

        //public void ReadSingleRow(DataGridView dgw, IDataRecord record)
        //{
        //    dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetInt32(4));

        //}

        //public void RefreshDataGrid(DataGridView dwg, string name_table)
        //{
        //    dwg.Rows.Clear();

        //    string query = $"SELECT * FROM {name_table}";

        //    SqlCommand command = new SqlCommand(query, this.GetConnection());
        //    this.OpenConnection();

        //    SqlDataReader reader = command.ExecuteReader();

        //    while (reader.Read())
        //    {
        //        ReadSingleRow(dwg, reader);
        //    }
        //    reader.Close();
        //    this.CloseConnection();
        //}

        public void DeleteRow(DataGridView dwg, string name_table, string key)
        {
            try
            {
                this.OpenConnection();

                int index = dwg.CurrentCell.RowIndex;
                
                for (int i = 0; i < dwg.Rows.Count; i++)
                {
                    if (i == index)
                    {
                        var id = dwg.Rows[i].Cells[0].Value;
                        var delete_query = $"DELETE FROM {name_table} where {key} = '{id}'";

                        SqlCommand command = new SqlCommand(delete_query, this.GetConnection());
                        command.ExecuteNonQuery();
                        MessageBox.Show("Запись была успешно удалена!");
                    }
                    
                }

                this.CloseConnection();
                dwg.Rows.RemoveAt(index);
                
            }
            catch(Exception ex)
            {
                this.CloseConnection();
                MessageBox.Show("Не удалось удалить запись!");
            }
        }
    }
}
