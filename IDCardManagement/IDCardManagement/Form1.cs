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
        String dataSourceType;
        String connectionString;
        String tableName;
        System.Drawing.Size dimensions;
        System.Drawing.Image backgroundImage;
        ArrayList fields;
        ArrayList selectedFields;
        String title;
        object senderform;
        string primaryKey;
        public Form1(object sender)
        {
            this.senderform = sender;
            InitializeComponent();
        }

        private void chooseFileBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image files (*.jpg;*.jpeg;*.bmp;*.gif;*.gif)|*.jpg;*.jpeg;*.bmp;*.gif;*.gif|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                backgroundImage = new System.Drawing.Bitmap(openFileDialog1.FileName);
                backgroundImageTxt.Text = openFileDialog1.FileName;
                pictureBox1.BackgroundImage = backgroundImage;
            }
        }

        private void chooseDatabaseBtn_Click(object sender, EventArgs e)
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataConnectionConfiguration dcs = new DataConnectionConfiguration(null);
            dcs.LoadConfiguration(dcd);

            if (DataConnectionDialog.Show(dcd) == DialogResult.OK)
            {

                databaseTxt.Text = dcd.ConnectionString;
                connectionString = dcd.ConnectionString;
                dataSourceType = dcd.SelectedDataSource.DisplayName;
                Console.WriteLine("here it is :" + dataSourceType);
                switch (dataSourceType)
                {
                    case "Microsoft SQL Server Compact 3.5":
                        try
                        {
                            using (SqlCeConnection con = new SqlCeConnection(connectionString))
                            {
                                tableTxt.Items.Clear();
                                con.Open();
                                using (SqlCeCommand command = new SqlCeCommand("SELECT table_name FROM INFORMATION_SCHEMA.Tables", con))
                                {
                                    SqlCeDataReader reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        tableTxt.Items.Add(reader.GetString(0));

                                    }
                                }
                            }
                            dcs.SaveConfiguration(dcd);
                            tableTxt.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to establish Connection..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    case "Microsoft SQL Server":
                        try
                        {
                            using (SqlConnection con = new SqlConnection(connectionString))
                            {
                                tableTxt.Items.Clear();
                                con.Open();
                                using (SqlCommand command = new SqlCommand("SELECT table_name FROM INFORMATION_SCHEMA.Tables", con))
                                {
                                    SqlDataReader reader = command.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        tableTxt.Items.Add(reader.GetString(0));

                                    }
                                }
                            }
                            dcs.SaveConfiguration(dcd);
                            tableTxt.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to establish Connection..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        break;
                    default:
                        MessageBox.Show("Not Implemented Yet..!!");
                        break;
                }
            }

        }




        private void Form1_Load(object sender, EventArgs e)
        {
            title = titleTxt.Text;
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
        private void nextFinishBtn_Click(object sender, EventArgs e)
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
                IDCard idcard = new IDCard(connectionString, dataSourceType, tableName, primaryKey, dimensions, backgroundImage, fields, selectedFields, title);
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

        private void backBtn_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex - 1 >= 0)
                tabControl1.SelectTab(tabControl1.SelectedIndex - 1);
            updateNavButtons();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            (this.senderform as Form2).Show();
            this.Close();

        }

        private void tableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            tableName = (tableTxt.SelectedItem.ToString());
            button4.Enabled = true;
            fields = new ArrayList();
            switch (dataSourceType)
            {
                case "Microsoft SQL Server Compact 3.5":
                    try
                    {
                        using (SqlCeConnection con = new SqlCeConnection(connectionString))
                        {
                            con.Open();
                            using (SqlCeCommand command = new SqlCeCommand("SELECT column_name FROM information_schema.columns WHERE (table_name = '" + tableName + "')", con))
                            {

                                SqlCeDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    fields.Add(reader.GetString(0));
                                    primaryKeyTxt.Items.Add(reader.GetString(0));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to establish Connection..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "Microsoft SQL Server":
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {

                            con.Open();
                            using (SqlCommand command = new SqlCommand("SELECT column_name FROM information_schema.columns WHERE (table_name = '" + tableName + "')", con))
                            {
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    fields.Add(reader.GetString(0));
                                    primaryKeyTxt.Items.Add(reader.GetString(0));

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to establish Connection..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                default:
                    MessageBox.Show("Not Implemented Yet..!!");
                    break;
            }

            listBox1.Items.AddRange(fields.ToArray());
            primaryKeyTxt.Enabled = true;

        }
        private void primaryKeyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            primaryKey = primaryKeyTxt.Text;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Add(listBox1.SelectedItem);
                listBox1.Items.Remove(listBox1.SelectedItem);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox2.Items.AddRange(listBox1.Items);
            listBox1.Items.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(listBox2.Items);
            listBox2.Items.Clear();


        }

        private void titleTxt_TextChanged(object sender, EventArgs e)
        {
            label9.Text = titleTxt.Text;
            title = label9.Text;
            if (label9.Width < pictureBox1.Width)
                label9.Left = pictureBox1.Left + (pictureBox1.Width - label9.Width) / 2;
            else label9.AutoSize = false; label9.Width = pictureBox1.Width - 2;
        }

        private void titleTxt_Leave(object sender, EventArgs e)
        {
            title = titleTxt.Text;
        }

    }
}

