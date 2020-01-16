using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DapperHelper
{
	static class Program
	{

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		public static extern bool AttachConsole(int dwProcessId);
		public const int ATTACH_PARENT_PROCESS = -1;

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
				AttachConsole(ATTACH_PARENT_PROCESS);


				if (args.Length != 4)
				{
					string info = "\r\nUsage:\r\n\tDapperHelper db_connection_string dest_folder_path code_namespace sqlType\r\n";
					Console.WriteLine(info);
					Console.Write("说明：sqlType必须是sqlserver或者mysql \r\n");
					Console.WriteLine("参数数量错误，当前数量：" + args.Length);

					return;
				}

				string connStr = args[0];
				string destPath = args[1];
				string namespace_ = args[2];
				string sqlType = args[3].ToLower();

				if (sqlType == "sqlserver")
				{
					SimpleCRUD.sqlType = SqlType.SqlServer;
				}
				else if (sqlType == "mysql")
				{
					SimpleCRUD.sqlType = SqlType.MySQL;
				}
				else
				{
					Console.Write(string.Format("未知的sqlType: {0} \r\n", sqlType));
					Console.WriteLine("");
					return;
				}

				Console.WriteLine(string.Format("connStr={0}", connStr));

				try
				{
					SimpleCRUD.Generate2(connStr, destPath, namespace_);

					Console.Write("finished");
				}
				catch (Exception ex)
				{
					Console.Write(ex.ToString());
				}
			}
		}
	}
}
