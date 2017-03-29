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
				if (args.Length != 3)
				{
					string info = "\r\nUsage:\r\n\tDapperHelper db_connection_string, dest_folder_path, code_namespace\r\n";

					AttachConsole(ATTACH_PARENT_PROCESS);
					Console.WriteLine(info);
					Console.WriteLine("");

					return;
				}

				string connStr = args[0];
				string destPath = args[1];
				string namespace_ = args[2];

				SimpleCRUD.Generate(connStr, destPath, namespace_);
			}
		}
	}
}
