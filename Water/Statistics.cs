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
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
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

        private void Form6_Load(object sender, EventArgs e)
        {
            start();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Form10 f10 = new Form10();
            f10.Tag = this;
            this.Visible = false;
            f10.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form11 f11 = new Form11();
            f11.Tag = this;
            this.Visible = false;
            f11.ShowDialog();
        } 

        private void button5_Click(object sender, EventArgs e)
        {
            Main f3 = new Main();
            f3.Visible = true;
            this.Visible = false;
        }
    }
}
