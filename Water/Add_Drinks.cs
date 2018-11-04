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
    public partial class Add_Drinks : Form
    {
        public Add_Drinks()
        {
            InitializeComponent();
        }

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public DataSet ds;
        public int ind;
        Main f3 = new Main();
        Profile f5 = new Profile();
        UserControl1 w1 = new UserControl1();

        private void Form4_Load(object sender, EventArgs e)
        {
            start();

            string quer1 = @"SELECT id FROM journal ORDER BY data desc limit 1";
            GetSet(quer1, "journal");
            f5.listBox6.DataSource = ds.Tables[0].DefaultView;
            f5.listBox6.DisplayMember = "journal";
            f5.listBox6.ValueMember = "id";
            ind = int.Parse(f5.listBox6.Text);
            
            quer1 = @"SELECT pol FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            f5.listBox2.DataSource = ds.Tables[0].DefaultView;
            f5.listBox2.DisplayMember = "bdw";
            f5.listBox2.ValueMember = "pol";            
            
            textBox9.Text = "" + dateTimePicker1.Value.Date.Year + "-0" + dateTimePicker1.Value.Date.Month + "-" + dateTimePicker1.Value.Date.Day;
        }

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

        private void button9_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();
            f3.Visible = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label1.Text + "','" + textBox1.Text + "','" + textBox1.Text + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;                        

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else 
                { 
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + "  мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox2.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label2.Text + "','" + textBox2.Text + "',-'" + Convert.ToInt32(textBox2.Text) * 0.2 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else
                {
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + "  мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox3.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label3.Text + "','" + textBox3.Text + "',-'" + Convert.ToInt32(textBox3.Text) * 1.5 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else
                {
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox4.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label4.Text + "','" + textBox4.Text + "',-'" + Convert.ToInt32(textBox4.Text) * 2.5 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else
                {
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox5.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label5.Text + "','" + textBox5.Text + "','" + Convert.ToInt32(textBox5.Text) * 0.8 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else
                {
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox6.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label6.Text + "','" + textBox6.Text + "','" + Convert.ToInt32(textBox6.Text) * 0.3 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else 
                { 
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox7.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label7.Text + "','" + textBox7.Text + "','" + Convert.ToInt32(textBox7.Text) * 0.2 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else 
                { 
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int p;
            if (textBox8.Text == "")
            {
                MessageBox.Show("Введите значение", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string quer1 = @"INSERT INTO dob_voda (id,drink,kolvo,kolvo_o,data) VALUES ('" + f5.listBox6.Text + "','" + label8.Text + "','" + textBox8.Text + "','" + Convert.ToInt32(textBox8.Text) * 0.5 + "', NOW());";
                GetSet(quer1, "dob_voda");

                quer1 = @"SELECT sum(kolvo) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox1.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox1.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox1.ValueMember = "sum(kolvo)";

                quer1 = @"SELECT sum(kolvo_o) FROM dob_voda WHERE id =" + ind + " and date(data)='" + textBox9.Text + "' group by date(data)";
                GetSet(quer1, "dob_voda");
                ((Main)this.Tag).listBox2.DataSource = ds.Tables[0].DefaultView;
                ((Main)this.Tag).listBox2.DisplayMember = "dob_voda";
                ((Main)this.Tag).listBox2.ValueMember = "sum(kolvo_o)";

                ((Main)this.Tag).elementHost1.Child = w1;

                if (f5.listBox2.Text == "Мужчина")
                {
                    w1.pictureBox1.Opacity = 100;
                }

                if (f5.listBox2.Text == "Женщина")
                {
                    w1.pictureBox2.Opacity = 100;
                }

                w1.progressBar1.Maximum = Convert.ToInt32(((Main)this.Tag).label5.Text);
                w1.progressBar1.Value = Convert.ToInt32(((Main)this.Tag).listBox2.Text);

                if (Convert.ToInt32(((Main)this.Tag).listBox2.Text) < w1.progressBar1.Maximum)
                {
                    ((Main)this.Tag).pictureBox4.Visible = false;
                    ((Main)this.Tag).label4.Visible = false;
                }
                else 
                { 
                    ((Main)this.Tag).pictureBox4.Visible = true;
                    ((Main)this.Tag).label4.Visible = true;
                }

                p = Convert.ToInt32((w1.progressBar1.Value * 100) / w1.progressBar1.Maximum);

                ((Main)this.Tag).label7.Text = "Сегодня: " + ((Main)this.Tag).listBox1.Text + " мл";
                ((Main)this.Tag).label8.Text = "Процент водного баланса составляет " + p + "%";

                ((Main)this.Tag).label12.Text = "" + w1.progressBar1.Value;
                ((Main)this.Tag).label13.Text = "" + w1.progressBar1.Maximum;

                f3.Visible = true;
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button1.Visible = true;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = true;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = true;
            button2.Visible = true;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = true;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = true;
            button3.Visible = true;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = true;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = true;
            button4.Visible = true;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = true;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = true;
            button5.Visible = true;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = true;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = true;
            button6.Visible = true;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = true;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = true;
            button7.Visible = true;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = true;
            label17.Visible = false;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = true;
            button8.Visible = true;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button1.Visible = true;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = true;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = true;
            button2.Visible = true;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = true;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = true;
            button3.Visible = true;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = true;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = true;
            button4.Visible = true;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = true;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = true;
            button5.Visible = true;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = true;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = true;
            button6.Visible = true;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = true;
            label16.Visible = false;
            label17.Visible = false;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = true;
            button7.Visible = true;
            textBox8.Visible = false;
            button8.Visible = false;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = true;
            label17.Visible = false;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            button1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            textBox3.Visible = false;
            button3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;
            textBox5.Visible = false;
            button5.Visible = false;
            textBox6.Visible = false;
            button6.Visible = false;
            textBox7.Visible = false;
            button7.Visible = false;
            textBox8.Visible = true;
            button8.Visible = true;

            label11.Visible = false;
            label9.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = true;
        }
    }
}
