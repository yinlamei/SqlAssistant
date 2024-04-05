using SqlAssistant.implement;
using SqlAssistant.InterFace;
using SqlAssistant.Option;
using System.Collections.Generic;

namespace SqlAssistant.API
{
    public class SqlManage
    {
        public ConItem _conItem=null;

        IEnumerable<IsqlTool> _clients;


		/// <summary>
		/// 构造函数
		/// </summary>
		public SqlManage(ConItem item,IEnumerable<IsqlTool> clients) {
            _conItem = item;
            _clients = clients;
            if (_clients is null || _clients.Count() == 0) throw new Exception("IsqlTool 没有实现");
        }

        public IsqlTool GetClient(SqlQuery query)
        {
			IsqlTool? client = null;
			if (_conItem.Type == 2)// sqlserver
				client = _clients.Where(r => r.GetType() == typeof(SqlServerTool))?.First();
			if (_conItem.Type == 1)// mysql
				client = _clients.Where(r => r.GetType() == typeof(MysqlTool))?.First();
            client.ConItem = query;
            return client;
		}

		public List<NodeData> GetDataBaseInfo(SqlQuery query) {

            _conItem = query;

            IsqlTool? client = GetClient(query);


			if (client is null ) throw new Exception("IsqlTool 找不到对应server的类型");

            List < NodeData >? db= client.GetDataBases();

            if (!string.IsNullOrWhiteSpace(_conItem.DbName) && db != null && db.Count > 0)
                db = db.Where(r => r.label.StartsWith(_conItem.DbName.Trim()))?.ToList();

            if (db is null) return new List<NodeData>();


            foreach (var item in db)
            {
                List<NodeData>? tables=client.GetTables(item.label)?.ToList();

                if (!string.IsNullOrWhiteSpace(_conItem.TableName) && tables != null && tables.Count > 0)
                    tables = tables.Where(r => _conItem.TableName.Trim().Contains(r.label))?.ToList();

                item.children = tables is null?null: tables.ToArray();
            }

            return db;

        }

        public List<CloumnInfo> GetCloumns(SqlQuery query)
        {

			IsqlTool? client = GetClient(query);

			List<CloumnInfo> data = new List<CloumnInfo>();
            data= client.GetCloumns(query.TableName, query.DbName);
            return data;
        }

        public string GenerateEnty(SqlQuery query)
        {

            _conItem = query;
            IsqlTool ? client = GetClient(query);

			client.GenerateEnty();

            return "success";
        }

        /// <summary>
        /// 生成sql文件
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string GenerateSqlFile(SqlQuery query)
        {
			_conItem = query;
			IsqlTool? client = GetClient(query);

			try
            {
              var tables=  client.GetTables(_conItem.DbName);
              if(tables!=null && tables.Count>0)
                {
                    foreach (var table in tables)
                    {
                        Task.Run(()=> { client.GenerateSqlFile(table.label); });
                    }
                }              
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


		/// <summary>
		/// 生成sql文件
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		public string GenerateMDFile(SqlQuery query)
		{
			_conItem = query;
			IsqlTool? client = GetClient(query);

			try
			{
				
					client.GenerateMDFile();
				return "success";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

		}


		public string GenerateMDsql(SqlQuery query)
		{

			_conItem = query;
			IsqlTool? client = GetClient(query);

			try
			{

				client.GenerateMDSql();
				return "success";
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

		}
	}
}
