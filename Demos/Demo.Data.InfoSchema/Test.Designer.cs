namespace Demo.Data.InfoSchema
{
    partial class Test
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.bnConnect = new System.Windows.Forms.Button();
            this.lblTable = new System.Windows.Forms.Label();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.btnConnectionStringBuilder = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.Location = new System.Drawing.Point(14, 12);
            this.lblConnectionString.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(130, 23);
            this.lblConnectionString.TabIndex = 0;
            this.lblConnectionString.Text = "Connection String:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(150, 12);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(772, 23);
            this.txtConnectionString.TabIndex = 1;
            // 
            // bnConnect
            // 
            this.bnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnConnect.Location = new System.Drawing.Point(975, 8);
            this.bnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bnConnect.Name = "bnConnect";
            this.bnConnect.Size = new System.Drawing.Size(72, 32);
            this.bnConnect.TabIndex = 7;
            this.bnConnect.Text = "Connect";
            this.bnConnect.Click += new System.EventHandler(this.bnConnect_Click);
            // 
            // lblTable
            // 
            this.lblTable.Location = new System.Drawing.Point(14, 45);
            this.lblTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(50, 23);
            this.lblTable.TabIndex = 6;
            this.lblTable.Text = "Table:";
            // 
            // cmbTable
            // 
            this.cmbTable.DropDownWidth = 218;
            this.cmbTable.FormattingEnabled = true;
            this.cmbTable.Location = new System.Drawing.Point(150, 45);
            this.cmbTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(254, 23);
            this.cmbTable.Sorted = true;
            this.cmbTable.TabIndex = 5;
            this.cmbTable.SelectedIndexChanged += new System.EventHandler(this.cmbTable_SelectedIndexChanged);
            // 
            // btnConnectionStringBuilder
            // 
            this.btnConnectionStringBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectionStringBuilder.Location = new System.Drawing.Point(930, 8);
            this.btnConnectionStringBuilder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnectionStringBuilder.Name = "btnConnectionStringBuilder";
            this.btnConnectionStringBuilder.Size = new System.Drawing.Size(38, 32);
            this.btnConnectionStringBuilder.TabIndex = 4;
            this.btnConnectionStringBuilder.Text = "…";
            this.btnConnectionStringBuilder.Click += new System.EventHandler(this.btnConnectionStringBuilder_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 89);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1064, 592);
            this.dataGridView.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bnConnect);
            this.panel1.Controls.Add(this.lblTable);
            this.panel1.Controls.Add(this.txtConnectionString);
            this.panel1.Controls.Add(this.cmbTable);
            this.panel1.Controls.Add(this.lblConnectionString);
            this.panel1.Controls.Add(this.btnConnectionStringBuilder);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 89);
            this.panel1.TabIndex = 4;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnConnectionStringBuilder;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button bnConnect;
        private System.Windows.Forms.Panel panel1;
    }
}
