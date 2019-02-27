using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Extenso.Collections;
using Extenso.Reflection;

namespace Extenso.Data.QueryBuilder
{
    public abstract class BaseSelectQueryBuilder : ISelectQueryBuilder
    {
        #region Non-Public Members

        protected string schema;
        protected readonly ICollection<string> groupByColumns = new List<string>();
        protected readonly IDictionary<string, string> selectedColumns = new Dictionary<string, string>();
        protected WhereStatement havingStatement = new WhereStatement();
        protected bool isDistinct;
        protected ICollection<JoinClause> joins = new List<JoinClause>();
        protected ICollection<OrderByClause> orderByStatement = new List<OrderByClause>();
        protected ICollection<string> selectedTables = new List<string>();
        protected WhereStatement whereStatement = new WhereStatement();
        protected IDictionary<string, string> tableAliases = new Dictionary<string, string>();

        protected int skipCount;
        protected int takeCount;

        #endregion Non-Public Members

        #region Public Methods

        public virtual ISelectQueryBuilder SelectAll()
        {
            selectedColumns.Clear();
            return this;
        }

        public virtual ISelectQueryBuilder SelectAs(string tableName, string column, string alias = null)
        {
            //selectedColumns.Clear();
            string columnName = CreateFieldName(tableName, column);
            selectedColumns.Add(columnName, alias);
            return this;
        }

