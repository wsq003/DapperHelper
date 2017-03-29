namespace DapperHelper
{
	partial class DapperHelper
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.btnRun = new System.Windows.Forms.Button();
			this.txtConnStr = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnRun
			// 
			this.btnRun.Location = new System.Drawing.Point(287, 97);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(172, 67);
			this.btnRun.TabIndex = 0;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// txtConnStr
			// 
			this.txtConnStr.Location = new System.Drawing.Point(16, 55);
			this.txtConnStr.Name = "txtConnStr";
			this.txtConnStr.Size = new System.Drawing.Size(468, 25);
			this.txtConnStr.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(143, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "ConnectionString:";
			// 
			// DapperHelper
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(525, 191);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtConnStr);
			this.Controls.Add(this.btnRun);
			this.Name = "DapperHelper";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DapperHelper";
			this.Load += new System.EventHandler(this.DapperHelper_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.TextBox txtConnStr;
		private System.Windows.Forms.Label label1;
	}
}

