using System.Windows.Forms;

namespace SqlAssistant
{
	partial class SqlAssistantMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			statusStrip1 = new StatusStrip();
			splitContainer1 = new SplitContainer();
			splitContainer3 = new SplitContainer();
			ConncomboBox = new ComboBox();
			splitContainer4 = new SplitContainer();
			tableLayoutPanel1 = new TableLayoutPanel();
			LikeText = new UserControl.UTextBoxt();
			tableLayoutPanel2 = new TableLayoutPanel();
			BeginTimePicker = new DateTimePicker();
			EndTimePicker = new DateTimePicker();
			label1 = new Label();
			TableText = new UserControl.UTextBoxt();
			splitContainer5 = new SplitContainer();
			DbText = new UserControl.UTextBoxt();
			SreachBtn = new Button();
			btnSelectMD = new Button();
			MdfileText = new UserControl.UTextBoxt();
			btnChangeMD4script = new Button();
			btnScript = new Button();
			btnMD = new Button();
			BtnEntity = new Button();
			NameSpaceText = new UserControl.UTextBoxt();
			splitContainer2 = new SplitContainer();
			DbTree = new TreeView();
			CloumnInfoGrid = new DataGridView();
			CloumnID = new DataGridViewTextBoxColumn();
			CloumnName = new DataGridViewTextBoxColumn();
			TypeName = new DataGridViewTextBoxColumn();
			Disctrible = new DataGridViewTextBoxColumn();
			openFileDialog1 = new OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
			splitContainer3.Panel1.SuspendLayout();
			splitContainer3.Panel2.SuspendLayout();
			splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
			splitContainer4.Panel1.SuspendLayout();
			splitContainer4.Panel2.SuspendLayout();
			splitContainer4.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer5).BeginInit();
			splitContainer5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel1.SuspendLayout();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)CloumnInfoGrid).BeginInit();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.ImageScalingSize = new Size(20, 20);
			statusStrip1.Location = new Point(6, 828);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new Padding(1, 0, 18, 0);
			statusStrip1.Size = new Size(1504, 22);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(6, 6);
			splitContainer1.Margin = new Padding(4);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(splitContainer3);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(splitContainer2);
			splitContainer1.Size = new Size(1504, 822);
			splitContainer1.SplitterDistance = 121;
			splitContainer1.SplitterWidth = 5;
			splitContainer1.TabIndex = 1;
			// 
			// splitContainer3
			// 
			splitContainer3.Dock = DockStyle.Fill;
			splitContainer3.Location = new Point(0, 0);
			splitContainer3.Margin = new Padding(4);
			splitContainer3.Name = "splitContainer3";
			splitContainer3.Orientation = Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			splitContainer3.Panel1.Controls.Add(ConncomboBox);
			// 
			// splitContainer3.Panel2
			// 
			splitContainer3.Panel2.Controls.Add(splitContainer4);
			splitContainer3.Size = new Size(1504, 121);
			splitContainer3.SplitterDistance = 29;
			splitContainer3.SplitterWidth = 5;
			splitContainer3.TabIndex = 0;
			// 
			// ConncomboBox
			// 
			ConncomboBox.Dock = DockStyle.Fill;
			ConncomboBox.FormattingEnabled = true;
			ConncomboBox.Location = new Point(0, 0);
			ConncomboBox.Margin = new Padding(4);
			ConncomboBox.Name = "ConncomboBox";
			ConncomboBox.Size = new Size(1504, 28);
			ConncomboBox.TabIndex = 0;
			ConncomboBox.SelectedIndexChanged += ConncomboBox_SelectedIndexChanged;
			ConncomboBox.SelectedValueChanged += ConncomboBox_SelectedValueChanged;
			// 
			// splitContainer4
			// 
			splitContainer4.Dock = DockStyle.Fill;
			splitContainer4.Location = new Point(0, 0);
			splitContainer4.Margin = new Padding(4);
			splitContainer4.Name = "splitContainer4";
			splitContainer4.Orientation = Orientation.Horizontal;
			// 
			// splitContainer4.Panel1
			// 
			splitContainer4.Panel1.Controls.Add(tableLayoutPanel1);
			// 
			// splitContainer4.Panel2
			// 
			splitContainer4.Panel2.Controls.Add(btnSelectMD);
			splitContainer4.Panel2.Controls.Add(MdfileText);
			splitContainer4.Panel2.Controls.Add(btnChangeMD4script);
			splitContainer4.Panel2.Controls.Add(btnScript);
			splitContainer4.Panel2.Controls.Add(btnMD);
			splitContainer4.Panel2.Controls.Add(BtnEntity);
			splitContainer4.Panel2.Controls.Add(NameSpaceText);
			splitContainer4.Size = new Size(1504, 87);
			splitContainer4.SplitterDistance = 37;
			splitContainer4.SplitterWidth = 5;
			splitContainer4.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.ColumnCount = 6;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43.49206F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56.50794F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 264F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 435F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 284F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 143F));
			tableLayoutPanel1.Controls.Add(LikeText, 2, 0);
			tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 3, 0);
			tableLayoutPanel1.Controls.Add(TableText, 1, 0);
			tableLayoutPanel1.Controls.Add(splitContainer5, 4, 0);
			tableLayoutPanel1.Controls.Add(DbText, 0, 0);
			tableLayoutPanel1.Controls.Add(SreachBtn, 5, 0);
			tableLayoutPanel1.Dock = DockStyle.Fill;
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(4);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 1;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel1.Size = new Size(1504, 37);
			tableLayoutPanel1.TabIndex = 0;
			// 
			// LikeText
			// 
			LikeText.Dock = DockStyle.Fill;
			LikeText.Location = new Point(381, 4);
			LikeText.Margin = new Padding(4);
			LikeText.Name = "LikeText";
			LikeText.PlaceHolderStr = "表名 like";
			LikeText.Size = new Size(256, 27);
			LikeText.TabIndex = 2;
			// 
			// tableLayoutPanel2
			// 
			tableLayoutPanel2.ColumnCount = 3;
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68.14815F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.8518524F));
			tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 176F));
			tableLayoutPanel2.Controls.Add(BeginTimePicker, 0, 0);
			tableLayoutPanel2.Controls.Add(EndTimePicker, 2, 0);
			tableLayoutPanel2.Controls.Add(label1, 1, 0);
			tableLayoutPanel2.Dock = DockStyle.Fill;
			tableLayoutPanel2.Location = new Point(645, 4);
			tableLayoutPanel2.Margin = new Padding(4);
			tableLayoutPanel2.Name = "tableLayoutPanel2";
			tableLayoutPanel2.RowCount = 1;
			tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel2.Size = new Size(427, 29);
			tableLayoutPanel2.TabIndex = 3;
			// 
			// BeginTimePicker
			// 
			BeginTimePicker.Dock = DockStyle.Fill;
			BeginTimePicker.Location = new Point(4, 4);
			BeginTimePicker.Margin = new Padding(4);
			BeginTimePicker.Name = "BeginTimePicker";
			BeginTimePicker.Size = new Size(163, 27);
			BeginTimePicker.TabIndex = 0;
			// 
			// EndTimePicker
			// 
			EndTimePicker.Dock = DockStyle.Fill;
			EndTimePicker.Location = new Point(254, 4);
			EndTimePicker.Margin = new Padding(4);
			EndTimePicker.Name = "EndTimePicker";
			EndTimePicker.Size = new Size(169, 27);
			EndTimePicker.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Dock = DockStyle.Fill;
			label1.Location = new Point(175, 0);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(71, 29);
			label1.TabIndex = 2;
			label1.Text = "——";
			// 
			// TableText
			// 
			TableText.Dock = DockStyle.Fill;
			TableText.Location = new Point(168, 4);
			TableText.Margin = new Padding(4);
			TableText.Name = "TableText";
			TableText.PlaceHolderStr = "完整表名，多个逗号隔开";
			TableText.Size = new Size(205, 27);
			TableText.TabIndex = 1;
			// 
			// splitContainer5
			// 
			splitContainer5.Dock = DockStyle.Fill;
			splitContainer5.Location = new Point(1080, 4);
			splitContainer5.Margin = new Padding(4);
			splitContainer5.Name = "splitContainer5";
			splitContainer5.Size = new Size(276, 29);
			splitContainer5.SplitterDistance = 132;
			splitContainer5.SplitterWidth = 5;
			splitContainer5.TabIndex = 4;
			// 
			// DbText
			// 
			DbText.Dock = DockStyle.Fill;
			DbText.Location = new Point(4, 4);
			DbText.Margin = new Padding(4);
			DbText.Name = "DbText";
			DbText.PlaceHolderStr = "请输入数据库名称";
			DbText.Size = new Size(156, 27);
			DbText.TabIndex = 0;
			// 
			// SreachBtn
			// 
			SreachBtn.Dock = DockStyle.Fill;
			SreachBtn.Location = new Point(1364, 4);
			SreachBtn.Margin = new Padding(4);
			SreachBtn.Name = "SreachBtn";
			SreachBtn.Size = new Size(136, 29);
			SreachBtn.TabIndex = 5;
			SreachBtn.Text = "搜索";
			SreachBtn.UseVisualStyleBackColor = true;
			SreachBtn.Click += SreachBtn_Click;
			// 
			// btnSelectMD
			// 
			btnSelectMD.Location = new Point(1216, 5);
			btnSelectMD.Margin = new Padding(4);
			btnSelectMD.Name = "btnSelectMD";
			btnSelectMD.Size = new Size(96, 33);
			btnSelectMD.TabIndex = 7;
			btnSelectMD.Text = "选择MD文件";
			btnSelectMD.UseVisualStyleBackColor = true;
			btnSelectMD.Click += btnSelectMD_Click;
			// 
			// MdfileText
			// 
			MdfileText.Location = new Point(685, 8);
			MdfileText.Margin = new Padding(4);
			MdfileText.Name = "MdfileText";
			MdfileText.PlaceHolderStr = "MD文件";
			MdfileText.Size = new Size(522, 27);
			MdfileText.TabIndex = 6;
			// 
			// btnChangeMD4script
			// 
			btnChangeMD4script.Location = new Point(1320, 5);
			btnChangeMD4script.Margin = new Padding(4);
			btnChangeMD4script.Name = "btnChangeMD4script";
			btnChangeMD4script.Size = new Size(167, 33);
			btnChangeMD4script.TabIndex = 5;
			btnChangeMD4script.Text = "MD转成建表语句";
			btnChangeMD4script.UseVisualStyleBackColor = true;
			btnChangeMD4script.Click += btnChangeMD4script_Click;
			// 
			// btnScript
			// 
			btnScript.Location = new Point(505, 5);
			btnScript.Margin = new Padding(4);
			btnScript.Name = "btnScript";
			btnScript.Size = new Size(96, 33);
			btnScript.TabIndex = 4;
			btnScript.Text = "生成建表语句";
			btnScript.UseVisualStyleBackColor = true;
			btnScript.Click += btnScript_Click;
			// 
			// btnMD
			// 
			btnMD.Location = new Point(370, 5);
			btnMD.Margin = new Padding(4);
			btnMD.Name = "btnMD";
			btnMD.Size = new Size(96, 33);
			btnMD.TabIndex = 3;
			btnMD.Text = "生成MD";
			btnMD.UseVisualStyleBackColor = true;
			btnMD.Click += btnMD_Click;
			// 
			// BtnEntity
			// 
			BtnEntity.Location = new Point(237, 5);
			BtnEntity.Margin = new Padding(4);
			BtnEntity.Name = "BtnEntity";
			BtnEntity.Size = new Size(96, 33);
			BtnEntity.TabIndex = 2;
			BtnEntity.Text = "生成实体";
			BtnEntity.UseVisualStyleBackColor = true;
			BtnEntity.Click += BtnEntity_Click;
			// 
			// NameSpaceText
			// 
			NameSpaceText.Location = new Point(4, 8);
			NameSpaceText.Margin = new Padding(4);
			NameSpaceText.Name = "NameSpaceText";
			NameSpaceText.PlaceHolderStr = "请输入实体的命名空间";
			NameSpaceText.Size = new Size(224, 27);
			NameSpaceText.TabIndex = 1;
			// 
			// splitContainer2
			// 
			splitContainer2.Dock = DockStyle.Fill;
			splitContainer2.Location = new Point(0, 0);
			splitContainer2.Margin = new Padding(4);
			splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			splitContainer2.Panel1.Controls.Add(DbTree);
			// 
			// splitContainer2.Panel2
			// 
			splitContainer2.Panel2.Controls.Add(CloumnInfoGrid);
			splitContainer2.Size = new Size(1504, 696);
			splitContainer2.SplitterDistance = 499;
			splitContainer2.SplitterWidth = 5;
			splitContainer2.TabIndex = 0;
			// 
			// DbTree
			// 
			DbTree.Dock = DockStyle.Fill;
			DbTree.Location = new Point(0, 0);
			DbTree.Margin = new Padding(4);
			DbTree.Name = "DbTree";
			DbTree.Size = new Size(499, 696);
			DbTree.TabIndex = 0;
			DbTree.NodeMouseClick += DbTree_NodeMouseClick;
			// 
			// CloumnInfoGrid
			// 
			CloumnInfoGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			CloumnInfoGrid.Columns.AddRange(new DataGridViewColumn[] { CloumnID, CloumnName, TypeName, Disctrible });
			CloumnInfoGrid.Dock = DockStyle.Fill;
			CloumnInfoGrid.Location = new Point(0, 0);
			CloumnInfoGrid.Margin = new Padding(4, 4, 6, 4);
			CloumnInfoGrid.Name = "CloumnInfoGrid";
			CloumnInfoGrid.RowHeadersWidth = 51;
			CloumnInfoGrid.Size = new Size(1000, 696);
			CloumnInfoGrid.TabIndex = 0;
			// 
			// CloumnID
			// 
			CloumnID.DataPropertyName = "CloumnID";
			CloumnID.FillWeight = 150F;
			CloumnID.HeaderText = "序号";
			CloumnID.MinimumWidth = 6;
			CloumnID.Name = "CloumnID";
			CloumnID.Width = 150;
			// 
			// CloumnName
			// 
			CloumnName.DataPropertyName = "CloumnName";
			CloumnName.FillWeight = 150F;
			CloumnName.HeaderText = "列名";
			CloumnName.MinimumWidth = 6;
			CloumnName.Name = "CloumnName";
			CloumnName.Width = 150;
			// 
			// TypeName
			// 
			TypeName.DataPropertyName = "TypeName";
			TypeName.FillWeight = 150F;
			TypeName.HeaderText = "数据类型";
			TypeName.MinimumWidth = 6;
			TypeName.Name = "TypeName";
			TypeName.Width = 150;
			// 
			// Disctrible
			// 
			Disctrible.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			Disctrible.DataPropertyName = "Disctrible";
			Disctrible.HeaderText = "说明";
			Disctrible.MinimumWidth = 6;
			Disctrible.Name = "Disctrible";
			// 
			// openFileDialog1
			// 
			openFileDialog1.FileName = "openFileDialog1";
			openFileDialog1.Filter = "Text Files (*.md)|*.md";
			// 
			// SqlAssistantMain
			// 
			AutoScaleDimensions = new SizeF(9F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = SystemColors.ControlLight;
			ClientSize = new Size(1516, 856);
			Controls.Add(splitContainer1);
			Controls.Add(statusStrip1);
			Margin = new Padding(4);
			Name = "SqlAssistantMain";
			Padding = new Padding(6);
			Text = "sql小助手";
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			splitContainer3.Panel1.ResumeLayout(false);
			splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
			splitContainer3.ResumeLayout(false);
			splitContainer4.Panel1.ResumeLayout(false);
			splitContainer4.Panel2.ResumeLayout(false);
			splitContainer4.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
			splitContainer4.ResumeLayout(false);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			tableLayoutPanel2.ResumeLayout(false);
			tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer5).EndInit();
			splitContainer5.ResumeLayout(false);
			splitContainer2.Panel1.ResumeLayout(false);
			splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)CloumnInfoGrid).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private StatusStrip statusStrip1;
		private SplitContainer splitContainer1;
		private SplitContainer splitContainer2;
		private TreeView DbTree;
		private DataGridView CloumnInfoGrid;
		private DataGridViewTextBoxColumn CloumnID;
		private DataGridViewTextBoxColumn CloumnName;
		private DataGridViewTextBoxColumn TypeName;
		private DataGridViewTextBoxColumn Disctrible;
		private SplitContainer splitContainer3;
		private ComboBox ConncomboBox;
		private SplitContainer splitContainer4;
		private TableLayoutPanel tableLayoutPanel1;
		private TableLayoutPanel tableLayoutPanel2;
		private DateTimePicker BeginTimePicker;
		private DateTimePicker EndTimePicker;
		private Label label1;
		private SplitContainer splitContainer5;
		private Button SreachBtn;
		private UserControl.UTextBoxt LikeText;
		private UserControl.UTextBoxt TableText;
		private UserControl.UTextBoxt DbText;
		private Button btnChangeMD4script;
		private Button btnScript;
		private Button btnMD;
		private Button BtnEntity;
		private UserControl.UTextBoxt NameSpaceText;
		private Button btnSelectMD;
		private UserControl.UTextBoxt MdfileText;
		private OpenFileDialog openFileDialog1;
	}
}
