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
		/// 
		/// </summary>
		/// <param name="dr">metaInfo table中的一行</param>
		/// <returns></returns>
		public static string ConvertType(DataRow dr)
		{
			return ConvertType(dr["type"].ToString(), dr["is_nullable"].ToString());
		}

		/// <summary>
		/// 把数据库类型转换为C#类型
		/// </summary>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public static string ConvertType(string sourceType, string is_nullable)
		{
			string destType = "";
			switch (sourceType.ToLower())
			{
				case "tinyint":
				case "smallint":
				case "int":
					destType = "int";
					break;
				case "bigint":
					destType = "long";
					break;
				case "float":
					destType = "float";
					break;
				case "numeric":
					destType = "double";
					break;
				case "char":
				case "text":
				case "ntext":
				case "nchar":
				case "nvarchar":
				case "varchar":
				case "sysname":
					destType = "string";
					break;
				case "time":
					destType = "string";
					break;
				case "datetime":
				case "datetime2":
				case "date":
				case "smalldatetime":
					destType = "DateTime";
					break;
				case "decimal":
					destType = "decimal";
					break;
				case "money":
					destType = "double";
					break;
				case "bit":
					destType = "bool";
					break;
				default:
					destType =
						sourceType;
					break;
			}
			if (is_nullable == "True")
			{
				//destType += "?";
			}
			return destType;
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
				comm.CommandText = string.Format("SELECT A.*, B.Name type FROM SYS.COLUMNS A, SYS.TYPES B WHERE A.SYSTEM_TYPE_ID = B.SYSTEM_TYPE_ID AND B.NAME != 'SYSNAME' AND A.OBJECT_ID = (SELECT OBJECT_ID FROM SYS.TABLES WHERE NAME = '{0}')  ORDER BY A.COLUMN_ID", tableName);
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
