using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIU_Batch_Compression
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 按钮点击事件，开始处理压缩任务
        private async void button3_Click(object sender, EventArgs e)
        {
            // 获取用户输入的待压缩文件夹路径和目标src路径
            string toolPath = textBox1.Text.Trim();
            string srcPath = textBox2.Text.Trim();
            // ch.bat 路径，假设跟 src 文件夹同级
            string chBatPath = Path.Combine(Path.GetDirectoryName(srcPath) ?? "", "ch.bat");

            // 路径检查：待压缩文件夹必须存在
            if (!Directory.Exists(toolPath))
            {
                MessageBox.Show("待压缩文件夹路径不存在");
                return;
            }
            // 目标 src 路径必须存在
            if (!Directory.Exists(srcPath))
            {
                MessageBox.Show("目标src路径不存在");
                return;
            }
            // ch.bat 必须存在
            if (!File.Exists(chBatPath))
            {
                MessageBox.Show($"找不到ch.bat，路径：{chBatPath}");
                return;
            }

            // 禁用按钮避免重复点击
            button3.Enabled = false;

            // 异步执行，防止界面卡顿
            await Task.Run(() =>
            {
                // 遍历待压缩文件夹下的所有子文件夹
                var dirs = Directory.GetDirectories(toolPath);
                for (int i = 0; i < dirs.Length; i++)
                {
                    var folder = dirs[i];
                    var folderName = Path.GetFileName(folder);
                    // 每个子文件夹下必须有PCsetup文件夹
                    var pcsetupSource = Path.Combine(folder, "PCsetup");
                    if (!Directory.Exists(pcsetupSource))
                        continue; // 没有PCsetup，跳过

                    // 目标路径：src下的PCsetup
                    var pcsetupDest = Path.Combine(srcPath, "PCsetup");

                    // 删除旧的PCsetup文件夹（如果存在）
                    DeleteIfExists(pcsetupDest);
                    // 复制新的PCsetup文件夹
                    CopyDirectory(pcsetupSource, pcsetupDest);

                    // 调用ch.bat压缩，参数是待压缩文件夹名
                    int exitCode = RunChBat(chBatPath, folderName);
                    if (exitCode != 0)
                        break; // 出错则终止循环
                }
            });

            // 任务完成，恢复按钮可用
            button3.Enabled = true;
            // 弹窗提示完成
            MessageBox.Show("全部处理完成");

            // 打开src路径同级目录下的rels文件夹
            string relsPath = Path.Combine(Path.GetDirectoryName(srcPath) ?? "", "rels");
            Process.Start("explorer.exe", relsPath);
        }

        // 删除文件夹（及其内容）如果存在
        static void DeleteIfExists(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        // 递归复制目录及文件
        static void CopyDirectory(string source, string target)
        {
            Directory.CreateDirectory(target);

            var files = Directory.GetFiles(source);
            for (int i = 0; i < files.Length; i++)
            {
                var destFile = Path.Combine(target, Path.GetFileName(files[i]));
                File.Copy(files[i], destFile, true);
            }

            var dirs = Directory.GetDirectories(source);
            for (int i = 0; i < dirs.Length; i++)
            {
                var destDir = Path.Combine(target, Path.GetFileName(dirs[i]));
                CopyDirectory(dirs[i], destDir);
            }
        }

        // 调用 ch.bat 进行压缩，参数是文件夹名
        static int RunChBat(string chBatPath, string param)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"\"{chBatPath}\" \"{param}\"\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                return proc.ExitCode;
            }
        }
    }
}
