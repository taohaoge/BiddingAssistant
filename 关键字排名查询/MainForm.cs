/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/1/30
 * 时间: 12:46
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using System.Linq;
using Mycollection;
using Microsoft;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
namespace 关键字排名查询
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		int keycount = 0;
		int keycounts = 1;
		int keycountmuch = 0;
		int searchkey = 0;
		Thread[] th = new Thread[20];
		string downloadurl = "http://qiannao.com/ls/taohaoge/936e9de2/";
		string commurl = "";
			string encoding = "";
			string toprule = "";
			string leftrule = "";
			string rghtrule = "";
			string naturerule = "";
			string _cookie = "";//爱站登陆的cookie
			private bool showing = true;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//窗体显示特效
		 　　Opacity = 0.0; //窗体透明度为0
		 　　fadeTimer.Start(); //计时开始
			Control.CheckForIllegalCrossThreadCalls = false;
//			 SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true); 
			button8.Enabled = false;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		 void MainFormLoad(object sender, EventArgs e)
		{
			getNode();
			if(comboBox1.Items.Count>0)
			{
				comboBox1.SelectedIndex = 0;
			}
			if(comboBox3.Items.Count>0)
			{
				comboBox3.SelectedIndex = 0;
			}
			if(comboBox5.Items.Count>0)
			{
				comboBox5.SelectedIndex = 0;
			}
			newbanben();
			listView4.FullRowSelect = true;
		}
		 private void newbanben()
		 {
		 	HttpHelper hp = new HttpHelper();
		 	HttpItem item = new HttpItem
		 	{
		 		URL = "http://qiannao.com/ls/taohaoge/936e9de2/",
		 		Method = "get",
		 	};
		 	string htmlcode = hp.GetHtml(item);
		 	if(htmlcode.Length >10)
		 	{
		 		Regex reg = new Regex("<a href=\"(?<key>.*?)\" target=\"_blank\">下载</a>");
			 	MatchCollection mc = reg.Matches(htmlcode);
			 	if(mc.Count==7)
			 	{
			 		toolStripLabel1.Text = "现在使用的是最新版本";
			 		toolStripLabel1.Enabled = false;
			 	}
			 	if(mc.Count>7)
			 	{
			 		toolStripLabel1.ForeColor = Color.Red;
			 		toolStripLabel1.Text = "有最新版本，点击下载";
			 		downloadurl = mc[0].Groups["key"].Value;
			 		toolStripLabel1.Enabled = true;
			 	}
			 	if(mc.Count<7)
			 	{
			 		toolStripLabel1.Text = "特权的高级用户";
			 		toolStripLabel1.Enabled = true;
			 	}
		 	}
		 	else
		 	{
		 		toolStripLabel1.ForeColor = Color.Red;
		 		toolStrip1.Text = "您网速出问题了,很多功能出故障啦！";
		 		toolStrip1.Enabled = false;
		 	}
		 	
		 }
		public void getNode()
		{
			comboBox1.Items.Clear();
			comboBox3.Items.Clear();
			comboBox5.Items.Clear();
			listView1.Items.Clear();
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach (XmlNode doc in xl) {
				Textmodel tm = new MainForm.Textmodel();
				tm.tid =  doc.Attributes[0].Value;
				tm.name =  doc.Attributes[1].Value;
				comboBox1.Items.Add(tm);
				comboBox3.Items.Add(tm);
				comboBox5.Items.Add(tm);
			}

		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox2.Items.Clear();
			listView1.Items.Clear();
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach (XmlNode doc in xl) {
//				MessageBox.Show("111");
//				MessageBox.Show(doc.Attributes[0].Value + "<>"+(comboBox1.SelectedItem as Textmodel).tid);
				if(doc.Attributes[0].Value ==(comboBox1.SelectedItem as Textmodel).tid)
				{
					XmlNodeList xml = doc.ChildNodes;
					foreach (XmlNode category in xml) {
						Textmodel tm = new MainForm.Textmodel();
						tm.Name = category.Attributes[1].Value;
						tm.tid = category.Attributes[0].Value;
						comboBox2.Items.Add(tm);
					}
				}
			}
			if(comboBox2.Items.Count>0)
			{
				comboBox2.SelectedIndex = 0;
			}
		}
		public class Textmodel
		{
			public	string name;
			public string tid;
			
			public string Tid {
				get { return tid; }
				set { tid = value; }
			}
			public string Name {
				get { return name; }
				set { name = value; }
			}
			public override string ToString()
			{
				return name;
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			listView1.Items.Clear();
			XElement xel = XElement.Load(@"data/keyword.xml");
			string url = (comboBox1.SelectedItem as MainForm.Textmodel).tid;
			string str = (comboBox2.SelectedItem as Textmodel).tid;
			var query = from x in xel.Descendants("keyword")
				where x.Parent.Attribute("id").Value == str && x.Parent.Parent.Attribute("id").Value == url
				select new
				{
					keywords = x.Value
				};
				int i = 1;
				foreach (var  p in query) {
					ListViewItem vi = new ListViewItem();
					vi.Text = i.ToString();
					vi.SubItems.Add(p.keywords.ToString());
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					listView1.Items.Add(vi);
//					listView1.Items[listView1.Items.Count-1].EnsureVisible;
					i++;
				}
		}
		
		void Button6Click(object sender, EventArgs e)
		{
			button8.Enabled = true;
			//设置搜索引擎选项
			rb_baidu.Enabled = false;
			rb_sougou.Enabled = false;
			rb_sousou.Enabled = false;
			rb_youdao.Enabled = false;
			progressBar1.Maximum = listView1.Items.Count;
			keycount = 0;
			progressBar1.Value = 0;
			button6.Enabled = false;
//			Thread th = new Thread(search);
//			th.IsBackground = true;
//			th.Start();
			sousuocheck();
			for(int i=0;i<th.Length;i++)
			{
			th[i] = new Thread(search);
			th[i].Start();
			}
			if(listView1.Items.Count ==0)
			{
				button6.Enabled = true;
				button8.Enabled = false;
			}
		}
		/// <summary>
		/// 判断搜索引擎
		/// </summary>
		private void sousuocheck()
		{
			if(rb_baidu.Checked)
			{
				commurl = "http://www.baidu.com/s?wd=";
				encoding = "UTF-8";
				toprule = "<span>(.*?)</span></a><div id=\"too";
				leftrule = "class=\"ec_url\">(.*?)</span><span *class=\"ec_date";
				rghtrule = "size=\"-1\" class=\"EC_url\">(.*?)</font></a>";
				naturerule = "lass=\"f13\"><span class=\"g\">(.*?)</span><div";
			}
				
			
			if(rb_sougou.Checked)
			{
				commurl = "http://www.sogou.com/web?query=";
				encoding = "gbk";
				toprule = "br/><cite>(.*?)</cite>";
				leftrule = "br/><cite>(.*?)</cite>";
				rghtrule = "[\\s]<cite>(.*?)</cite>";
				naturerule = "cacheresult_info_[\\d](.*?)</cite>";
			}
			//分割点id="side"
			if(rb_sousou.Checked)
			{
				commurl = "http://www.soso.com/w.q?pid=s.idx&cid=s.idx.se&w=";
				encoding = "br/><cite>(.*?)</cite>";
				toprule = "br/><cite>(.*?)</cite>";
				leftrule = "br/><cite>(.*?)</cite>";
				 rghtrule = "br/><cite>(.*?)</cite>";
				naturerule = "rl\"><cite>(.*?)</cite>";
			}
			//分割点id="splink"
			if(rb_youdao.Checked)
			{
				commurl = "http://www.youdao.com/search?q=";
				encoding = "UTF-8";
			     toprule = "<br\\s+/><span class=\"l\">(.*?)</span></a>";
				leftrule = "<br\\s+/><span class=\"l\">(.*?)</span></a>";
				rghtrule = "<p\\s+class=\"s\">([\\s\\S]*?)</p";
			   naturerule = "<cite>([\\s\\S]*?)</ci";
			}
		}
		private void search()
		{
			BaiduRank bk = new BaiduRank();
			
			while(keycount<listView1.Items.Count)
			{
				lock(this)
				{
					Panduan();
					string htmlcode = bk.GetHtmcode(commurl,encoding,listView1.Items[keycount].SubItems[1].Text,textBox1.Text.Trim(),textBox2.Text.Trim(),textBox3.Text.Trim());
					//string htmlcode = bk.GetHtmcode(listView1.Items[keycount].SubItems[1].Text,textBox1.Text.Trim(),textBox2.Text.Trim(),textBox3.Text.Trim());
					listView1.Items[keycount].SubItems[2].Text =Convert.ToString( bk.Gettoprand(htmlcode,GetTopLevelDomain(comboBox1.Text),toprule));
					int leftorright = 0;
					leftorright = bk.Getleftrand(htmlcode,GetTopLevelDomain(comboBox1.Text),leftrule);
					if(rb_sousou.Checked)
					{
						if(Convert.ToInt32(leftorright)>3)
						{
							listView1.Items[keycount].SubItems[3].Text = Convert.ToString("0");
							listView1.Items[keycount].SubItems[4].Text = Convert.ToString((Convert.ToInt32(leftorright)-3).ToString());
						}
						else
						{
							listView1.Items[keycount].SubItems[3].Text = Convert.ToString(leftorright);
							listView1.Items[keycount].SubItems[4].Text = Convert.ToString("0");
						}
					}
					else
					{
						listView1.Items[keycount].SubItems[3].Text = Convert.ToString(bk.Getleftrand(htmlcode,GetTopLevelDomain(comboBox1.Text),leftrule));
						listView1.Items[keycount].SubItems[4].Text = Convert.ToString(bk.Getrightrand(htmlcode,GetTopLevelDomain(comboBox1.Text),rghtrule));
					}
					
					listView1.Items[keycount].SubItems[5].Text = Convert.ToString( bk.Getnature(htmlcode,GetTopLevelDomain(comboBox1.Text),naturerule));
					keycount+=1;
					this.listView1.Items[keycount-1].EnsureVisible();
					if(progressBar1.Value<progressBar1.Maximum)
					{
						progressBar1.Value+=1;
						button8.Enabled = true;
					}
					if(progressBar1.Value==progressBar1.Maximum)
					{
						button6.Enabled = true;
						button8.Enabled = false;
					}
					Panduan();
					
				}
			}
		}
		void Panduan()
		{
				//判断是否超范围
				if(keycount>listView1.Items.Count-1)
				{
					Thread.CurrentThread.Abort();
				}
		}
		void Button7Click(object sender, EventArgs e)
		{
			exportexcel();
			}
		private void exportexcel()
		{
			SaveFileDialog sa = new SaveFileDialog();
			sa.DefaultExt = "xls";
			sa.Filter = "EXCEL文件(*.xls)|*.xls";
			sa.FileName = DateTime.Now.Month+"-"+DateTime.Now.Day+" "+DateTime.Now.Hour+"-"+DateTime.Now.Minute+" "+"排名查询";
			if(sa.ShowDialog()!=DialogResult.OK) return;
			if(sa.FileName == "")
			{
				MessageBox.Show("文件名不能为空");
				return;
			}
			FileStream onjfilestream;
			StreamWriter objstreamwriter;
			StringBuilder sb = new StringBuilder();
			sb.Append("关键字"+"\t"+"顶部排名"+"\t"+"左侧排名"+"\t"+"右侧排名"+"\t"+"自然排名"+"\r\n");
			onjfilestream =new FileStream(sa.FileName,FileMode.OpenOrCreate,FileAccess.Write);
			objstreamwriter = new StreamWriter(onjfilestream,System.Text.Encoding.Unicode);
			for(int i=0;i<listView1.Items.Count;i++)
			{
				sb.Append(listView1.Items[i].SubItems[1].Text+"\t"+listView1.Items[i].SubItems[2].Text+"\t"+listView1.Items[i].SubItems[3].Text+"\t"+listView1.Items[i].SubItems[4].Text+"\t"+listView1.Items[i].SubItems[5].Text+"\r\n");
			}
			objstreamwriter.WriteLine(sb.ToString());
			objstreamwriter.Close();
			onjfilestream.Close();
		}		
		void GroupBox3Enter(object sender, EventArgs e)
		{
			
		}
		
		void Button8Click(object sender, EventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
				if(th[i].ThreadState != ThreadState.Stopped)
				{
					th[i].Abort();
				}
			}
			button8.Enabled = false;
			button6.Enabled = true;
			//设置搜索引擎选项
			rb_baidu.Enabled = true;
			rb_sougou.Enabled = true;
			rb_sousou.Enabled = true;
			rb_youdao.Enabled = true;
		}
		
		void ListView1DoubleClick(object sender, EventArgs e)
		{
			webBrowser1.Navigate("http://www.baidu.com/s?wd="+listView1.SelectedItems[0].SubItems[1].Text);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			Addweb aw = new Addweb();
			aw.ShowDialog();
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			Addgroup ag = new Addgroup();
			ag.ShowDialog();
			
		}
		
		void Button3Click(object sender, EventArgs e)
		{
			Addkeywords ak  =new Addkeywords();
			ak.ShowDialog();
		}
		
		void Button9Click(object sender, EventArgs e)
		{
			//优化建议的文本
//			StringBuilder sb = new StringBuilder();
			//竞价建议的文本
			int left = 3;
			int right = 2;
			int ziran = 5;
			if( textBox6.Text.Trim()!= "" && Convert.ToInt32(textBox6.Text.Trim())>0)
			{
				left = Convert.ToInt32(textBox4.Text.Trim());
			}
			if(textBox5.Text.Trim()!= "" && Convert.ToInt32(textBox5.Text.Trim())>0)
			{
				right = Convert.ToInt32(textBox4.Text.Trim());
			}
			if(textBox4.Text.Trim()!= "" && Convert.ToInt32(textBox4.Text.Trim())>0)
			{
				ziran = Convert.ToInt32(textBox4.Text.Trim());
			}
			StringBuilder sb1 = new StringBuilder();
			for(int i=0;i<listView1.Items.Count;i++)
			{
				if(listView1.Items[i].SubItems[2].Text != "")
				{
				//均无排名
				if(Convert.ToInt32(listView1.Items[i].SubItems[2].Text) == 0 && Convert.ToInt32(listView1.Items[i].SubItems[3].Text) == 0 && Convert.ToInt32(listView1.Items[i].SubItems[4].Text) == 0 && Convert.ToInt32(listView1.Items[i].SubItems[5].Text) == 0 )
				{
					sb1.AppendLine(listView1.Items[i].SubItems[1].Text) ;
				}
				//竞价右边排名低 且无自然排名
				if(right< Convert.ToInt32(listView1.Items[i].SubItems[4].Text) && Convert.ToInt32(listView1.Items[i].SubItems[5].Text)==0 )
				{sb1.AppendLine(listView1.Items[i].SubItems[1].Text +"\t右"+listView1.Items[i].SubItems[4].Text) ;}
				//竞价右边排名低 且自然排名低
				if(right< Convert.ToInt32(listView1.Items[i].SubItems[4].Text) && Convert.ToInt32(listView1.Items[i].SubItems[5].Text)>ziran)
				{sb1.AppendLine(listView1.Items[i].SubItems[1].Text +"\t右"+listView1.Items[i].SubItems[4].Text);}
				//竞价左边排名低 且无自然排名
				if(left< Convert.ToInt32(listView1.Items[i].SubItems[3].Text) && Convert.ToInt32(listView1.Items[i].SubItems[5].Text)==0 )
				{sb1.AppendLine(listView1.Items[i].SubItems[1].Text +"\t左"+listView1.Items[i].SubItems[3].Text);}
				//竞价左边排名低 且自然排名低
				if(left< Convert.ToInt32(listView1.Items[i].SubItems[3].Text) && Convert.ToInt32(listView1.Items[i].SubItems[5].Text)>ziran)
				{sb1.AppendLine(listView1.Items[i].SubItems[1].Text +"\t左"+listView1.Items[i].SubItems[3].Text);}
				}
			}
			SaveFileDialog sf = new SaveFileDialog();
			sf.Filter = "文本文档(*.txt)|*.txt";
			if(sf.ShowDialog() != DialogResult.OK) return;
			if(sf.FileName=="") return;
				
				FileStream fs = new FileStream(sf.FileName,FileMode.OpenOrCreate,FileAccess.Write);
				StreamWriter sw= new StreamWriter(fs,Encoding.Unicode);
				if(sb1.ToString()=="")
				{
					sb1.AppendLine("优化的很好，再接再厉！！！");
				}
				sw.WriteLine(sb1.ToString());
				sw.Close();
				fs.Close();

		}
		
		void Button4Click(object sender, EventArgs e)
		{
			getNode();
			if(comboBox1.Items.Count>0)
			{
				comboBox1.SelectedIndex = 0;
			}
			if(comboBox3.Items.Count>0)
			{
				comboBox3.SelectedIndex = 0;
			}
			if(comboBox5.Items.Count>0)
			{
				comboBox5.SelectedIndex = 0;
			}
		}
		
		
		void ToolStripLabel1Click(object sender, EventArgs e)
		{
			
			 string target = downloadurl;  
			 try 
			  {  
			     System.Diagnostics.Process.Start(target); 
			  } 
			  catch (System.ComponentModel.Win32Exception noBrowser) 
			  { 
			    if (noBrowser.ErrorCode==-2147467259) 
			      MessageBox.Show(noBrowser.Message); 
			  } 
			    catch (System.Exception other) 
			  {  
			    MessageBox.Show(other.Message); 
			  } 
		}
		
		void Button18Click(object sender, EventArgs e)
		{
			HttpHelper hp_aizhan=new HttpHelper();
			HttpItem item = new HttpItem
			{
				URL = "http://www.aizhan.com/login.php",
                Cookie = "PHPSESSID=555812a3b185868a66570897520822c9;userId=84994;userName=1125314524%40qq.com;userGroup=1;userSecure=CK6d2b%2F3M0pqOfcpRXKlSRp58FoesGLHGUwLvUjeeG7AAdw4ZHLcfnun080%3D",
				Method = "post",
				UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*",//    可选项有默认值
                ContentType = "text/html",//返回类型    可选项有默认值
				Postdata = "refer=&email=1125314524@qq.com&password=aiz593327584",
			};
			
			string  htmlcode = hp_aizhan.GetHtml(item);
			_cookie = item.Cookie;
			//File.WriteAllText("爱占数据.txt",htmlcode);
			if(!htmlcode.Contains("1125314524"))
			{
				MessageBox.Show("出现异常，可能无法查询关键字指数");
			}
			
			if(searchkey ==0)
			{
				listView2.Items.Clear();
			}
//			keycounts = 1;
//			searchkey = 0;
			getKeywords(textBox14.Text.Trim());
			for(int m = 0;m<th.Count();m++)
			{
				th[m] = new Thread(getMoreword);
				th[m].IsBackground = true;
				th[m].Start();
			}

		}
		/// <summary>
		/// 获得关键字
		/// </summary>
		/// <param name="keyword"></param>
		private void getKeywords(string keyword)
		{
			//百度"=0\">(?<key>.*?)</a></th"
			//getAllkeyword("http://www.7c.com/keyword/"+keyword+"/","target=\"_blank\">(.*?)</a></td>");
			if(cb_baidu.Checked)
			{
				//"src=[\\d]\">(?<key>[\\w\\d\\u4e00-\\u9fa5]*?)</a></th"
				getAllkeyword("http://www.baidu.com/s?wd="+keyword,"src=[\\d]{1,2}\">(?<key>[\\u4e00-\\u9fa5]*?)</a></th");
			}
			//搜狗http://
			if(cb_sougou.Checked)
			{
				getAllkeyword("www.sogou.com/web?query="+keyword,"sogou_99999_[\\d]\">(?<key>.*?)</a></td>");
			}
			//搜搜
			if(cb_sousou.Checked)
			{
				getAllkeyword("http://www.soso.com/q?w="+keyword,"<td valign=\"top\"><a href=\".*?\">(?<key>.*?)</a></td>");
			}
			//有道
			if(cb_youdao.Checked)
			{
				getAllkeyword("http://www.youdao.com/search?q="+keyword,"this, '(?<key>.*?)', 'result.rel");
			}
			//淘宝
			if(cb_taobao.Checked)
			{
				getAllkeyword("http://s.taobao.com/search?q="+keyword,"click=[\\d]{1,2}\">(?<key>.*?)<span class=\"divide\">");
			}
			//拍拍
//			if(cb_paipai .Checked)
//			{
				getAllkeyword("http://s.taobao.com/search?q="+keyword,"click=[\\d]{1,2}\">(?<key>.*?)<span class=\"divide\">");
//				getAllkeyword("www.sogou.com/web?query="+keyword,"sogou_99999_[\\d]\">(?<key>.*?)</a></td>");
//			}
			//知道
			if(cb_zhidao.Checked)
			{
				getAllkeyword("http://zhidao.baidu.com/search?word="+keyword,"<a href=\"/search?.*?\">(?<key>.*?)</a>");
			}
			//问问
			if(cb_wenwen.Checked)
			{
				getAllkeyword("http://wenwen.soso.com/z/Search.e?sp="+keyword,"/z/Searc.*?\">(?<key>.*?)</a></li>");
			}
			//谷歌
//			if(cb_guge.Checked)
//			{
//				getAllkeyword("http://www.google.com.hk/search?q="+keyword,"sogou_99999_[\\d]\">(?<key>.*?)</a></td>");
//			}
			//雅虎
			if(cb_yahu.Checked)
			{
				getAllkeyword("http://www.yahoo.cn/s?q="+keyword,"from=commend\">(?<key>.*?)</a>");
			}
			//360
//			if(cb_360.Checked)
//			{
//				getAllkeyword("http://www.so.com/s?q="+keyword,"sogou_99999_[\\d]\">(?<key>.*?)</a></td>");
//			}
			//中搜
//			if(cb_zhongsou.Checked)
//			{
//				getAllkeyword("www.sogou.com/web?query="+keyword,"sogou_99999_[\\d]\">(?<key>.*?)</a></td>");
//			}
		}
		private void getAllkeyword(string url,string regrule)
		{
			
		//	string htmlcode = bk.GetHtmcode(keyword,textBox1.Text,textBox2.Text,textBox3.Text);
		//监测是否超范围
		if(Convert.ToInt32(tb_keycount.Text)<=keycounts && Convert.ToInt32(tb_keycount.Text)!=0)
		{
			for(int i=0;i<th.Length;i++)
			{
				if(th[i]!=null)
				{
					if(th[i].ThreadState != ThreadState.Stopped)
					{
						th[i].Abort();
					}
				}
			}
			keycounts = 1;
			searchkey = 0;
		}
		if(Convert.ToInt32(tb_keycount.Text)==0)
		{
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				th[i].Abort();
			}
			}
			keycounts = 1;
			searchkey = 0;
		}
		HttpHelper hp = new HttpHelper();
		HttpItem item = new HttpItem
		{
			URL = url,
			Method = "GET",
			UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
			ProxyIp = textBox1.Text,
			ProxyPwd = textBox3.Text,
			ProxyUserName = textBox2.Text,
		};
		string htmlcode  = hp.GetHtml(item);
		//同步获得创意
		if(checkBox2.Checked)
		{
			getchuangyi(htmlcode,1);
		}
			Regex reg = new Regex(regrule,RegexOptions.IgnoreCase);
			MatchCollection mc = reg.Matches(htmlcode);
			if(mc.Count>0)
			{
				foreach(Match mt in mc)
				{
					if(chongfu(mt.Groups["key"].Value) )
					{
						ListViewItem vi = new ListViewItem();
						vi.Text = keycounts.ToString();
						string miankey = mt.Groups["key"].Value;
						vi.SubItems.Add(miankey);
						string tishi = "";
						vi.SubItems.Add("0");
						vi.SubItems.Add("0");
						vi.SubItems.Add("未知");
						vi.SubItems.Add("未知");
						listView2.Items.Add(vi);
						if(checkBox1.Checked)
						{
							listView2.Items[listView2.Items.Count-1].EnsureVisible();
						}
						if(radioButton2.Checked)
						{
							Getindex(keycounts-1);
						}
						keycounts+=1;
						textBox7.Text="关键字："+mt.Groups["key"].Value+tishi+"\r\n总数："+listView2.Items.Count.ToString();
					}
				}
			}
		}
		private void Getindex(int index)
		{
			//搜索指数
			string mainkey = listView2.Items[index].SubItems[1].Text;
			HttpHelper hp_aizhan=new HttpHelper();
			HttpItem item1 = new HttpItem
			{
				Method = "GET",
				URL = "http://ci.aizhan.com/"+mainkey+"/",
				Cookie = _cookie,
				UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)",
//							ProxyIp = textBox1.Text,
//							ProxyPwd = textBox3.Text,
//							ProxyUserName = textBox2.Text,
			};
			string htmlcode1 = hp_aizhan.GetHtml(item1);
			//File.CreateText("文件夹/关键字"+mainkey+".txt");
