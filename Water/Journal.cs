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
    public partial class Journal : Form
    {
        public Journal()
        {
            InitializeComponent();
        }        

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public DataSet ds;
        public int ind;
        Profile f5 = new Profile();

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

        private void Form8_Load(object sender, EventArgs e)
        {
            start();

            string quer1 = @"SELECT id FROM journal ORDER BY data desc limit 1";
            GetSet(quer1, "journal");
            f5.listBox6.DataSource = ds.Tables[0].DefaultView;
            f5.listBox6.DisplayMember = "journal";
            f5.listBox6.ValueMember = "id";
            ind = int.Parse(f5.listBox6.Text);

            quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind;
            dataGridView1.DataSource = GetTable(quer1);

            if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Width = 108;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Дата";
                dataGridView1.Columns[2].HeaderText = "Запись";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();
            f3.Visible = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length < 1)
            {
                MessageBox.Show("Введите данные!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else
            {
                string quer1 = @"INSERT INTO dnevnik (id,zap,data) VALUES ('" + f5.listBox6.Text + "','" + richTextBox1.Text + "', NOW());";
                GetSet(quer1, "dnevnik");
                MessageBox.Show("Данные успешно добавлены", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind;
                
                dataGridView1.DataSource = GetTable(quer1);

                if (dataGridView1.Columns.Count != 0)
                {
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 108;
                    dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Дата";
                    dataGridView1.Columns[2].HeaderText = "Запись";
                }

                richTextBox1.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind;
            dataGridView1.DataSource = GetTable(quer1);

            if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Width = 108;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Дата";
                dataGridView1.Columns[2].HeaderText = "Запись";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "" + dateTimePicker1.Value.Date.Year + "-" + dateTimePicker1.Value.Date.Month + "-" + dateTimePicker1.Value.Date.Day;
            textBox2.Text = "" + dateTimePicker2.Value.Date.Year + "-" + dateTimePicker2.Value.Date.Month + "-" + dateTimePicker2.Value.Date.Day;
            
            string quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind + " and date(data) between '" + textBox1.Text + "' and '" + textBox2.Text + "'";
            dataGridView1.DataSource = GetTable(quer1);

            if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Width = 108;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[1].HeaderText = "Дата";
                dataGridView1.Columns[2].HeaderText = "Запись";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox3.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                DialogResult result = MessageBox.Show(" Удалить данные? ", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (result)
                {
                    case DialogResult.Yes:
                        {
                            string quer = @"DELETE FROM dnevnik WHERE id_zap ='" + textBox3.Text + "';";
                            dataGridView1.DataSource = GetTable(quer);

                            break;
                        }
                    case DialogResult.No:
                        {
                            break;
                        }
                }

                string quer1 = @"SELECT id_zap, data, zap FROM dnevnik WHERE id = " + ind;
                dataGridView1.DataSource = GetTable(quer1);

                if (dataGridView1.Columns.Count != 0)
                {
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 108;
                    dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dataGridView1.Columns[0].HeaderText = "ID";
                    dataGridView1.Columns[1].HeaderText = "Дата";
                    dataGridView1.Columns[2].HeaderText = "Запись";
                }
            }
            else MessageBox.Show("Выберите запись", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Edit f9 = new Edit();

            int index;

            if (textBox3.Text != "")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    index = dataGridView1.CurrentRow.Index;

                    f9.textBox1.Text = dataGridView1[0, index].Value.ToString();
                    f9.richTextBox1.Text = dataGridView1[2, index].Value.ToString();
                }

                this.Visible = false;
                f9.ShowDialog();
            }
            else MessageBox.Show("Выберите запись", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
        }
    }
}
