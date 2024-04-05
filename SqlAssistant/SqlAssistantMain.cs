
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlAssistant.API;
using SqlAssistant.implement;
using SqlAssistant.InterFace;
using SqlAssistant.Option;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using static Dm.net.buffer.ByteArrayBuffer;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SqlAssistant
{
	public partial class SqlAssistantMain : Form
	{


		public SqlQuery sqlQuery = null;

		List<IsqlTool> _clients = null;

		DbConfig config = null;

		SqlManage sqlManage = null;

		List<NodeData> TreeData = new List<NodeData>();

		string FilePath = "C:\\Users\\Desktop\\model";

		public SqlAssistantMain()
		{
			InitializeComponent();

			config = new DbConfig();
			new ConfigurationBuilder()
				.AddJsonFile(AppContext.BaseDirectory + "Connctions.json")
			.Build()
			.GetSection("Config")
			.Bind(config);
			this.ConncomboBox.DataSource = config.Hosts;
			this.ConncomboBox.DisplayMember = "ShowText";
			var firstItem = config.Hosts.First();

			sqlQuery = new SqlQuery()
			{
				IP = firstItem.IP,
				Type = firstItem.Type,
				Sa = firstItem.Sa,
				Passaord = firstItem.Passaord,
				DbName = "txy.basic",
				TableName = "tt_advertisement"
			};



			_clients = new List<IsqlTool>();
			_clients.Add(new MysqlTool(sqlQuery));
			_clients.Add(new SqlServerTool(sqlQuery));

			this.CloumnInfoGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			this.CloumnInfoGrid.AutoGenerateColumns = false;
			this.CloumnInfoGrid.AutoSize = true;


			sqlManage = new SqlManage(sqlQuery, _clients);

			this.NameSpaceText.Text = "SqlTool.Option";
			this.MdfileText.Text = "C:\\Users\\Desktop\\model\\20231215.md";

			this.BeginTimePicker.Value = DateTime.Now.AddYears(-10);
			this.EndTimePicker.Value = DateTime.Now.AddDays(2);
		}



		private void SreachBtn_Click(object sender, EventArgs e)
		{
			sqlQuery = GetQuery();

			if (string.IsNullOrEmpty(sqlQuery.DbName) || string.IsNullOrEmpty(sqlQuery.TableName))
			{
				TreeData = sqlManage.GetDataBaseInfo(sqlQuery);
				this.DbTree.Nodes.Clear();
				BindTree(TreeData);
			}

			else if (!string.IsNullOrEmpty(sqlQuery.TableName))
			{
				this.CloumnInfoGrid.DataSource = sqlManage.GetCloumns(sqlQuery);
			}
		}


		private void BtnEntity_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "请选择保存文件夹";
			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				FilePath = dialog.SelectedPath;
				sqlQuery = GetQuery();
				sqlManage.GenerateEnty(sqlQuery);
				MessageBox.Show("生成成功！");
			}
		}

		private void btnMD_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "请选择保存文件夹";
			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				FilePath = dialog.SelectedPath;
				sqlQuery = GetQuery();
				sqlManage.GenerateMDFile(sqlQuery);
				MessageBox.Show("生成成功！");
			}
		}

		private void btnScript_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "请选择保存文件夹";
			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				FilePath = dialog.SelectedPath;
				sqlQuery = GetQuery();
				sqlManage.GenerateSqlFile(sqlQuery);
				MessageBox.Show("生成成功！");
			}
		}

		private void btnSelectMD_Click(object sender, EventArgs e)
		{
			var result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				MdfileText.Text = openFileDialog1.FileName.Trim();
			}
		}

		private void btnChangeMD4script_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "请选择保存文件夹，根据当前连接字符串生成对应的数据库类型的脚本";
			var result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				FilePath = dialog.SelectedPath;
				sqlQuery = GetQuery();
				sqlManage.GenerateMDsql(sqlQuery);
				MessageBox.Show("生成成功！");
			}


		}

		private void ConncomboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var Item = (Host)this.ConncomboBox.SelectedItem;
			if (Item == null || sqlQuery == null) return;
			sqlQuery.IP = Item.IP;
			sqlQuery.Type = Item.Type;
			sqlQuery.Sa = Item.Sa;
			sqlQuery.Passaord = Item.Passaord;

		}

		/// <summary>
		/// 获取查询条件
		/// </summary>
		/// <returns></returns>
		private SqlQuery GetQuery()
		{
			var Item = (Host)this.ConncomboBox.SelectedItem;
			SqlQuery Userquery = new SqlQuery();
			Userquery.IP = Item.IP;
			Userquery.Type = Item.Type;
			Userquery.Sa = Item.Sa;
			Userquery.Passaord = Item.Passaord;

			Userquery.DbName = DbText.Text.Trim();
			Userquery.TableName = TableText.Text.Trim();
			Userquery.TableSearch = LikeText.Text.Trim();

			Userquery.CreateDateArr = new string[] { BeginTimePicker.Text.Trim(), EndTimePicker.Text.Trim() };

			Userquery.NameSpace = NameSpaceText.Text.Trim();
			Userquery.FilePath = FilePath;
			Userquery.NameSpace = NameSpaceText.Text.Trim();
			Userquery.MDFilePath = MdfileText.Text.Trim();
			return Userquery;
		}



		private void BindTree(List<NodeData> Data, TreeNode parent = null)
		{
			if (Data == null || Data.Count == 0) return;

			foreach (NodeData node in Data)
			{
				string dis = node.label + (string.IsNullOrWhiteSpace(node.discrible) ? "" : "  【" + node.discrible + "】");

				TreeNode treeNode = new TreeNode(dis);
				treeNode.Tag = node.DbName + ":" + node.label;
				if (parent == null)
				{
					this.DbTree.Nodes.Add(treeNode);
				}
				else
				{
					parent.Nodes.Add(treeNode);
				}
				if (node.children != null && node.children.Length > 0)
				{
					BindTree(node.children.ToList(), treeNode);
				}
			}
		}

		private void DbTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			string[] arr = e.Node.Tag.ToString().Split(":");
			string db = arr[0];
			string label = arr[1];


			if (e.Node.Level == 0)
			{
				this.DbText.Text = db;
				this.TableText.Text = string.Empty;
			}

			else if (e.Node.Level == 1)
			{
				var pdata = TreeData.Where(r => r.label.Equals(db) && r.DbName.Equals(db)).First();
				var data = pdata.children.Where(r => r.label.Equals(label) && r.DbName.Equals(db)).First();
				this.TableText.Text = data.label;
				this.DbText.Text = data.DbName;
				SreachBtn_Click(null, null);
			}
		}

		private void Reset()
		{
			DbText.Text=string.Empty;
			TableText.Text=string.Empty ;
			LikeText.Text=string.Empty ;
		}
		private void ConncomboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			Reset();
		}
	}
}
