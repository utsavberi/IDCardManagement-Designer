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
        public int Y { get; set; }
        public int X { get; set; }
        private IDCard idcard;
        PictureBox pictureBox1;
        Panel panel1;
        Label label1;

        public Form2()
        {
            InitializeComponent();

        }

        //called in "new" subroutine
        public Form2(IDCard idcard)
        {
            InitializeComponent();
            this.idcard = idcard;
            Form2_LoadFile(null, null);
        }

        private void Form2_Load(object o, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Click on Open or New to start working...";
            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                fontToolStripComboBox.Items.Add(font.Name);
            }


        }

        private void Form2_LoadFile(object sender, EventArgs e)
        {
            if (idcard.backgroundImage != null) panel1.BackgroundImage = idcard.backgroundImage;
            else { panel1.BackgroundImage = null; }
            toolStripStatusLabel1.Text = "Right Click to add Fields...";
            label1.Text = idcard.title;
            label1.MouseDown += tmplbl_MouseDown;
            ControlMover.Init(label1);
            //ControlMover.Init(panel1);
            if (pictureBox1 != null)
                ControlMover.Init(pictureBox1);
            panel1.Visible = true;

            panel1.Size = new Size(idcard.dimensions.Width * 10, idcard.dimensions.Height * 10);
            panel1.Left = ((Width - panel1.Width) / 2)-20;
            panel1.Top = (Height - panel1.Height) / 2;
            rectangleShape1.Visible = true;
            rectangleShape1.Width = panel1.Width;
            rectangleShape1.Height = panel1.Height;
            rectangleShape1.Left = panel1.Left + 5;
            rectangleShape1.Top = panel1.Top + 5;
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
                this.Text = openFileDialog1.FileName + " - IDCard Designer";
                ArrayList fields = new ArrayList();
                ArrayList selectedFields = new ArrayList();
                Image backgroundImage = null;
                string connectionString = "", tableName = "", title = "";
                Size dimensions = new Size();

                //if (panel1 != null) 
                //panel1 = new Panel();
                panel1.Controls.Clear();
                panel1.ContextMenuStrip = contextMenuStrip1;
                //label1 = new Label();

                using (XmlTextReader reader = new XmlTextReader(openFileDialog1.FileName))
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
                                    //Console.WriteLine("alice blue");
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
                                    break;
                                case "field":
                                    fields.Add(reader.ReadString());
                                    //Console.WriteLine("fields:"+reader.ReadString());
                                    //Console.WriteLine("name: " + " value :" + reader.Value);
                                    break;
                                case "selectedField":
                                    //Console.WriteLine("selectedfields:"+reader.ReadString());
                                    selectedFields.Add(reader.ReadString());
                                    break;

                            }

                            #endregion
                        }
                    }
                idcard = new IDCard(connectionString, tableName, dimensions, backgroundImage, fields, selectedFields, title);

                Form2_LoadFile(null, null);
            }
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

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                X = Cursor.Position.X - panel1.Left;
                Y = Cursor.Position.Y - panel1.Top - 30;
                Console.WriteLine(X + "lkkn :" + Y);
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
            {
                if (ctl is Label) if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle) ((Label)ctl).Font = new Font(fontToolStripComboBox.Text, ctl.Font.Size);
            }
        }

        private void fontSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        ((Label)ctl).Font = new Font(ctl.Font.FontFamily.ToString(), Convert.ToInt32(fontSizeToolStripComboBox.Text));
                }
            }

        }

        private void backcolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).BackColor = colorDialog1.Color;
                    }
                }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            foreach (Control ctl in panel1.Controls) { if (ctl is Label) { ((Label)ctl).BorderStyle = BorderStyle.None; } }

        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in panel1.Controls)
            {
                if (ctl is Label)
                {
                    if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                        panel1.Controls.Remove(ctl);
                    // ctl.Dispose();
                    //((Label)ctl).BackColor = colorDialog1.Color;
                }
            }

        }

        private void forecolorToolStripButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                foreach (Control ctl in panel1.Controls)
                {
                    if (ctl is Label)
                    {
                        if (((Label)ctl).BorderStyle == BorderStyle.FixedSingle)
                            ((Label)ctl).ForeColor = colorDialog1.Color;
                    }
                }

        }

        //save
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                using (XmlWriter wrt = XmlWriter.Create(saveFileDialog1.FileName))
                {
                    this.Text = openFileDialog1.FileName + " - IDCard Designer";
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
                    wrt.WriteAttributeString("connectionString", idcard.connectionString);
                    wrt.WriteAttributeString("height", idcard.dimensions.Height.ToString());
                    wrt.WriteAttributeString("width", idcard.dimensions.Width.ToString());
                    wrt.WriteAttributeString("tableName", idcard.tableName);
                    wrt.WriteAttributeString("title", label1.Text);//idcard.title
                    string imagebase64String;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        idcard.backgroundImage.Save(ms, idcard.backgroundImage.RawFormat);
                        byte[] imageBytes = ms.ToArray();
                        imagebase64String = Convert.ToBase64String(imageBytes);
                    }
                    wrt.WriteAttributeString("backgroundImage", imagebase64String);
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
                }
        }


        //new
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1(this);
            //Hide();
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


    }
}
