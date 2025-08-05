using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIU_Batch_Compression
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "版本：v1.0.0"; // ← 改成你当前的版本号
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();           // 创建 Form4 实例
            form4.FormClosed += (s, args) => this.Close(); // 当 Form4 关闭时退出程序（可选）
            form4.Show();                        // 显示 Form4
            this.Hide();                         // 隐藏当前 Form1，实现“跳转”效果
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.FormClosed += (s, args) => this.Close(); // 确保关闭 Form2 时也退出程序
            form2.Show();
            this.Hide(); // 只是隐藏，不销毁
        }

    }
}