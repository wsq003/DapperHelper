using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
	class CreateHelper
	{
		/// <summary>
		/// 把数据库类型转换为C#类型
		/// </summary>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public static string ConvertType(string sourceType)
		{
			switch (sourceType.ToLower())
			{
				case "tinyint":
				case "smallint":
				case "int":
					return "int?";
				case "bigint":
					return "long";
				case "float":
					return "float?";
				case "numeric":
					return "double?";
				case "char":
				case "text":
				case "ntext":
				case "nchar":
				case "nvarchar":
				case "varchar":
				case "sysname":
					return "string";
				case "time":
					return "string";
				case "datetime":
				case "datetime2":
				case "date":
				case "smalldatetime":
					return "DateTime?";
				case "decimal":
					return "decimal?";
				case "money":
					return "double";
				default:
					return
						sourceType;
			}
		}

		/// <summary>
		/// 获取数据库表的meta信息，放入dtMetaInfo
		/// </summary>
		/// <returns></returns>
		public static DataTable GetMetaInfo(string connStr, string tableName)
		{
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				DataTable dtMetaInfo = new DataTable();
				SqlCommand comm = new SqlCommand();
				comm.Connection = conn;
				comm.CommandText = "SELECT A.NAME NAME , B.NAME TYPE, A.MAX_LENGTH LENGTH FROM SYS.COLUMNS A, SYS.TYPES B WHERE A.SYSTEM_TYPE_ID = B.SYSTEM_TYPE_ID AND B.NAME != 'SYSNAME' AND A.OBJECT_ID = (SELECT OBJECT_ID FROM SYS.TABLES WHERE NAME = '" + tableName + "')  ORDER BY A.COLUMN_ID";
				using (SqlDataAdapter da = new SqlDataAdapter(comm))
				{
					da.Fill(dtMetaInfo);
				}
				dtMetaInfo.TableName = tableName;

				return dtMetaInfo;
			}
		}

	}
}
