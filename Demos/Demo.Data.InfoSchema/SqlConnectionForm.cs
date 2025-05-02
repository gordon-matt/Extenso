using System.ComponentModel;
using Extenso.Collections;
using Extenso.Data.Common;
using Extenso.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Demo.Data.InfoSchema;

public partial class SqlConnectionForm : Form
{
    private const string SQL_CONNECTION_STRING_FORMAT = "Data Source={0};Initial Catalog={1};User={2}Password={3}";
    private const string SQL_CONNECTION_STRING_FORMAT_WA = "Data Source={0};Initial Catalog={1};Integrated Security=true";

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool AllowTableSelection
    {
        get => cmbTable.Visible;
        set
        {
            lblTable.Visible = value;
            cmbTable.Visible = value;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string UserName
    {
        get => txtUserName.Text.Trim();
        set => txtUserName.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string Password
    {
        get => txtPassword.Text.Trim();
        set => txtPassword.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
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

            return IntegratedSecurity
                ? string.Format(SQL_CONNECTION_STRING_FORMAT_WA, Server, Database)
                : string.Format(SQL_CONNECTION_STRING_FORMAT, Server, Database, UserName, Password);
        }
    }

    public SqlConnectionForm()
    {
        InitializeComponent();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void btnRefreshServers_Click(object sender, EventArgs e)
    {
        //SqlDataSourceEnumerator.Instance.GetAvailableSqlServers().ForEach(x => cmbServerName.Items.Add(x));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void btnTestConnection_Click(object sender, EventArgs e)
    {
        using var connection = new SqlConnection(ConnectionString);
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbDatabase.SelectedIndex != -1)
        {
            cmbTable.Items.Clear();

            if (AllowTableSelection)
            {
                using var connection = new SqlConnection(ConnectionString);
                string databaseName = cmbDatabase.SelectedItem.ToString();
                connection.GetTableNames(databaseName).ForEach(x => cmbTable.Items.Add(x));
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void cmbServerName_DropDown(object sender, EventArgs e)
    {
        if (cmbServerName.Items.Count == 0)
        {
            //SqlDataSourceEnumerator.Instance.GetAvailableSqlServers().ForEach(x => cmbServerName.Items.Add(x));
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void rbUseWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
    {
        lblUserName.Enabled = rbUseWindowsAuthentication.Checked;
        txtUserName.Enabled = rbUseWindowsAuthentication.Checked;
        lblPassword.Enabled = rbUseWindowsAuthentication.Checked;
        txtPassword.Enabled = rbUseWindowsAuthentication.Checked;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
    private void cmbDatabase_DropDown(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Server))
        {
            cmbDatabase.Items.Clear();
            using var connection = new SqlConnection(ConnectionString);
            connection.GetDatabaseNames().ForEach(x => cmbDatabase.Items.Add(x));
        }
    }
}