//			File.WriteAllText("文件夹/关键字"+mainkey+".txt",htmlcode1);
			if(htmlcode1.Contains("您查询操作过于频繁"))
			{
				MessageBox.Show("爱站网提示：查询操作过于频繁，暂时停止指数查询功能");
				radioButton1.Checked = true;
			}
			else
			{
				Regex reg1 = new Regex("<tr>[\\s\\S]*?</tr>");
				MatchCollection mc1 = reg1.Matches(htmlcode1);
				
				foreach(Match match1 in mc1)
				{
					if(match1.Value.Contains("nofollow\"><font color=\"#ff0000\">"+mainkey+"</font></a>"))
					{
						Regex reg2 = new Regex("right\">(?<key>.*?)</td>");
						MatchCollection mc2 =reg2.Matches(match1.Value);
						if(mc2.Count == 3)
						{
							listView2.Items[index].SubItems[2].Text = mc2[0].Groups["key"].Value;
							listView2.Items[index].SubItems[3].Text = mc2[1].Groups["key"].Value;
						}
						reg2 = new Regex("blank\">(?<key>.*?)</a>");
						mc2 =reg2.Matches(match1.Value);
						if(mc2.Count == 2)
						{
							listView2.Items[index].SubItems[4].Text = mc2[0].Groups["key"].Value;
							listView2.Items[index].SubItems[5].Text = mc2[1].Groups["key"].Value;
						}
					}
				}
				Thread.Sleep(1000);
			}
		}
		/// <summary>
		/// 监测关键字是否重复
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		bool chongfu(string str)
		{			
			//监测关键字是否重复
			bool result = true;
			for(int j=0;j<listView2.Items.Count;j++)
			{
				if(listView2.Items[j] != null)
				{
				if(str == listView2.Items[j].SubItems[1].Text)
				{result = false;}
				}
			}
			if(str.Contains(textBox13.Text) == false)
			{
				result = false;
			}
			if(str.Contains(textBox12.Text) && textBox12.Text.Trim().Length>1)
			{
				result = false;
			}
			if(str.Length<1)
			{
				result = false;
			}
			return result;
		}
		/// <summary>
		/// 获取更多关键字
		/// </summary>
		private void getMoreword()
		{
			while(listView2.Items.Count>2 && searchkey <= keycounts && searchkey <listView2.Items.Count)
			{
				getKeywords(listView2.Items[searchkey].SubItems[1].Text);
				searchkey++;
				
			}
		}
		private bool thlive()
		{
			bool result = false;
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				result = true;
			}
			}
			return result;
		}
		void Button15Click(object sender, EventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				th[i].Abort();
			}
			}
			keycounts = 1;
			searchkey = 0;
		}
		
		void Button17Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			for(int i=0;i<listView2.Items.Count;i++)
			{
				sb.AppendLine(listView2.Items[i].SubItems[1].Text);
			}
			Clipboard.SetDataObject(sb.ToString());
		}
		
		
		void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if(rb_baidu.Checked)
			{
				webBrowser1.Navigate("http://www.baidu.com/s?wd="+listView1.SelectedItems[0].SubItems[1].Text);
			}
			if(rb_sougou.Checked)
			{
				webBrowser1.Navigate("http://www.sogou.com/web?query="+listView1.SelectedItems[0].SubItems[1].Text);
			}
			if(rb_sousou.Checked)
			{
				webBrowser1.Navigate("http://www.soso.com/w.q?pid=s.idx&cid=s.idx.se&w="+listView1.SelectedItems[0].SubItems[1].Text);
			}
			if(rb_youdao.Checked)
			{
				webBrowser1.Navigate("http://www.youdao.com/search?q="+listView1.SelectedItems[0].SubItems[1].Text);
			}
		}
		
		void TextBox14TextChanged(object sender, EventArgs e)
		{
			textBox13.Text = textBox14.Text;
		}
		
		void ToolStripButton1Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex =0;
		}
		
		void ToolStripButton2Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 2;
		}
		
		void ToolStripLabel2Click(object sender, EventArgs e)
		{
			 string target = "http://sighttp.qq.com/authd?IDKEY=ee417b236365946fd5b00593390c8b9c00114ae4a910815f";  
			 try 
			  {  
			     System.Diagnostics.Process.Start(target); 
			  } 
			  catch (System.ComponentModel.Win32Exception noBrowser) 
			  { 
			    if (noBrowser.ErrorCode==-2147467259) 
			      MessageBox.Show(noBrowser.Message); 
			  } 
			    catch (System.Exception other) 
			  {  
			    MessageBox.Show(other.Message); 
			  } 
		}
		
		void 排名查询ToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 0;
		}
		
		void 关键字采集ToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 2;
		}
		
		void 退出ToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 1;
		}
		
		void 单站排名查询ToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 0;
		}
		
		void 多站排名查询ToolStripMenuItemClick(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 1;
		}
		
		void ComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox4.Items.Clear();
			listView3.Items.Clear();
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach (XmlNode doc in xl) {
				if(doc.Attributes[0].Value ==(comboBox1.SelectedItem as Textmodel).tid)
				{
					XmlNodeList xml = doc.ChildNodes;
					foreach (XmlNode category in xml) {
						Textmodel tm = new MainForm.Textmodel();
						tm.Name = category.Attributes[1].Value;
						tm.tid = category.Attributes[0].Value;
						comboBox4.Items.Add(tm);
					}
				}
			}
			if(comboBox4.Items.Count>0)
			{
				comboBox4.SelectedIndex = 0;
			}
		}
		
		void ComboBox4SelectedIndexChanged(object sender, EventArgs e)
		{
			listView3.Items.Clear();
			XElement xel = XElement.Load(@"data/keyword.xml");
			string url = (comboBox3.SelectedItem as MainForm.Textmodel).tid;
			string str = (comboBox4.SelectedItem as Textmodel).tid;
			var query = from x in xel.Descendants("keyword")
				where x.Parent.Attribute("id").Value == str && x.Parent.Parent.Attribute("id").Value == url
				select new
				{
					keywords = x.Value
				};
				int i = 1;
				foreach (var  p in query) {
					ListViewItem vi = new ListViewItem();
					vi.Text = i.ToString();
					vi.SubItems.Add(p.keywords.ToString());
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					vi.SubItems.Add("");
					listView3.Items.Add(vi);
					i++;
				}
		}
		
		void Button13Click(object sender, EventArgs e)
		{
			if(comboBox5.Text.Length>0 && !l3chongfu(comboBox5.Text))
			{
				ColumnHeader ch = new ColumnHeader();
				ch.Text = comboBox5.Text;
				ch.Width = 150;
				ch.TextAlign = HorizontalAlignment.Center;
				listView3.Columns.Add(ch);
				
			}
		}
		private bool l3chongfu(string str)
		{
			bool result = false;
			for(int i=0;i<listView3.Columns.Count;i++)
				{
				if(str == listView3.Columns[i].Text)
				{
					result = true;
				}
				}
			return result;
		}
		
		void Button14Click(object sender, EventArgs e)
		{
			if(listView3.Columns.Count>=4)
			{
				sousuocheck();
				keycountmuch = 0;
				button19.Enabled = true;
				button20.Enabled = true;
				progressBar2.Maximum = listView3.Items.Count;
				progressBar2.Value = 0;
				button14.Enabled = false;
				for(int i=0;i<th.Length;i++)
				{
				th[i] = new Thread(searchmuch);
				th[i].Start();
				}
			}
			else
			{
				MessageBox.Show("请添加多个网址");
			}
			if(listView3.Items.Count ==0)
			{
				button14.Enabled = true;
				button19.Enabled = false;
				button20.Enabled = false;
			}
		}
		private void searchmuch()
		{
			BaiduRank bk = new BaiduRank();
			while(keycountmuch<listView3.Items.Count)
			{
				lock(this)
				{
					l3Panduan();
					int count = listView3.Columns.Count;
					//string htmlcode = bk.GetHtmcode(listView3.Items[keycountmuch].SubItems[1].Text,textBox1.Text.Trim(),textBox2.Text.Trim(),textBox3.Text.Trim());
					string htmlcode = bk.GetHtmcode(commurl,encoding,listView3.Items[keycountmuch].SubItems[1].Text,textBox1.Text.Trim(),textBox2.Text.Trim(),textBox3.Text.Trim());
					for(int i=2;i<count;i++)
					{
						int rank = 0;
						string linshi = "";
						rank = bk.Gettoprand(htmlcode,GetTopLevelDomain(listView3.Columns[i].Text),toprule);
						if(rank != 0 && rb_baidu.Checked)
						{
							linshi = "顶"+Convert.ToString(rank);
						}
						rank = bk.Getleftrand(htmlcode,GetTopLevelDomain(listView3.Columns[i].Text),leftrule);
						if(rank != 0)
						{
							if(rb_sousou.Checked)
							{
								if(rank<4)
								{
									linshi ="左"+Convert.ToString(rank);
								}
								else
								{
									linshi ="右"+Convert.ToString(rank-3);
								}
							}
							else
							{
								linshi ="左"+Convert.ToString(rank);
							}
						}
						rank = bk.Getrightrand(htmlcode,GetTopLevelDomain(listView3.Columns[i].Text),rghtrule);
						if(rank != 0)
						{
							if(!rb_sousou.Checked)
							{
								linshi ="右" + Convert.ToString(rank);
							}
						}
						rank = bk.Getnature(htmlcode,GetTopLevelDomain(listView3.Columns[i].Text),naturerule);
						if(rank != 0)
						{
							if(linshi == "")
							{
								linshi ="自然" + Convert.ToString(rank);
							}
							else
							{
								linshi +=",自然" +Convert.ToString(rank);
							}
						}
						if(linshi == "")
						{
							linshi = "无";
						}
							listView3.Items[keycountmuch].SubItems[i].Text = linshi;
							linshi = "";
					}
					
					
					
//					listView1.Items[keycount].SubItems[2].Text =Convert.ToString( bk.Gettoprand(htmlcode,comboBox1.Text));
//					listView1.Items[keycount].SubItems[3].Text = Convert.ToString( bk.Getleftrand(htmlcode,comboBox1.Text));
//					listView1.Items[keycount].SubItems[4].Text = Convert.ToString( bk.Getrightrand(htmlcode,comboBox1.Text));
//					listView1.Items[keycount].SubItems[5].Text = Convert.ToString( bk.Getnature(htmlcode,comboBox1.Text));
					keycountmuch+=1;
					listView3.Items[keycountmuch-1].EnsureVisible();
					if(progressBar2.Value<progressBar2.Maximum)
					{
						progressBar2.Value+=1;
						button19.Enabled = true;
						button20.Enabled = true;
					}
					if(progressBar2.Value==progressBar2.Maximum)
					{
						button14.Enabled = true;
						button19.Enabled = false;
						button20.Enabled = false;
					}
					l3Panduan();
					
				}
			}
		}
		void l3Panduan()
		{
				//判断是否超范围
				if(keycountmuch>listView3.Items.Count-1)
				{
					Thread.CurrentThread.Abort();
				}
		}
		
		void Button20Click(object sender, EventArgs e)
		{
			button14.Enabled = true;
			button19.Enabled = false;
			button20.Enabled = false;
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				th[i].Abort();
			}
			}
			keycountmuch = 0;
		}
		
		
		void Button11Click(object sender, EventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				th[i].Abort();
			}
			}
			keycount = 0;
		}
		
		void Button19Click(object sender, EventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
			if(th[i].ThreadState != ThreadState.Stopped)
			{
				th[i].Abort();
			}
			}
		}
		
		void Button12Click(object sender, EventArgs e)
		{
			exportexcel2();
		}
		private void exportexcel2()
		{
			SaveFileDialog sa = new SaveFileDialog();
			sa.DefaultExt = "xls";
			sa.Filter = "EXCEL文件(*.xls)|*.xls";
			sa.FileName = DateTime.Now.Month+"-"+DateTime.Now.Day+" "+DateTime.Now.Hour+"-"+DateTime.Now.Minute+" "+"多站排名查询";
			if(sa.ShowDialog()!=DialogResult.OK) return;
			if(sa.FileName == "")
			{
				MessageBox.Show("文件名不能为空");
				return;
			}
			FileStream onjfilestream;
			StreamWriter objstreamwriter;
			StringBuilder sb = new StringBuilder();
			
			onjfilestream =new FileStream(@sa.FileName,FileMode.OpenOrCreate,FileAccess.Write);
			objstreamwriter = new StreamWriter(onjfilestream,System.Text.Encoding.Unicode);
				string headkey = "";
				for(int j=0;j<listView3.Columns.Count;j++)
				{
					headkey+=listView3.Columns[j].Text+"\t";
				}
				sb.AppendLine(headkey);
			for(int i=0;i<listView3.Items.Count;i++)
			{ 
				string keykey = "";
				for(int j=0;j<listView3.Columns.Count;j++)
				{
					keykey+=listView3.Items[i].SubItems[j].Text+"\t";
				}
				sb.AppendLine(keykey);
			}
			objstreamwriter.WriteLine(sb.ToString());
			objstreamwriter.Close();
			onjfilestream.Close();
		}
		
		void 常见问题ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			suggest su = new suggest();
			su.ShowDialog();
		}
		
		void 常见问题ToolStripMenuItemClick(object sender, EventArgs e)
		{
			suggest su  = new suggest();
			su.ShowDialog();
		}
		
		void ToolStripButton6Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 3;
		}
		
		void ToolStripMenuItem3Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 3;
		}
		
		void 提建议ToolStripMenuItemClick(object sender, EventArgs e)
		{
			SendMail sm = new SendMail();
			sm.ShowDialog();
		}
		
		void 提建议ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			SendMail sm = new SendMail();
			sm.ShowDialog();
		}
		
		void ToolStripButton5Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 4;
		}
		
		void Button21Click(object sender, EventArgs e)
		{
			string keyword = "";
			listView4.Items.Clear();
			if(checkBox2.Checked)
			{
					MessageBox.Show("请通过采集关键字获取");
			}
			else
			{
				keyword = tb_key8.Text;
				getchuangyi(keyword);
			}
			button21.Enabled = true;
			button25.Enabled = false;
		}
		
		/// <summary>
		/// 获取百度创意
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void getchuangyi(string htmlcode,int shuzi)
		{
			BaiduRank bk = new BaiduRank();
			StringBuilder sb = new StringBuilder();
			int j = listView4.Items.Count+1;	
			sb.AppendLine("<td class=\"f\"><h3 class=\"t EC_PP\"><a .*?>(?<title>.*?)</a></h3><font size=-1>(?<dec1>.*?)</font><br>");
			Regex reg = new Regex(sb.ToString(),RegexOptions.IgnoreCase);
			MatchCollection mc = reg.Matches(htmlcode);
			if(mc.Count>0)
			{
				for(int i=0;i<mc.Count;i++)
				{
					ListViewItem item = new ListViewItem();
					item.Text = j.ToString();
					string title =mc[i].Groups["title"].Value ;
						title = title.Replace("<font color=#cc0000>","");
						title = title.Replace("</font>","");
					string dec = mc[i].Groups["dec1"].Value;
						dec = dec.Replace("<font color=#cc0000>","");
						dec = dec.Replace("</font>","");
						if(!chuangyichongfu(title))
						{
							item.SubItems.Add(title);
							item.SubItems.Add(dec);
							item.SubItems.Add("");
							listView4.Items.Add(item);
							j++;
						}
					
				}
			}
			//二次获取
			sb.Clear();
			sb.AppendLine("<a.*?><font size=\"3\">(?<title>.*?)</a><br>[\\s\\S]*?");
			sb.AppendLine("<a.*?><font size=\"-1\" color=\"#000000\">(?<dec1>.*?)<br><font size=\"-1\" color=\"#008000\">(?<dec2>.*?)</font></a>");
			reg = new Regex(sb.ToString(),RegexOptions.IgnoreCase);
			mc = reg.Matches(htmlcode);
			if(mc.Count>0)
			{
				for(int i=0;i<mc.Count;i++)
				{
					ListViewItem item = new ListViewItem();
					item.Text = j.ToString();
					string title =mc[i].Groups["title"].Value ;
						title = title.Replace("<font color=#cc0000>","");
						title = title.Replace("</font>","");
					string dec = mc[i].Groups["dec1"].Value;
						dec = dec.Replace("<font color=#cc0000>","");
						dec = dec.Replace("</font>","");
						if(!chuangyichongfu(title))
						{
							item.SubItems.Add(title);
							item.SubItems.Add(dec);
							item.SubItems.Add("");
							listView4.Items.Add(item);
							j++;
						}
					
				}
			}	
		}
		private void getchuangyi(string keyword)
		{
			BaiduRank bk = new BaiduRank();
			StringBuilder sb = new StringBuilder();
			int j = listView4.Items.Count+1;	
			string htmlcode = bk.GetHtmcode(keyword,textBox1.Text,textBox2.Text,textBox3.Text);
			sb.AppendLine("<td class=\"f\"><h3 class=\"t EC_PP\"><a .*?>(?<title>.*?)</a></h3><font size=-1>(?<dec1>.*?)</font><br>");
			Regex reg = new Regex(sb.ToString(),RegexOptions.IgnoreCase);
			MatchCollection mc = reg.Matches(htmlcode);
			if(mc.Count>0)
			{
				for(int i=0;i<mc.Count;i++)
				{
					ListViewItem item = new ListViewItem();
					item.Text = j.ToString();
					string title =mc[i].Groups["title"].Value ;
						title = title.Replace("<font color=#cc0000>","");
						title = title.Replace("</font>","");
					string dec = mc[i].Groups["dec1"].Value;
						dec = dec.Replace("<font color=#cc0000>","");
						dec = dec.Replace("</font>","");
						if(!chuangyichongfu(title))
						{
							item.SubItems.Add(title);
							item.SubItems.Add(dec);
							item.SubItems.Add("");
							listView4.Items.Add(item);
							j++;
						}
				}
			}
			//二次获取
			sb.Clear();
			sb.AppendLine("<a.*?><font size=\"3\">(?<title>.*?)</a><br>[\\s\\S]*?");
			sb.AppendLine("<a.*?><font size=\"-1\" color=\"#000000\">(?<dec1>.*?)<br><font size=\"-1\" color=\"#008000\">(?<dec2>.*?)</font></a>");
			reg = new Regex(sb.ToString(),RegexOptions.IgnoreCase);
			mc = reg.Matches(htmlcode);
			if(mc.Count>0)
			{
				for(int i=0;i<mc.Count;i++)
				{
					ListViewItem item = new ListViewItem();
					item.Text = j.ToString();
					string title =mc[i].Groups["title"].Value ;
						title = title.Replace("<font color=#cc0000>","");
						title = title.Replace("</font>","");
					string dec = mc[i].Groups["dec1"].Value;
						dec = dec.Replace("<font color=#cc0000>","");
						dec = dec.Replace("</font>","");
						if(!chuangyichongfu(title))
						{
							item.SubItems.Add(title);
							item.SubItems.Add(dec);
							item.SubItems.Add("");
							listView4.Items.Add(item);
							j++;
						}
					
				}
			}	
//			chuangyistart++;
		}
		//判断创意列表是都有重复
		bool chuangyichongfu(string title)
		{
			bool result = false;
			if(title.Length == 0)
			{
				result = true;
			}
			for(int i=0;i<listView4.Items.Count;i++)
			{
				if(listView4.Items[i].SubItems != null)
				{
					if(listView4.Items[i].SubItems[1].Text == title)
					{
						result = true;
					}
				}
			}
			return result;
		}
		void Tb_ctitleTextChanged(object sender, EventArgs e)
		{
			lb_title.Text = tb_ctitle.Text.Length.ToString()+"/30";
			xianshi();
		}
		
		void Tb_cdec1TextChanged(object sender, EventArgs e)
		{
			lb_dec1.Text = tb_cdec1.Text.Length.ToString()+"/45";
			xianshi();
		}
		
		void Tb_cdbc2TextChanged(object sender, EventArgs e)
		{
			lb_dec2.Text = tb_cdbc2.Text.Length.ToString()+"/45";
			xianshi();
		}
		
		void Button22Click(object sender, EventArgs e)
		{
			ListViewItem item = new ListViewItem();
			item.Text = (listView5.Items.Count+1).ToString();
			item.SubItems.Add(tb_ctitle.Text);
			item.SubItems.Add(tb_cdec1.Text);
			item.SubItems.Add(tb_cdbc2.Text);
			item.SubItems.Add(tb_url.Text);
			item.SubItems.Add(tb_xianshiurl.Text);
			listView5.Items.Add(item);
		}
		
		void ListView4MouseClick(object sender, MouseEventArgs e)
		{
			tb_ctitle.Text = listView4.SelectedItems[0].SubItems[1].Text;
			tb_cdec1.Text = listView4.SelectedItems[0].SubItems[2].Text;
			tb_cdbc2.Text = listView4.SelectedItems[0].SubItems[3].Text;
			tb_ctitle.Text = tb_ctitle.Text.Replace("&gt;",">");
			tb_ctitle.Text = tb_ctitle.Text.Replace("&lt;","<");
			tb_cdec1.Text = tb_cdec1.Text.Replace("&gt;",">");
			tb_cdec1.Text = tb_cdec1.Text.Replace("&lt;","<");
			tb_cdbc2.Text = tb_cdbc2.Text.Replace("&gt;",">");
			tb_cdbc2.Text = tb_cdbc2.Text.Replace("&lt;","<");
		}
		private void xianshi()
		{
			rb_left.Text = "";
			if(tb_ctitle.Text.Length<30)
			{
				rb_left.AppendText(tb_ctitle.Text);
			}
			else
			{
				rb_left.AppendText(tb_ctitle.Text.Substring(0,30));
			}
			rb_left.AppendText("\r\n");
			if(tb_cdec1.Text.Length<45)
			{
				rb_left.AppendText(tb_cdec1.Text);
			}
			else
			{
				rb_left.AppendText(tb_cdec1.Text.Substring(0,45));
			}
			
			if(tb_cdbc2.Text.Length<45)
			{
				rb_left.AppendText(tb_cdbc2.Text);
			}
			else
			{
				rb_left.AppendText(tb_cdbc2.Text.Substring(0,45));
			}
			rb_left.AppendText("\r\n");
			rb_left.AppendText(tb_xianshiurl.Text);
			Regex reg = new Regex("{(?<key>.*?)}");
			MatchCollection mc = reg.Matches(rb_left.Text);
			string content = rb_left.Text;
			if(mc.Count>0)
			{
				int strlength = 0;
				for(int i=0;i<mc.Count;i++)
				{
					rb_left.Text = rb_left.Text.Replace("{"+mc[i].Groups["key"].Value+"}",mc[i].Groups["key"].Value);
				}
				for(int i=0;i<mc.Count;i++)
				{
					
					int m = content.IndexOf("{");
					int n = content.IndexOf("}")+1;
					rb_left.Select(m+strlength-i,n-m-2);
					rb_left.SelectionColor = Color.Red;
					strlength += n-1;
					content = content.Substring(n);
					
				}
			}
			//显示顶部的链接效果
			rb_top.Text = "";
			if(tb_ctitle.Text.Length<30)
			{
				rb_top.AppendText(tb_ctitle.Text);
			}
			else
			{
				rb_top.AppendText(tb_ctitle.Text.Substring(0,30));
			}
			rb_top.AppendText("\r\n");
			if(tb_cdec1.Text.Length<45)
			{
				rb_top.AppendText(tb_cdec1.Text);
			}
			else
			{
				rb_top.AppendText(tb_cdec1.Text.Substring(0,45));
			}
			rb_top.AppendText(tb_xianshiurl.Text);
			reg = new Regex("{(?<key>.*?)}");
			mc = reg.Matches(rb_top.Text);
			content = rb_top.Text;
			if(mc.Count>0)
			{
				int strlength = 0;
				for(int i=0;i<mc.Count;i++)
				{
					rb_top.Text = rb_top.Text.Replace("{"+mc[i].Groups["key"].Value+"}",mc[i].Groups["key"].Value);
				}
				for(int i=0;i<mc.Count;i++)
				{
					
					int m = content.IndexOf("{");
					int n = content.IndexOf("}")+1;
					rb_top.Select(m+strlength-i,n-m-2);
					rb_top.SelectionColor = Color.Red;
					strlength += n-1;
					content = content.Substring(n);
					
				}
			}
			//显示右边的链接效果
			rb_right.Text = "";
			if(tb_ctitle.Text.Length<14)
			{
				rb_right.AppendText(tb_ctitle.Text);
			}
			else
			{
				rb_right.AppendText(tb_ctitle.Text.Substring(0,14));
			}
			rb_right.AppendText("\r\n");
			if(tb_cdec1.Text.Length<45)
			{
				rb_right.AppendText(tb_cdec1.Text+" "+tb_xianshiurl.Text);
			}
			else
			{
				rb_right.AppendText(tb_cdec1.Text.Substring(0,45)+" "+tb_xianshiurl.Text);
			}
			reg = new Regex("{(?<key>.*?)}");
			mc = reg.Matches(rb_right.Text);
			content = rb_right.Text;
			if(mc.Count>0)
			{
				int strlength = 0;
				for(int i=0;i<mc.Count;i++)
				{
					rb_right.Text = rb_right.Text.Replace("{"+mc[i].Groups["key"].Value+"}",mc[i].Groups["key"].Value);
				}
				for(int i=0;i<mc.Count;i++)
				{
					
					int m = content.IndexOf("{");
					int n = content.IndexOf("}")+1;
					rb_right.Select(m+strlength-i,n-m-2);
					rb_right.SelectionColor = Color.Red;
					strlength += n-1;
					content = content.Substring(n);
					
				}
			}
		}
		
		void Tb_xianshiurlTextChanged(object sender, EventArgs e)
		{
			xianshi();
		}
		
		void Lb_leftMouseHover(object sender, EventArgs e)
		{
			lb_left.ForeColor = Color.Yellow;
			lb_right.ForeColor = Color.Black;
			lb_top.ForeColor = Color.Black;
			gbleft.Visible = true;
			gb_right.Visible = false;
			gb_top.Visible = false;
		}
		
		void Lb_topMouseHover(object sender, EventArgs e)
		{
			lb_left.ForeColor = Color.Black;
			lb_right.ForeColor = Color.Black;
			lb_top.ForeColor = Color.Yellow;
			gb_top.Left = gbleft.Left+groupBox10.Left;
			gb_top.Top = gbleft.Top+groupBox10.Top;
			gbleft.Visible = false;
			gb_right.Visible = false;
			gb_top.Visible = true;
		}
		
		void Lb_rightMouseHover(object sender, EventArgs e)
		{
			lb_left.ForeColor = Color.Black;
			lb_right.ForeColor = Color.Yellow;
			lb_top.ForeColor = Color.Black;
			gb_right.Left = gbleft.Left+groupBox10.Left;
			gb_right.Top = gbleft.Top+groupBox10.Top;
			gbleft.Visible = false;
			gb_right.Visible = true;
			gb_top.Visible = false;
		}
		
		void Button23Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("创意标题\t创意描述1\t创意描述2\t访问URL\t显示URL\t创意状态\t启用/暂停");
			for(int i=0;i<listView5.Items.Count;i++)
			{
				sb.AppendLine(listView5.Items[i].SubItems[1].Text+"\t"+listView5.Items[i].SubItems[2].Text+"\t"+listView5.Items[i].SubItems[3].Text+"\t"+listView5.Items[i].SubItems[4].Text+"\t"+listView5.Items[i].SubItems[5].Text+"\t有效\t启用");
			}
			Clipboard.SetDataObject(sb.ToString());
		}
	
		
		void Button25Click(object sender, EventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
				if(th[i].ThreadState != ThreadState.Stopped)
				{
					th[i].Abort();
				}
			}
			button21.Enabled = true;
			button25.Enabled = false;
		}
		
		void ToolStripMenuItem2Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 4;
		}
		
		void 软件介绍ToolStripMenuItemClick(object sender, EventArgs e)
		{
			About ab = new About();
			ab.ShowDialog();
		}
		
		void Label25Click(object sender, EventArgs e)
		{
			tbreplace(tb_ctitle);
		}
		private void tbreplace(TextBox tb)
		{
			tb.Text=tb.Text.Substring(0,tb.SelectionStart)+"{"+tb.SelectedText+"}"+tb.Text.Substring(tb.SelectionStart+tb.SelectionLength);
		}
		
		void Label26Click(object sender, EventArgs e)
		{
			tbreplace(tb_cdec1);
		}
		
		void Label24Click(object sender, EventArgs e)
		{
			tbreplace(tb_cdbc2);
		}
		
		void Tb_keycountTextChanged(object sender, EventArgs e)
		{
			
			if(tb_keycount.Text == "")
			{
				tb_keycount.Text = "0";
			}
		}
		
		void Tb_keycountKeyPress(object sender, KeyPressEventArgs e)
		{
			 e.Handled = e.KeyChar < '0' || e.KeyChar > '9' ; 
			 if(e.KeyChar==(char)8)   //允许输入删除
		       {   
		             e.Handled=false;
		        }
		}
		
		
		void 全部复制ToolStripMenuItemClick(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("创意标题\t创意描述1\t创意描述2\t访问URL\t显示URL\t创意状态\t启用/暂停");
			for(int i=0;i<listView5.Items.Count;i++)
			{
				sb.AppendLine(listView5.Items[i].SubItems[1].Text+"\t"+listView5.Items[i].SubItems[2].Text+"\t"+listView5.Items[i].SubItems[3].Text+"\t"+listView5.Items[i].SubItems[4].Text+"\t"+listView5.Items[i].SubItems[5].Text+"\t有效\t启用");
			}
			Clipboard.SetDataObject(sb.ToString());
		}
		
		void 全部删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			listView5.Items.Clear();
		}
		
		void 复制ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(listView5.SelectedIndices!=null && listView5.SelectedIndices.Count >0)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("创意标题\t创意描述1\t创意描述2\t访问URL\t显示URL\t创意状态\t启用/暂停");
				ListView.SelectedIndexCollection sc = listView5.SelectedIndices;
				for(int i=0;i<sc.Count;i++)
				{
					sb.AppendLine(listView5.Items[sc[i]].SubItems[1].Text+"\t"+listView5.Items[sc[i]].SubItems[2].Text+"\t"+listView5.Items[sc[i]].SubItems[3].Text+"\t"+listView5.Items[sc[i]].SubItems[4].Text+"\t"+listView5.Items[sc[i]].SubItems[5].Text+"\t有效\t启用");
				}
				Clipboard.SetDataObject(sb.ToString());
			}
			
			
		}
		
		
		void ListView5MouseUp(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				listView5.ContextMenuStrip = contextMenuStrip1;
				listView5.ContextMenuStrip.Show();
				
			}
		}
		
		void 删除ToolStripMenuItemClick(object sender, EventArgs e)
		{
			if(listView5.SelectedIndices!=null && listView5.SelectedIndices.Count >0)
			{
				ListView.SelectedIndexCollection sc = listView5.SelectedIndices;
				for(int i=0;i<sc.Count;i++)
				{
					listView5.Items.Remove(listView5.Items[sc[i]]);
				}
				for(int i=0;i<listView5.Items.Count;i++)
				{
					listView5.Items[i].SubItems[0].Text = (i+1).ToString();
				}
			}
		}
		//添加渐隐渐现
