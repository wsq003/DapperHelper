using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
	class DALHelper
	{
		public string ConnectionString = "";
		/// <summary>
		/// 表的meta信息
		/// </summary>
		public DataTable _dtMetaInfo;
		/// <summary>
		/// 代码的命名空间
		/// </summary>
		public string _namespace;
		/// <summary>
		/// 
		/// </summary>
		public string _tableName;

		public DALHelper(string connStr, DataTable dt, string namespace_)
		{
			ConnectionString = connStr;
			_dtMetaInfo = dt;

			_namespace = namespace_;
			_tableName = dt.TableName;
		}

		public string CreateDAL()
		{
			StringBuilder sb = new StringBuilder();

			//head
			sb.AppendFormat("using System;\r\n");
			sb.AppendFormat("\r\n");
			sb.AppendFormat("namespace {0}\r\n", _namespace);
			sb.AppendFormat("{{\r\n");
			sb.AppendFormat("\tpublic class {0}SQL\r\n", _tableName);
			sb.AppendFormat("\t{{\r\n");

			//body
			sb.AppendFormat("\t\tpublic static string insert=\"{0}\";\r\n", genInsert());
			sb.AppendFormat("\t\tpublic static string update=\"{0}\";\r\n", genUpdate());
			sb.AppendFormat("\t\tpublic static string select=\"{0}\";\r\n", genSelect());
			sb.AppendFormat("\t\tpublic static string delete=\"{0}\";\r\n", genDelete());

			//tail
			sb.AppendFormat("\t}} //end of class\r\n");
			sb.AppendFormat("}} //end of namespace\r\n");

			return sb.ToString();
		}

		private string genDelete()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DELETE FROM [" + _dtMetaInfo.TableName + "]");
			sb.Append(GetWhereCondition());
			return sb.ToString();
		}

		private string genUpdate()
		{
			StringBuilder updateStr = new StringBuilder();
			updateStr.Append("UPDATE [" + _dtMetaInfo.TableName + "] SET ");
			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				if (_dtMetaInfo.Rows[i]["name"].ToString().ToLower().Equals("id")
				   || _dtMetaInfo.Rows[i]["name"].ToString().ToLower().Equals("adder")
				   || _dtMetaInfo.Rows[i]["name"].ToString().ToLower().Equals("addtime")
				   || _dtMetaInfo.Rows[i]["name"].ToString().ToLower().Equals("adddate"))
					continue;
				updateStr.Append("[" + _dtMetaInfo.Rows[i]["name"].ToString().ToUpper() + "] = @" + _dtMetaInfo.Rows[i]["name"].ToString().ToLower() + ", ");
			}
			updateStr.Remove(updateStr.Length - 2, 2);
			updateStr.Append(GetWhereCondition());
			return updateStr.ToString();
		}

		private string genSelect()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT ");
			foreach (DataRow dr in _dtMetaInfo.Rows)
			{
				sb.Append("[" + dr["name"].ToString().ToUpper() + "], ");
			}
			sb.Remove(sb.Length - 2, 2);
			sb.Append(" FROM [" + _dtMetaInfo.TableName + "]");
			return sb.ToString();
		}

		private string genInsert()
		{
			StringBuilder insert = new StringBuilder();
			insert.Append("INSERT INTO [" + _dtMetaInfo.TableName + "]( ");
			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				if (_dtMetaInfo.Rows[i]["name"].ToString().ToLower().Equals("id"))
				{
					continue;
				}
				insert.Append("[" + _dtMetaInfo.Rows[i]["name"].ToString().ToUpper() + "], ");
			}
			insert.Remove(insert.Length - 2, 2);
			insert.Append(") VALUES (");

			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				insert.Append("@" + _dtMetaInfo.Rows[i]["name"].ToString().ToLower() + ", ");
			}
			if (insert.Length > 0)
			{
				insert.Remove(insert.Length - 2, 2);
				insert.Append(")");
			}
			return insert.ToString();
		}

		protected string GetWhereCondition()
		{
			string sql = string.Format("SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE where table_name = '{0}'", _dtMetaInfo.TableName);
			DataTable dt = new DataTable();
			SqlCommand comm = new SqlCommand();
			comm.Connection = new SqlConnection(ConnectionString);
			comm.CommandText = sql;
			using (SqlDataAdapter da = new SqlDataAdapter(comm))
			{
				da.Fill(dt);
			}
			StringBuilder condition = new StringBuilder();
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					condition.Append(string.Format("{0} = @{0} and ", dr["column_name"].ToString()));
				}

			}
			string conditions = "";
			if (condition.Length > 0)
			{
				condition.Remove(condition.Length - 5, 5);
				conditions = " WHERE " + condition.ToString();
			}
			return conditions;
		}
	}
}
