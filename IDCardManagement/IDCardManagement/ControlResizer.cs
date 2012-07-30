using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardManagement
{
    class ControlResizer
    {
        public static void Init(Control control)
        {
            bool mouseClicked = false;

            PictureBox handleSE = new PictureBox();
            handleSE.Cursor = Cursors.SizeNWSE;
            handleSE.BackColor = System.Drawing.Color.Transparent;
            handleSE.BackgroundImageLayout = ImageLayout.Stretch;
            handleSE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Bottom)) | System.Windows.Forms.AnchorStyles.Right)));
            handleSE.Left = control.Width - 6;
            handleSE.Top = control.Height - 6;
            control.Controls.Add(handleSE);
            handleSE.BackColor = System.Drawing.Color.Black;
            handleSE.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleSE.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleSE.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Height = handleSE.Top + e.Y;
                    control.Width = handleSE.Left + e.X;
                }
            };

            PictureBox handleE = new PictureBox();
            handleE.Cursor = Cursors.SizeWE;
            handleE.BackColor = System.Drawing.Color.Transparent;
            handleE.BackgroundImageLayout = ImageLayout.Stretch;
            handleE.Anchor = ((System.Windows.Forms.AnchorStyles)(( System.Windows.Forms.AnchorStyles.Right)));
            handleE.Left = control.Width - 6;
            handleE.Top = (control.Height-6)/2;
            handleE.Height = 6;
            control.Controls.Add(handleE);
            handleE.BackColor = System.Drawing.Color.Black;
            handleE.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleE.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleE.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    //control.Height = handleSE.Top + e.Y;
                    control.Width = handleSE.Left + e.X;
                }
            };

            PictureBox handleS = new PictureBox();
            handleS.Cursor = Cursors.SizeNS;
            handleS.BackColor = System.Drawing.Color.Transparent;
            handleS.BackgroundImageLayout = ImageLayout.Stretch;
            handleS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Bottom)) )));
            handleS.Left = (control.Width-6)/2;
            handleS.Width = 6;
            handleS.Top = control.Height-6;
            control.Controls.Add(handleS);
            handleS.BackColor = System.Drawing.Color.Black;
            handleS.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleS.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleS.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Height = handleSE.Top + e.Y;
                    //control.Width = handleSE.Left + e.X;
                }
            };

            PictureBox handleN = new PictureBox();
            handleN.Cursor = Cursors.SizeNS;
            handleN.BackColor = System.Drawing.Color.Transparent;
            handleN.BackgroundImageLayout = ImageLayout.Stretch;
            handleN.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)))));
            handleN.Left = (control.Width-6 )/ 2;
            handleN.Width =6;
            handleN.Top = 0;//control.Height - 10;
            handleN.Height = 6;
            control.Controls.Add(handleN);
            handleN.BackColor = System.Drawing.Color.Black;
            handleN.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleN.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleN.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    
                    control.Height = control.Height - e.Y  ;
                    control.Top = control.Top + e.Y;
                                       
                }
            };


            PictureBox handleW = new PictureBox();
            handleW.Cursor = Cursors.SizeWE;
            handleW.BackColor = System.Drawing.Color.Transparent;
            handleW.BackgroundImageLayout = ImageLayout.Stretch;
            handleW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left)));
            handleW.Left = 0;// control.Width - 10;
            handleW.Top = (control.Height-6 )/ 2;
            handleW.Height = 6;
            handleW.Width = 6;
            control.Controls.Add(handleW);
            handleW.BackColor = System.Drawing.Color.Black;
            handleW.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleW.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleW.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Width = control.Width - e.X;
                    control.Left = control.Left + e.X;
                                       
                   
                }
            };


            PictureBox handleSW = new PictureBox();
            handleSW.Cursor = Cursors.SizeNESW;
            handleSW.BackColor = System.Drawing.Color.Transparent;
            handleSW.BackgroundImageLayout = ImageLayout.Stretch;
            handleSW.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Bottom)) | System.Windows.Forms.AnchorStyles.Left)));
            handleSW.Left = 0;//control.Width - 10;
            handleSW.Top = control.Height - 6;
            handleSW.Width = 6;
            control.Controls.Add(handleSW);
            handleSW.BackColor = System.Drawing.Color.Black;
            handleSW.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleSW.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleSW.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Width = control.Width - e.X;
                    control.Left = control.Left + e.X;
                    control.Height = handleSE.Top + e.Y;
                }
            };


            PictureBox handleNW = new PictureBox();
            handleNW.Cursor = Cursors.SizeNWSE;
            handleNW.BackColor = System.Drawing.Color.Transparent;
            handleNW.BackgroundImageLayout = ImageLayout.Stretch;
            handleNW.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)) | System.Windows.Forms.AnchorStyles.Left)));
            handleNW.Left = 0;//control.Width - 10;
            handleNW.Top = 0;//control.Height - 10;
            handleNW.Width = 6;
            handleNW.Height = 6;
            control.Controls.Add(handleNW);
            handleNW.BackColor = System.Drawing.Color.Black;
            handleNW.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleNW.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleNW.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Width = control.Width - e.X;
                    control.Left = control.Left + e.X;
                    control.Height = control.Height - e.Y;
                    control.Top = control.Top + e.Y;
                             
                }
            };

            PictureBox handleNE = new PictureBox();
            handleNE.Cursor = Cursors.SizeNESW;
            handleNE.BackColor = System.Drawing.Color.Transparent;
            handleNE.BackgroundImageLayout = ImageLayout.Stretch;
            handleNE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)) | System.Windows.Forms.AnchorStyles.Right)));
            handleNE.Left = control.Width - 6;
            handleNE.Top = 0;//control.Height - 10;
            handleNE.Width = 6;
            handleNE.Height = 6;
            control.Controls.Add(handleNE);
            handleNE.BackColor = System.Drawing.Color.Black;
            handleNE.MouseDown += delegate(object sender, MouseEventArgs e) { mouseClicked = true; };
            handleNE.MouseUp += delegate(object sender, MouseEventArgs e) { mouseClicked = false; };
            handleNE.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (mouseClicked)
                {
                    control.Height = control.Height - e.Y;
                    control.Top = control.Top + e.Y;
                    control.Width = handleSE.Left + e.X;

                }
            };
        }




    }
}
