using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
	class EntityHelper
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
		/// 实体类的名称
		/// </summary>
		public string _tableName;

		public EntityHelper(string connStr, DataTable dt, string namespace_)
        {
			ConnectionString = connStr;
			_dtMetaInfo = dt;

			_namespace = namespace_;
			_tableName = dt.TableName;
        }

		public string CreatePropertyEntity()
		{
			StringBuilder sb = new StringBuilder();

			//head
			sb.AppendFormat("using System;\r\n");
			sb.AppendFormat("\r\n");
			sb.AppendFormat("namespace {0}\r\n", _namespace);
			sb.AppendFormat("{{\r\n");
			sb.AppendFormat("\tpublic class {0}Entity\r\n", _tableName);
			sb.AppendFormat("\t{{\r\n");

			//body
			foreach (DataRow dr in _dtMetaInfo.Rows)
			{
				sb.AppendFormat("\t\tpublic {0} {1};\r\n", CreateHelper.ConvertType(dr["type"].ToString()), dr["name"]);
			}

			//tail
			sb.AppendFormat("\t}} //end of class\r\n");
			sb.AppendFormat("}} //end of namespace\r\n");

			return sb.ToString();
		}

	}
}
