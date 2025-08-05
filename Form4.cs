using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LIU_Batch_Compression
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.FormClosed += (s, args) => this.Close(); // 确保关闭 Form2 时也退出程序
            form2.Show();
            this.Hide(); // 只是隐藏，不销毁
        }


        /// <summary>
        /// 按钮点击事件：解压指定文件夹下所有 exe 文件
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = textBox1.Text.Trim();
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("文件夹不存在！");
                return;
            }

            // 只取当前目录的 .7z 文件
            string[] archives = Directory.GetFiles(folderPath, "*.7z", SearchOption.TopDirectoryOnly);

            string sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";

            foreach (string archive in archives)
            {
                // 1) 生成同名目录  e.g.  D:\xx\test.7z  ->  D:\xx\test
                string targetDir = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(archive));
                Directory.CreateDirectory(targetDir);

                // 2) 解压到该目录  （-o 指定输出目录）
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = $"x \"{archive}\" -o\"{targetDir}\" -y",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = folderPath            // 可有可无，这里无所谓
                };

                using (Process proc = Process.Start(psi))
                {
                    proc.WaitForExit();
                }
            }

            MessageBox.Show("全部解压完成！");
        }


    }
}
