/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/3
 * 时间: 10:39
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace 关键字排名查询
{
	/// <summary>
	/// Description of Addkeywords.
	/// </summary>
	public partial class Addkeywords : Form
	{
		public Addkeywords()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void AddkeywordsLoad(object sender, EventArgs e)
		{
			
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach (XmlNode doc in xl) {
				MainForm.Textmodel tm = new MainForm.Textmodel();
				tm.name =  doc.Attributes[1].Value;
				tm.tid =  doc.Attributes[0].Value;
				comboBox1.Items.Add(tm);
			}
					if(comboBox1.Items.Count>0)
					{comboBox1.SelectedIndex = 0;}
		}
		
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			comboBox2.Items.Clear();
			dataGridView1.Rows.Clear();
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach(XmlNode xn in xl)
			{
				if(xn.Attributes[0].Value == (comboBox1.SelectedItem as MainForm.Textmodel).tid)
				{
					XmlNodeList xml = xn.ChildNodes;
					foreach (XmlNode category in xml) {
						MainForm.Textmodel tm = new MainForm.Textmodel();
						tm.Name = category.Attributes[1].Value;
						tm.tid = category.Attributes[0].Value;
						comboBox2.Items.Add(tm);
					}
					if(comboBox2.Items.Count>0)
					{comboBox2.SelectedIndex = 0;}
				}
			}
		}
		
		void ComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			dataGridView1.Rows.Clear();
			
			XElement xel = XElement.Load(@"data/keyword.xml");
			string url = (comboBox1.SelectedItem as MainForm.Textmodel).tid;
			string str = (comboBox2.SelectedItem as MainForm.Textmodel).tid;
			var query = from x in xel.Descendants("keyword")
				where x.Parent.Attribute("id").Value == str && x.Parent.Parent.Attribute("id").Value == url
				select new
				{
					keywords = x.Value,
					id = x.Attribute("id").Value
				};
				int i = 1;
				foreach (var  p in query) {
					MainForm.Textmodel tm = new MainForm.Textmodel();
					tm.name = p.keywords;
					tm.tid = p.id;
					dataGridView1.Rows.Add(tm);
					i++;
				}
		}
		/// <summary>
		/// 添加新的数据
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Button1Click(object sender, EventArgs e)
		{
			string[] textrow = textBox1.Text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			foreach(string key in textrow)
			{
				if(chongfu(key))
				{
				MainForm.Textmodel tm = new MainForm.Textmodel();
				tm.Name = key.Trim();
				MD5_ md5 = new MD5_();
				tm.tid = md5.Encrypt(key.Trim());
				//xml添加节点
				XmlDocument xd = new XmlDocument();
				xd.Load(@"data/keyword.xml");
				XmlNode xn = xd.SelectSingleNode("//document/url[@name='"+comboBox1.Text+"']/category[@name='"+comboBox2.Text+"']");
				XmlElement xe = xd.CreateElement("keyword");
				xe.SetAttribute("id",tm.tid);
				xe.InnerText = tm.name;
				xn.AppendChild(xe);
				
				xd.Save(@"data/keyword.xml");
					dataGridView1.Rows.Add(tm);
				}
				textBox1.Text = "";
			}
		}
		/// <summary>
		/// 判断重复
		/// </summary>
		/// <returns></returns>
		private bool chongfu(string keywords)
		{
			bool result = true;
			if(keywords.Trim() != "")
			{
				for(int i=0;i<dataGridView1.Rows.Count;i++)
				{
					if(dataGridView1.Rows[i].Cells[0].Value as MainForm.Textmodel != null)
					{
					string str = "";
					str = (dataGridView1.Rows[i].Cells[0].Value as MainForm.Textmodel).name;
					if(keywords.Equals(str))
					{result = false;}
					}
				}
			}
			else
			{
				result = false;
			}

			return result;
		}
		
		void DataGridView1CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if(e.RowIndex > -1 && e.ColumnIndex ==1)
			{
				if(dataGridView1.Rows.Count>1)
				{
					if(dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
					{
					XmlDocument xd = new XmlDocument();
					xd.Load(@"data/keyword.xml");
					XmlNode xm = xd.SelectSingleNode("//document/url[@name='"+comboBox1.Text+"']/category[@name='"+comboBox2.Text+"']/keyword[@id='"+(dataGridView1.Rows[e.RowIndex].Cells[0].Value as MainForm.Textmodel).tid+"']");
					xm.ParentNode.RemoveChild(xm);
					xd.Save(@"data/keyword.xml");
					dataGridView1.Rows.RemoveAt(e.RowIndex);
					}
				}
			}
		}
	}
}
