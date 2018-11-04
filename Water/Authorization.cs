using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Water
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            start();
        }

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public static string USERNAME = "",PASSWORD = "";
        public DataSet ds;
        Regex _RUN = new Regex("[a-w,A-W,0-9,_,]");

        public void start()
        {
            Opacity = 0;
            Timer timer = new Timer();
            timer.Tick += new EventHandler((sender, e) =>
            {
                if ((Opacity += 0.05) == 1) timer.Stop();
            });
            timer.Interval = 25;
            timer.Start();
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();

            if (textBox1.Text.Length < 1 && textBox2.Text.Length < 1) { MessageBox.Show("Введите логин и пароль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (textBox1.Text.Length < 1) { MessageBox.Show("Введите логин", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (textBox2.Text.Length < 1) { MessageBox.Show("Введите пароль", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            
            if (_RUN.IsMatch(textBox1.Text) && _RUN.IsMatch(textBox2.Text))
            {
                string quer = @"SELECT id FROM bdw WHERE login LIKE '" + textBox1.Text + "' and pass LIKE '" + textBox2.Text + "' LIMIT 1;";
                GetSet(quer, "bdw");  
                listBox1.DataSource = ds.Tables[0].DefaultView;
                listBox1.DisplayMember = "bdw";
                listBox1.ValueMember = "id";

                if (listBox1.Text != "")
                    {
                        quer = @"INSERT INTO journal (id, data) VALUES ('" + int.Parse(listBox1.Text) + "', NOW());";
                        GetSet(quer, "journal");
                        this.Visible = false;

                        f3.ShowDialog();
                    }
                else MessageBox.Show("Неверные учетные данные", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Вы ввели некорректные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            Registration f2 = new Registration();   
            f2.Tag = this;
            this.Visible = false;
            f2.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }    
    }
}
