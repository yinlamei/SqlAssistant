namespace SqlAssistant.Option
{
    /// <summary>
    /// 查询的字符串
    /// </summary>
    public class SqlQuery: ConItem
    {

    }
    /// <summary>
    /// 查询的字符串
    /// </summary>
    public class ConItem
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnctionStr
        {
            get
            {
                if (Type == 1)
                    return $"data source={IP};port=3306;initial catalog={DbName};uid={Sa};pwd={Passaord};";
                else if (Type == 2)
                    return $"data source={IP};initial catalog={DbName};persist security info=True;user id={Sa};password={Passaord};MultipleActiveResultSets=True;";
                else return string.Empty;
            }
        }


        public string IP { get; set; }

		public string Sa { get; set; }

		public string Passaord { get; set; }
		/// <summary> 
		/// 类型：1 mysql  2sql server 
		/// </summary>
		public int Type { get; set; }

        /// <summary>
        /// 需要查询的table的名字
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 搜索表名称
        /// </summary>
        public string TableSearch { get; set; }     
        
        public string[] CreateDateArr { get; set; }


		/// <summary>
		/// 搜索表名称
		/// </summary>
		public string MDFilePath { get; set; }
	}
}
