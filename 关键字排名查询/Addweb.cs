/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/3
 * 时间: 10:21
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
namespace 关键字排名查询
{
	/// <summary>
	/// Description of Addweb.
	/// </summary>
	public partial class Addweb : Form
	{
		public Addweb()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void AddwebLoad(object sender, EventArgs e)
		{
			XmlDocument xd = new XmlDocument();
			xd.Load(@"data/keyword.xml");
			XmlNodeList xl = xd.SelectSingleNode("document").ChildNodes;
			foreach (XmlNode xd1 in xl) {
				MainForm.Textmodel tm = new MainForm.Textmodel();
				tm.Name = xd1.Attributes[1].Value;
				tm.tid = xd1.Attributes[0].Value;
				dataGridView1.Rows.Add(tm);
			}
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
					XmlNode xm = xd.SelectSingleNode("//document/url[@name='"+(dataGridView1.Rows[e.RowIndex].Cells[0].Value as MainForm.Textmodel).name+"']");
					xm.ParentNode.RemoveChild(xm);
					xd.Save(@"data/keyword.xml");
					dataGridView1.Rows.RemoveAt(e.RowIndex);
					}
				}
			}
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			this.Close();
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(chongfu(textBox1.Text.Trim()))
			{	
				MainForm.Textmodel tm = new MainForm.Textmodel();
				tm.Name = textBox1.Text.Trim();
				MD5_ md5 = new MD5_();
				tm.tid = md5.Encrypt(textBox1.Text.Trim());
				//xml添加节点
				XmlDocument xd = new XmlDocument();
				xd.Load(@"data/keyword.xml");
				XmlNode xn = xd.SelectSingleNode("document");
				XmlElement xe = xd.CreateElement("url");
				xe.SetAttribute("id",tm.tid);
				xe.SetAttribute("name",tm.name);
				xe.InnerText = "";
				xn.AppendChild(xe);
				
				xd.Save(@"data/keyword.xml");
				
				dataGridView1.Rows.Add(tm);
				textBox1.Text = "";
			}
			
			
		}
		/// <summary>
		/// 监测重复网址
		/// </summary>
		bool chongfu(string str)
		{
			bool result = true;
			if(str.Trim().Length>0)
			{
				for(int i=0;i<dataGridView1.Rows.Count;i++)
				{
					if(dataGridView1.Rows[i].Cells[0].Value != null)
					{
					if(str.Equals((dataGridView1.Rows[i].Cells[0].Value as MainForm.Textmodel).name))
					{
						result = false;
					}
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}
		
	}
}
