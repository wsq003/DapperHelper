using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
	/// <summary>
	/// 某个字段的信息
	/// </summary>
	public class ColumnInfo
	{
		/// <summary>
		/// 字段名称
		/// </summary>
		public string name;

		/// <summary>
		/// C#里面的字段类型
		/// </summary>
		public string csType;

		/// <summary>
		/// 是否自增长
		/// </summary>
		public bool is_identity;

		/// <summary>
		/// 是否可为NULL
		/// </summary>
		public bool is_nullable;

		/// <summary>
		/// 是否主键字段之一
		/// </summary>
		public bool is_primeKey;

		/// <summary>
		/// 字段注释
		/// </summary>
		public string comment;
	}

	/// <summary>
	/// 数据库表的meta信息
	/// </summary>
	public class MetaInfo
	{
		public string TableName;
		public List<ColumnInfo> columns = new List<ColumnInfo>();
	}

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

		static List<string> nullables = new List<string> { "int", "long", "double", "DateTime" };

		/// <summary>
		/// 把sql server数据库类型转换为C#类型
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
			if (is_nullable == "True" && nullables.Contains(destType))
			{
				//destType += "?";
			}
			return destType;
		}

		/// <summary>
		/// 获取数据库表的meta信息，放入dtMetaInfo
		/// </summary>
		/// <returns></returns>
		public static MetaInfo GetMetaInfo(string connStr, string tableName)
		{
			string sql = "select T.*, d.COLUMN_NAME as primeKey from "
				+" (select T.*, C.value as comment from "
				+" ( SELECT A.*, B.Name as type FROM SYS.COLUMNS A, SYS.TYPES B "
				+"  WHERE A.SYSTEM_TYPE_ID = B.SYSTEM_TYPE_ID and B.NAME != 'SYSNAME' "
				+"  AND A.OBJECT_ID = (SELECT OBJECT_ID FROM SYS.TABLES WHERE NAME = 'openaccount')  ) T "
				+"  left join (select * from sys.extended_properties where name='MS_Description' ) C on T.object_id = c.major_id AND T.column_id=c.minor_id "
				+ "  ) T left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE d on d.table_name='openaccount' and d.column_name=T.name  order by column_id ";

			sql = sql.Replace("openaccount", tableName);
			using (SqlConnection conn = new SqlConnection(connStr))
			{
				DataTable dtMetaInfo = new DataTable();
				SqlCommand comm = new SqlCommand();
				comm.Connection = conn;
				comm.CommandText = sql;
				using (SqlDataAdapter da = new SqlDataAdapter(comm))
				{
					da.Fill(dtMetaInfo);
				}
				dtMetaInfo.TableName = tableName;

				MetaInfo meta = new MetaInfo();
				meta.TableName = tableName;
				foreach (DataRow row in dtMetaInfo.Rows)
				{
					ColumnInfo col = new ColumnInfo();
					col.name = row["name"].ToString();
					col.csType = ConvertType(row["type"].ToString(), row["is_nullable"].ToString());
					col.is_identity = row["is_identity"].ToString() == "True";
					col.is_nullable = row["is_nullable"].ToString() == "True";
					col.is_primeKey = row["primeKey"].ToString().Length > 0;
					col.comment = row["comment"].ToString();

					meta.columns.Add(col);
				}
				return meta;
			}
		}

	}
}
