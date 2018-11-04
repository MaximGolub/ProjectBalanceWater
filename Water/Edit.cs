using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Water
{
    public partial class Edit : Form
    {
        public Edit()
        {
            InitializeComponent();
        }

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public DataSet ds;
        public int ind;        
        Profile f5 = new Profile();
        Journal f8 = new Journal();

        public void GetSet(string queryString, string dataTableName)
        {
            MySqlConnection SConnection = new MySqlConnection(conString);
            MySqlCommand SCommand = SConnection.CreateCommand();
            SCommand.CommandText = queryString;
            MySqlDataAdapter SDataAdapter = new MySqlDataAdapter();
            SDataAdapter.SelectCommand = SCommand;
            ds = new DataSet();
            SConnection.Open();
            SDataAdapter.Fill(ds, dataTableName);
            SConnection.Close();
        }

        private DataTable GetTable(string queryString)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(conString))
            {
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    {
                        if (dr.HasRows) dt.Load(dr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        
        private void start()
        {
            Opacity = 0;
            Timer timer1 = new Timer();
            timer1.Tick += new EventHandler((sender1, e1) =>
            {
                if ((Opacity += 0.05) == 1)
                    timer1.Stop();
            });
            timer1.Interval = 25;
            timer1.Start();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            start();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length < 1)
            {
                MessageBox.Show("Введите данные!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string sql = "UPDATE dnevnik SET zap='" + richTextBox1.Text + "' where id_zap='" + textBox1.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    string quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind;
                    f8.dataGridView1.DataSource = GetTable(quer1);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    if (f8.dataGridView1.Columns.Count != 0)
                    {
                        f8.dataGridView1.Columns[1].Width = 108;
                        f8.dataGridView1.Columns[0].HeaderText = "ID";
                        f8.dataGridView1.Columns[1].HeaderText = "Дата";
                        f8.dataGridView1.Columns[2].HeaderText = "Запись";
                    }

                    MessageBox.Show("Данные успешно обновлены", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    f8.Visible = true;
                    this.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f8.Visible = true;
            this.Close();
        }     
    }
}
