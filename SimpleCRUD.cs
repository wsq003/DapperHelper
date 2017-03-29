using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace DapperHelper
{
	/// <summary>
	/// 为数据库生成实体类和CRUD语句。目前仅支持sql server
	/// </summary>
	public class SimpleCRUD
	{
		static DataSet ExecSql(string connStr, string sql)
		{
			DataSet ds = new DataSet();
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

		/// <summary>
		/// 基于数据库表信息，生成实体类和CRUD语句
		/// </summary>
		/// <param name="connectionString">数据库连接串</param>
		/// <param name="destPath">生成的C#代码文件放到哪个目录去</param>
		/// <param name="codeNamespace">代码放到哪个命名空间去</param>
		public static void Generate(string connectionString, string destPath = "d:\\code", string codeNamespace = "SimpleCRUD")
		{
			string sql = "select name from sys.tables ORDER BY NAME";
			var ds = ExecSql(connectionString, sql);

			foreach (DataRow row in ds.Tables[0].Rows)
			{
				string tableName = row["name"].ToString();
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
