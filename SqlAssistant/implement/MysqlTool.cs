using SqlSugar;
using SqlAssistant.InterFace;
using SqlAssistant.Option;
using SqlAssistant.Option.page;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace SqlAssistant.implement
{
    public class MysqlTool : IsqlTool
    {
        public ConItem _conItem = null;

		public ConItem ConItem { get { return _conItem; } set { _conItem = value; } }


		/// <summary>
		/// 构造函数
		/// </summary>
		public MysqlTool(ConItem item)
        {
            _conItem = item;
        }

        /// <summary>
        /// 获取client
        /// </summary>
        /// <returns></returns>
        private SqlSugarScope GetClient()
        {
            SqlSugarScope db = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = GetStr(),
                //连接符字串           
                DbType = SqlSugar.DbType.MySql,
                //数据库类型        
                IsAutoCloseConnection = true //不设成true要手动close     
            });

            return db;
        }

        /// <summary>
        /// 获取表的数据行
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<CloumnInfo> GetCloumns(string TableName,string DbName)
        {
            string sql = @$"
                SELECT 
                COLUMN_NAME ,
                COLUMN_COMMENT ,
                COLUMN_TYPE               
                FROM
                INFORMATION_SCHEMA.COLUMNS a
                INNER JOIN INFORMATION_SCHEMA.TABLES b ON a.table_name = b.table_name
                and  a.table_schema = b.table_schema 
                WHERE
                a.table_schema = '{DbName}' and a.table_name='{TableName}'    
                ORDER BY ordinal_position
                ";

            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<CloumnInfo> Cloumns = new List<CloumnInfo>();

            if (dt == null || dt.Rows.Count == 0) return Cloumns;
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                Cloumns.Add(new CloumnInfo()
                {
                    CloumnID = i,
                    CloumnName = row["COLUMN_NAME"].ToString(),
                    TypeName = row["COLUMN_TYPE"].ToString(),
                    Disctrible = row["COLUMN_COMMENT"].ToString()
                });
                i = i + 1;
            }
            return Cloumns;
        }
        /// <summary>
        /// 获取所有的数据库
        /// </summary>
        /// <returns></returns>
        public List<NodeData> GetDataBases()
        {
            string sql = "SHOW databases";
            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<NodeData> dbs = new List<NodeData>();

            if (dt == null || dt.Rows.Count == 0) return dbs;

            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                dbs.Add(new NodeData()
                {
                    id = i,
                    label = row["Database"].ToString(),
                    DbName= row["Database"].ToString()
				});
                i=i+1;
            }

            return dbs.OrderBy(r=>r.label).ToList();
        }

        /// <summary>
        /// 获取数据库的表
        /// </summary>
        /// <param name="DataBasesName"></param>
        /// <returns></returns>
        public List<NodeData> GetTables(string DataBasesName)
        {
            string sql = $"SELECT table_name,TABLE_COMMENT   FROM information_schema.TABLES  where  table_schema =   '{DataBasesName}'";

            if (!string.IsNullOrWhiteSpace(_conItem.TableSearch))
                sql = sql + $" and table_name like '%{_conItem.TableSearch}%'";


            if (!string.IsNullOrWhiteSpace(_conItem.TableName))
            {
                var arr = _conItem.TableName.Split(',');
                for (int ki = 0; ki < arr.Length; ki++)
                {
                    arr[ki] = "'" + arr[ki].Trim() + "'";
                }
                sql = sql + $" and table_name in ({string.Join(",", arr)})";
            }


            if (_conItem.CreateDateArr != null && _conItem.CreateDateArr.Length == 2)
                sql = sql + $"  and ( create_time between '{_conItem.CreateDateArr[0]}' and  '{_conItem.CreateDateArr[1]}'  or   update_time between '{_conItem.CreateDateArr[0]}' and  '{_conItem.CreateDateArr[1]}' )";



            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<NodeData> tables = new List<NodeData>();

            if (dt == null || dt.Rows.Count == 0) return tables;
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                tables.Add(new NodeData()
                {
                    DbName= DataBasesName,
                    id = i,
                    label = row["table_name"].ToString(),//表名
                    discrible = row["table_comment"]?.ToString()	//表的说明			
				});
                i = i + 1;
            }
            return tables.OrderBy(r=>r.label).ToList();
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public string  GetStr()
        {
            StringBuilder conn = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_conItem.DbName))
            {
                int Index = _conItem.ConnctionStr.IndexOf("catalog");
                conn.Append(_conItem.ConnctionStr.Substring(0, Index));
                conn.Append($"catalog={_conItem.DbName};");
                var tempstr = _conItem.ConnctionStr.Substring(Index);
                Index = tempstr.IndexOf(";");
                conn.Append(tempstr.Substring(Index));
            }
            else conn.Append(_conItem.ConnctionStr);

            return conn.ToString();
        }
        
        /// <summary>
        /// 选择的table，生成model
        /// </summary>
        public void GenerateEnty()
        {
            List<string> tables = new List<string>();

            //搜索单个表
            if (!string.IsNullOrWhiteSpace(_conItem.TableSearch) || (_conItem.CreateDateArr != null && _conItem.CreateDateArr.Length == 2))
            {
                var tableList = GetTables(_conItem.DbName);
                if (tableList != null && tableList.Count() > 0)
                    tables = tableList.Select(t => t.label).ToList();
            }
            //搜索多个表
            else if (!string.IsNullOrWhiteSpace(_conItem.TableName))
            {
                foreach (var item in _conItem.TableName.Split(",").ToList())
                {
                    if (!string.IsNullOrWhiteSpace(item) && !tables.Contains(item.Trim()))
                        tables.Add(item.Trim());
                }
            }

            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = GetStr(),//连接符字串
                DbType = SqlSugar.DbType.MySql,//数据库类型
                IsAutoCloseConnection = true //不设成true要手动close
            });
            //生成指定的model
            if (tables.Count > 0)
                db.DbFirst.Where(r => tables.Contains(r)).IsCreateAttribute().IsCreateDefaultValue().CreateClassFile(_conItem.FilePath, _conItem.NameSpace);
            //生成数据库所有的model
            else
                db.DbFirst.IsCreateAttribute().IsCreateDefaultValue().CreateClassFile(_conItem.FilePath, _conItem.NameSpace);
        }

        /// <summary>
        /// 建表语句，保存成.sql文件
        /// </summary>
        /// <param name="TableName"></param>
        /// <exception cref="Exception"></exception>
        public void GenerateSqlFile( string TableName)
        {
            string sql = $"show create table  {TableName} ;";

            DataTable dt = GetClient().Ado.GetDataTable(sql);
            if (dt == null || dt.Rows.Count == 0) throw new Exception("没有找到建表语句");

            string tableSql= dt.Rows[0]["Create Table"].ToString();

            //写入sql文档
            string path = @$"{_conItem.FilePath}\{TableName}.sql";
            if (File.Exists(path))
                File.Delete(path);


            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(tableSql);
            }
        }

        /// <summary>
        /// 获取索引信息
        /// </summary>
        /// <returns></returns>
        public List<IndexModel> GetIndex ()
        {
          var version=  GetClient().Ado.GetDataTable("select  @@version as version").Rows[0]["version"].ToString();
            var INNODB_INDEXES = "INNODB_INDEXES";
            var INNODB_TABLES = "INNODB_TABLES";

            if (Convert.ToInt32( version.Split(".")[0])<8)
            {
                INNODB_INDEXES = "INNODB_SYS_INDEXES";
                INNODB_TABLES = "INNODB_SYS_TABLES";
            }

            string sql = $@"
                        SELECT a.name,a.N_FIELDS,page_no,type,b.SPACE,a.INDEX_ID
                        FROM information_schema.{INNODB_INDEXES} a
                        inner  join  information_schema.{INNODB_TABLES} b on a.TABLE_ID=b.TABLE_ID
                        WHERE b.name = '{_conItem.DbName}/{_conItem.TableName}';";

            List<IndexModel> indexModels= new List<IndexModel>();

            DataTable dt = GetClient().Ado.GetDataTable(sql);
            if (dt == null || dt.Rows.Count == 0) return indexModels;


            foreach (DataRow row in dt.Rows)
            {
                indexModels.Add(new IndexModel()
                {
                    name = row["name"].ToString(),
                    fields = row["N_FIELDS"].ToString(),
                    pageno = row["page_no"].ToString(),
                    type = row["type"].ToString(),
                    space = row["SPACE"].ToString(),
                    IndexID = row["INDEX_ID"].ToString()
                    
                });
            }
            return indexModels;
        }

        /// <summary>
        /// 生成MD文件
        /// </summary>
		public void GenerateMDFile( )
		{
			List<NodeData> tables = new List<NodeData>();

			var tableList = GetTables(_conItem.DbName);
            if (tableList != null && tableList.Count() > 0)

                if (!string.IsNullOrWhiteSpace(_conItem.TableName))
                {
                    foreach (var item in _conItem.TableName.Split(",").ToList())
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                            tables.AddRange(tableList.Where(r=>r.label.Contains(item.Trim())).ToArray());
                    }
                }
                else
                {
                    tables = tableList.ToList();
                }

			string name = $"{_conItem.DbName}{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.md";
			string filePath = Path.Combine(_conItem.FilePath, name);
			if (tables is null || tables.Count == 0)
				return;


			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			StringBuilder sb = new StringBuilder();


			foreach (var item in tables)
			{
				sb.AppendLine($"#### {item.label}  {item.discrible??""}");

				List<CloumnInfo> cloumns = GetCloumns(item.label, _conItem.DbName);

				sb.AppendLine($"| 字段名      | 字段类型    | 字段说明                  |");
				sb.AppendLine($"| ----------- | ----------- | ------------------------- |");
				foreach (var cloumn in cloumns)
				{
					sb.AppendLine($"| {cloumn.CloumnName}      | {cloumn.TypeName}    | {cloumn.Disctrible}                  |");
				}

				sb.AppendLine("");
			}

			File.AppendAllText(filePath, sb.ToString());
		}

        /// <summary>
        /// 把MD文件转换成sql与
        /// </summary>
        /// <exception cref="Exception"></exception>
		public void GenerateMDSql()
		{
            if(!File.Exists( _conItem.MDFilePath))
            {
                throw new Exception("MD文件不存在");
            }

			string[] Lines = File.ReadAllLines(_conItem.MDFilePath);

            StringBuilder sb=new StringBuilder ();

            int startLine = 0;
            int endLine = 0;
            int currentLine = 0;
            bool isAddTable = false;
            foreach (string line in Lines)
            {
                if(line.Trim().StartsWith("####"))
                {
                    if (startLine == 0 && !isAddTable)
                    {
                        startLine = currentLine;
                        isAddTable= true;
                    }
                    else
                    {
                        endLine = currentLine - 1;
                        sb.Append(DetailTable(Lines, startLine, endLine));
                        startLine = currentLine;
                    }
				}
                currentLine = currentLine + 1;
			}

			//处理最后一张表
			sb.Append(DetailTable(Lines, endLine + 1, Lines.Length-1));


			string name = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.sql";
			string filePath = Path.Combine(_conItem.FilePath, name);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}

			File.AppendAllText(filePath, sb.ToString());
		}

        /// <summary>
        /// 处理一张表的数据
        /// </summary>
        /// <param name="columns">数据源</param>
        /// <param name="startLine">开始行</param>
        /// <param name="endLine">结束行</param>
        /// <returns></returns>
        private string DetailTable(string[] columns,int startLine,int  endLine)
        {
            int titleLine = 0;
            for (int i = startLine; i <= endLine; i++)
            {
                if (columns[i].Trim().StartsWith("####"))
                {
                    titleLine = i;
                    continue;
                }
            }
          
			string title = columns[titleLine];
			title= title.Replace("####","");

            string TableName = string.Empty;
            string Discrible= string.Empty;
            StringBuilder sb=new StringBuilder();

            foreach (var item in title.Split(" "))
            {
                if(string.IsNullOrWhiteSpace( item.Trim()))
                { continue; }

                if (string.IsNullOrEmpty(TableName)) TableName = item.Trim();
                else Discrible= item.Trim();
			}

			sb.Append(@$"



                        CREATE TABLE `{TableName.Trim()}` (
                       ");

            string IdentityColumn = string.Empty;
			for (int i = titleLine + 1; i <= endLine; i++)
            {
                if (!columns[i].Trim().StartsWith("|") || columns[i].Trim().Contains("--") || columns[i].Trim().Contains("字段类型"))
                    continue;

                string ColumnName = string.Empty;
				string ColumnType = string.Empty;
				string ColumnDis = string.Empty;

                int index = 0;

                foreach (var dis in columns[i].Split("|").Where(r => !string.IsNullOrWhiteSpace(r.Trim())))
                {
                    if (index > 2) continue;
                    if (index == 0) {
                        ColumnName = dis.Trim(); 
                        if(string.IsNullOrWhiteSpace(IdentityColumn)) IdentityColumn= dis.Trim();

					}
					if (index == 1) ColumnType = dis.Trim();
					if (index == 2) ColumnDis = dis.Trim();
                    index = index + 1;
				}

				sb.Append(@$"
                              `{ColumnName.Trim()}` {ColumnType.Trim()}  COMMENT '{ColumnDis.Trim()}',
                       ");
			}

			sb.Append(@$"
                              PRIMARY KEY (`{IdentityColumn.Trim()}`)
                            )ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '{Discrible}' ROW_FORMAT = Dynamic;
                       ");
			return sb.ToString();
        }
	}
}
