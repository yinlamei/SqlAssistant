using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAssistant.Option
{
	public class DbConfig
	{
		public Host[] Hosts { get; set; }
	}

	public class Host
	{
		public string IP { get; set; }

		public string Sa { get; set; }

		public string Passaord { get; set; }

		public string  ShowText{ get { return $"IP:【{IP}】,账号：【{Sa}】，密码：【{Passaord}】"; } }
		public int Type { get; set; }
	}

}
