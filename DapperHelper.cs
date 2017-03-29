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
<<<<<<< HEAD
			string connStr = ""; ;
			SimpleCRUD.Generate(connStr);
=======
			//Data Source=192.168.2.112;Initial Catalog=DB_DomesticFutures;User ID=sa;Password=taotao778899!;Persist Security Info=False;Max Pool Size=100
			SimpleCRUD.Generate("Data Source=192.168.2.112;Initial Catalog=DB_DomesticFutures;User ID=sa;Password=taotao778899!;Persist Security Info=False;Max Pool Size=100");
>>>>>>> 548e819f4d68922ec18a6dcf64f0bfc38c92534a
		}
	}
}
