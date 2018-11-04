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
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();      
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            start();
            string[] myList = new string[2];
            myList[0] = "Мужчина";
            myList[1] = "Женщина";
            comboBox1.Items.AddRange(myList);
            comboBox1.SelectedIndex = 0;
        }

        public static string conString = "Database=water; DataSource=localhost; UserId=root; Password='1234';";
        public static string USERNAME = "", EMAIL = "";
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
        
        public static DataTable SQL_GET(string queryString)
        {
            DataTable dt = new DataTable(); MySqlConnection con;
            using (con = new MySqlConnection(conString))
            {
                MySqlCommand com = new MySqlCommand(queryString, con);
                try
                {
                    con.Open();
                    using (MySqlDataReader dr = com.ExecuteReader())
                    { if (dr.HasRows) dt.Load(dr); }
                }
                catch (Exception)
                { con.Close(); MessageBox.Show("Ошибка базы данных"); return null; }
            }
            con.Close(); return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Authorization f1 = new Authorization();

            if (textBox1.Text.Length < 1) textBox1.BackColor = Color.Red;
            if (textBox3.Text.Length < 1 || Convert.ToInt32(textBox3.Text) == 0 || Convert.ToInt32(textBox3.Text) > 300) textBox3.BackColor = Color.Red;
            if (textBox4.Text.Length < 1 || Convert.ToInt32(textBox4.Text) == 0 || Convert.ToInt32(textBox4.Text) > 300) textBox4.BackColor = Color.Red;          
            if (textBox7.Text.Length < 1) textBox7.BackColor = Color.Red;
            if (textBox8.Text.Length < 1) textBox8.BackColor = Color.Red;

            if (textBox1.Text.Length < 1 || comboBox1.Text.Length < 1 || textBox3.Text.Length < 1 || Convert.ToInt32(textBox3.Text) == 0 || Convert.ToInt32(textBox3.Text) > 300 || textBox4.Text.Length < 1 || Convert.ToInt32(textBox4.Text) == 0 || Convert.ToInt32(textBox4.Text) > 300 || dateTimePicker1.Value.Date.Year > dateTimePicker2.Value.Date.Year || dateTimePicker1.Value.Date.Month > dateTimePicker2.Value.Date.Month || dateTimePicker1.Value.Date.Day > dateTimePicker2.Value.Date.Day) { MessageBox.Show("Вы ввели некорректные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            
            if (Regex.IsMatch(textBox8.Text, @"[a-zA-Zа-яА-Я,-;:]{6,50}"))
            {
                using (MySqlConnection con = new MySqlConnection(conString))
                {
                    try
                    {
                        if (_RUN.IsMatch(textBox7.Text))
                        {
                            DataTable data1 = SQL_GET("SELECT id FROM bdw WHERE login LIKE '" + textBox7.Text + "' LIMIT 1;");
                            if (data1 != null && data1.Rows.Count > 0)
                            {
                                USERNAME = textBox7.Text;
                                data1 = SQL_GET("SELECT id FROM bdw WHERE login LIKE '" + textBox7.Text + "' LIMIT 1;");
                                textBox7.BackColor = Color.Red;
                                MessageBox.Show("Пользователь с таким логином уже существует", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else if (data1 != null)
                            {
                                string sql = "INSERT INTO bdw (name, pol, rise, weight, bd, login, pass, sport)" + "VALUES ('" + textBox1.Text + "', '" + comboBox1.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + dateTimePicker1.Value.Date.Year.ToString() + "-" + dateTimePicker1.Value.Date.Month.ToString() + "-" + dateTimePicker1.Value.Date.Day.ToString() + "','" + textBox7.Text + "','" + textBox8.Text + "','Выкл');";
                                MySqlCommand cmd = new MySqlCommand(sql, con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Регистрация прошла успешно", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                f1.Visible = true;
                                this.Close();
                            }
                        }
                        else { MessageBox.Show("Вы ввели некорректные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else { textBox8.BackColor = Color.Red; MessageBox.Show("Вы ввели некорректные учетные данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1) ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            textBox1.BackColor = Color.White;
        }
        
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsSymbol(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            if (char.IsPunctuation(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            if (char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;       
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.White;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsSymbol(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
            if (char.IsPunctuation(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
            if (char.IsLetter(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.BackColor = Color.White;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != 8) e.Handled = true;
            if (!((e.KeyChar >= ' ' && e.KeyChar <= 'z') || e.KeyChar == 8)) e.Handled = true;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            textBox7.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Authorization f1 = new Authorization();
            f1.Visible = true;
            this.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox8.BackColor = Color.White;
        }
    }
}