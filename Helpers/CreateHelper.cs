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
			return ConvertType(dr["type"].ToString());
		}

		static List<string> nullables = new List<string> { "int", "long", "double", "DateTime" };

		/// <summary>
		/// 把sql server数据库类型转换为C#类型
		/// </summary>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public static string ConvertType(string sourceType)
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

			return destType;
		}

		static bool IsTrue(object o)
		{
			var ss = o.ToString();
			return (ss == "True" || ss == "1");
		}

		public static Dictionary<string, MetaInfo> GetMetaInfo2(string connStr)
		{
			string sql = "";

			if (SimpleCRUD.sqlType == SqlType.SqlServer)
			{
				sql = "select T.*, charindex(d.COLUMN_NAME, T.name) as primeKey from "
	+ " (select T.*, C.value as comment from "
	+ " ( SELECT X.name as table_name, A.*, B.Name as type FROM SYS.COLUMNS A, SYS.TYPES B, sys.tables X "
	+ "  WHERE A.SYSTEM_TYPE_ID = B.SYSTEM_TYPE_ID and B.NAME != 'SYSNAME' "
	+ "  AND A.OBJECT_ID = X.object_id  ) T "
	+ "  left join (select * from sys.extended_properties where name='MS_Description' ) C on T.object_id = c.major_id AND T.column_id=c.minor_id "
	+ "  ) T left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE d on d.table_name=T.table_name and d.column_name=T.name  order by table_name, column_id ";
			}
			else if (SimpleCRUD.sqlType == SqlType.MySQL)
			{
				sql = "select *, COLUMN_NAME as name, data_type as type, COLUMN_COMMENT as comment, extra='auto_increment' as is_identity, column_key='pri' as primeKey"
					+ " from information_schema.COLUMNS where TABLE_SCHEMA=database(); ";
			}
			else
			{
				throw new Exception("尚未支持的sqlType");
			}

			DataSet ds = SimpleCRUD.ExecSql(connStr, sql);

			DataTable dtMetaInfo = ds.Tables[0];

			Dictionary<string, MetaInfo> ret = new Dictionary<string, MetaInfo>();

			foreach (DataRow row in dtMetaInfo.Rows)
			{
				string tableName = row["table_name"].ToString();
				if (!ret.ContainsKey(tableName))
				{
					MetaInfo temp = new MetaInfo();
					temp.TableName = tableName;
					ret.Add(tableName, temp);
				}
				MetaInfo meta = ret[tableName];

				ColumnInfo col = new ColumnInfo();
				col.name = row["name"].ToString();
				col.csType = ConvertType(row["type"].ToString());
				col.is_identity = IsTrue(row["is_identity"]);
				col.is_nullable = IsTrue(row["is_nullable"]);
				col.is_primeKey = IsTrue(row["primeKey"]);
				col.comment = row["comment"].ToString();

				meta.columns.Add(col);
			}

			return ret;
		}

		/// <summary>
		/// 获取数据库表的meta信息，放入dtMetaInfo
		/// </summary>
		/// <returns></returns>
		public static MetaInfo GetMetaInfo(string connStr, string tableName)
		{
			string sql = "";

			if (SimpleCRUD.sqlType == SqlType.SqlServer)
			{
				sql = "select T.*, charindex(d.COLUMN_NAME, T.name) as primeKey from "
					 + " (select T.*, C.value as comment from "
					 + " ( SELECT A.*, B.Name as type FROM SYS.COLUMNS A, SYS.TYPES B "
					 + "  WHERE A.SYSTEM_TYPE_ID = B.SYSTEM_TYPE_ID and B.NAME != 'SYSNAME' "
					 + "  AND A.OBJECT_ID = (SELECT OBJECT_ID FROM SYS.TABLES WHERE NAME = 'openaccount')  ) T "
					 + "  left join (select * from sys.extended_properties where name='MS_Description' ) C on T.object_id = c.major_id AND T.column_id=c.minor_id "
					 + "  ) T left join INFORMATION_SCHEMA.KEY_COLUMN_USAGE d on d.table_name='openaccount' and d.column_name=T.name  order by column_id ";
				sql = sql.Replace("openaccount", tableName);
			}
			else if (SimpleCRUD.sqlType == SqlType.MySQL)
			{
				sql = "select *, COLUMN_NAME as name, data_type as type, COLUMN_COMMENT as comment, extra='auto_increment' as is_identity, column_key='pri' as primeKey"
					+ " from information_schema.COLUMNS where table_name = 'openaccount' and TABLE_SCHEMA=database(); ";
				sql = sql.Replace("openaccount", tableName);
			}
			else
			{
				throw new Exception("尚未支持的sqlType");
			}

			DataSet ds = SimpleCRUD.ExecSql(connStr, sql);

			DataTable dtMetaInfo = ds.Tables[0];
				
			dtMetaInfo.TableName = tableName;

			MetaInfo meta = new MetaInfo();
			meta.TableName = tableName;
			foreach (DataRow row in dtMetaInfo.Rows)
			{
				ColumnInfo col = new ColumnInfo();
				col.name = row["name"].ToString();
				col.csType = ConvertType(row["type"].ToString());
				col.is_identity = IsTrue(row["is_identity"]);
				col.is_nullable = IsTrue(row["is_nullable"]);
				col.is_primeKey = IsTrue(row["primeKey"]);
				col.comment = row["comment"].ToString();

				meta.columns.Add(col);
			}
			return meta;
		}

	}
}
