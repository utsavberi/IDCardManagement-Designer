using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace IDCardManagement
{
    public partial class Form2 : Form
    {
        private int Y;
        private int X;
        private IDCard idcard;
        PictureBox pictureBox1;
        Panel panel1;
        Label titleLbl;
        String filename;

        public Form2()
        {
            InitializeComponent();
            gridStatus = 0;

        }

        //called when "file->new" 
        public Form2(IDCard idcard)
        {
            InitializeComponent();
            this.idcard = idcard;
            Form2_LoadFile();
        }

        private void Form2_Load(object o, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click on Open or New to start working...";
            foreach (FontFamily font in System.Drawing.FontFamily.Families) fontToolStripComboBox.Items.Add(font.Name);

        }

        private void Form2_LoadFile()
        {
            if (idcard.backgroundImage != null) panel1.BackgroundImage = idcard.backgroundImage;
            toolStripStatusLabel1.Text = "Right Click to add Fields...";

            titleLbl.Text = idcard.title;
            titleLbl.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(titleLbl);

            if (pictureBox1 == null)
            {
                pictureBox1 = new PictureBox();
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox1.SetBounds(10, 10, 90, 90);
                pictureBox1.BackgroundImage = global::IDCardManagement.Properties.Resources.avatar;
                panel1.Controls.Add(pictureBox1);

            }
            ControlMover.Init(pictureBox1);

            panel1.Visible = true;
            panel1.Size = new Size(idcard.dimensions.Width * 10, idcard.dimensions.Height * 10);
            panel1.Left = ((Width - panel1.Width) / 2) - 20;
            panel1.Top = (Height - panel1.Height) / 2;

            rectangleShape1.Visible = true;
            rectangleShape1.SetBounds(panel1.Left + 5, panel1.Top + 5, panel1.Width, panel1.Height);

            contextMenuStrip1.Items.Clear();
            foreach (String str in idcard.selectedFields)
            {
                ToolStripItem tmp = contextMenuStrip1.Items.Add(str);
                tmp.Click += tmpToolStripItem_Click;
            }

        }


        //open
        private void openToolStripButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                this.Text = filename + " - IDCard Designer";

                ArrayList fields = new ArrayList();
                ArrayList selectedFields = new ArrayList();

                Image backgroundImage = null;
                string connectionString = "", tableName = "", title = "", dataSourceType = "", primaryKey = "";

                Size dimensions = new Size();

                panel1.Controls.Clear();
                panel1.ContextMenuStrip = contextMenuStrip1;
                try
                {
                    using (XmlTextReader reader = new XmlTextReader(filename))
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                #region
                                switch (reader.Name)
                                {
                                    case "label":
                                        Label tmp = new Label();
                                        tmp.Text = reader.GetAttribute("text");
                                        tmp.Top = Convert.ToInt32(reader.GetAttribute("top"));
                                        tmp.Left = Convert.ToInt32(reader.GetAttribute("left"));
                                        panel1.Controls.Add(tmp);
                                        tmp.MouseDown += tmplbl_MouseDown;
                                        ControlMover.Init(tmp);
                                        tmp.AutoSize = true;
                                        tmp.Font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(reader.GetAttribute("font"));
                                        tmp.BackColor = Color.FromArgb(Convert.ToInt32(reader.GetAttribute("backcolor")));
                                        tmp.ForeColor = Color.FromArgb(Convert.ToInt32(reader.GetAttribute("forecolor")));
                                        break;
                                    case "pictureBox":

                                        pictureBox1 = new PictureBox();
                                        panel1.Controls.Add(pictureBox1);
                                        pictureBox1.BackgroundImage = global::IDCardManagement.Properties.Resources.avatar;
                                        pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                                        pictureBox1.Left = Convert.ToInt32(reader.GetAttribute("left"));
                                        pictureBox1.Top = Convert.ToInt32(reader.GetAttribute("top"));
                                        pictureBox1.Height = Convert.ToInt32(reader.GetAttribute("height"));
                                        pictureBox1.Width = Convert.ToInt32(reader.GetAttribute("width"));
                                        break;

                                    case "idCard":
                                        dimensions.Height = Convert.ToInt32(reader.GetAttribute("height"));
                                        dimensions.Width = Convert.ToInt32(reader.GetAttribute("width"));
                                        String base64String;
                                        if ((base64String = reader.GetAttribute("backgroundImage")) != null)
                                        {
                                            // Console.WriteLine(base64String);
                                            byte[] imageBytes = Convert.FromBase64String(base64String);
                                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                            // Convert byte[] to Image
                                            ms.Write(imageBytes, 0, imageBytes.Length);
                                            backgroundImage = Image.FromStream(ms, true);

                                        }
                                        title = reader.GetAttribute("title");
                                        tableName = reader.GetAttribute("tableName");
                                        connectionString = reader.GetAttribute("connnectionString");
                                        dataSourceType = reader.GetAttribute("dataSourceType");
                                        primaryKey = reader.GetAttribute("primaryKey");
                                        break;
                                    case "field":
                                        fields.Add(reader.ReadString());
                                        break;
                                    case "selectedField":
                                        selectedFields.Add(reader.ReadString());
                                        break;

                                }

                                #endregion
                            }
                        }
                    idcard = new IDCard(connectionString, dataSourceType, tableName, primaryKey, dimensions, backgroundImage, fields, selectedFields, title);
                    Form2_LoadFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }



        private void tmplbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; }
                    if (ctl is PictureBox) { ((PictureBox)ctl).BorderStyle = BorderStyle.None; }
                }

            Label tmp = sender as Label;
            tmp.BorderStyle = BorderStyle.FixedSingle;
            fontToolStripComboBox.Text = tmp.Font.FontFamily.Name;
            fontSizeToolStripComboBox.Text = ((int)tmp.Font.Size).ToString();
            toolStripButton1.BackColor = tmp.ForeColor;
            toolStripButton2.BackColor = tmp.BackColor;


        }
        private void tmpToolStripItem_Click(object sender, EventArgs e)
        {

            ToolStripItem clickedItem = (ToolStripItem)sender;
            Label tmp = new Label();
            tmp.BackColor = Color.Transparent;
            tmp.Text = clickedItem.Text;
            tmp.Left = X;
            tmp.Top = Y;
            tmp.AutoSize = true;
            tmp.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(tmp);
            panel1.Controls.Add(tmp);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            X = Cursor.Position.X - panel1.Left;
            Y = Cursor.Position.Y - panel1.Top - 30;
            if (addTextMode == 1)
            {
                this.Cursor = Cursors.Arrow;
                TextBox tmpTxt = new TextBox();
                tmpTxt.SetBounds(X, Y, 100, 50);
                tmpTxt.BorderStyle = BorderStyle.FixedSingle;
                //tmpTxt.BackColor = Color.Transparent;
                panel1.Controls.Add(tmpTxt);
                tmpTxt.KeyUp += tmpTxt_KeyUp;
                tmpTxt.Leave += tmpTxt_Leave;
                panel1.Controls.Add(tmpTxt);
                addTextMode = 0;
                tmpTxt.Select();

            }
            //if (e.Button == MouseButtons.Right)
            {

            }

        }

        void tmpTxt_Leave(object sender, EventArgs e)
        {
            panel1.Controls.Remove(sender as TextBox);
            (sender as TextBox).Dispose();
        }

        void tmpTxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Label tmp = new Label();
                tmp.BackColor = Color.Transparent;
                tmp.AutoSize = true;
                tmp.MouseDown += tmplbl_MouseDown;
                ControlMover.Init(tmp);
                panel1.Controls.Add(tmp);
                tmp.Left = (sender as TextBox).Left;
                tmp.Top = (sender as TextBox).Top;
                tmp.Text = (sender as TextBox).Text;
                panel1.Controls.Remove(sender as TextBox);
                (sender as TextBox).Dispose();
            }
            if (e.KeyCode == Keys.Escape)
            {
                panel1.Controls.Remove(sender as TextBox);
                (sender as TextBox).Dispose();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void Form2_Click(object sender, EventArgs e)
        {
            if (panel1 != null)
                foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void fontToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
                if (ctl is Label && ((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                    ((Label)ctl).Font = new Font(fontToolStripComboBox.Text, ctl.Font.Size);
        }

        private void fontSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
                if (ctl is Label && ((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                    ((Label)ctl).Font = new Font(ctl.Font.FontFamily.ToString(), Convert.ToInt32(fontSizeToolStripComboBox.Text));


        }

        private void backcolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                    if (ctl is Label && ((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        ((Label)ctl).BackColor = colorDialog1.Color;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            foreach (Control ctl in panel1.Controls)
                if (ctl is Label)
                    ((Label)ctl).BorderStyle = BorderStyle.None;
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
                if (ctl is Label && ((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                    panel1.Controls.Remove(ctl);
        }

        private void forecolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                    if (ctl is Label && ((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        ((Label)ctl).ForeColor = colorDialog1.Color;
        }

        //save
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (filename == null)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    filename = saveFileDialog1.FileName;
            }
            if (filename != null)
                using (XmlWriter wrt = XmlWriter.Create(filename))
                {
                    #region
                    this.Text = filename + " - IDCard Designer";
                    wrt.WriteStartDocument();
                    wrt.WriteStartElement("panel");
                    foreach (Control ctl in this.panel1.Controls)
                    {
                        if (ctl is Label)
                        {
                            wrt.WriteStartElement("label");
                            wrt.WriteAttributeString("text", ((Label)ctl).Text);
                            wrt.WriteAttributeString("top", ((Label)ctl).Top.ToString());
                            wrt.WriteAttributeString("left", ((Label)ctl).Left.ToString());
                            wrt.WriteAttributeString("backcolor", ((Label)ctl).BackColor.ToArgb().ToString()); //Color.FromArgb(int);
                            wrt.WriteAttributeString("forecolor", ((Label)ctl).ForeColor.ToArgb().ToString());
                            wrt.WriteAttributeString("font", TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(((Label)ctl).Font)); //Font font = (Font)converter.ConvertFromString(fontString);
                            wrt.WriteEndElement();

                        }
                        if (ctl is PictureBox)
                        {
                            wrt.WriteStartElement("pictureBox");
                            wrt.WriteAttributeString("left", ((PictureBox)ctl).Left.ToString());
                            wrt.WriteAttributeString("top", ((PictureBox)ctl).Top.ToString());
                            wrt.WriteAttributeString("height", ((PictureBox)ctl).Height.ToString());
                            wrt.WriteAttributeString("width", ((PictureBox)ctl).Width.ToString());
                            wrt.WriteEndElement();
                        }

                    }

                    wrt.WriteStartElement("idCard");
                    wrt.WriteAttributeString("dataSourceType", idcard.dataSourceType);
                    wrt.WriteAttributeString("connectionString", idcard.connectionString);
                    wrt.WriteAttributeString("primaryKey", idcard.primaryKey);
                    wrt.WriteAttributeString("height", idcard.dimensions.Height.ToString());
                    wrt.WriteAttributeString("width", idcard.dimensions.Width.ToString());
                    wrt.WriteAttributeString("tableName", idcard.tableName);
                    wrt.WriteAttributeString("title", titleLbl.Text);
                    if (idcard.backgroundImage != null)
                    {
                        string imagebase64String;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            idcard.backgroundImage.Save(ms, idcard.backgroundImage.RawFormat);
                            byte[] imageBytes = ms.ToArray();
                            imagebase64String = Convert.ToBase64String(imageBytes);
                        }
                        wrt.WriteAttributeString("backgroundImage", imagebase64String);
                    }
                    foreach (string str in idcard.fields)
                    {

                        wrt.WriteElementString("field", str);
                        wrt.WriteElementString("field", str);

                    }
                    foreach (string str in idcard.selectedFields)
                    {
                        wrt.WriteElementString("selectedField", str);
                    }
                    wrt.WriteEndElement();//idcard
                    wrt.WriteEndElement();//panel
                    wrt.WriteEndDocument();
                    #endregion
                }
        }


        //new
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1(this);
            frm.Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).Font = fontDialog1.Font;
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        int gridStatus;

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            if (gridStatus == 0)
            {


                Pen p = new Pen(Color.Gray);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                int numOfCells = 50, cellSize = 30;
                for (int i = 0; i < numOfCells; i++)
                {
                    // Vertical
                    g.DrawLine(p, i * cellSize, 0, i * cellSize, numOfCells * cellSize);
                    // Horizontal
                    g.DrawLine(p, 0, i * cellSize, numOfCells * cellSize, i * cellSize);
                }
                gridStatus = 1;
            }
            else
            {
                gridStatus = 0;
                //  panel1 = new Panel(); Form2_LoadFile(null, null);//
                //g.Clear(Color.Transparent);
                //panel1.BackgroundImage = idcard.backgroundImage;
                panel1.Invalidate();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            if (gridStatus == 1)
            {
                Graphics g = e.Graphics;
                Pen p = new Pen(Color.Gray);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                int numOfCells = 50, cellSize = 30;
                for (int i = 0; i < numOfCells; i++)
                {
                    // Vertical
                    g.DrawLine(p, i * cellSize, 0, i * cellSize, numOfCells * cellSize);
                    // Horizontal
                    g.DrawLine(p, 0, i * cellSize, numOfCells * cellSize, i * cellSize);
                }
            }
        }

        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                deleteToolStripButton_Click(null, null);
        }

        int addTextMode = 0;
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            addTextMode = 1;
            this.Cursor = Cursors.IBeam;

        }


    }
}
