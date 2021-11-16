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
            this.lblServer = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.cbUseWindowsAuthentication = new System.Windows.Forms.CheckBox();
            this.dgvForeignKeyInfo = new System.Windows.Forms.DataGridView();
            this.lblForeignKeyInfo = new System.Windows.Forms.Label();
            this.dgvColumnInfo = new System.Windows.Forms.DataGridView();
            this.lblColumnInfo = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForeignKeyInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(14, 10);
            this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(42, 15);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Server:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(321, 98);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(88, 27);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(94, 7);
            this.cmbServer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(313, 23);
            this.cmbServer.TabIndex = 3;
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(94, 135);
            this.cmbDatabase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(313, 23);
            this.cmbDatabase.TabIndex = 5;
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(14, 138);
            this.lblDatabase.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(58, 15);
            this.lblDatabase.TabIndex = 4;
            this.lblDatabase.Text = "Database:";
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(94, 38);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(313, 23);
            this.txtUserName.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(94, 68);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(313, 23);
            this.txtPassword.TabIndex = 7;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(14, 42);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(68, 15);
            this.lblUserName.TabIndex = 8;
            this.lblUserName.Text = "User Name:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(14, 72);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(60, 15);
            this.lblPassword.TabIndex = 9;
            this.lblPassword.Text = "Password:";
            // 
            // lbTables
            // 
            this.lbTables.FormattingEnabled = true;
            this.lbTables.ItemHeight = 15;
            this.lbTables.Location = new System.Drawing.Point(14, 166);
            this.lbTables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lbTables.Name = "lbTables";
            this.lbTables.Size = new System.Drawing.Size(394, 319);
            this.lbTables.TabIndex = 10;
            this.lbTables.SelectedIndexChanged += new System.EventHandler(this.lbTables_SelectedIndexChanged);
            // 
            // cbUseWindowsAuthentication
            // 
            this.cbUseWindowsAuthentication.AutoSize = true;
            this.cbUseWindowsAuthentication.Checked = true;
            this.cbUseWindowsAuthentication.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseWindowsAuthentication.Location = new System.Drawing.Point(94, 103);
            this.cbUseWindowsAuthentication.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbUseWindowsAuthentication.Name = "cbUseWindowsAuthentication";
            this.cbUseWindowsAuthentication.Size = new System.Drawing.Size(179, 19);
            this.cbUseWindowsAuthentication.TabIndex = 11;
            this.cbUseWindowsAuthentication.Text = "Use Windows Authentication";
            this.cbUseWindowsAuthentication.UseVisualStyleBackColor = true;
            this.cbUseWindowsAuthentication.CheckedChanged += new System.EventHandler(this.cbUseWindowsAuthentication_CheckedChanged);
            // 
            // dgvForeignKeyInfo
            // 
            this.dgvForeignKeyInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvForeignKeyInfo.Location = new System.Drawing.Point(419, 25);
            this.dgvForeignKeyInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvForeignKeyInfo.Name = "dgvForeignKeyInfo";
            this.dgvForeignKeyInfo.Size = new System.Drawing.Size(581, 210);
            this.dgvForeignKeyInfo.TabIndex = 12;
            // 
            // lblForeignKeyInfo
            // 
            this.lblForeignKeyInfo.AutoSize = true;
            this.lblForeignKeyInfo.Location = new System.Drawing.Point(415, 7);
            this.lblForeignKeyInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForeignKeyInfo.Name = "lblForeignKeyInfo";
            this.lblForeignKeyInfo.Size = new System.Drawing.Size(96, 15);
            this.lblForeignKeyInfo.TabIndex = 13;
            this.lblForeignKeyInfo.Text = "Foreign Key Info:";
            // 
            // dgvColumnInfo
            // 
            this.dgvColumnInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumnInfo.Location = new System.Drawing.Point(419, 276);
            this.dgvColumnInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvColumnInfo.Name = "dgvColumnInfo";
            this.dgvColumnInfo.Size = new System.Drawing.Size(581, 210);
            this.dgvColumnInfo.TabIndex = 14;
            // 
            // lblColumnInfo
            // 
            this.lblColumnInfo.AutoSize = true;
            this.lblColumnInfo.Location = new System.Drawing.Point(415, 257);
            this.lblColumnInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColumnInfo.Name = "lblColumnInfo";
            this.lblColumnInfo.Size = new System.Drawing.Size(77, 15);
            this.lblColumnInfo.TabIndex = 15;
            this.lblColumnInfo.Text = "Column Info:";
            // 
            // lblCount
            // 
            this.lblCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(14, 499);
            this.lblCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(69, 15);
            this.lblCount.TabIndex = 16;
            this.lblCount.Text = "Row Count:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 523);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblColumnInfo);
            this.Controls.Add(this.dgvColumnInfo);
            this.Controls.Add(this.lblForeignKeyInfo);
            this.Controls.Add(this.dgvForeignKeyInfo);
            this.Controls.Add(this.cbUseWindowsAuthentication);
            this.Controls.Add(this.lbTables);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvForeignKeyInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumnInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.ListBox lbTables;
        private System.Windows.Forms.CheckBox cbUseWindowsAuthentication;
        private System.Windows.Forms.DataGridView dgvForeignKeyInfo;
        private System.Windows.Forms.Label lblForeignKeyInfo;
        private System.Windows.Forms.DataGridView dgvColumnInfo;
        private System.Windows.Forms.Label lblColumnInfo;
        private System.Windows.Forms.Label lblCount;
    }
}

