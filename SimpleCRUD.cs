using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;

namespace DapperHelper
{
	public enum SqlType
	{
		SqlServer,
		MySQL,
		Oracle
	}

	/// <summary>
	/// 为数据库生成实体类和CRUD语句。目前仅支持sql server
	/// </summary>
	public class SimpleCRUD
	{
		public static SqlType sqlType = SqlType.SqlServer;

		public static DataSet ExecSql(string connStr, string sql)
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

		static List<string> GetTables(string connStr)
		{
			string sql = "select name from sys.tables ORDER BY NAME";
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
			foreach (string tableName in tbls)
			{
				if (tables != null && tables.Count > 0 && !tables.Contains(tableName.ToLower()))
				{
					continue;
				}
				var meta = CreateHelper.GetMetaInfo(connectionString, tableName);

				var dal = new DALHelper(connectionString, meta, codeNamespace);
				var s1 = dal.CreateDAL();

				var ent = new EntityHelper(connectionString, meta, codeNamespace);
				var s2 = ent.CreatePropertyEntity();

				if (!Directory.Exists(destPath))
				{
					Directory.CreateDirectory(destPath);
				}
				File.WriteAllText(string.Format("{0}/{1}Sql.cs", destPath, tableName), s1);
				File.WriteAllText(string.Format("{0}/{1}Entity.cs", destPath, tableName), s2);
			}

		}
	}
}
