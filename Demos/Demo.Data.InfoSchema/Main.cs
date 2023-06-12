using System;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using Extenso;
using Extenso.Data.Common;
using Extenso.Data.MySql;
using Extenso.Data.Npgsql;
using Extenso.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;

namespace Demo.Data.InfoSchema
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private enum DataSource : byte
        {
            SqlServer,
            MySql,
            PostgreSql
        }

        private string ConnectionString => txtConnectionString.Text;

        private string SelectedDatabase => cmbDatabase.Text;
        private DataSource SelectedDataSource => (DataSource)cmdDataSource.SelectedItem;
        private string SelectedSchema => cmbSchema.Text;

        private DbConnection CreateConnection() => SelectedDataSource switch
        {
            DataSource.SqlServer => new SqlConnection(ConnectionString),
            DataSource.MySql => new MySqlConnection(ConnectionString),
            DataSource.PostgreSql => new NpgsqlConnection(ConnectionString),
            _ => null,
        };

        #region Event Handlers

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
        private void btnConnect_Click(object sender, EventArgs e)
        {
            using var connection = CreateConnection();
            if (!connection.Validate())
            {
                return;
            }

            switch (SelectedDataSource)
            {
                case DataSource.SqlServer: cmbDatabase.DataSource = (connection as SqlConnection).GetDatabaseNames(); break;
                case DataSource.MySql: cmbDatabase.DataSource = (connection as MySqlConnection).GetDatabaseNames(); break;
                case DataSource.PostgreSql: cmbDatabase.DataSource = (connection as NpgsqlConnection).GetDatabaseNames(); break;
                default: break;
            }

            cmbDatabase.Select();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
        private void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabase.SelectedIndex != -1)
            {
                using var connection = CreateConnection();
                connection.Open();
                connection.ChangeDatabase(SelectedDatabase);
                switch (SelectedDataSource)
                {
                    case DataSource.SqlServer:
                        cmbSchema.DataSource = (connection as SqlConnection).GetSchemaNames();
                        cmbSchema.Select();
                        break;

                    case DataSource.MySql:
                        lbTables.DataSource = (connection as MySqlConnection).GetTableNames(includeViews: true);
                        lbTables.Select();
                        break;

                    case DataSource.PostgreSql:
                        cmbSchema.DataSource = (connection as NpgsqlConnection).GetSchemaNames();
                        cmbSchema.Select();
                        break;

                    default: break;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
        private void cmbSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSchema.SelectedIndex != -1)
            {
                using var connection = CreateConnection();
                connection.Open();
                connection.ChangeDatabase(SelectedDatabase);
                switch (SelectedDataSource)
                {
                    case DataSource.SqlServer: lbTables.DataSource = (connection as SqlConnection).GetTableNames(includeViews: true, SelectedSchema); break;
                    case DataSource.MySql: lbTables.DataSource = (connection as MySqlConnection).GetTableNames(includeViews: true); break;
                    case DataSource.PostgreSql: lbTables.DataSource = (connection as NpgsqlConnection).GetTableNames(includeViews: true, SelectedSchema); break;
                    default: break;
                }
            }

            lbTables.Select();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
        private void cmdDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSchema.Enabled = SelectedDataSource != DataSource.MySql;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Exception to naming rule for WinForms event handlers.")]
        private void lbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTables.SelectedIndex != -1)
            {
                using var connection = CreateConnection();
                connection.Open();
                connection.ChangeDatabase(SelectedDatabase);
                string tableName = lbTables.SelectedItem.ToString();

                switch (SelectedDataSource)
                {
                    case DataSource.SqlServer:
                        {
                            var sqlConnection = connection as SqlConnection;
                            lblCount.Text = $"Row Count: {sqlConnection.GetRowCount(SelectedSchema, tableName)}";
                            dgvForeignKeyInfo.DataSource = sqlConnection.GetForeignKeyData(tableName, SelectedSchema);
                            dgvColumnInfo.DataSource = sqlConnection.GetColumnData(tableName, SelectedSchema);
                        }
                        break;

                    case DataSource.MySql:
                        {
                            var mySqlConnection = connection as MySqlConnection;
                            lblCount.Text = $"Row Count: {mySqlConnection.GetRowCount(tableName)}";
                            dgvForeignKeyInfo.DataSource = mySqlConnection.GetForeignKeyData(tableName);
                            dgvColumnInfo.DataSource = mySqlConnection.GetColumnData(tableName);
                        }
                        break;

                    case DataSource.PostgreSql:
                        {
                            var npgsqlConnection = connection as NpgsqlConnection;
                            lblCount.Text = $"Row Count: {npgsqlConnection.GetRowCount(SelectedSchema, tableName)}";
                            dgvForeignKeyInfo.DataSource = npgsqlConnection.GetForeignKeyData(tableName, SelectedSchema);
                            dgvColumnInfo.DataSource = npgsqlConnection.GetColumnData(tableName, SelectedSchema);
                        }
                        break;

                    default: break;
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            cmdDataSource.DataSource = EnumExtensions.GetValues<DataSource>();
        }

        #endregion Event Handlers
    }
}