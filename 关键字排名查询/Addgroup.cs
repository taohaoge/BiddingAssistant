/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/3
 * 时间: 10:33
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
	/// Description of Addgroup.
	/// </summary>
	public partial class Addgroup : Form
	{
		public Addgroup()
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
		
		void AddgroupLoad(object sender, EventArgs e)
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
						dataGridView1.Rows.Add(tm);
					}
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			if(panduanchongfu())
			{
				MainForm.Textmodel tm = new MainForm.Textmodel();
				tm.Name = textBox1.Text.Trim();
				if(tm.Name.Contains("http://"))
				{
					tm.Name = tm.Name.Replace("http://","");
				}
				MD5_ md5 = new MD5_();
				tm.tid = md5.Encrypt(textBox1.Text.Trim());
				//xml添加节点
				XmlDocument xd = new XmlDocument();
				xd.Load(@"data/keyword.xml");
				XmlNode xn = xd.SelectSingleNode("//document/url[@name='"+comboBox1.Text+"']");
				XmlElement xe = xd.CreateElement("category");
				xe.SetAttribute("id",tm.tid);
				xe.SetAttribute("name",tm.name);
				xe.InnerText = "";
				xn.AppendChild(xe);
				
				xd.Save(@"data/keyword.xml");
				dataGridView1.Rows.Add(tm);
				textBox1.Text = "";
			}
				
		}
		private bool panduanchongfu()
		{
			bool result = true;
			if(textBox1.Text.Trim() != "")
			{
				for(int i=0;i<dataGridView1.Rows.Count;i++)
				{
					if(dataGridView1.Rows[i].Cells[0].Value as MainForm.Textmodel != null)
					{
					string str = "";
					str = (dataGridView1.Rows[i].Cells[0].Value as MainForm.Textmodel).name;
					if(textBox1.Text.Equals(str))
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
					XmlNode xm = xd.SelectSingleNode("//document/url[@name='"+comboBox1.Text+"']/category[@name='"+(dataGridView1.Rows[e.RowIndex].Cells[0].Value as MainForm.Textmodel).name+"']");
					xm.ParentNode.RemoveChild(xm);
					xd.Save(@"data/keyword.xml");
					dataGridView1.Rows.RemoveAt(e.RowIndex);
					}
				}
			}
		}
		
	}
}
