using MySqlConnector;
using SqlSugar;
using SqlAssistant.InterFace;
using SqlAssistant.Option;
using System.Data;
using System.Text;

namespace SqlAssistant.implement
{
    public class SqlServerTool : IsqlTool
    {
        public ConItem _conItem = null;

        public ConItem ConItem { get { return _conItem; } set{ _conItem = value; } }

		/// <summary>
		/// 构造函数
		/// </summary>
		public SqlServerTool(ConItem item)
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
                DbType = SqlSugar.DbType.SqlServer,
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
        public List<CloumnInfo> GetCloumns(string TableName, string DbName)
        {
            string sql = @$"
                SELECT a.column_id,  
                a.name AS CloumnName,
				a.max_length,
				a.precision,
				a.scale,
                types.name AS TypeName ,
                value AS Disctrible
                FROM  [sys].[all_columns] a
                INNER JOIN sys.types ON  types.user_type_id=a.user_type_id
                LEFT   JOIN  [sys].[extended_properties]
                ON extended_properties.minor_id = a.column_id AND  major_id=a.object_id
                WHERE a.object_id=OBJECT_ID('{TableName}')
                ORDER BY a.column_id 
                ";

            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<CloumnInfo> Cloumns = new List<CloumnInfo>();

            if (dt == null || dt.Rows.Count == 0) return Cloumns;

            foreach (DataRow row in dt.Rows)
            {
                string TypeName = row["TypeName"].ToString();

				string max_length = row["max_length"].ToString();
				string precision = row["precision"].ToString();
				string scale = row["scale"].ToString();

                if(TypeName.Equals("nvarchar"))
                {
                    int len = Convert.ToInt32(max_length);
                    if (len == -1)
						TypeName = $"{TypeName}(max)";
                    else 
					TypeName = $"{TypeName}({len/2})";
				}

				else if (TypeName.Equals("decimal"))
				{
					TypeName = $"{TypeName}({Convert.ToInt32(precision)},{Convert.ToInt32(scale)})";
				}

				Cloumns.Add(new CloumnInfo()
                {
                    CloumnID = Convert.ToInt32(row["column_id"].ToString()),
                    CloumnName = row["CloumnName"].ToString(),
                    TypeName = TypeName,
                    Disctrible = row["Disctrible"].ToString()
                });
            }
            return Cloumns;
        }

        public List<NodeData> GetDataBases()
        {
            string sql = "SELECT database_id,name  FROM sys.databases";
            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<NodeData> dbs= new List<NodeData>();

            if (dt == null || dt.Rows.Count == 0) return dbs;

            foreach (DataRow row in dt.Rows)
            {
                dbs.Add(new NodeData()
                {
                    id = Convert.ToInt32(  row["database_id"].ToString()),
                    label= row["name"].ToString(),
                    DbName= row["name"].ToString()
				}) ;
            }

            return dbs.OrderBy(r => r.label).ToList();
        }

        public List<NodeData> GetTables(string DataBasesName)
        {
            string sql = $"SELECT name,object_id FROM [{DataBasesName}].sys.tables  WHERE type = 'U'";


            if (!string.IsNullOrWhiteSpace(_conItem.TableSearch))
                sql = sql + $" and name like '%{_conItem.TableSearch}%'";

    
            if (!string.IsNullOrWhiteSpace(_conItem.TableName))
            {
                var arr = _conItem.TableName.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = "'" + arr[i].Trim() + "'";
                }
                sql = sql + $" and name in ({ string.Join(",",arr)})";
            }


                if (_conItem.CreateDateArr !=null && _conItem.CreateDateArr.Length==2)
                sql = sql + $"  and ( create_date between '{_conItem.CreateDateArr[0]}' and  '{_conItem.CreateDateArr[1]}'  or   modify_date between '{_conItem.CreateDateArr[0]}' and  '{_conItem.CreateDateArr[1]}' )";


