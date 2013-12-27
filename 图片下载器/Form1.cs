using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace 图片下载器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(txtDesUrl.Text, @"\w{1}:(\\?\S*)+"))
            {
                MessageBox.Show("保存路径设置错误，请重新设置！");
                return;
            }
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;//设置抓取源代码为utf-8编码
            string content = "";
            try
            {
                content = wc.DownloadString(txtSourceUrl.Text);
            }
            catch (Exception)
            {
                //btnDownLoad_Click(null, null);
                MessageBox.Show("网络连接错误，请重新下载！");
                return;
            }
            //百度图片正则表达式
            //string pattern = @"thumbUR[\s\S]+?objURL[^\s]+?(?<url>[a-zA-Z]+://[^\s]*?.jpg)";
            //百度空间图片正则表达式
            //string pattern = @"<p>(?<name>\w+)&shy;</p><p>&shy;</p><p><img\salt=""图片""\s+src=""(?<url>\w+://[\w.?/?]+.jpg)""";
            string pattern = txtRegex.Text;
            MatchCollection matchs = Regex.Matches(content, pattern);
            if (DownLoad(matchs, txtDesUrl.Text))
            {
                MessageBox.Show("下载完成！");
            }
            else
            {
                MessageBox.Show("下载失败！");
            }
        }

        private static bool DownLoad(MatchCollection matchs,string path)
        {
            WebClient wc = new WebClient();
            int count = 0;
            foreach (Match match in matchs)
            {
                string strUrl = match.Groups["url"].Value;
                string name = string.Format("{0}\\{1}.jpg", path, match.Groups["name"].Value);
                try
                {
                    wc.DownloadFile(strUrl, name);
                }
                catch (Exception)
                {
                    continue;
                }
                count++;
            }
            wc.Dispose();
            return true;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            txtDesUrl.Text = fbd.SelectedPath;
        }

    }
}
