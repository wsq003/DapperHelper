﻿using System;
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
			sb.AppendFormat("namespace {0}.SQL\r\n", _namespace);
			sb.AppendFormat("{{\r\n");
			sb.AppendFormat("\tpublic partial class {0}\r\n", _tableName);
			sb.AppendFormat("\t{{\r\n");

			//body
			sb.AppendFormat("\t\tpublic static string insert = \"{0}\";\r\n", genInsert());
			sb.AppendFormat("\t\tpublic static string update = \"{0}\";\r\n", genUpdate());
			sb.AppendFormat("\t\tpublic static string select = \"{0}\";\r\n", genSelect());
			sb.AppendFormat("\t\tpublic static string delete = \"{0}\";\r\n", genDelete());
			sb.AppendFormat("\t\tpublic static string exist = \"{0}\";\r\n", genExist());

			//tail
			sb.AppendFormat("\t}} //end of class\r\n");
			sb.AppendFormat("}} //end of namespace\r\n");

			return sb.ToString();
		}

		private string genInsert()
		{
			StringBuilder insert = new StringBuilder();
			insert.Append("INSERT INTO [" + _dtMetaInfo.TableName + "] (");
			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				if (_dtMetaInfo.Rows[i]["is_identity"].ToString() == "1")
				{
					continue;
				}
				insert.Append("[" + _dtMetaInfo.Rows[i]["name"].ToString() + "], ");
			}
			insert.Remove(insert.Length - 2, 2);
			insert.Append(") VALUES (");

			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				insert.AppendFormat("@{0}, ", _dtMetaInfo.Rows[i]["name"].ToString());
			}
			if (insert.Length > 0)
			{
				insert.Remove(insert.Length - 2, 2);
				insert.Append(")");
			}
			return insert.ToString();
		}

		private string genUpdate()
		{
			StringBuilder updateStr = new StringBuilder();
			updateStr.AppendFormat("UPDATE [{0}] SET ", _dtMetaInfo.TableName);
			for (int i = 0; i < _dtMetaInfo.Rows.Count; i++)
			{
				if (_dtMetaInfo.Rows[i]["is_identity"].ToString() == "1")
				{
					continue;
				}
				updateStr.AppendFormat("[{0}]=@{0}, ", _dtMetaInfo.Rows[i]["name"].ToString());
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
				sb.AppendFormat("[{0}], ", dr["name"].ToString());
			}
			sb.Remove(sb.Length - 2, 2);
			sb.AppendFormat(" FROM [{0}]", _dtMetaInfo.TableName);
			return sb.ToString();
		}

		private string genDelete()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("DELETE FROM [{0}]", _dtMetaInfo.TableName);
			sb.Append(GetWhereCondition());
			return sb.ToString();
		}

		string genExist()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("select count(1) FROM [{0}]", _dtMetaInfo.TableName);
			sb.Append(GetWhereCondition());
			return sb.ToString();
		}

		/// <summary>
		/// 基于主键信息生成 “前面带空格的where条件”
		/// </summary>
		/// <returns></returns>
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
					condition.AppendFormat("{0} = @{0} and ", dr["column_name"].ToString());
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
