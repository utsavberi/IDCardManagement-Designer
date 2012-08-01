using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCardManagement
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        public static void showSplashScreen()
        {
            SplashScreen sc = new SplashScreen();
            sc.showSplash();
        }

        public void showSplash()
        {

            this.Opacity = 0;
            this.Show();
            Timer t = new Timer();
            t.Interval = 100;
            t.Enabled = true;
            t.Tick += t_Tick;

        }
        double k = .2;
        void t_Tick(object sender, EventArgs e)
        {
            if (Opacity == 1) k = -.3;
            this.Opacity += k;
            if (Opacity == 0) Close();
            //this.Close();
        }
    }
}
