using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Denizhan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SQLiteConnection sqlite_conn;
        private void btn_tikla_Click(object sender, EventArgs e)
        {
            SQLiteCommand sqlite_cmd;
            using (sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;"))
            {
                try
                {
                    sqlite_cmd = sqlite_conn.CreateCommand();
                    sqlite_conn.Open();

                    if (!TabloVarMi("tbl_Records", "Data Source=database.db;Version=3;New=True;Compress=True;"))
                    {
                        sqlite_cmd.CommandText = "CREATE TABLE tbl_Records (id INTEGER PRIMARY KEY AUTOINCREMENT, rc_date varchar(50));";
                        sqlite_cmd.ExecuteNonQuery(); 
                        MessageBox.Show("Tablo oluştu");
                    }
                    else
                    {
                        DataSet ds = new DataSet();
                        sqlite_cmd.CommandText = "INSERT INTO tbl_Records (rc_date) VALUES ('" + DateTime.Now.ToLongTimeString() + "');";
                        sqlite_cmd.ExecuteNonQuery();
                        sqlite_cmd.CommandText = "SELECT rc_date FROM tbl_Records";
                        var da = new SQLiteDataAdapter(sqlite_cmd.CommandText, sqlite_conn);
                        da.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0].DefaultView;
                    }
                }
                catch
                {
                    MessageBox.Show("bağlantı hatası");
                }
            }
        }
        public bool TabloVarMi(String tableName, string sDatabaseName)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
            bool snc = false;
            try
            {
                sqlite_cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "'";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                if (sqlite_datareader.Read())
                {
                    snc = true;
                }
                sqlite_datareader.Close();
                return snc;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablo mevcut");
                return false;
            }
        }
    }
}