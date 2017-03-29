using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper
{
	class LangHelper
	{
		public enum eLanguageType
		{
			Chinese,
			English
		}

		#region Fields
		/// <summary>
		/// 支持采用忽略大小写的 key
		/// </summary>
		private Hashtable Language = new Hashtable(StringComparer.OrdinalIgnoreCase);
		#endregion


		#region Constructor
		public LangHelper(eLanguageType language)
		{
			if (language == eLanguageType.Chinese)
			{
				initLanguage_Chinese();
			}
		}
		#endregion


		#region Public Methods
		public string GetValueByKey(string key)
		{
			if (Language.Contains(key.ToUpper()))
			{
				return Language[key.ToUpper()].ToString();
			}
			else
			{
				return key;
			}
		}
		#endregion




		#region Private Methods
		private void initLanguage_Chinese()
		{
			LangSystemType();
			LangPeople();
			// Language.Add("", "");
			#region Common
			Language.Add("ID", "ID");
			Language.Add("ORDER_ID", "顺序");
			Language.Add("ORDERID", "顺序");
			Language.Add("ORDER", "顺序");
			Language.Add("STATUS", "状态");
			Language.Add("TYPE", "类型");
			Language.Add("TYPEID", "类型");
			Language.Add("REASON", "原因");
			Language.Add("PARTNER", "合作伙伴");
			Language.Add("PARTNERTYPE", "合作伙伴类型");
			Language.Add("LABLE", "标记");
			Language.Add("STAGE", "阶段");
			Language.Add("FIELDS", "领域");
			Language.Add("WEBSITE", "网址");
			Language.Add("FOUNDER", "创始人");
			Language.Add("SCALE", "规模");
			#endregion
			#region Study
			Language.Add("C_NAME", "课程名称");
			Language.Add("S_TITLE", "题目");
			Language.Add("PASSER", "启用人");
			Language.Add("PASS_TIME", "启用时间");
			Language.Add("TITLE", "题目");
			Language.Add("RESOURCE", "资源");
			Language.Add("AUTHOR", "作者");
			Language.Add("DESCRIPTION", "讲解");

			#endregion
			#region Student
			Language.Add("SCHOOLID", "学校");
			Language.Add("GRADE", "年级");
			Language.Add("ISRELATION", "关系户");
			Language.Add("EMAIL", "邮箱");
			Language.Add("FATHERNAME", "父亲姓名");
			Language.Add("FATHERPHONENUM", "父亲手机号码");
			Language.Add("MOTHERNAME", "母亲姓名");
			Language.Add("MOTHERPHONENUM", "母亲手机号码");
			Language.Add("SUPERVISOR", "跟进人");
			Language.Add("PHONENUM", "手机号");
			Language.Add("ORIGIN", "来源");
			Language.Add("STUDENTNUM", "学生号");
			Language.Add("STUDENTNAME", "学生姓名");
			#endregion
			#region Boc
			Language.Add("WHAT_DAY", "星期几");
			Language.Add("COMMENT", "点评");
			Language.Add("DESCRIPTIONS", "描述");
			Language.Add("", "");
			//Language.Add("", "");
			//Language.Add("", "");
			#endregion
			#region Vote
			Language.Add("VOTENAME", "投票名称");
			Language.Add("VOTETYPE", "投票类型");
			Language.Add("ISANONY", "匿名");
			Language.Add("ISPUBLIC", "公布结果");
			//Language.Add("", "");
			#endregion
			#region 文件上传
			Language.Add("FTYPE", "文件类别");
			Language.Add("FNAME", "文件名称");
			#endregion
			#region CMP
			LangCMPType();
			#endregion

		}


		private void LangSystemType()
		{
			Language.Add("BROWER", "浏览器");
			Language.Add("OS", "操作系统");
			Language.Add("SOLUTION", "解决方案");
			Language.Add("EDITION", "版本");
			Language.Add("PLATFORM", "平台");
			Language.Add("WEBSITEURL", "网址地址");
			Language.Add("WEBSITENAME", "网站名称");
		}
		private void LangPeople()
		{
			Language.Add("NAME", "姓名");
			Language.Add("USERNAME", "用户姓名");
			Language.Add("ADD_TIME", "添加时间");
			Language.Add("ADDER", "添加人");
			Language.Add("CREATE_DATE", "创建时间");
			Language.Add("BIRTHDAY", "生日");
			Language.Add("LAST_SAVER", "最后保存人");
			Language.Add("LAST_SAVETIME", "最后保存时间");
			Language.Add("ADDR", "地址");
			Language.Add("START_TIME", "开始时间");
			Language.Add("END_TIME", "结束时间");
			Language.Add("ADDTIME", "添加时间");
			Language.Add("MAJOR", "专业");
			Language.Add("SEX", "性别");
			Language.Add("TELEPHONE", "手机号");
			Language.Add("COMPANY", "公司");
			Language.Add("ISMARRIED", "婚否");
			Language.Add("FEEDBACKER", "反馈人");

			Language.Add("USERTYPE", "用户类型");

		}
		private void LangCMPType()
		{
			Language.Add("CLASSNAME", "班级名称");
			Language.Add("CLASSNUM", "班号");
			Language.Add("CLASSROOM", "教室");
			Language.Add("ProjID", "项目ID");
			Language.Add("STARTTIME", "开班日期");
			Language.Add("ENDTIME", "结班日期");
			Language.Add("CAMPUS", "校区");
			Language.Add("MEMO", "备注");
			Language.Add("DETAIL", "详情");
			Language.Add("PLANNAME", "计划名称");
			Language.Add("TYPENAME", "类别");
			Language.Add("DURATION", "周期");
			Language.Add("SCOPE", "范围");
			Language.Add("RESTTIME", "休息日");
			Language.Add("TEACHTIME", "授课时间");
			Language.Add("REQUIREMENT", "入班门槛");
			Language.Add("GOALSCORE", "目标分数");
			Language.Add("MAXSTU", "最大人数");
			Language.Add("MAXNUM", "最大预约人数");
			Language.Add("EXTRANUM", "额外预约人数");
			Language.Add("STUNUMTYPE", "人数类型");
			Language.Add("TOTALCOUNT", "总课次");
			Language.Add("TOTALPRICE", "总价");
			Language.Add("STARTDATE", "开班日期");
			Language.Add("ENDDATE", "接班日期");
			Language.Add("PRICE", "课程单价");
		}

		#endregion
	}
}
