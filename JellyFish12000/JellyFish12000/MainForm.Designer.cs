namespace JellyFish12000
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.domeViewer1 = new JellyFish12000.DomeViewer();
            this.SuspendLayout();
            // 
            // domeViewer1
            // 
            this.domeViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.domeViewer1.Location = new System.Drawing.Point(12, 12);
            this.domeViewer1.Name = "domeViewer1";
            this.domeViewer1.Size = new System.Drawing.Size(452, 383);
            this.domeViewer1.TabIndex = 3;
            this.domeViewer1.Text = "domeViewer1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 407);
            this.Controls.Add(this.domeViewer1);
            this.Name = "MainForm";
            this.Text = "test";
            this.ResumeLayout(false);

		}

		#endregion

        private DomeViewer domeViewer1;

	}
}