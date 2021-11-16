namespace Demo.Data.InfoSchema
{
    partial class Main
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnection = new System.Windows.Forms.Label();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.dgvForeignKeyInfo = new System.Windows.Forms.DataGridView();
            this.dgvColumnInfo = new System.Windows.Forms.DataGridView();
            this.lblCount = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabColumnInfo = new System.Windows.Forms.TabPage();
            this.tabForeignKeyInfo = new System.Windows.Forms.TabPage();
            this.cmbSchema = new System.Windows.Forms.ComboBox();
            this.lblSchema = new System.Windows.Forms.Label();
            this.cmdDataSource = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForeignKeyInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnInfo)).BeginInit();
            this.tabs.SuspendLayout();
            this.tabColumnInfo.SuspendLayout();
            this.tabForeignKeyInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(1202, 9);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(88, 27);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(92, 41);
            this.cmbDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(313, 23);
            this.cmbDatabase.TabIndex = 5;
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(12, 44);
            this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(58, 15);
            this.lblDatabase.TabIndex = 4;
            this.lblDatabase.Text = "Database:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Location = new System.Drawing.Point(412, 11);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(782, 23);
            this.txtConnectionString.TabIndex = 7;
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Location = new System.Drawing.Point(12, 15);
            this.lblConnection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(72, 15);
            this.lblConnection.TabIndex = 9;
            this.lblConnection.Text = "Connection:";
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.ItemHeight = 15;
            this.lbTables.Location = new System.Drawing.Point(92, 99);
            this.lbTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(313, 589);
            this.lbTables.TabIndex = 10;
            this.lbTables.SelectedIndexChanged += new System.EventHandler(this.lbTables_SelectedIndexChanged);
            // 
            // dgvForeignKeyInfo
            // 
            this.dgvForeignKeyInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvForeignKeyInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvForeignKeyInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvForeignKeyInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvForeignKeyInfo.Name = "dgvForeignKeyInfo";
            this.dgvForeignKeyInfo.Size = new System.Drawing.Size(868, 616);
            this.dgvForeignKeyInfo.TabIndex = 12;
            // 
            // dgvColumnInfo
            // 
            this.dgvColumnInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvColumnInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvColumnInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvColumnInfo.Name = "dgvColumnInfo";
            this.dgvColumnInfo.Size = new System.Drawing.Size(868, 616);
            this.dgvColumnInfo.TabIndex = 14;
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(14, 695);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(69, 15);
            this.lblCount.TabIndex = 16;
            this.lblCount.Text = "Row Count:";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabColumnInfo);
            this.tabs.Controls.Add(this.tabForeignKeyInfo);
            this.tabs.Location = new System.Drawing.Point(412, 41);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(882, 650);
            this.tabs.TabIndex = 17;
            // 
            // tabColumnInfo
            // 
            this.tabColumnInfo.Controls.Add(this.dgvColumnInfo);
            this.tabColumnInfo.Location = new System.Drawing.Point(4, 24);
            this.tabColumnInfo.Name = "tabColumnInfo";
            this.tabColumnInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabColumnInfo.Size = new System.Drawing.Size(874, 622);
            this.tabColumnInfo.TabIndex = 0;
            this.tabColumnInfo.Text = "Column Info";
            this.tabColumnInfo.UseVisualStyleBackColor = true;
            // 
            // tabForeignKeyInfo
            // 
            this.tabForeignKeyInfo.Controls.Add(this.dgvForeignKeyInfo);
            this.tabForeignKeyInfo.Location = new System.Drawing.Point(4, 24);
            this.tabForeignKeyInfo.Name = "tabForeignKeyInfo";
            this.tabForeignKeyInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabForeignKeyInfo.Size = new System.Drawing.Size(874, 622);
            this.tabForeignKeyInfo.TabIndex = 1;
            this.tabForeignKeyInfo.Text = "Foreign Key Info";
            this.tabForeignKeyInfo.UseVisualStyleBackColor = true;
            // 
            // cmbSchema
            // 
            this.cmbSchema.FormattingEnabled = true;
            this.cmbSchema.Location = new System.Drawing.Point(92, 70);
            this.cmbSchema.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSchema.Name = "cmbSchema";
            this.cmbSchema.Size = new System.Drawing.Size(313, 23);
            this.cmbSchema.TabIndex = 19;
            this.cmbSchema.SelectedIndexChanged += new System.EventHandler(this.cmbSchema_SelectedIndexChanged);
            // 
            // lblSchema
            // 
            this.lblSchema.AutoSize = true;
            this.lblSchema.Location = new System.Drawing.Point(12, 73);
            this.lblSchema.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSchema.Name = "lblSchema";
            this.lblSchema.Size = new System.Drawing.Size(52, 15);
            this.lblSchema.TabIndex = 18;
            this.lblSchema.Text = "Schema:";
            // 
            // cmdDataSource
            // 
            this.cmdDataSource.FormattingEnabled = true;
            this.cmdDataSource.Location = new System.Drawing.Point(92, 11);
            this.cmdDataSource.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmdDataSource.Name = "cmdDataSource";
            this.cmdDataSource.Size = new System.Drawing.Size(313, 23);
            this.cmdDataSource.TabIndex = 20;
            this.cmdDataSource.SelectedIndexChanged += new System.EventHandler(this.cmdDataSource_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 719);
            this.Controls.Add(this.cmdDataSource);
            this.Controls.Add(this.cmbSchema);
            this.Controls.Add(this.lblSchema);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvForeignKeyInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnInfo)).EndInit();
            this.tabs.ResumeLayout(false);
            this.tabColumnInfo.ResumeLayout(false);
            this.tabForeignKeyInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.DataGridView dgvForeignKeyInfo;
        private System.Windows.Forms.DataGridView dgvColumnInfo;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabColumnInfo;
        private System.Windows.Forms.TabPage tabForeignKeyInfo;
        private System.Windows.Forms.ComboBox cmbSchema;
        private System.Windows.Forms.Label lblSchema;
        private System.Windows.Forms.ComboBox cmdDataSource;
    }
}

