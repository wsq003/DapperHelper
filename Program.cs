using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DapperHelper
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new DapperHelper());
			}

			if (args.Length > 0)
			{
				string connStr = args[0];
				string destPath = args[1];
				string namespace_ = args[2];

				SimpleCRUD.Generate(connStr, destPath, namespace_);
			}
		}
	}
}
