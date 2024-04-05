using Microsoft.VisualBasic.FileIO;
using SqlAssistant.implement;
using SqlAssistant.Option;
using SqlAssistant.Option.page;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace SqlAssistant.API
{

    public class ReadIdb
    {

        private MysqlTool _clinent = null;
        public ConItem _conItem = null;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="clinent"></param>
        public ReadIdb(ConItem item, MysqlTool clinent) {
            _clinent = clinent;
            _conItem = item;
        }

        /// <summary>
        /// 获取索引信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<IndexModel> GetIndex(SqlQuery query)
        {

            _conItem.Type = query.Type;
            _conItem.DbName = query.DbName;
            _conItem.TableName = query.TableName;


            List<IndexModel> data = new List<IndexModel>();
            data = _clinent.GetIndex();
            return data;
        }

        public const int PageSize = 16384;

        public dynamic ReadFile( PageQuery query )
        {
            
            string filePath = @"C:\Users\fineex.com\Desktop\tc_category.ibd";

            if (!string.IsNullOrWhiteSpace(query.idbPath))
                filePath = query.idbPath;

            if (! System.IO.File.Exists(filePath))
                return null;

            byte[] data = System.IO.File.ReadAllBytes(filePath);//以byte格式获取文件数据
            int page = query.PageNO;
            int start = page * PageSize;

            #region file header
            List<headItem> FileHeader = new List<headItem>();
            FileHeader.Add(new headItem() {
                 Name= "FIL_PAGE_SPACE_OR_CHKSUM",
                 Value= ConvertString(data.Skip(start).Take(4).ToArray()),
                 Order=1,
                 Len=4,
                 Dis= "页的校验和（checksum值） 字节"
                });
                start = start + 4;

            string pageNo = BitConverter.ToInt32(data.Skip(start).Take(4).Reverse().ToArray()).ToString();
            FileHeader.Add(new headItem()
                {
                    Name = "FIL_PAGE_OFFSET",
                    Value = pageNo+"(10进制)",
                    Order = 2,
                    Len = 4,
                    Dis = "页号 4字节"
                });
                start = start + 4;

            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_PREV",
                Value = ConvertString(data.Skip(start).Take(4).ToArray()),
                Order = 3,
                Len = 4,
                Dis = "上一个页的页号 4字节"
            });
            start = start + 4;

            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_NEXT",
                Value = ConvertString(data.Skip(start).Take(4).ToArray()),
                Order = 4,
                Len = 4,
                Dis = "下一个页的页号 4字节"
            });
            start = start + 4;

            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_LSN",
                Value = ConvertString(data.Skip(start).Take(8).ToArray()),
                Order = 5,
                Len =8,
                Dis = "页面被最后修改时对应的日志序列位置（英文名是：Log Sequence Number）8字节"
            });
            start = start + 8;

            

            string FIL_PAGE_TYPE = ConvertString(data.Skip(start).Take(2).ToArray());
            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_TYPE",
                Value = FIL_PAGE_TYPE,
                Order = 6,
                Len = 2,
                Dis = " 该页的类型 2字节,"+ GetFileTypeDis(FIL_PAGE_TYPE)
            });
            start = start + 2;


            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_FILE_FLUSH_LSN",
                Value = ConvertString(data.Skip(start).Take(8).ToArray()),
                Order = 7,
                Len = 8,
                Dis = " 仅在系统表空间的一个页中定义，代表文件至少被刷新到了对应的LSN值 8字节"
            });
            start = start + 8;

            var SPACE = data.Skip(start).Take(4).ToArray();           
            if (BitConverter.IsLittleEndian)
                Array.Reverse(SPACE);
            var SPACEid = BitConverter.ToInt32(SPACE);
            FileHeader.Add(new headItem()
            {
                Name = "FIL_PAGE_ARCH_LOG_NO_OR_SPACE_ID",
                Value = SPACEid.ToString()+("(此值是int类型和上表对应)"),
                Order = 8,
                Len = 4,
                Dis = " 页属于哪个表空间4字节"
            });
            start = start + 4;
            #endregion

            #region page header
            List<headItem> PageHeader = new List<headItem>();
            int slot = BitConverter.ToInt16(data.Skip(start).Take(2).Reverse().ToArray());
            if (FIL_PAGE_TYPE.Trim().ToLower().Equals("45 bf"))//只实现了数据页的读取
            {
                //=====================page head
               

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_N_DIR_SLOTS",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 1,
                    Len = 2,
                    Dis = " 在页目录中的槽数量 2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_HEAP_TOP",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 2,
                    Len = 2,
                    Dis = " 还未使用的空间最小地址，也就是说从该地址之后就是Free Space  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_N_HEAP",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 3,
                    Len = 2,
                    Dis = "本页中的记录的数量（包括最小和最大记录以及标记为删除的记录）  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_FREE",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 4,
                    Len = 2,
                    Dis = "第一个已经标记为删除的记录地址（各个已删除的记录通过next_record也会组成一个单链表，这个单链表中的记录可以被重新利用）  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_GARBAGE",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 5,
                    Len = 2,
                    Dis = "已删除记录占用的字节数  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_LAST_INSERT",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 6,
                    Len = 2,
                    Dis = "最后插入记录的位置  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_DIRECTION",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 7,
                    Len = 2,
                    Dis = "最后一条记录插入的方向  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_N_DIRECTION",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 8,
                    Len = 2,
                    Dis = "一个方向连续插入的记录数量，如果最后一条记录的插入方向改变了的话，这个状态的值会被清零重新统计。 2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_N_RECS",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 9,
                    Len = 2,
                    Dis = "该页中记录的数量（不包括最小和最大记录以及被标记为删除的记录）  2字节"
                });
                start = start + 2;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_MAX_TRX_ID",
                    Value = ConvertString(data.Skip(start).Take(8).ToArray()),
                    Order = 10,
                    Len = 8,
                    Dis = "修改当前页的最大事务ID，该值仅在二级索引中定义  2字节"
                });
                start = start + 8;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_LEVEL",
                    Value = ConvertString(data.Skip(start).Take(2).ToArray()),
                    Order = 11,
                    Len = 2,
                    Dis = "当前页在B+树中所处的层级  2字节"
                });
                start = start + 2;


                var indexData = data.Skip(start).Take(8).ToArray();
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(indexData);
                var indexid = BitConverter.ToInt64(indexData);
                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_INDEX_ID",
                    Value = indexid.ToString() + "(int 类型 ，和上表一致)",
                    Order = 12,
                    Len = 8,
                    Dis = "索引ID，表示当前页属于哪个索引"
                });
                start = start + 8;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_BTR_SEG_LEAF",
                    Value = ConvertString(data.Skip(start).Take(10).ToArray()),
                    Order = 13,
                    Len = 10,
                    Dis = "B+树叶子段的头部信息，仅在B+树的Root页定义 "
                });
                start = start + 10;

                PageHeader.Add(new headItem()
                {
                    Name = "PAGE_BTR_SEG_TOP",
                    Value = ConvertString(data.Skip(start).Take(10).ToArray()),
                    Order = 14,
                    Len = 10,
                    Dis = "B+树非叶子段的头部信息，仅在B+树的Root页定义   "
                });
                start = start + 10;
            }
            #endregion page header

                #region file trailer,放在file header里面
            FileHeader.Add(new headItem()
            {
                Name = "file trailer校验和",
                Value = ConvertString(data.Skip((query.PageNO+1)*PageSize-8).Take(4).ToArray()),
                Order = 9,
                Len = 4,
                Dis = "File Header和File Trailer都有校验和，如果两者一致则表示数据页是完整的"
            });
            FileHeader.Add(new headItem()
            {
                Name = "file trailer （LSN）",
                Value = ConvertString(data.Skip((query.PageNO + 1) * PageSize - 4).Take(4).ToArray()),
                Order = 10,
                Len = 4,
                Dis = "代表页面被最后修改时对应的日志序列位置（LSN）"
            });
            #endregion

            
            if (FIL_PAGE_TYPE.Trim().ToLower().Equals("45 bf") || FIL_PAGE_TYPE.Trim().ToLower().Equals("45 bd") )//只实现了数据页的读取
            {
                var InfimumSupremum = ReadInfimumSupremumRow(start, data);
                PageHeader.AddRange(InfimumSupremum.Item2);
                var rows = GetRows(InfimumSupremum.Item3, data, InfimumSupremum.Item4);

                var PageDirectory = new List<headItem>();
                if (FIL_PAGE_TYPE.Trim().ToLower().Equals("45 bf"))
                 PageDirectory = GetPageDirectory(slot, data, query.PageNO);


                return new { FileHeader = FileHeader, PageHeader = PageHeader, Rows = rows, PageDirectory = PageDirectory };
            }


            return new { FileHeader= FileHeader, PageHeader= PageHeader , Rows= new List<headItem>(), PageDirectory= new List<headItem>() };
        }


        string RowType = "数据行0 索引行1  Infimum行2  Supremum行3";
        /// <summary>
        /// 获取InfimumSupremum
        /// </summary>
        /// <param name="start"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private (int, List<headItem>,int,int  ) ReadInfimumSupremumRow(int start, byte[] data)
        {
            List<headItem> InfimumSupremum = new List<headItem>();

            start = start + 2;
            var rowType = data.Skip(start).Take(1).First() & 7;
            start = start + 1;
            var NextRow= BitConverter.ToInt16( data.Skip(start).Take(2).Reverse().ToArray());
            start = start + 2;

            int NextRowStart = start;
            int Offset = NextRow;
            InfimumSupremum.Add(new headItem()
            {
                Name = "Infimum",
                Value =$"RowType={rowType},NextRow={NextRow},start={start}",
                Order = 15,
                Len = 13,
                Dis = $"RowType:{RowType}"
            });
            start = start + 8;


            start = start + 2;
            var SupremumrowType = data.Skip(start).Take(1).First() & 7;
            start = start + 1;
            var SupremumNextRow = BitConverter.ToInt16(data.Skip(start).Take(2).Reverse().ToArray());
            start = start + 2;
            InfimumSupremum.Add(new headItem()
            {
                Name = "Supremum",
                Value = $"RowType={SupremumrowType},NextRow={SupremumNextRow},start={start}",
                Order = 16,
                Len = 13,
                Dis = $"RowType:{RowType}"
            });
            start = start + 8;


            return (start, InfimumSupremum, NextRowStart, Offset);
        }
        private string ConvertString(Byte[] bts)
        {
            StringBuilder sb=new StringBuilder ();
            for (int i = 0; i < bts.Length; i++)
            {
                var item = Convert.ToString(bts[i], 16);
                if (item.Length < 2)
                    item = "0" + item;
                sb.Append(item);
                sb.Append(' ');
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取页面数据行
        /// </summary>
        /// <param name="start"></param>
        /// <param name="data"></param>
        /// <param name="Offset"></param>
        /// <returns></returns>
        private List<headItem> GetRows(int start, byte[] data,int Offset)
        {
            var cc =ConvertString( data.Skip(start).Take(Offset).ToArray());
            //变长字段长度和null（1字节）所占的字节
            int VariableLen = Offset - 8 - 13; //'Infimum'+Supremum长度

            List<headItem> rows=new List<headItem>();

            if (VariableLen < 0) return rows;

            var NextRow = BitConverter.ToInt16(data.Skip(start+Offset-2).Take(2).Reverse().ToArray());
            var rowType = data.Skip(start + Offset - 3).Take(1).First() & 7;

            int i = 1;
            int len = 0;
            if (NextRow < 0) len = Offset;
            //找到这一页的所有数据行
            while ((rowType == 0 || rowType == 1))
            {
                string value= string.Empty;
                string Name = string.Empty;
                

                if(rowType==0)
                {
                    Name = "数据行";
                    if (NextRow > 0) len = NextRow- VariableLen; //最后一行的下一行地址，指向Supremum行
                    value = ConvertString(data.Skip(start + Offset).Take(len).ToArray());
                }
                else
                {
                    Name = "索引行";
                    if(NextRow>0) len = NextRow - VariableLen; //最后一行的下一行地址，指向Supremum行
                    value = ConvertString(data.Skip(start + Offset).Take(len).ToArray());
                }
                rows.Add(new headItem()
                {
                    Name = Name,
                    Value = value,
                    Order = i,
                    Len = len,
                    Dis = $"RowType={rowType},NextRow={NextRow},start={start}"
                });
                 start = start + Offset;
                 Offset = NextRow;
                 NextRow = BitConverter.ToInt16(data.Skip(start + Offset - 2).Take(2).Reverse().ToArray());
                 rowType = data.Skip(start + Offset - 3).Take(1).First() & 7;
                i=i+1;
            }

            return rows;
        }

        /// <summary>
        /// PageDirectory 页目录
        /// </summary>
        /// <param name="Slot">slot的数量</param>
        /// <param name="data"></param>
        /// <param name="PageNo">页</param>
        /// <returns></returns>
        private List<headItem> GetPageDirectory(int Slot, byte[] data,int PageNo)
        {
            List<headItem> PageDirectory=new List<headItem>();

            if (Slot==0 || Slot== 65535) return PageDirectory;

            int bytes = Slot * 2;
            int begin = (PageNo + 1) * PageSize - 8;
            for (int i = 1; i <= Slot; i++)
            {
                string value = ConvertString(data.Skip(begin - i * 2).Take(2).ToArray());
                int pageOffeet = BitConverter.ToInt16(data.Skip(begin - i * 2).Take(2).Reverse().ToArray());
                string keys= ConvertString(data.Skip(PageNo*PageSize+pageOffeet).Take(10).ToArray());
                PageDirectory.Add(new headItem()
                {
                    Name = $"slot{i}",
                    Value = value,
                    Order = i,
                    Len = 2,
                    Dis = $"keys={keys}"
                });
            }
            return PageDirectory;
        }
   
        private string GetFileTypeDis(string FileType)
        {
            string FileTypeDis =string.Empty;
            switch(FileType.Trim().ToLower())
            {
                case "00 02": FileTypeDis = "FIL_PAGE_UNDO_LOG(Undo日志页)                  "; break;
                case "00 03": FileTypeDis = "FIL_PAGE_INODE(段信息节点)                     "; break;
                case "00 04": FileTypeDis = "FIL_PAGE_IBUF_FREE_LIST(Insert Buffer空闲列表) "; break;
                case "00 05": FileTypeDis = "FIL_PAGE_IBUF_BITMAP(Insert Buffer位图)        "; break;
                case "00 06": FileTypeDis = "FIL_PAGE_TYPE_SYS(系统页)                      "; break;
                case "00 07": FileTypeDis = "FIL_PAGE_TYPE_TRX_SYS(事务系统页)              "; break;
                case "00 08": FileTypeDis = "FIL_PAGE_TYPE_FSP_HDR(表空间头部信息)          "; break;
                case "00 09": FileTypeDis = "FIL_PAGE_TYPE_XDES(拓展描述页)                 "; break;
                case "00 0a": FileTypeDis = "FIL_PAGE_TYPE_BLOB(溢出页)                     "; break;
                case "45 bf": FileTypeDis = "FIL_PAGE_INDEX(索引页，即数据页)               "; break;
                case "45 bd": FileTypeDis = "FIL_PAGE_SDI    表结构的元数据信息             "; break;
            }

            return FileTypeDis;
        }
    
    }
}