        public virtual ISelectQueryBuilder Select(string tableName, params string[] columns)
        {
            selectedColumns.Clear();
            string enclosedTableName = EncloseTable(tableName);
            foreach (string column in columns)
            {
                selectedColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)), null);
            }
            return this;
        }

        public virtual ISelectQueryBuilder Select(IEnumerable<TableColumnPair> columns)
        {
            selectedColumns.Clear();
            foreach (var column in columns)
            {
                selectedColumns.Add(CreateFieldName(column.TableName, column.ColumnName), null);
            }
            return this;
        }

        public virtual ISelectQueryBuilder SelectCount()
        {
            selectedColumns.Clear();
            selectedColumns.Add("COUNT(1)", null);
            return this;
        }

        public virtual ISelectQueryBuilder Distinct(bool isDistinct = true)
        {
            this.isDistinct = isDistinct;
            return this;
        }

        public virtual ISelectQueryBuilder From(string tableName)
        {
            selectedTables.Clear();
            selectedTables.Add(EncloseTable(tableName));
            return this;
        }

        public virtual ISelectQueryBuilder From(params string[] tableNames)
        {
            selectedTables.Clear();
            foreach (string table in tableNames)
            {
                selectedTables.Add(EncloseTable(table));
            }
            return this;
        }

        public virtual ISelectQueryBuilder Join(
            JoinType joinType,
            string toTableName,
            string toColumnName,
            ComparisonOperator comparisonOperator,
            string fromTableName,
            string fromColumnName)
        {
            var join = new JoinClause(
                joinType,
                toTableName,
                toColumnName,
                comparisonOperator,
                fromTableName,
                fromColumnName);

            joins.Add(join);
            return this;
        }

        public virtual ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And)
        {
            var whereClause = new WhereClause(
                logicOperator,
                tableName,
                column,
                comparisonOperator,
                value);

            whereStatement.AddClause(whereClause);
            return this;
        }

        public virtual ISelectQueryBuilder Where(WhereStatement whereStatement)
        {
            this.whereStatement = whereStatement;
            return this;
        }

        public virtual ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection order)
        {
            var orderByClause = new OrderByClause(
                string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column)),
                order);

            orderByStatement.Add(orderByClause);
            return this;
        }

        public virtual ISelectQueryBuilder GroupBy(string tableName, params string[] columns)
        {
            string enclosedTableName = EncloseTable(tableName);
            foreach (string column in columns)
            {
                groupByColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)));
            }
            return this;
        }

        public virtual ISelectQueryBuilder GroupBy(IEnumerable<TableColumnPair> columns)
        {
            foreach (var column in columns)
            {
                groupByColumns.Add(CreateFieldName(column.TableName, column.ColumnName));
            }
            return this;
        }

        public virtual ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And)
        {
            var whereClause = new WhereClause(
                logicOperator,
                tableName,
                column,
                comparisonOperator,
                value);

            havingStatement.AddClause(whereClause);
            return this;
        }

        public virtual ISelectQueryBuilder Having(WhereStatement havingStatement)
        {
            this.havingStatement = havingStatement;
            return this;
        }

        public virtual ISelectQueryBuilder Skip(int count)
        {
            skipCount = count;
            return this;
        }

        public virtual ISelectQueryBuilder Take(int count)
        {
            takeCount = count;
            return this;
        }

        public abstract string BuildQuery();

        #endregion Public Methods

        #region Non-Public Methods

        protected abstract string EncloseIdentifier(string identifier);

        protected virtual string EncloseTable(string tableName)
        {
            if (!string.IsNullOrEmpty(schema))
            {
                return string.Concat(schema, '.', EncloseIdentifier(tableName));
            }
            return EncloseIdentifier(tableName);
        }

        protected virtual string CreateFieldName(string tableName, string column)
        {
            return string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column));
        }

        protected virtual string CreateWhereStatement(WhereStatement statement)
        {
            var sb = new StringBuilder();
            sb.Append("WHERE ");

            bool isFirst = true;
            foreach (var clause in statement.Clauses)
            {
                CreateWhereClause(clause, sb, isFirst);
                isFirst = false;
            }

            return sb.ToString();
        }

        protected virtual void CreateWhereClause(WhereClause clause, StringBuilder sb, bool isFirst = false)
        {
            if (!isFirst)
            {
                sb.Append(clause.LogicOperator == LogicOperator.Or ? "OR " : "AND ");
            }

            string fieldName = string.Concat(EncloseTable(clause.Table), '.', EncloseIdentifier(clause.Column));

            sb.Append("(");
            sb.Append(CreateComparisonClause(fieldName, clause.ComparisonOperator, clause.Value));
            sb.Append(") ");
            sb.Append(" ");

            if (!clause.SubClauses.IsNullOrEmpty())
            {
                sb.Append("AND (");

                isFirst = true;
                foreach (var subClause in clause.SubClauses)
                {
                    CreateWhereClause(subClause, sb, isFirst);
                    isFirst = false;
                }
                sb.Append(") ");
            }
        }

        protected virtual string CreateComparisonClause(string fieldName, ComparisonOperator comparisonOperator, object value)
        {
            string output = string.Empty;
            if (value != null && value != System.DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case ComparisonOperator.EqualTo: output = fieldName + " = " + FormatSQLValue(value); break;
                    case ComparisonOperator.NotEqualTo: output = fieldName + " <> " + FormatSQLValue(value); break;
                    case ComparisonOperator.GreaterThan: output = fieldName + " > " + FormatSQLValue(value); break;
                    case ComparisonOperator.GreaterThanOrEqualTo: output = fieldName + " >= " + FormatSQLValue(value); break;
                    case ComparisonOperator.LessThan: output = fieldName + " < " + FormatSQLValue(value); break;
                    case ComparisonOperator.LessThanOrEqualTo: output = fieldName + " <= " + FormatSQLValue(value); break;
                    case ComparisonOperator.Like: output = fieldName + " LIKE " + FormatSQLValue(value); break;
                    case ComparisonOperator.NotLike: output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value); break;
                    case ComparisonOperator.In: output = fieldName + " IN (" + FormatSQLValue(value) + ")"; break;
                    case ComparisonOperator.Contains: output = string.Format("{0} LIKE {1}", fieldName, FormatSQLValue("%" + value + "%")); break;
                    case ComparisonOperator.NotContains: output = string.Format("NOT {0} LIKE {1}", fieldName, FormatSQLValue("%" + value + "%")); break;
                    case ComparisonOperator.StartsWith: output = string.Format("{0} LIKE {1}", fieldName, FormatSQLValue(value + "%")); break;
                    case ComparisonOperator.EndsWith: output = string.Format("{0} LIKE {1}", fieldName, FormatSQLValue("%" + value)); break;
                }
            }
            else // value==null	|| value==DBNull.Value
            {
                if ((comparisonOperator != ComparisonOperator.EqualTo) && (comparisonOperator != ComparisonOperator.NotEqualTo))
                {
                    throw new Exception("Cannot use comparison operator " + comparisonOperator.ToString() + " for NULL values.");
                }
                else
                {
                    switch (comparisonOperator)
                    {
                        case ComparisonOperator.EqualTo: output = fieldName + " IS NULL"; break;
                        case ComparisonOperator.NotEqualTo: output = "NOT " + fieldName + " IS NULL"; break;
                    }
                }
            }
            return output;
        }

        protected virtual string FormatSQLValue(object someValue)
        {
            string formattedValue = string.Empty;
            // string StringType = Type.GetType("string").Name;
            // string DateTimeType = Type.GetType("DateTime").Name;

            if (someValue == null)
            {
                formattedValue = "NULL";
                return formattedValue;
            }

            var type = someValue.GetType();

            if (type.IsCollection())
            {
                var collection = (someValue as IEnumerable);
                Type collectionType;

                var typeInfo = type.GetTypeInfo();
                if (typeInfo.IsGenericType)
                {
                    collectionType = typeInfo.GetGenericArguments().Single();
                }
                else
                {
                    var firstItem = collection.OfType<object>().FirstOrDefault();
                    if (firstItem == null)
                    {
                        formattedValue = "NULL";
                        return formattedValue;
                    }
                    else
                    {
                        collectionType = firstItem.GetType();
                    }
                }

                switch (collectionType.Name)
                {
                    case "String":
                        formattedValue = string.Join("','", collection.OfType<string>()).EnquoteSingle();
                        break;

                    case "DateTime":
                        formattedValue = string.Join("','", collection.OfType<DateTime>().Select(x => x.ToString("yyyy/MM/dd HH:mm:ss"))).EnquoteSingle();
                        break;

                    case "Guid":
                        formattedValue = string.Join("','", collection.OfType<Guid>().Select(x => x.ToString())).EnquoteSingle();
                        break;

                    case "SqlLiteral":
                        formattedValue = string.Join(",", collection.OfType<SqlLiteral>().Select(x => x.Value));
                        break;

                    case "DBNull": formattedValue = "NULL"; break;
                    default: formattedValue = string.Join(",", collection.OfType<object>()); break;
                }
            }
            else
            {
                switch (type.Name)
                {
                    case "String": formattedValue = string.Format("'{0}'", ((string)someValue).Replace("'", "''")); break;
                    case "DateTime": formattedValue = string.Format("'{0:yyyy/MM/dd HH:mm:ss}'", (DateTime)someValue); break;
                    case "Guid": formattedValue = string.Format("'{0}'", (Guid)someValue); break;
                    case "Boolean": formattedValue = (bool)someValue ? "1" : "0"; break;
                    case "SqlLiteral": formattedValue = ((SqlLiteral)someValue).Value; break;
                    case "DBNull": formattedValue = "NULL"; break;
                    default: formattedValue = someValue.ToString(); break;
                }
            }
            return formattedValue;
        }

        #endregion Non-Public Methods
    }
}