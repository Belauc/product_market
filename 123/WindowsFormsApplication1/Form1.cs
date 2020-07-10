using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string UsersDirectory = Environment.CurrentDirectory;
        public Form1()
        {   
            InitializeComponent();
            parseItems();

        }
        double ski = 1, buy = 1;
        bool isWrite = false;

        private void saveToolStripMenu()
        {
            isWrite = true;
            StringBuilder listViewContent = new StringBuilder();
            

            foreach (ListViewItem item in this.listView1.Items)
            {
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    listViewContent.Append(subItem.Text); 
                    listViewContent.Append('|');
                }
                listViewContent.Append(Environment.NewLine);
            }
            
            using (TextWriter tw = new StreamWriter(UsersDirectory + "\\Tovar.txt"))
            {
                tw.WriteLine(listViewContent.ToString()); 
                tw.Close();
            }

        }
        private void parseItems()
        {
            listView1.Items.Clear(); 
            using (StreamReader sReader = new StreamReader(UsersDirectory + "\\Tovar.txt")) 
            {
                string line;
                while ((line = sReader.ReadLine()) != null)
                {
                    string[] values = line.Split('|'); 
                    listView1.Items.Add(new ListViewItem(values));
                }
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(comboBox1, "Если выбран оптовый покупатель, скида автоматически составляет - 10%, но кол-во покупаемых товаров не может быть меньше 10");
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            ListViewItem ListItem = new ListViewItem(textBox4.Text);
            ListItem.SubItems.Add(textBox5.Text);
            ListItem.SubItems.Add(textBox6.Text);
            ListItem.SubItems.Add(textBox7.Text);
            listView1.Items.Add(ListItem);
            MessageBox.Show("Данные добавлены!", "Электронный магазин");
            saveToolStripMenu();
        }

        private void button_AddCheck_Click(object sender, EventArgs e)
        {
            int a,b;
            double price;
            b = Int32.Parse(textBox2.Text);
            bool info = false;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.SubItems[0].Text == textBox3.Text)
                {
                    a = Convert.ToInt32(item.SubItems[2].Text);
                    if (a > b)
                    {
                        a = a - b;
                        richTextBox1.Text += "Артикул - " + item.SubItems[0].Text + " Товар - " + item.SubItems[1].Text + " Колличество - " + b + " Цена - " + Convert.ToInt32(item.SubItems[3].Text)*b*buy*ski + "\n";
                        price = Convert.ToInt32(textBox1.Text);
                        price += Convert.ToInt32(item.SubItems[3].Text) * b * buy * ski;
                        textBox1.Text = price.ToString();
                        item.SubItems[2].Text = a.ToString();
                    }
                    else if (a == b)
                    {
                        richTextBox1.Text += "Артикул - " + item.SubItems[0].Text + " Товар - " + item.SubItems[1].Text + " Колличество - " + b + " Цена - " + Convert.ToInt32(item.SubItems[3].Text) * b*buy * ski + "\n";
                        price = Convert.ToInt32(textBox1.Text);
                        price += Convert.ToInt32(item.SubItems[3].Text) * b* buy * ski;
                        textBox1.Text = price.ToString();
                        item.Remove();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка! Кол-во товаров на складе меньше требуемого", "Электронный магазин");
                    }
                    info = true;
                }
            }
            if(info == false)
            {
                MessageBox.Show("Ошибка! На складе нет требуемого товара", "Электронный магазин");
            }

        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = "0"; textBox2.Text = ""; textBox3.Text = "";
            comboBox1.Text = ""; comboBox2.Text = "";
            ski = 1; buy = 1;
            parseItems();
        }

        private void button_Buy_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = "";
            comboBox1.Text = ""; comboBox2.Text = "";
            ski = 1; buy = 1;
            saveToolStripMenu();
            MessageBox.Show("Поздравляем, Ваш товар приобретен", "Электронный магазин");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Оптовый покупатель")
               buy = 0.9;
            else
               buy=1;


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string skidka = comboBox2.Text;
            switch (skidka)
            {
                case "5%": ski = 0.95;
                    break;
                case "10%": ski = 0.9;
                    break;
                case "15%": ski = 0.85;
                    break;
                case "20%": ski = 0.8;
                    break;
                default:
                    break;
            }
        }
    }
}