                Console.WriteLine(DataBasesName);
            DataTable dt = GetClient().Ado.GetDataTable(sql);
            List<NodeData> tables = new List<NodeData>();

            if (dt == null || dt.Rows.Count == 0) return tables;

            foreach (DataRow row in dt.Rows)
            {
                tables.Add(new NodeData()
                {
                    DbName=DataBasesName,
                    id = Convert.ToInt32(row["object_id"].ToString()),
                    label = row["name"].ToString()
                });
            }
            return tables.OrderBy(r => r.label).ToList();
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <returns></returns>
        public string GetStr()
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
        public void GenerateEnty()
        {
            List<string> tables=new List<string>();

            //搜索单个表
            if (!string.IsNullOrWhiteSpace( _conItem.TableSearch) || (_conItem.CreateDateArr != null && _conItem.CreateDateArr.Length == 2))
            {
                var tableList = GetTables(_conItem.DbName);
                if (tableList!=null && tableList.Count()>0)
                    tables=tableList.Select(t => t.label).ToList();
            }
            //搜索多个表
            else  if(!string.IsNullOrWhiteSpace(_conItem.TableName))
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
                DbType = SqlSugar.DbType.SqlServer,//数据库类型
                IsAutoCloseConnection = true //不设成true要手动close
            });

