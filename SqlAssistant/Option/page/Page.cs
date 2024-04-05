namespace SqlAssistant.Option.page
{
    public class headItem
    {
        /// <summary>
        /// 名称
        /// </summary>
         public string Name { get; set; }
        /// <summary>
        /// 值
        /// </summary>
         public string  Value { get; set; }
        /// <summary>
        /// 长度
        /// </summary>
         public int Len  {get; set; }
        /// <summary>
        /// 排序
        /// </summary>
         public int Order { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Dis { get; set; }
    }
    public class PageHead
    {
        /// <summary>
        ///  页的校验和（checksum值） 4
        /// </summary>
        public int FIL_PAGE_SPACE_OR_CHKSUM { get; set; }

        /// <summary>
        /// 页号 4
        /// </summary>
        public int FIL_PAGE_OFFSET { get; set; }

        /// <summary>
        ///  上一个页的页号 4
        /// </summary>
        public int FIL_PAGE_PREV { get; set; }
        /// <summary>
        /// 下一个页的页号
        /// </summary>
        public int FIL_PAGE_NEXT { get; set; }
        /// <summary>
        /// 页面被最后修改时对应的日志序列位置（英文名是：Log Sequence Number）8
        /// </summary>
        public int FIL_PAGE_LSN { get; set; }
        /// <summary>
        /// 该页的类型 2
        /// </summary>
        public int FIL_PAGE_TYPE { get; set; }
        /// <summary>
        /// 仅在系统表空间的一个页中定义，代表文件至少被刷新到了对应的LSN值 8
        /// </summary>
        public int FIL_PAGE_FILE_FLUSH_LSN { get; set; }

        /// <summary>
        /// 页属于哪个表空间4
        /// </summary>
        public int FIL_PAGE_ARCH_LOG_NO_OR_SPACE_ID { get; set; }

    }
}
