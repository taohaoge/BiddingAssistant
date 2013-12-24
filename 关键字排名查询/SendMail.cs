/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/23
 * 时间: 13:19
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text;
using System.Net;
namespace 关键字排名查询
{
	/// <summary>
	/// Description of SendMail.
	/// </summary>
	public partial class SendMail : Form
	{
		public SendMail()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
//			if(SendEMail(,,))
//			{
//				MessageBox.Show("发送成功");
//			}
//			else
//			{
//				MessageBox.Show("啊哦，发送失败");
//			}
			 try {
			 	MailMessage mail = new MailMessage();//实例化一封邮件
	            mail.From = new MailAddress("jjxzs2013@sina.cn","jjxzs2013@sina.cn");//设置发件人名称和发件人邮箱地址
	            mail.To.Add("jjxzs2013@sina.cn");
	            mail.Subject = tb_title.Text;//设置邮件主题
	            mail.Body = tb_content.Text;//设置邮件正文
	            mail.BodyEncoding = Encoding.UTF8;//设置正文编码方式
	            mail.IsBodyHtml = false;//设置正文是否以html格式发送
	//            if (attachList != null && attachList.Count > 0 && attachList != null)//判断是否存在附件
	//            {
	//                foreach (string str in attachList)//遍历附件集合
	//                {
	//                    if (str != null && str != "" && File.Exists(str))//如果绝对路径不为空，且该路径下的文件存在
	//                    {
	//                        mail.Attachments.Add(new Attachment(str));//为该邮件添加一个附件
	//                    }
	//                }
	//            }
	
	            SmtpClient smtp = new SmtpClient();//实例化一个用于发送邮件的SMTP客户端
	            smtp.Host = "smtp.sina.com";//设置SMTP服务器地址（用于发送邮件的邮箱的SMTP服务器地址）
	            smtp.Credentials = new NetworkCredential("jjxzs2013@sina.cn", "jjxzs2013");//用于发送邮件的邮箱的登录名和密码
	            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//设置该邮件通过网络方式发送到SMTP服务器
	            smtp.Send(mail);//发送
	            MessageBox.Show("发送完成");
	            this.Close();
			 } 
			 catch (SmtpException ex)
				    {
				        MessageBox.Show(ex.Message);
				    }
				    catch(Exception ex2)
				    {
				        MessageBox.Show(ex2.Message);
				    }
		}

		
		void SendMailLoad(object sender, EventArgs e)
		{
			Control.CheckForIllegalCrossThreadCalls = false;
		}
	}
}
