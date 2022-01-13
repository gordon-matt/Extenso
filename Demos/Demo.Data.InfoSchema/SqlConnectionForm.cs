using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Extenso.Collections;
using Extenso.Data.Common;
using Extenso.Data.SqlClient;

namespace Demo.Data.InfoSchema
{
    public partial class SqlConnectionForm : Form
    {
        private const string SQL_CONNECTION_STRING_FORMAT = "Data Source={0};Initial Catalog={1};User={2}Password={3}";
        private const string SQL_CONNECTION_STRING_FORMAT_WA = "Data Source={0};Initial Catalog={1};Integrated Security=true";

        public bool AllowTableSelection
        {
            get => cmbTable.Visible;
            set
            {
                lblTable.Visible = value;
                cmbTable.Visible = value;
            }
        }

        public string Server
        {
            get
            {
                if (cmbServerName.SelectedIndex != -1)
                {
                    return cmbServerName.SelectedItem.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(cmbServerName.Text))
                {
                    return cmbServerName.Text;
                }
                return string.Empty;
            }
            set
            {
                if (cmbServerName.Items.Count > 0)
                {
                    cmbServerName.SelectedItem = value;
                }
                else { cmbServerName.Text = value; }
            }
        }

        public string Database
        {
            get
            {
                if (cmbDatabase.SelectedIndex != -1)
                {
                    return cmbDatabase.SelectedItem.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(cmbDatabase.Text))
                {
                    return cmbDatabase.Text;
                }
                return "master";
            }
            set
            {
                if (cmbDatabase.Items.Count > 0)
                {
                    cmbDatabase.SelectedItem = value;
                }
                else { cmbDatabase.Text = value; }
            }
        }

        public string UserName
        {
            get => txtUserName.Text.Trim();
            set => txtUserName.Text = value;
        }

        public string Password
        {
            get => txtPassword.Text.Trim();
            set => txtPassword.Text = value;
        }

        public bool IntegratedSecurity
        {
            get => rbUseWindowsAuthentication.Checked;
            set => rbUseWindowsAuthentication.Checked = value;
        }

        public string ConnectionString
        {
            get
            {
                #region Checks

                if (string.IsNullOrWhiteSpace(Server))
                {
                    MessageBox.Show(
                        "Server is invalid. Please try again.",
                        "Invalid",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return string.Empty;
                }
                if (string.IsNullOrWhiteSpace(Database))
                {
                    MessageBox.Show(
                        "Database is invalid. Please try again.",
                        "Invalid",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return string.Empty;
                }
                if (!IntegratedSecurity)
                {
                    if (string.IsNullOrWhiteSpace(UserName))
                    {
                        MessageBox.Show(
                            "User Name is invalid. Please try again.",
                            "Invalid",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return string.Empty;
                    }
                }

                #endregion Checks

                if (IntegratedSecurity)
                {
                    return string.Format(SQL_CONNECTION_STRING_FORMAT_WA, Server, Database);
                }
                else
                {
                    return string.Format(SQL_CONNECTION_STRING_FORMAT, Server, Database, UserName, Password);
                }
            }
        }

        public SqlConnectionForm()
        {
            InitializeComponent();
        }

        private void btnRefreshServers_Click(object sender, EventArgs e)
        {
            //SqlDataSourceEnumerator.Instance.GetAvailableSqlServers().ForEach(x => cmbServerName.Items.Add(x));
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (connection.Validate())
                {
                    MessageBox.Show(
                        "Connected to Sql Server.",
                        "Success!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    MessageBox.Show(
                        "Could not connect to Sql Server.",
                        "Error!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabase.SelectedIndex != -1)
            {
                cmbTable.Items.Clear();

                if (AllowTableSelection)
                {
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        string databaseName = cmbDatabase.SelectedItem.ToString();
                        connection.GetTableNames(databaseName).ForEach(x => cmbTable.Items.Add(x));
                    }
                }
            }
        }

        private void cmbServerName_DropDown(object sender, EventArgs e)
        {
            if (cmbServerName.Items.Count == 0)
            {
                //SqlDataSourceEnumerator.Instance.GetAvailableSqlServers().ForEach(x => cmbServerName.Items.Add(x));
            }
        }

        private void rbUseWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            lblUserName.Enabled = rbUseWindowsAuthentication.Checked;
            txtUserName.Enabled = rbUseWindowsAuthentication.Checked;
            lblPassword.Enabled = rbUseWindowsAuthentication.Checked;
            txtPassword.Enabled = rbUseWindowsAuthentication.Checked;
        }

        private void cmbDatabase_DropDown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Server))
            {
                cmbDatabase.Items.Clear();
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
                }
            }
        }
    }
}