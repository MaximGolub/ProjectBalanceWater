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
using System.Drawing.Printing;
using MySql.Data.MySqlClient;

namespace Water
{
    public partial class Useful_Tips : Form
    {
        public Useful_Tips()
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

        private void Form7_Load(object sender, EventArgs e)
        {
            start();
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = true;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = false;
            button9.Visible = true;
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile2.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = true;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = false;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile3.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = true;
            button12.Visible = false;
            button13.Visible = false;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile4.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = true;
            button13.Visible = false;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile5.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            start();
            pictureBox1.Visible = true;
            richTextBox1.Visible = true;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button12.Visible = false;
            button13.Visible = true;

            try
            {
                using (StreamReader sr = new StreamReader("D://Программы//Колледж//Диплом//Water//Файлы//TestFile6.txt"))
                {
                    String line = sr.ReadToEnd();
                    richTextBox1.Text = "" + line;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("The file could not be read:");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();
            f3.Visible = true;
            this.Close();
        }

        void prin1(object sender, PrintPageEventArgs e)
        {
            Graphics grap = e.Graphics;
            grap.DrawString(richTextBox1.Text, Font, new SolidBrush(Color.Black), 0, 0);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }     

        private void button9_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile2.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile3.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile4.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile5.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            PrintDocument printText = new PrintDocument();
            printText.DocumentName = "D://Программы//Колледж//Диплом//Water//Файлы//TestFile6.txt";
            printDialog.Document = printText;
            printDialog.AllowSelection = true;
            printDialog.AllowSomePages = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printText.PrintPage += new PrintPageEventHandler(prin1);
                printText.Print();
            }
        }
    }
}
