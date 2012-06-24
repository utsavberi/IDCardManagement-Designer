using System;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using System.Data.SqlClient;
using System.Collections;
using System.Data.SqlServerCe;

namespace IDCardManagement
{
    public partial class Form1 : Form
    {
        String connectionString;
        String tableName;
        System.Drawing.Size dimensions;
        System.Drawing.Image backgroundImage;
        ArrayList fields;
        ArrayList selectedFields;
        String title;
        object senderform;
        public Form1(object sender)
        {
            this.senderform = sender;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files (*.jpg;*.jpeg;*.bmp;*.gif;*.gif)|*.jpg;*.jpeg;*.bmp;*.gif;*.gif|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                backgroundImage = new System.Drawing.Bitmap(openFileDialog1.FileName);
                textBox1.Text = openFileDialog1.FileName;
                pictureBox1.BackgroundImage = backgroundImage;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);
            dcs.LoadConfiguration(dcd);

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {

                textBox2.Text = dcd.ConnectionString;
                connectionString = dcd.ConnectionString;
                comboBox1.Enabled = true;
                using (SqlCeConnection con = new SqlCeConnection(connectionString))
                {
                    comboBox1.Items.Clear();
                    con.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT table_name FROM INFORMATION_SCHEMA.Tables", con))
                    {
                        SqlCeDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));

                        }
                    }
                }
                //textBox1.Text = dcd.SelectedDataSource.DisplayName;
            }
            dcs.SaveConfiguration(dcd);
        }


        private void test_Click(object sender, EventArgs e)
        {
            pictureBox1.Height = (int)numericUpDown1.Value * 3;
            pictureBox1.Width = (int)numericUpDown2.Value * 3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            title = textBox3.Text;
            dimensions = new System.Drawing.Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            updateNavButtons();

            tabControl1.Top = -21;
            pictureBox1.Width = (int)numericUpDown1.Value * 3;
            pictureBox1.Height = (int)numericUpDown2.Value * 3;
            pictureBox1.Left = (groupBox1.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (groupBox1.Height - pictureBox1.Height) / 2;

            label9.Top = pictureBox1.Top + 5;
            label9.Left = pictureBox1.Left + (pictureBox1.Width - label9.Width) / 2;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dimensions.Width = (int)numericUpDown1.Value;
            pictureBox1.Width = (int)numericUpDown1.Value * 3;
            pictureBox1.Left = (groupBox1.Width - pictureBox1.Width) / 2;

            label9.Left = pictureBox1.Left + (pictureBox1.Width - label9.Width) / 2;

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            dimensions.Height = (int)numericUpDown2.Value;
            pictureBox1.Height = (int)numericUpDown2.Value * 3;
            pictureBox1.Top = (groupBox1.Height - pictureBox1.Height) / 2;

            label9.Top = pictureBox1.Top + 5;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = (groupBox1.Width - pictureBox1.Width) / 2;
            pictureBox1.Top = (groupBox1.Height - pictureBox1.Height) / 2;

            label9.Top = pictureBox1.Top + 5;
            label9.Left = pictureBox1.Left + (pictureBox1.Width - label9.Width) / 2;
        }

        //next/finish button
        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex + 1 < tabControl1.TabCount)
            {
                tabControl1.SelectTab(tabControl1.SelectedIndex + 1);
                updateNavButtons();
            }
            else
            {
                (this.senderform as Form2).Hide();
                selectedFields = new ArrayList();
                foreach (string str in listBox2.Items) selectedFields.Add(str);
                IDCard idcard = new IDCard(connectionString, tableName, dimensions, backgroundImage, fields, selectedFields, title);
                Form2 frm = new Form2(idcard);
                frm.Show();
                this.Close();
            }


        }

        private void updateNavButtons()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    button3.Enabled = false;
                    button4.Text = "Next";
                    break;
                case 1:
                    button3.Enabled = true;
                    button4.Text = "Finish";
                    // this.Height = 292;
                    //this.Width = 462;
                    break;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex - 1 >= 0)
                tabControl1.SelectTab(tabControl1.SelectedIndex - 1);
            updateNavButtons();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            (this.senderform as Form2).Show();
            this.Close();
            //if (MessageBox.Show("Really Quit?", "Confirm Exit", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            //    this.Close();
            //}
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = (comboBox1.SelectedItem.ToString());
            button4.Enabled = true;
            fields = new ArrayList();
            using (SqlCeConnection con = new SqlCeConnection(connectionString))
            {
                con.Open();
                using (SqlCeCommand command = new SqlCeCommand("SELECT column_name FROM information_schema.columns WHERE (table_name = '" + tableName + "')", con))
                {

                    SqlCeDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        fields.Add(reader.GetString(0));

                    }
                }
            }
            listBox1.DataSource = fields;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox2.Items.Remove(listBox2.SelectedItem);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.AddRange(listBox1.Items);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label9.Text = textBox3.Text;
            title = label9.Text;
            if (label9.Width < pictureBox1.Width)
            {
                label9.Left = pictureBox1.Left + (pictureBox1.Width - label9.Width) / 2;

            }
            else { label9.AutoSize = false; label9.Width = pictureBox1.Width - 2; }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            title = textBox3.Text;
        }

    }
}

