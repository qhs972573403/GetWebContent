using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
namespace GetWebContent
{
    public partial class Frm_Main : Form
    {
        private WebDownloader m_wd = new WebDownloader();
        public Frm_Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strContent 
                = m_wd.GetPageByHttpWebRequest(this.textBoxUrl.Text, Encoding.UTF8);



            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument
            {
                OptionAddDebuggingAttributes = false,
                OptionAutoCloseOnEnd = true,
                OptionFixNestedTags = true,
                OptionReadEncoding = true
            };

            htmlDoc.LoadHtml(strContent);
            string strTitle = "";
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//title");
            // Extract Title
            if (!Equals(nodes, null))
            {
                strTitle = string.Join(";", nodes.
                    Select(n => n.InnerText).
                    ToArray()).Trim();
            }
            strTitle = strTitle.Replace("博客园", "");
            strTitle = Regex.Replace(strTitle, @"[|/\;:*?<>&#-]", "").ToString();
            strTitle = Regex.Replace(strTitle, "[\"]", "").ToString();
            this.textBoxTitle.Text = strTitle.TrimEnd();
                    

            IEnumerable<HtmlNode> NodesMainContent = htmlDoc.DocumentNode.QuerySelectorAll(this.textBoxCssPath.Text);

            if (NodesMainContent.Count() > 0)
            {
                this.richTextBox1.Text = NodesMainContent.ToArray()[0].OuterHtml;
                this.webBrowser1.DocumentText = this.richTextBox1.Text;
            }

        }

        /*
  站点		--->  CSS路径
"Cnblogs"	---> "div#cnblogs_post_body"
"Csdn"		---> "div#article_content.article_content"
"51CTO"		---> "div.showContent"
"Iteye"		---> "div#blog_content.blog_content"
"ItPub"		---> "div.Blog_wz1"
"ChinaUnix" ---> "div.Blog_wz1"
          */
      

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            this.textBoxUrl.Text = "http://www.cnblogs.com/ice-river/p/4110799.html";
            this.textBoxCssPath.Text = "div#cnblogs_post_body";
        }
    }
}
