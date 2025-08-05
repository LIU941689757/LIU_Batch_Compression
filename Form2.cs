using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LIU_Batch_Compression
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 使用模态窗口显示说明
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 helpWindow = new Form3();
            helpWindow.ShowDialog(); // 使用模态窗口显示说明
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();           // 创建 Form4 实例
            form4.FormClosed += (s, args) => this.Close(); // 当 Form4 关闭时退出程序（可选）
            form4.Show();                        // 显示 Form4
            this.Hide();                         // 隐藏当前 Form1，实现“跳转”效果
        }

        /// <summary>
        /// 按钮点击事件：开始批量拷贝PCsetup并调用ch.bat打包
        /// </summary>
        private async void button3_Click(object sender, EventArgs e)
        {
            // 获取用户输入的路径
            string toolPath = textBox1.Text.Trim();    // 待压缩文件夹根目录
            string srcPath = textBox2.Text.Trim();     // 目标src目录
            string chBatPath = Path.Combine(Path.GetDirectoryName(srcPath) ?? "", "ch.bat"); // ch.bat脚本完整路径

            // 检查各路径和批处理脚本是否存在
            if (!Directory.Exists(toolPath) || !Directory.Exists(srcPath) || !File.Exists(chBatPath))
            {
                MessageBox.Show("路径或ch.bat不存在");
                return;
            }

            button3.Enabled = false;           // 禁用按钮防止重复点击
            progressBar1.Value = 0;            // 进度条重置
            var folders = Directory.GetDirectories(toolPath); // 获取所有待处理子文件夹
            progressBar1.Maximum = folders.Length > 0 ? folders.Length : 1; // 设置进度条最大值
            List<string> failedList = new List<string>(); // 记录失败的文件夹

            // 开启后台任务处理所有子文件夹
            await Task.Run(() => BatchProcessAllFolders(folders, srcPath, chBatPath, failedList));

            button3.Enabled = true;                    // 恢复按钮
            progressBar1.Value = progressBar1.Maximum; // 进度条满格
            MessageBox.Show("全部处理完成");           // 弹窗提示

            // 如果有失败的文件夹，统一弹窗显示
            if (failedList.Count > 0)
            {
                string msg = "以下文件夹处理失败：\n" + string.Join("\n", failedList);
                MessageBox.Show(msg);
            }

            // 打开src目录同级的rels文件夹
            string relsPath = Path.Combine(Path.GetDirectoryName(srcPath) ?? "", "rels");
            Process.Start("explorer.exe", relsPath);
        }

        /// <summary>
        /// 批量处理所有子文件夹，按顺序逐一处理
        /// </summary>
        /// <param name="folders">待处理文件夹路径数组</param>
        /// <param name="srcPath">目标src目录</param>
        /// <param name="chBatPath">ch.bat脚本路径</param>
        /// <param name="failedList">记录处理失败的目录</param>
        private void BatchProcessAllFolders(string[] folders, string srcPath, string chBatPath, List<string> failedList)
        {
            for (int i = 0; i < folders.Length; i++)
            {
                // 顺序处理每一个子目录
                bool ok = ProcessSingleFolder(folders[i], srcPath, chBatPath);

                // 进度条增加，Invoke保证线程安全
                this.Invoke(new Action(() => progressBar1.Value = i + 1));

                // 如果失败则记录
                if (!ok) failedList.Add(folders[i]);
            }
        }

        /// <summary>
        /// 处理单个子文件夹：拷贝PCsetup并调用批处理打包
        /// </summary>
        /// <param name="folder">当前待处理子目录</param>
        /// <param name="srcPath">目标src目录</param>
        /// <param name="chBatPath">ch.bat脚本路径</param>
        /// <returns>处理成功返回true，失败返回false</returns>
        private bool ProcessSingleFolder(string folder, string srcPath, string chBatPath)
        {
            var pcsetupSource = Path.Combine(folder, "PCsetup"); // 源PCsetup目录
            var pcsetupDest = Path.Combine(srcPath, "PCsetup");  // 目标PCsetup目录

            // 如果没有PCsetup目录，直接跳过
            if (!Directory.Exists(pcsetupSource)) return true;

            try
            {
                // 删除目标PCsetup（如果已存在）
                if (Directory.Exists(pcsetupDest))
                    Directory.Delete(pcsetupDest, true);

                // 递归复制PCsetup目录
                CopyDirectory(pcsetupSource, pcsetupDest);

                // 调用ch.bat批处理进行打包，参数为当前子文件夹名
                int exitCode = RunChBat(chBatPath, Path.GetFileName(folder));
                if (exitCode != 0)
                {
                    // 批处理失败，弹窗提示
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"调用批处理失败，目录：{folder}，退出码：{exitCode}");
                    }));
                    return false;
                }
                return true; // 成功
            }
            catch (Exception ex)
            {
                // 出现异常，弹窗提示
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"处理目录失败: {folder}\n{ex.Message}");
                }));
                return false;
            }
        }

        /// <summary>
        /// 递归复制目录及其所有内容
        /// </summary>
        /// <param name="source">源目录路径</param>
        /// <param name="target">目标目录路径</param>
        private void CopyDirectory(string source, string target)
        {
            Directory.CreateDirectory(target); // 创建目标目录
            // 复制所有文件
            foreach (var file in Directory.GetFiles(source))
                File.Copy(file, Path.Combine(target, Path.GetFileName(file)), true);
            // 递归复制所有子目录
            foreach (var dir in Directory.GetDirectories(source))
                CopyDirectory(dir, Path.Combine(target, Path.GetFileName(dir)));
        }

        /// <summary>
        /// 执行ch.bat批处理脚本
        /// </summary>
        /// <param name="chBatPath">ch.bat脚本完整路径</param>
        /// <param name="param">参数（当前子目录名）</param>
        /// <returns>脚本的退出码（0为成功）</returns>
        private int RunChBat(string chBatPath, string param)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", $"/c \"\"{chBatPath}\" \"{param}\"\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process proc = Process.Start(psi);
            proc.WaitForExit();
            int exitCode = proc.ExitCode;
            proc.Dispose();
            return exitCode;
        }
    }
}
