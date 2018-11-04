using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace Water
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public DataSet ds;
        public int ind;
        UserControl1 w1 = new UserControl1();
        
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

        private void Form5_Load(object sender, EventArgs e)
        {
            start();
            string[] myList = new string[2];
            myList[0] = "Мужчина";
            myList[1] = "Женщина";
            comboBox1.Items.AddRange(myList);

            string[] myList2 = new string[4];
            myList2[0] = "Выкл";
            myList2[1] = "Легкая";
            myList2[2] = "Умеренная";
            myList2[3] = "Тяжелая";
            comboBox2.Items.AddRange(myList2);

            string quer1 = @"SELECT id FROM journal ORDER BY data desc limit 1";
            GetSet(quer1, "journal");
            listBox6.DataSource = ds.Tables[0].DefaultView;
            listBox6.DisplayMember = "journal";
            listBox6.ValueMember = "id";
            ind = int.Parse(listBox6.Text);

            quer1 = @"SELECT name FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox1.DataSource = ds.Tables[0].DefaultView;
            listBox1.DisplayMember = "bdw";
            listBox1.ValueMember = "name";
            textBox1.Text = "" + listBox1.Text;

            quer1 = @"SELECT pol FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox2.DataSource = ds.Tables[0].DefaultView;
            listBox2.DisplayMember = "bdw";
            listBox2.ValueMember = "pol";
            comboBox1.Text = "" + listBox2.Text;

            quer1 = @"SELECT rise FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox3.DataSource = ds.Tables[0].DefaultView;
            listBox3.DisplayMember = "bdw";
            listBox3.ValueMember = "rise";
            textBox3.Text = "" + listBox3.Text;

            quer1 = @"SELECT weight FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox4.DataSource = ds.Tables[0].DefaultView;
            listBox4.DisplayMember = "bdw";
            listBox4.ValueMember = "weight";
            textBox4.Text = "" + listBox4.Text;

            quer1 = @"SELECT bd FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox5.DataSource = ds.Tables[0].DefaultView;
            listBox5.DisplayMember = "bdw";
            listBox5.ValueMember = "bd";
            dateTimePicker1.Text = "" + listBox5.Text;

            quer1 = @"SELECT sport FROM bdw WHERE id = " + ind;
            GetSet(quer1, "bdw");
            listBox7.DataSource = ds.Tables[0].DefaultView;
            listBox7.DisplayMember = "bdw";
            listBox7.ValueMember = "sport";
            comboBox2.Text = "" + listBox7.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();
            f3.Visible = true;

            if (comboBox1.Text == "Мужчина")
            {
                w1.pictureBox1.Opacity = 100;
                if (textBox4.Text != "")
                {
                    int y = Convert.ToInt32(textBox4.Text);
                    int z = y * 35;
                    w1.progressBar1.Maximum = z;
                }
                else
                {
                    textBox4.Text = listBox4.Text;
                    int y = Convert.ToInt32(textBox4.Text);
                    int z = y * 35;
                    w1.progressBar1.Maximum = z;
                }
            }

            if (comboBox1.Text == "Женщина")
            {
                w1.pictureBox2.Opacity = 100;
                if (textBox4.Text != "")
                {
                    int y = Convert.ToInt32(textBox4.Text);
                    int z = y * 31;
                    w1.progressBar1.Maximum = z;
                    ((Main)this.Tag).label5.Text = "" + z;
                }
                else
                {
                    textBox4.Text = listBox4.Text;
                    int y = Convert.ToInt32(textBox4.Text);
                    int z = y * 31;
                    w1.progressBar1.Maximum = z;
                    ((Main)this.Tag).label5.Text = "" + z;
                }
            }

            if (comboBox2.Text == "Легкая")
            {
                comboBox2.Text = listBox7.Text;

                int q = Convert.ToInt32(((Main)this.Tag).label5.Text);
                
                w1.progressBar1.Maximum += 300;
                q += 300;
                ((Main)this.Tag).label5.Text = "" + q;
            }

            if (comboBox2.Text == "Умеренная")
            {
                comboBox2.Text = listBox7.Text;

                int q = Convert.ToInt32(((Main)this.Tag).label5.Text);

                w1.progressBar1.Maximum += 500;
                q += 500;
                ((Main)this.Tag).label5.Text = "" + q;
            }

            if (comboBox2.Text == "Тяжелая")
            {
                comboBox2.Text = listBox7.Text;

                int q = Convert.ToInt32(((Main)this.Tag).label5.Text);

                w1.progressBar1.Maximum += 800;
                q += 800;
                ((Main)this.Tag).label5.Text = "" + q;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Length < 1 || textBox3.Text.Length < 1 || Convert.ToInt32(textBox3.Text) == 0 || Convert.ToInt32(textBox3.Text) > 300 || textBox4.Text.Length < 1 || Convert.ToInt32(textBox4.Text) == 0 || Convert.ToInt32(textBox4.Text) > 300) { MessageBox.Show("Вы ввели некорректные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            else
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    string sql = "UPDATE bdw SET name='" + textBox1.Text + "', pol='" + comboBox1.Text + "', rise='" + textBox3.Text + "', weight='" + textBox4.Text + "', bd='" + dateTimePicker1.Value.Date.Year.ToString() + "-" + dateTimePicker1.Value.Date.Month.ToString() + "-" + dateTimePicker1.Value.Date.Day.ToString() + "', sport='" + comboBox2.Text + "' where id=" + ind;
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Данные успешно обновлены", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1) ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsSymbol(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            if (char.IsPunctuation(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            if (char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsSymbol(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
            if (char.IsPunctuation(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
            if (char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Категория нагрузки: Легкая \nФигурное катание, фристайл, конный спорт, гимнастика, прыжки в воду, стрельба из лука.\n\nКатегория нагрузки: Умеренная \nБег, плавание, велосипедный спорт, фехтование, хоккей, футбол, баскетбол, теннис.\n\nКатегория нагрузки: Тяжелая \nБокс,восточные единоборства, легкоатлетические прыжки, метания, тяжелая атлетика.", "Режим Спорт", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
