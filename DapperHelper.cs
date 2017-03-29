using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DapperHelper
{
	public partial class DapperHelper : Form
	{
		public DapperHelper()
		{
			InitializeComponent();
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			string connStr = ""; ;
			SimpleCRUD.Generate(connStr);
		}
	}
}