            //生成指定的model
            if(tables.Count>0)
                db.DbFirst.Where(r=>tables.Contains(r)).IsCreateAttribute().IsCreateDefaultValue().CreateClassFile(_conItem.FilePath, _conItem.NameSpace);
            //生成数据库所有的model
            else
                db.DbFirst.IsCreateAttribute().IsCreateDefaultValue().CreateClassFile(_conItem.FilePath, _conItem.NameSpace);
        }

        /// <summary>
        /// 建表语句，保存成.sql文件
        /// </summary>
        /// <param name="TableName"></param>
        /// <exception cref="Exception"></exception>
        public void GenerateSqlFile(string TableName)
        {
            var speType = "text,ntext,image,xml".Split(",").ToList();
            bool has_TEXTIMAGE_ON = false;
            //获取聚集索引,检查是否有聚集所有
            string PKName = string.Empty;
            string PkStr = @$"
                SELECT  CONSTRAINT_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                WHERE TABLE_NAME = '{TableName}'
                AND CONSTRAINT_TYPE = 'PRIMARY KEY';
            ";
            DataTable pkdt = GetClient().Ado.GetDataTable(PkStr);
            if (pkdt != null && pkdt.Rows.Count > 0)
                PKName = pkdt.Rows[0]["CONSTRAINT_NAME"].ToString();


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET ANSI_NULLS ON");
            sb.AppendLine("GO");
            sb.AppendLine("SET QUOTED_IDENTIFIER ON");
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"CREATE TABLE [dbo].[{TableName}](");


            string Cloumnsql = @$"
            SELECT  a.COLUMN_NAME,a.DATA_TYPE,a.COLUMN_DEFAULT,a.IS_NULLABLE,CHARACTER_MAXIMUM_LENGTH,
            COALESCE('('+ 
            CASE WHEN CHARACTER_MAXIMUM_LENGTH=-1 THEN 'MAX' ELSE  CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR) END 
            + ')', '') AS lenDis,
            COALESCE('DEFAULT '+ COLUMN_DEFAULT, '') DefaultDis,
            NUMERIC_PRECISION,
            NUMERIC_SCALE,
            CHARACTER_MAXIMUM_LENGTH,
            CAST(IDENT_SEED('{TableName}') AS VARCHAR) AS SEED,
            CAST(IDENT_INCR('{TableName}') AS VARCHAR) AS  INCR
            FROM INFORMATION_SCHEMA.COLUMNS a
            WHERE a.TABLE_NAME = '{TableName}'
            ORDER BY ORDINAL_POSITION;
            ";
            DataTable dt = GetClient().Ado.GetDataTable(Cloumnsql);
            if (dt == null || dt.Rows.Count == 0)  throw new Exception("没有找到数据行");

            int DTi = 1;
            foreach (DataRow row in dt.Rows)
            {
                string COLUMN_NAME = row["COLUMN_NAME"].ToString();
                string nullDis = row["IS_NULLABLE"].ToString() == "NO" ? "NOT" : "";
                string Defalt = row["DefaultDis"].ToString();
                string lenDis = row["lenDis"].ToString();
                string SEED = row["SEED"].ToString();
                string INCR = row["INCR"].ToString();
                string DATA_TYPE= row["DATA_TYPE"].ToString();
                string? CHARACTER_MAXIMUM_LENGTH = row["CHARACTER_MAXIMUM_LENGTH"]?.ToString();

                //长度和经度
                if (DATA_TYPE.ToLower().Equals("decimal"))
                    DATA_TYPE = $" {DATA_TYPE}({row["NUMERIC_PRECISION"].ToString()},{row["NUMERIC_SCALE"].ToString()})";

                if (speType.Contains(DATA_TYPE.ToLower()) || (CHARACTER_MAXIMUM_LENGTH!=null && CHARACTER_MAXIMUM_LENGTH.Equals("-1")) )
                    has_TEXTIMAGE_ON = true;

                string checkIdnity = @$"
                            SELECT id
                            FROM syscolumns
                            WHERE OBJECT_NAME(id) = '{TableName}'
                            AND name ='{COLUMN_NAME}'
                            AND COLUMNPROPERTY(id,
                            name,
                            'IsIdentity') = 1 
                ";
                string IdentityDis = string.Empty;
                DataTable chdt = GetClient().Ado.GetDataTable(checkIdnity);
                if (chdt != null && chdt.Rows.Count > 0)
                    IdentityDis = $"IDENTITY({SEED},{INCR})";

                string cloumnde = $"[{COLUMN_NAME}]  {DATA_TYPE}  {lenDis}  {IdentityDis}   {nullDis} NUll   {Defalt}";

                //如果不是聚集索引，最后一行没有逗号
                if (DTi==dt.Rows.Count && string.IsNullOrWhiteSpace(PKName))
                    sb.AppendLine(cloumnde);
                else
                sb.AppendLine(cloumnde+" ,");

                DTi = DTi + 1;
            }

            //有聚集索引
            if (!string.IsNullOrWhiteSpace(PKName))
            {
                sb.AppendLine($" CONSTRAINT [{PKName}] PRIMARY KEY CLUSTERED ");
                sb.AppendLine("(");

                //获取主键列表
                string pkCloumns = $@"
                SELECT  COLUMN_NAME 
                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                WHERE CONSTRAINT_NAME = '{PKName}'
                ORDER BY ORDINAL_POSITION;
            ";

                DataTable pkCloumnsdt = GetClient().Ado.GetDataTable(pkCloumns);
                if (pkCloumnsdt == null || pkCloumnsdt.Rows.Count == 0) throw new Exception("没有找到数据行");
                int pkCloumnsdtCount = 1;
                foreach (DataRow row in pkCloumnsdt.Rows)
                {
                    string COLUMN_NAME = $"[{row["COLUMN_NAME"].ToString()}] ASC";
                    if(pkCloumnsdtCount== pkCloumnsdt.Rows.Count)
                        sb.AppendLine(COLUMN_NAME);
                    else
                         sb.AppendLine(COLUMN_NAME + " ,");

                    pkCloumnsdtCount = pkCloumnsdtCount + 1;
                }
                sb.AppendLine(")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");

            }
            string TEXTIMAGE_ON_Str = has_TEXTIMAGE_ON ? " TEXTIMAGE_ON [PRIMARY]" : "";
            sb.AppendLine(@$") ON [PRIMARY] {TEXTIMAGE_ON_Str}");
            sb.AppendLine("GO");

            sb.AppendLine();
            sb.AppendLine();


            //==============================表的注释

            var disCol = GetCloumns(TableName,_conItem.DbName);

            if (disCol != null && disCol.Where(r => !string.IsNullOrWhiteSpace(r.Disctrible)).Count() > 0)
            {
                foreach (var item in disCol.Where(r => !string.IsNullOrWhiteSpace(r.Disctrible)).ToList())
                {
                    //说明
                    string discrible = item.Disctrible;
                    //列名
                    string CloumnName = item.CloumnName;
                    sb.AppendLine($"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{discrible}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{TableName}', @level2type=N'COLUMN',@level2name=N'{CloumnName}'");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                    sb.AppendLine();
                }
            }

            //写入sql文档
            string path = @$"{_conItem.FilePath}\{TableName}.sql";
            if (File.Exists(path))
                File.Delete(path);


            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine(sb.ToString());
            }
        }

		/// <summary>
		/// 生成MD文件
		/// </summary>
		public void GenerateMDFile()
		{

			List<string> tables = new List<string>();

			var tableList = GetTables(_conItem.DbName);
			if (tableList != null && tableList.Count() > 0)

				if (!string.IsNullOrWhiteSpace(_conItem.TableName))
				{
					foreach (var item in _conItem.TableName.Split(",").ToList())
					{
						if (!string.IsNullOrWhiteSpace(item) && !tables.Contains(item.Trim()))
							tables.Add(item.Trim());
					}
				}
				else
				{
					tables = tableList.Select(t => t.label).ToList();
				}

			string name = $"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.md";
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
				sb.AppendLine($"#### {item}");

				List<CloumnInfo> cloumns = GetCloumns(item, _conItem.DbName);

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
			if (!File.Exists(_conItem.MDFilePath))
			{
				throw new Exception("MD文件不存在");
			}

			string[] Lines = File.ReadAllLines(_conItem.MDFilePath);

			StringBuilder sb = new StringBuilder();

			int startLine = 0;
			int endLine = 0;
			int currentLine = 0;
			bool isAddTable = false;
			foreach (string line in Lines)
			{
				if (line.Trim().StartsWith("####"))
				{
					if (startLine == 0 && !isAddTable)
					{
						startLine = currentLine;
						isAddTable = true;
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
			sb.Append(DetailTable(Lines, endLine + 1, Lines.Length - 1));


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
		private string DetailTable(string[] columns, int startLine, int endLine)
		{
            //注释的语句
            List<string> listDis = new List<string>();

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
			title = title.Replace("####", "");

			string TableName = string.Empty;
			string Discrible = string.Empty;
			StringBuilder sb = new StringBuilder();

			foreach (var item in title.Split(" "))
			{
				if (string.IsNullOrWhiteSpace(item.Trim()))
				{ continue; }

				if (string.IsNullOrEmpty(TableName)) TableName = item.Trim();
				else Discrible = item.Trim();
			}

			sb.Append(@$"
                 CREATE TABLE [dbo].[{TableName.Trim()}](
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
					if (index == 0)
					{
						ColumnName = dis.Trim();
						if (string.IsNullOrWhiteSpace(IdentityColumn)) IdentityColumn = dis.Trim();

					}
					if (index == 1) ColumnType = dis.Trim();
					if (index == 2) ColumnDis = dis.Trim();
					index = index + 1;
				}

				sb.Append(@$"
                                [{ColumnName.Trim()}]  {ColumnType.Trim()}    ,
                       ");
                if (!string.IsNullOrEmpty(ColumnDis.Trim())) listDis.Add(@$"
                    EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{ColumnDis.Trim()}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{TableName.Trim()}', @level2type=N'COLUMN',@level2name=N'{ColumnName.Trim()}'
                    GO
                  ");
			}

			sb.Append(@$"
                         CONSTRAINT [PK_{IdentityColumn.Trim()}] PRIMARY KEY CLUSTERED 
                        (
	                        [{IdentityColumn.Trim()}] ASC
                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                        ) ON [PRIMARY]
                        GO
                       ");

            foreach (var item in listDis)
            {
                sb.AppendLine(item);
            }
			return sb.ToString();
		}
	}
}
