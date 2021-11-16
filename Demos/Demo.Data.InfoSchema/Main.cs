using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Extenso.Data.Common;
using Extenso.Data.SqlClient;

namespace Demo.Data.InfoSchema
{
    public partial class Main : Form
    {
        private const string SQL_CONNECTION_STRING_FORMAT = "Data Source={0};Initial Catalog={1};User={2}Password={3}";
        private const string SQL_CONNECTION_STRING_FORMAT_WA = "Data Source={0};Initial Catalog={1};Integrated Security=true";

        private string ConnectionString
        {
            get
            {
                if (cbUseWindowsAuthentication.Checked)
                {
                    return string.Format(
                        SQL_CONNECTION_STRING_FORMAT_WA,
                        cmbServer.SelectedItem.ToString(),
                        cmbDatabase.SelectedIndex != -1 ? cmbDatabase.SelectedItem.ToString() : "master");
                }
                else
                {
                    return string.Format(
                        SQL_CONNECTION_STRING_FORMAT,
                        cmbServer.SelectedItem.ToString(),
                        cmbDatabase.SelectedIndex != -1 ? cmbDatabase.SelectedItem.ToString() : "master",
                        txtUserName.Text.Trim(),
                        txtPassword.Text);
                }
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbServer.Items.AddRange(new[] { "." });
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if (!connection.Validate())
                { return; }

                cmbDatabase.DataSource = connection.GetDatabaseNames();
                //cmbDatabase.Items.AddRange(connection.GetDatabaseNames());
            }
        }

        private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabase.SelectedIndex != -1)
            {
                //lbTables.Items.Clear();
                using (var connection = new SqlConnection(ConnectionString))
                {
                    lbTables.DataSource = connection.GetTableNames();
                    //lbTables.Items.AddRange(connection.GetTableNames());
                }
            }
        }

        private void cbUseWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            txtUserName.Enabled = txtPassword.Enabled = !cbUseWindowsAuthentication.Checked;
        }

        private void lbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTables.SelectedIndex != -1)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    string tableName = lbTables.SelectedItem.ToString();
                    lblCount.Text = $"Row Count: {connection.GetRowCount("dbo", tableName)}";
                    dgvForeignKeyInfo.DataSource = connection.GetForeignKeyData(tableName);
                    dgvColumnInfo.DataSource = connection.GetColumnData(tableName);
                }
            }
        }
    }
}