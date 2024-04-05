namespace SqlAssistant.UserControl
{
	partial class UTextBoxt:TextBox
	{
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();			
		}

		public String PlaceHolderStr { get; set; }
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == 0xF || m.Msg == 0x133)
			{
				WmPaint(ref m);
			}
		}
		private void WmPaint(ref Message m)
		{
			Graphics g = Graphics.FromHwnd(base.Handle);
			if (!String.IsNullOrEmpty(this.PlaceHolderStr) && string.IsNullOrEmpty(this.Text))
				g.DrawString(this.PlaceHolderStr, this.Font, new SolidBrush(Color.LightGray), 0, 0);
		}

		#endregion
	}
}
