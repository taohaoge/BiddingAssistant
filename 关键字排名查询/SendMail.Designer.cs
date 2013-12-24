/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/23
 * 时间: 13:19
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
namespace 关键字排名查询
{
	partial class SendMail
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendMail));
			this.tb_title = new System.Windows.Forms.TextBox();
			this.tb_content = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tb_title
			// 
			this.tb_title.Location = new System.Drawing.Point(55, 12);
			this.tb_title.Name = "tb_title";
			this.tb_title.Size = new System.Drawing.Size(285, 21);
			this.tb_title.TabIndex = 0;
			// 
			// tb_content
			// 
			this.tb_content.Location = new System.Drawing.Point(55, 39);
			this.tb_content.Multiline = true;
			this.tb_content.Name = "tb_content";
			this.tb_content.Size = new System.Drawing.Size(285, 109);
			this.tb_content.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(42, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "标题：";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 89);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "内容：";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(258, 154);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(82, 25);
			this.button1.TabIndex = 4;
			this.button1.Text = "发送";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// SendMail
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(353, 184);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tb_content);
			this.Controls.Add(this.tb_title);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "SendMail";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "建议";
			this.Load += new System.EventHandler(this.SendMailLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox tb_content;
		private System.Windows.Forms.TextBox tb_title;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
	}
}
