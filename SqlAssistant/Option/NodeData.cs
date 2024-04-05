namespace SqlAssistant.Option
{
    /// <summary>
    /// 树形结构的数据
    /// </summary>
    public class NodeData
    {
        /// <summary>
        /// id
        /// </summary>
      public int   id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
      public string  label { get; set; }

	  public string discrible { get; set; }

		/// <summary>
		/// 数据库名称
		/// </summary>
		public string DbName { get; set; }
		/// <summary>
		/// 子
		/// </summary>
		public NodeData[]? children { get; set; }
    }



}