//		protected override void OnLoad(EventArgs e)
//				{
//				 //渐显
//				 this.Load += (_, __) =>
//				 {
//				  this.Opacity = 0.0;
//				  var _timerIn = new Timer()
//				  {
//				   Interval = 200
//				  };
//				  _timerIn.Tick += (_s, _e) =>
//				  {
//				   var inOp = this.Opacity + 0.2;
//				   if (inOp >= 1.0) { inOp = 1.0; _timerIn.Stop(); }
//				   this.Opacity = inOp;
//				  };
//				  _timerIn.Start();
//				 };
//				
//				 //渐隐
//				 var _enableClose = false;
//				 this.FormClosing += (os, oe) =>
//				 {
//				  if (!_enableClose)
//				  {
//				   oe.Cancel = true;
//				   var _timerOut = new Timer()
//				   {
//				    Interval = 200
//				   };
//				   _timerOut.Tick += (_s, _e) =>
//				   {
//				    var inOp = this.Opacity - 0.2;
//				    if (inOp <= 0.0) { _enableClose = true; inOp = 0.0; _timerOut.Stop(); this.Close(); }
//				    this.Opacity = inOp;
//				   };
//				   _timerOut.Start();
//				  }
//				 };
//				 base.OnLoad(e);
//				}
		
		
		void FadeTimerTick(object sender, EventArgs e)
		{
			double d = 0.10;
   　　if (showing)
   　　{
   　　　　if (Opacity + d >= 1.0)
   　　　　{
  　　 　　　　Opacity = 1.0;
 　　 　　fadeTimer.Stop();
 　　　　 }
  　　　　else
 　　　　 {
 　　　　 　　Opacity += d;
 　　　　 }
 　　 }
 　　 else
 　　 { 
 　　　　 if (Opacity - d <= 0.0)
  　　　　{
  　　　　　　Opacity = 0.0;
 　　　　　　 fadeTimer.Stop();
 　　　　 }else
 　　　　{
  　　　　　　Opacity -= d;
 　　　　 }
  　　}
		}
		
		void Button16Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("序号\t关键字\t搜索量\t收录数\t第一名\t第二名");
			for(int i=0;i<listView2.Items.Count;i++)
			{
				sb.AppendLine(listView2.Items[i].SubItems[0].Text+"\t"+listView2.Items[i].SubItems[1].Text+"\t"+listView2.Items[i].SubItems[2].Text+"\t"+listView2.Items[i].SubItems[3].Text+"\t"+listView2.Items[i].SubItems[4].Text+"\t"+listView2.Items[i].SubItems[5].Text+"\t");
			}
			Clipboard.SetDataObject(sb.ToString());
		}
		
		void Button10Click(object sender, EventArgs e)
		{
			if(textBox1.Text.Length>0)
			{
				HttpHelper http = new HttpHelper();
				HttpItem item = new HttpItem
				{
					URL = "http://wwww.baidu.om",
					ProxyIp = textBox1.Text,
					ProxyPwd = textBox3.Text,
					ProxyUserName = textBox2.Text,
				};
				string htmlcode = http.GetHtml(item);
				if(htmlcode.Contains("百度"))
				{
					MessageBox.Show("代理可用");
				}
				else
				{
					MessageBox.Show("代理不可用");
				}
			}
		}
		
		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			for(int i=0;i<th.Length;i++)
			{
				if(th[i] !=null)
				{
					if(th[i].ThreadState != ThreadState.Stopped)
					{
						th[i].Abort();
					}
				}
			}
		}
		public string GetTopLevelDomain(string domain)
    {
        string str = domain;
        if (str.IndexOf(".") > 0)
        {
            string[] strArr = str.Split(':')[0].Split('.');
            if (IsNumeric(strArr[strArr.Length-1]))
            {
                return str;
            }
            else
            {
                string domainRules = "||com.cn|net.cn|org.cn|gov.cn|com.hk|公司|中国|网络|com|net|org|int|edu|gov|mil|arpa|Asia|biz|info|name|pro|coop|aero|museum|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cf|cg|ch|ci|ck|cl|cm|cn|co|cq|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|ee|eg|eh|es|et|ev|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gh|gi|gl|gm|gn|gp|gr|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|in|io|iq|ir|is|it|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|ml|mm|mn|mo|mp|mq|mr|ms|mt|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nt|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|pt|pw|py|qa|re|ro|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sy|sz|tc|td|tf|tg|th|tj|tk|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|va|vc|ve|vg|vn|vu|wf|ws|ye|yu|za|zm|zr|zw|";
                string tempDomain;
                if (strArr.Length >= 4)
                {
                    tempDomain = strArr[strArr.Length - 3] + "." + strArr[strArr.Length - 2] + "." + strArr[strArr.Length - 1];
                    if (domainRules.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 4] + "." + tempDomain;
                    }
                }
                if (strArr.Length >= 3)
                {
                    tempDomain = strArr[strArr.Length - 2] + "." + strArr[strArr.Length - 1];
                    if (domainRules.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 3] + "." + tempDomain;
                    }
                }
                if (strArr.Length >= 2)
                {
                    tempDomain = strArr[strArr.Length - 1];
                    if (domainRules.IndexOf("|" + tempDomain + "|") > 0)
                    {
                        return strArr[strArr.Length - 2] + "." + tempDomain;
                    }
                }
            }
        }
        return str;
    }
     public static bool IsNumeric(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            int len = value.Length;
            if ('-' != value[0] && '+' != value[0] && !char.IsNumber(value[0]))
            {
                return false;
            }
            for (int i = 1; i < len; i++)
            {
                if (!char.IsNumber(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
		
	}
}
