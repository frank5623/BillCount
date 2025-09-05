using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace BillCount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            update();
        }
        public void tryinput() 
        {
            string filePath = Path.Combine(Application.StartupPath, "Data", "records.txt");
            
            textBox3.Text = "";

            string date = dateTimePicker1.Text; //日期
            ///////////////////////////////////////////
            string status = "";
            
            if (comboBox1.SelectedItem != null)
            {
                status = comboBox1.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("請先選擇支出或輸入！");
                return;
            }
            ///////////////////////////////////////////
            string classify = "";

            if (comboBox2.SelectedItem != null)
            {
                classify = comboBox2.SelectedItem.ToString();
            }
            else
            {
                MessageBox.Show("請先選擇一個分類！");
                return;
            }   
            ///////////////////////////////////////////
            int money = 0;

            if (textBox1.Text != "")
            {
                money = Convert.ToInt32(textBox1.Text);
            }
            else
            {
                MessageBox.Show("請先輸入金額！");
                return;
            }   
            ///////////////////////////////////////////
            string notes = textBox2.Text;

            
            textBox3.Text += date + "\t" + status + "\t" + classify + "\t" + money + "\t" + notes + "\t";
        }

        public void tryinsert(TextBox tt)
        {
            try
            {
                string folderPath = Path.Combine(Application.StartupPath, "Data"); // 或直接 "Data"
                string filePath = Path.Combine(folderPath, "records.txt");

                // 確保資料夾存在
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string newContent = tt.Text;
                if(string.IsNullOrEmpty(newContent))
                {
                    return;
                }
                else
                {
                    File.AppendAllText(filePath, newContent + Environment.NewLine);
                    MessageBox.Show("新增成功!!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            tryinput();
            
            tryinsert(textBox3);

            update();
        }

        //限制可選項目 我想的
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear(); // ✅ 先清空上一次的選項
            if (comboBox1.SelectedIndex == 0) //chatgpt 列出這些項目
            {
                comboBox2.Items.Add("餐飲");       // 早餐、午餐、晚餐、飲料、零食
                comboBox2.Items.Add("交通");       // 油錢、大眾運輸、計程車、高鐵
                comboBox2.Items.Add("娛樂");       // 電影、KTV、遊戲、旅遊
                comboBox2.Items.Add("購物");       // 衣服、日用品、電器、網購
                comboBox2.Items.Add("居住");       // 房租、房貸、水電瓦斯、網路
                comboBox2.Items.Add("醫療保健");   // 看病、藥品、健身房、保險
                comboBox2.Items.Add("教育學習");   // 學費、補習、課程、書籍
                comboBox2.Items.Add("人情交際");   // 紅包、聚餐、送禮
                comboBox2.Items.Add("其他");       // 不方便分類的支出
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                comboBox2.Items.Add("薪資");
                comboBox2.Items.Add("獎金");
                comboBox2.Items.Add("投資收益");   // 股息、利息、基金回報
                comboBox2.Items.Add("兼職/外快");
                comboBox2.Items.Add("禮金/紅包");
                comboBox2.Items.Add("其他收入");
            }
            comboBox2.SelectedIndex = 0;
        }

        public void update()
        {
            string filePath = Path.Combine(Application.StartupPath, "Data", "records.txt");

            if (File.Exists(filePath))
            {
                // 清空 DataGridView
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();

                // 讀取所有行
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length > 0)
                {
                    // 第一行當標題
                    string[] headers = lines[0].Split('\t'); // 用 Tab 分隔
                    foreach (string header in headers)
                    {
                        dataGridView1.Columns.Add(header, header);
                    }

                    // 其餘行當資料
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] cells = lines[i].Split('\t');
                        dataGridView1.Rows.Add(cells);
                    }
                }
            }
            else
            {
                MessageBox.Show("找不到記事本檔案！");
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            update();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Application.StartupPath, "Data"); // 或直接 "Data"
            string filePath = Path.Combine(folderPath, "records.txt");

            // 確保資料夾存在
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string content = "日期	支出/收入	分類	金額	備註\r\n";

            
            // 單存寫入 (覆蓋原本內容)
            File.WriteAllText(filePath, content);

            update();
        }
    }
}
