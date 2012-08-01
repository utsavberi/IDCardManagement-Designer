using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardManagement
{
    class ControlMover
    {
        //public enum Direction
        //{
        //    Any,
        //    Horizontal,
        //    Vertical
        //}

        //public static void Init(Control control)
        //{
        //    Init(control);//, Direction.Any);
        //}

        public static void Init(Control control)//, Direction direction)
        {
            Init(control, control, false);//, direction);
        }

        public static void Init(Control control, Control container, Boolean multipleMove)//, Direction direction)
        {
            bool Dragging = false;
            Point DragStart = Point.Empty;
            control.MouseDown += delegate(object sender, MouseEventArgs e)
            {
                Dragging = true;
                DragStart = new Point(e.X, e.Y);
                control.Capture = true;
            };
            control.MouseUp += delegate(object sender, MouseEventArgs e)
            {
                Dragging = false;
                control.Capture = false;
            };
            control.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                if (Dragging)
                {
                    if (multipleMove == false)
                    {
                        //if (direction != Direction.Vertical)
                        container.Left = Math.Max(0, e.X + container.Left - DragStart.X);
                        //if (direction != Direction.Horizontal)
                        container.Top = Math.Max(0, e.Y + container.Top - DragStart.Y);
                    }
                    else
                    {
                        foreach (Control ctl in container.Controls)
                        {
                            if (ctl is Label && (ctl as Label).BorderStyle == BorderStyle.FixedSingle)
                               
                                {
                                    ctl.Left = Math.Max(0, e.X + ctl.Left - DragStart.X);
                                    ctl.Top = Math.Max(0, e.Y + ctl.Top - DragStart.Y);
                                }
                        }
                    }
                }
            };
        }
    }
}
