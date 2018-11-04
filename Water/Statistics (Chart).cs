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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }
        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public DataSet ds;
        public int ind;

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

        private void Form11_Load(object sender, EventArgs e)
        {
            Profile f5 = new Profile();
            start();

            string quer1 = @"SELECT id FROM journal ORDER BY data desc limit 1";
            GetSet(quer1, "journal");
            f5.listBox6.DataSource = ds.Tables[0].DefaultView;
            f5.listBox6.DisplayMember = "journal";
            f5.listBox6.ValueMember = "id";
            ind = int.Parse(f5.listBox6.Text);

            quer1 = @"SELECT date(data), sum(kolvo) FROM dob_voda WHERE id = " + ind + " group by date(data)";
            dataGridView1.DataSource = GetTable(quer1);

            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                chart1.Series[0].Points.AddXY(dataGridView1.Rows[i].Cells[0].Value, dataGridView1.Rows[i].Cells[1].Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            textBox2.Text = "" + dateTimePicker2.Value.Date.Year + "-" + dateTimePicker2.Value.Date.Month + "-" + dateTimePicker2.Value.Date.Day;
            textBox3.Text = "" + dateTimePicker3.Value.Date.Year + "-" + dateTimePicker3.Value.Date.Month + "-" + dateTimePicker3.Value.Date.Day;

            string quer1 = @"SELECT date(data), sum(kolvo) FROM dob_voda WHERE id = " + ind + " and date(data) between '" + textBox2.Text + "' and '" + textBox3.Text + "' group by date(data)";
            dataGridView1.DataSource = GetTable(quer1);

            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                chart1.Series[0].Points.AddXY(dataGridView1.Rows[i].Cells[0].Value, dataGridView1.Rows[i].Cells[1].Value);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string quer1 = @"SELECT date(data), sum(kolvo) FROM dob_voda WHERE id = " + ind + " group by date(data)";
            dataGridView1.DataSource = GetTable(quer1);

            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                chart1.Series[0].Points.AddXY(dataGridView1.Rows[i].Cells[0].Value, dataGridView1.Rows[i].Cells[1].Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Statistics f6 = new Statistics();
            this.Visible = false;
            f6.Visible = true;
        }
    }
}
