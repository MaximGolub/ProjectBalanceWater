using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Water
{
    public partial class Main : Form
    {
        public Main()
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
        
        private void Form3_Load(object sender, EventArgs e)
        {
            start();

            Profile f5 = new Profile();
            UserControl1 w1 = new UserControl1();
            elementHost1.Child = w1;

            string[] myList = new string[2];
            myList[0] = "Мужчина";
            myList[1] = "Женщина";
            f5.comboBox1.Items.AddRange(myList);

            string[] myList2 = new string[4];
            myList2[0] = "Выкл";
            myList2[1] = "Легкая";
            myList2[2] = "Умеренная";
            myList2[3] = "Тяжелая";
            f5.comboBox2.Items.AddRange(myList2);

            string quer1 = @"SELECT id FROM journal ORDER BY data desc limit 1";
            GetSet(quer1, "journal");
            f5.listBox6.DataSource = ds.Tables[0].DefaultView;
            f5.listBox6.DisplayMember = "journal";
            f5.listBox6.ValueMember = "id";
            ind = int.Parse(f5.listBox6.Text);

            quer1 = @"SELECT name FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            f5.listBox1.DataSource = ds.Tables[0].DefaultView;
            f5.listBox1.DisplayMember = "bdw";
            f5.listBox1.ValueMember = "name";

            quer1 = @"SELECT weight FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            f5.listBox4.DataSource = ds.Tables[0].DefaultView;
            f5.listBox4.DisplayMember = "bdw";
            f5.listBox4.ValueMember = "weight";

            quer1 = @"SELECT pol FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            f5.listBox2.DataSource = ds.Tables[0].DefaultView;
            f5.listBox2.DisplayMember = "bdw";
            f5.listBox2.ValueMember = "pol";

            if (f5.listBox2.Text == "Мужчина")
            {
                w1.pictureBox1.Opacity = 100;
                int y = Convert.ToInt32(f5.listBox4.Text);
                int z = y * 35;
                w1.progressBar1.Maximum = z;
                label5.Text = "" + z;
            }

            if (f5.listBox2.Text == "Женщина")
            {
                w1.pictureBox2.Opacity = 100;
                int y = Convert.ToInt32(f5.listBox4.Text);
                int z = y * 31;
                w1.progressBar1.Maximum = z;
                label5.Text = "" + z;
            }
            
            quer1 = @"SELECT sport FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            f5.listBox7.DataSource = ds.Tables[0].DefaultView;
            f5.listBox7.DisplayMember = "bdw";
            f5.listBox7.ValueMember = "sport";
            f5.comboBox2.Text = "" + f5.listBox7.Text;

            if (f5.comboBox2.Text == "Легкая")
            {
                f5.comboBox2.Text = f5.listBox7.Text;

                int h = Convert.ToInt32(label5.Text);

                w1.progressBar1.Maximum += 300;
                label5.Text = "" + h;
            }

            if (f5.comboBox2.Text == "Умеренная")
            {
                f5.comboBox2.Text = f5.listBox7.Text;

                int q = Convert.ToInt32(label5.Text);

                w1.progressBar1.Maximum += 500;
                q += 500;
                label5.Text = "" + q;
            }

            if (f5.comboBox2.Text == "Тяжелая")
            {
                f5.comboBox2.Text = f5.listBox7.Text;

                int q = Convert.ToInt32(label5.Text);

                w1.progressBar1.Maximum += 800;
                q += 800;
                label5.Text = "" + q;
            }

            textBox3.Text = "" + dateTimePicker1.Value.Date.Year + "-0" + dateTimePicker1.Value.Date.Month + "-" + dateTimePicker1.Value.Date.Day;

            quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox3.Text + "' group by date(data)";
            GetSet(quer1, "dob_voda");
            listBox1.DataSource = ds.Tables[0].DefaultView;
            listBox1.DisplayMember = "dob_voda";
            listBox1.ValueMember = "sum(kolvo)";

            quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox3.Text + "' group by date(data)";
            GetSet(quer1, "dob_voda");
            listBox2.DataSource = ds.Tables[0].DefaultView;
            listBox2.DisplayMember = "dob_voda";
            listBox2.ValueMember = "sum(kolvo_o)";
            
            string newLine;
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("http://pogodnik.com/xml/informer/ru/vertical/01/zsvKzw%3D%3D");
                StreamReader sr = new StreamReader(stream);
                while ((newLine = sr.ReadLine()) != null) richTextBox1.Text += newLine;
                stream.Close();
            }
            catch (Exception) { webBrowser1.Visible = false; label6.Visible = false; }
            
            if (Regex.IsMatch(richTextBox1.Text, @"[+]([0-9]|[0-9][0-9])[°C]"))
            {
                Regex reg = new Regex(textBox1.Text + @"[+]([0-9]|[0-9][0-9])[°C]", RegexOptions.IgnoreCase);
                MatchCollection math = reg.Matches(richTextBox1.Text);
                textBox1.Text = math[0].ToString().Replace(textBox1.Text + " ", "");
            }
            
            string text = textBox1.Text;
            text = text.Trim();
            text = text.Trim(new char[] { '+', '°' });
            textBox1.Text = text;

            if (textBox1.Text != "")
            {
                int f = Convert.ToInt32(textBox1.Text);
                int q = Convert.ToInt32(label5.Text);
                if (f > 20)
                {
                    w1.progressBar1.Maximum += 1000;
                    pictureBox6.Visible = true;
                    label10.Visible = true;
                    q += 1000;
                    label5.Text = "" + q;
                }
            }

            if (Regex.IsMatch(richTextBox1.Text, @"[-]([0-9]|[0-9][0-9])[°C]"))
            {
                Regex reg1 = new Regex(textBox2.Text + @"[-]([0-9]|[0-9][0-9])[°C]", RegexOptions.IgnoreCase);
                MatchCollection math1 = reg1.Matches(richTextBox1.Text);
                textBox2.Text = math1[0].ToString().Replace(textBox2.Text + " ", "");
            }

            string text2 = textBox2.Text;
            text2 = text2.Trim();
            text2 = text2.Trim(new char[] { '-', '°' });
            textBox2.Text = text2;

            if (textBox2.Text != "")
            {
                int g = Convert.ToInt32(textBox2.Text);
                int q = Convert.ToInt32(label5.Text);
                if (g >= 1)
                {
                    w1.progressBar1.Maximum -= 500;
                    pictureBox7.Visible = true;
                    label11.Visible = true;
                    q -= 500;
                    label5.Text = "" + q;
                }
            }
            
            if (listBox1.Text != "")
            {
                if (Convert.ToInt32(listBox2.Text) < w1.progressBar1.Maximum)
                {
                    pictureBox4.Visible = false;
                    label4.Visible = false;
                }
                if (Convert.ToInt32(listBox2.Text) > w1.progressBar1.Maximum)
                {
                    pictureBox4.Visible = true;
                    label4.Visible = true;
                }

                label7.Text = "Сегодня: " + listBox1.Text + " мл";
            }
            else
            {
                label7.Text = "Сегодня: 0 мл";
            }

            if (listBox2.Text != "")
            {
                w1.progressBar1.Value = Convert.ToInt32(listBox2.Text);
                label9.Text = "Водный Баланс: " + listBox2.Text + " мл";
            }
            else
            {
                label9.Text = "Водный Баланс: 0 мл";
            }

            label12.Text = "" + w1.progressBar1.Value;
            label13.Text = "" + w1.progressBar1.Maximum;

            int p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);
            
            label1.Text = "Здравствуйте, " + f5.listBox1.Text;
            label3.Text = "Норма: " + w1.progressBar1.Maximum +" мл";
            label8.Text = "Процент водного баланса составляет " + p +"%";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Add_Drinks f4 = new Add_Drinks();
            f4.Tag = this;
            this.Visible = false;
            f4.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Profile f5 = new Profile();
            f5.Tag = this;
            this.Visible = false;
            f5.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Statistics f6 = new Statistics();
            f6.Tag = this;
            this.Visible = false;
            f6.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Useful_Tips f7 = new Useful_Tips();
            f7.Tag = this;
            this.Visible = false;
            f7.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Journal f8 = new Journal();
            f8.Tag = this;
            this.Visible = false;
            f8.ShowDialog();
        }

        private void elementHost1_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }   
    }
}
