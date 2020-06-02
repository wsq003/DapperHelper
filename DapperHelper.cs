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

		private void DapperHelper_Load(object sender, EventArgs e)
		{

		}
		private void btnRun_Click(object sender, EventArgs e)
		{
			try
			{
				string connStr = txtConnStr.Text;
				SimpleCRUD.Generate(connStr);

				MessageBox.Show("finished");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

	}
}
