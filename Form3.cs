using System;
using System.Windows.Forms;

namespace LIU_Batch_Compression
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            this.Text = "使用说明";
            this.Width = 500;
            this.Height = 400;

            TextBox textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new System.Drawing.Font("Microsoft YaHei", 10),
                Text = @"【使用说明】
                E:\SOURCE\BAT\TOOL
                E:\SOURCE\BAT\PACKAGE\SRC

                1. 工具路径：
                   选择包含多个子目录的根文件夹，每个子目录中应包含“PCsetup”。

                2. 目标目录（src）：
                   指向 ch.bat 所在目录，PCsetup 会被复制到这里。

                3. ch.bat：
                   应存在于目标目录中，将被调用执行打包命令。

                4. 点击“开始”按钮后：
                   - 程序会遍历每个子文件夹
                   - 拷贝 PCsetup 到目标
                   - 调用 ch.bat 并传入子文件夹名作为参数
                   - 显示进度和错误信息

                5. 执行完成后会自动打开 rels 目录。
"
            };

            this.Controls.Add(textBox);
        }
    }
}
