/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/2/6
 * 时间: 10:42
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Windows.Forms;

namespace 关键字排名查询
{
	/// <summary>
	/// Description of DoubleBufferListView.
	/// </summary>
	public class DoubleBufferListView:ListView
	{
		public DoubleBufferListView() 
        { 
            SetStyle(ControlStyles.DoubleBuffer | 
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true); 
            UpdateStyles(); 
        } 
	}
}
