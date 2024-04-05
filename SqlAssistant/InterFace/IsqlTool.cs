using SqlAssistant.Option;

namespace SqlAssistant.InterFace
{
    public interface IsqlTool
    {
		public ConItem ConItem { get; set; }
		/// <summary>
		/// 获取服务器上所有的数据库
		/// </summary>
		/// <returns></returns>
		public List<NodeData> GetDataBases();

        /// <summary>
        /// 获取数据库所有的表
        /// </summary>
        /// <param name="DataBasesName"></param>
        /// <returns></returns>
        public List<NodeData> GetTables( string DataBasesName);

        /// <summary>
        /// 获取表所有的字段、类型、说明
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<CloumnInfo> GetCloumns(string TableName, string DbName);

        /// <summary>
        /// 生成model
        /// </summary>
        public void GenerateEnty();

        /// <summary>
        /// 建表语句
        /// </summary>
        /// <param name="TableName"></param>
        public void GenerateSqlFile(string TableName);


        /// <summary>
        /// 生成 MD文件
        /// </summary>
        /// <param name="TableName"></param>
		public void GenerateMDFile();


        /// <summary>
        /// 把MD文件生成sql脚本
        /// </summary>
		public void GenerateMDSql();
		

	}
}
