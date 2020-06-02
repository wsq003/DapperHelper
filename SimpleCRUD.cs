using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace DapperHelper
{
	/// <summary>
	/// 数据库类型
	/// </summary>
	public enum SqlType
	{
		SqlServer,
		MySQL,
		Oracle
	}

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

	/// <summary>
	/// 为数据库生成实体类和CRUD语句。目前支持sqlserver和mysql
	/// </summary>
	public class SimpleCRUD
	{
		public static SqlType sqlType { get; set; }

		private static DataSet ExecSql(string connStr, string sql)
		{
			DataSet ds = new DataSet();
			if (sqlType == SqlType.SqlServer)
			{
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					SqlCommand cmd = new SqlCommand(sql, conn);

					using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
					{
						adapter.Fill(ds);
						return ds;
					}
				}
			}
			else if (sqlType == SqlType.MySQL)
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				{
					MySqlCommand cmd = new MySqlCommand(sql, conn);

					using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
					{
						adapter.Fill(ds);
						return ds;
					}
				}
			}
			else
			{
				throw new Exception("尚未支持的sqlType");
			}
		}

		private static List<string> GetTables(string connStr)
		{
			string sql = "select name from sys.tables where is_ms_shipped=0 ORDER BY NAME";
			if (sqlType == SqlType.MySQL)
			{
				sql = "SHOW TABLES";
			}
			var ds = ExecSql(connStr, sql);

			List<string> ret = new List<string>();
			if (sqlType == SqlType.SqlServer)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					ret.Add(row["name"].ToString());
				}
				return ret;
			}
			else if (sqlType == SqlType.MySQL)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					ret.Add(row[0].ToString());
				}
				return ret;		
			}
			else
			{
				throw new Exception("尚未支持的sqlType");
			}
		}

		/// <summary>
		/// 基于数据库表信息，生成实体类和CRUD语句
		/// </summary>
		/// <param name="connectionString">数据库连接串</param>
		/// <param name="destPath">生成的C#代码文件放到哪个目录去</param>
		/// <param name="codeNamespace">代码放到哪个命名空间去</param>
		/// <param name="tables">为空或者为null则生成数据库所有表的代码。都要小写</param>
		public static void Generate(string connectionString, string destPath = "d:\\code", string codeNamespace = "SimpleCRUD", List<string> tables = null)
		{
			var tbls = GetTables(connectionString);
			var metas = GetMetaInfo(connectionString);

			foreach (string tableName in tbls)
			{
				if (tables != null && tables.Count > 0 && !tables.Contains(tableName.ToLower()))
				{
					continue;
				}

				var meta = metas[tableName];

				var dal = new DALHelper(meta, codeNamespace);
				var s1 = dal.CreateDAL();

				var ent = new EntityHelper(meta, codeNamespace);
				var s2 = ent.CreatePropertyEntity();

				var fac = new FacadeHelper(meta, codeNamespace);
				var s3 = fac.CreateFacade();

				if (!Directory.Exists(destPath))
				{
					Directory.CreateDirectory(destPath);
				}
				File.WriteAllText(string.Format("{0}/{1}Sql.cs", destPath, tableName), s1);
				File.WriteAllText(string.Format("{0}/{1}Entity.cs", destPath, tableName), s2);

				if (ConfigurationManager.AppSettings["genFacade"] == "1")
				{
					File.WriteAllText(string.Format("{0}/{1}Facade.cs", destPath, tableName), s3);
				}
			}

		}

		/// <summary>
		/// 把sql server数据库类型转换为C#类型
		/// </summary>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		private static string ConvertType(string sourceType)
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
				case "varbinary":
					destType = "byte[]";
					break;
				default:
					destType =
						sourceType;
					break;
			}

			return destType;
		}

		private static bool IsTrue(object o)
		{
			var ss = o.ToString();
			return (ss == "True" || ss == "1");
		}

		/// <summary>
		/// 获取所有table的meta信息
		/// </summary>
		/// <param name="connStr"></param>
		/// <returns></returns>
		private static Dictionary<string, MetaInfo> GetMetaInfo(string connStr)
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
				sql = "select *, COLUMN_NAME as name, data_type as type, COLUMN_COMMENT as comment, extra='auto_increment' as is_identity, lower(column_key)='pri' as primeKey"
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
	}
}
