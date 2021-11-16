namespace Demo.Data.InfoSchema
{
    partial class SqlConnectionForm
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
            this.lblServerName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnRefreshServers = new System.Windows.Forms.Button();
            this.cmbServerName = new System.Windows.Forms.ComboBox();
            this.grpLogOnServer = new System.Windows.Forms.GroupBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.rbUseSqlServerAuthentication = new System.Windows.Forms.RadioButton();
            this.rbUseWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.grpConnectDatabase = new System.Windows.Forms.GroupBox();
            this.lblTable = new System.Windows.Forms.Label();
            this.cmbTable = new System.Windows.Forms.ComboBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.grpLogOnServer.SuspendLayout();
            this.grpConnectDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.Location = new System.Drawing.Point(12, 9);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(78, 20);
            this.lblServerName.TabIndex = 4;
            this.lblServerName.Text = "Server name";
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(109, 77);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(248, 23);
            this.txtUserName.TabIndex = 5;
            // 
            // btnRefreshServers
            // 
            this.btnRefreshServers.Location = new System.Drawing.Point(309, 33);
            this.btnRefreshServers.Name = "btnRefreshServers";
            this.btnRefreshServers.Size = new System.Drawing.Size(75, 28);
            this.btnRefreshServers.TabIndex = 6;
            this.btnRefreshServers.Text = "Refresh";
            this.btnRefreshServers.Click += new System.EventHandler(this.btnRefreshServers_Click);
            // 
            // cmbServerName
            // 
            this.cmbServerName.DropDownWidth = 291;
            this.cmbServerName.FormattingEnabled = true;
            this.cmbServerName.Location = new System.Drawing.Point(12, 35);
            this.cmbServerName.Name = "cmbServerName";
            this.cmbServerName.Size = new System.Drawing.Size(291, 21);
            this.cmbServerName.Sorted = true;
            this.cmbServerName.TabIndex = 7;
            this.cmbServerName.DropDown += new System.EventHandler(this.cmbServerName_DropDown);
            // 
            // grpLogOnServer
            // 
            this.grpLogOnServer.Controls.Add(this.lblPassword);
            this.grpLogOnServer.Controls.Add(this.txtPassword);
            this.grpLogOnServer.Controls.Add(this.lblUserName);
            this.grpLogOnServer.Controls.Add(this.rbUseSqlServerAuthentication);
            this.grpLogOnServer.Controls.Add(this.rbUseWindowsAuthentication);
            this.grpLogOnServer.Controls.Add(this.txtUserName);
            this.grpLogOnServer.Location = new System.Drawing.Point(12, 62);
            this.grpLogOnServer.Name = "grpLogOnServer";
            this.grpLogOnServer.Size = new System.Drawing.Size(372, 141);
            this.grpLogOnServer.TabIndex = 8;
            this.grpLogOnServer.TabStop = false;
            this.grpLogOnServer.Text = "Log on to the server";
            // 
            // lblPassword
            // 
            this.lblPassword.Enabled = false;
            this.lblPassword.Location = new System.Drawing.Point(32, 106);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(62, 20);
            this.lblPassword.TabIndex = 8;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(109, 106);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(248, 23);
            this.txtPassword.TabIndex = 7;
            // 
            // lblUserName
            // 
            this.lblUserName.Enabled = false;
            this.lblUserName.Location = new System.Drawing.Point(32, 77);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(71, 20);
            this.lblUserName.TabIndex = 6;
            this.lblUserName.Text = "User Name";
            // 
            // rbUseSqlServerAuthentication
            // 
            this.rbUseSqlServerAuthentication.Location = new System.Drawing.Point(14, 51);
            this.rbUseSqlServerAuthentication.Name = "rbUseSqlServerAuthentication";
            this.rbUseSqlServerAuthentication.Size = new System.Drawing.Size(189, 20);
            this.rbUseSqlServerAuthentication.TabIndex = 1;
            this.rbUseSqlServerAuthentication.Text = "Use SQL Server Authentication";
            // 
            // rbUseWindowsAuthentication
            // 
            this.rbUseWindowsAuthentication.Checked = true;
            this.rbUseWindowsAuthentication.Location = new System.Drawing.Point(14, 25);
            this.rbUseWindowsAuthentication.Name = "rbUseWindowsAuthentication";
            this.rbUseWindowsAuthentication.Size = new System.Drawing.Size(180, 20);
            this.rbUseWindowsAuthentication.TabIndex = 0;
            this.rbUseWindowsAuthentication.Text = "Use Windows Authentication";
            this.rbUseWindowsAuthentication.CheckedChanged += new System.EventHandler(this.rbUseWindowsAuthentication_CheckedChanged);
            // 
            // grpConnectDatabase
            // 
            this.grpConnectDatabase.Controls.Add(this.lblTable);
            this.grpConnectDatabase.Controls.Add(this.cmbTable);
            this.grpConnectDatabase.Controls.Add(this.lblDatabase);
            this.grpConnectDatabase.Controls.Add(this.cmbDatabase);
            this.grpConnectDatabase.Location = new System.Drawing.Point(12, 221);
            this.grpConnectDatabase.Name = "grpConnectDatabase";
            this.grpConnectDatabase.Size = new System.Drawing.Size(372, 108);
            this.grpConnectDatabase.TabIndex = 9;
            this.grpConnectDatabase.TabStop = false;
            this.grpConnectDatabase.Text = "Connect to a database";
            // 
            // lblTable
            // 
            this.lblTable.Location = new System.Drawing.Point(32, 51);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(40, 20);
            this.lblTable.TabIndex = 11;
            this.lblTable.Text = "Table";
            this.lblTable.Visible = false;
            // 
            // cmbTable
            // 
            this.cmbTable.DropDownWidth = 248;
            this.cmbTable.FormattingEnabled = true;
            this.cmbTable.Location = new System.Drawing.Point(109, 51);
            this.cmbTable.Name = "cmbTable";
            this.cmbTable.Size = new System.Drawing.Size(248, 21);
            this.cmbTable.Sorted = true;
            this.cmbTable.TabIndex = 10;
            this.cmbTable.Visible = false;
            // 
            // lblDatabase
            // 
            this.lblDatabase.Location = new System.Drawing.Point(32, 24);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(61, 20);
            this.lblDatabase.TabIndex = 9;
            this.lblDatabase.Text = "Database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownWidth = 248;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(109, 24);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(248, 21);
            this.cmbDatabase.Sorted = true;
            this.cmbDatabase.TabIndex = 0;
            this.cmbDatabase.DropDown += new System.EventHandler(this.cmbDatabase_DropDown);
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(283, 335);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(177, 335);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "OK";
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestConnection.Location = new System.Drawing.Point(11, 335);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(100, 30);
            this.btnTestConnection.TabIndex = 12;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // SqlConnectionForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(395, 377);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpConnectDatabase);
            this.Controls.Add(this.grpLogOnServer);
            this.Controls.Add(this.cmbServerName);
            this.Controls.Add(this.btnRefreshServers);
            this.Controls.Add(this.lblServerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Connection";
            this.grpLogOnServer.ResumeLayout(false);
            this.grpLogOnServer.PerformLayout();
            this.grpConnectDatabase.ResumeLayout(false);
            this.grpConnectDatabase.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnRefreshServers;
        private System.Windows.Forms.ComboBox cmbServerName;
        private System.Windows.Forms.GroupBox grpLogOnServer;
        private System.Windows.Forms.RadioButton rbUseSqlServerAuthentication;
        private System.Windows.Forms.RadioButton rbUseWindowsAuthentication;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.GroupBox grpConnectDatabase;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.ComboBox cmbTable;
        private System.Windows.Forms.Label lblDatabase;
    }
}