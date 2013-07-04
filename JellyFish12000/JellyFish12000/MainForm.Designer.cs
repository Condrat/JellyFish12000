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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DomeConsole = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.XBeeComPorts = new System.Windows.Forms.ComboBox();
            this.XBeeConnectButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.domeViewer1 = new JellyFish12000.DomeViewer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.domeViewer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(570, 529);
            this.splitContainer1.SplitterDistance = 407;
            this.splitContainer1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.DomeConsole, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(570, 118);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // DomeConsole
            // 
            this.DomeConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DomeConsole.Location = new System.Drawing.Point(3, 33);
            this.DomeConsole.Name = "DomeConsole";
            this.DomeConsole.ReadOnly = true;
            this.DomeConsole.Size = new System.Drawing.Size(564, 82);
            this.DomeConsole.TabIndex = 17;
            this.DomeConsole.Text = "";
            this.DomeConsole.TextChanged += new System.EventHandler(this.DomeConsole_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.XBeeComPorts);
            this.panel1.Controls.Add(this.XBeeConnectButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 24);
            this.panel1.TabIndex = 0;
            // 
            // XBeeComPorts
            // 
            this.XBeeComPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.XBeeComPorts.FormattingEnabled = true;
            this.XBeeComPorts.Location = new System.Drawing.Point(3, 1);
            this.XBeeComPorts.Name = "XBeeComPorts";
            this.XBeeComPorts.Size = new System.Drawing.Size(121, 21);
            this.XBeeComPorts.TabIndex = 18;
            // 
            // XBeeConnectButton
            // 
            this.XBeeConnectButton.Location = new System.Drawing.Point(129, 0);
            this.XBeeConnectButton.Name = "XBeeConnectButton";
            this.XBeeConnectButton.Size = new System.Drawing.Size(74, 23);
            this.XBeeConnectButton.TabIndex = 17;
            this.XBeeConnectButton.Text = "Connect";
            this.XBeeConnectButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(209, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Find Children";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // domeViewer1
            // 
            this.domeViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.domeViewer1.Location = new System.Drawing.Point(0, 0);
            this.domeViewer1.Name = "domeViewer1";
            this.domeViewer1.Size = new System.Drawing.Size(570, 407);
            this.domeViewer1.TabIndex = 4;
            this.domeViewer1.Text = "domeViewer1";
            this.domeViewer1.Click += new System.EventHandler(this.domeViewer1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 529);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Jellyfish Simulator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DomeViewer domeViewer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox DomeConsole;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button XBeeConnectButton;
        private System.Windows.Forms.ComboBox XBeeComPorts;
        private System.Windows.Forms.Button button1;


    }
}