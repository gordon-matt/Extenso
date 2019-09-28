using Extenso.Collections;
using Extenso.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public virtual ISelectQueryBuilder Select(SqlLiteral literal)
        {
            selectedColumns.Add(literal.Value, null);
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

        public virtual ISelectQueryBuilder Where(WhereClause whereClause)
        {
            whereStatement.AddClause(whereClause);
            return this;
        }

        public virtual ISelectQueryBuilder Where(WhereStatement whereStatement)
        {
            this.whereStatement = whereStatement;
            return this;
        }

        public virtual ISelectQueryBuilder Where(string literal)
        {
            whereStatement = WhereStatement.CreateFromLiteral(literal);
            return this;
        }

        public virtual ISelectQueryBuilder OrderBy(string column, SortDirection order)
        {
            orderByStatement.Add(new OrderByClause(column, order));
            return this;
        }

        public virtual ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection order)
        {
            orderByStatement.Add(new OrderByClause(CreateFieldName(tableName, column), order));
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

        public virtual ISelectQueryBuilder Having(WhereClause whereClause)
        {
            havingStatement.AddClause(whereClause);
            return this;
        }

        public virtual ISelectQueryBuilder Having(WhereStatement havingStatement)
        {
            this.havingStatement = havingStatement;
            return this;
        }

        public virtual ISelectQueryBuilder Having(string literal)
        {
            havingStatement = WhereStatement.CreateFromLiteral(literal);
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
            return $"{EncloseTable(tableName)}.{EncloseIdentifier(column)}";
        }

        protected virtual string CreateWhereStatement(WhereStatement statement, bool isHaving)
        {
            if (!string.IsNullOrEmpty(statement.Literal))
            {
                if (isHaving)
                {
                    return $"HAVING {statement.Literal}";
                }

                return $"WHERE {statement.Literal}";
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(isHaving ? "HAVING " : "WHERE ");

                bool isFirst = true;
                foreach (var clause in statement)
                {
                    CreateWhereClause(clause, sb, isFirst);
                    isFirst = false;
                }

                return sb.ToString();
            }
        }

        protected virtual void CreateWhereClause(WhereClause clause, StringBuilder sb, bool isFirst)
        {
            if (!isFirst)
            {
                sb.Append(clause.LogicOperator == LogicOperator.Or ? "OR " : "AND ");
            }

            // Encapsulate if this is a container OR if there are subclauses
            bool doEncapsulateOuter = clause.IsContainerOnly || !clause.SubClauses.IsNullOrEmpty();

            if (doEncapsulateOuter)
            {
                sb.Append("(");
            }

            if (!clause.IsContainerOnly)
            {
                sb.Append(CreateComparisonClause(clause.Table, clause.Column, clause.ComparisonOperator, clause.Value));
                sb.Append(" ");
            }

            if (!clause.SubClauses.IsNullOrEmpty())
            {
                bool doEncapsulateInner = (!clause.IsContainerOnly);

                if (doEncapsulateInner)
                {
                    sb.Append("AND (");
                }

                for (int i = 0; i < clause.SubClauses.Count; i++)
                {
                    var subClause = clause.SubClauses.ElementAt(i);
                    CreateWhereClause(subClause, sb, i == 0);
                }
                if (doEncapsulateInner)
                {
                    sb.Append(") ");
                }
            }

            if (doEncapsulateOuter)
            {
                sb.Append(") ");
            }

            sb.Append(" ");
        }

        protected virtual string CreateComparisonClause(string tableName, string columnName, ComparisonOperator comparisonOperator, object value)
        {
            string fieldName = CreateFieldName(tableName, columnName);

            string output = string.Empty;
            if (value != null && value != DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case ComparisonOperator.EqualTo: output = $"{fieldName} = {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.NotEqualTo: output = $"{fieldName} <> {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.GreaterThan: output = $"{fieldName} > {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.GreaterThanOrEqualTo: output = $"{fieldName} >= {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.LessThan: output = $"{fieldName} < {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.LessThanOrEqualTo: output = $"{fieldName} <= {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.Like: output = $"{fieldName} LIKE {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.NotLike: output = $"NOT {fieldName} LIKE {FormatSQLValue(value)}"; break;
                    case ComparisonOperator.In:
                        {
                            var type = value.GetType();
                            if (type.IsCollection())
                            {
                                var collection = (value as IEnumerable);
                                var collectionType = GetCollectionType(collection, type);
                                if (collectionType == typeof(DateTime))
                                {
                                    // IN() won't work for dates. Need to substitue multiple date ranges instead..
                                    //  ..for the start and end of each day in `value`
                                    var values = collection.OfType<DateTime>().ToArray();
                                    var clause = GetDateRangesForInOperator(tableName, columnName, values);
                                    var sb = new StringBuilder();
                                    CreateWhereClause(clause, sb, true);
                                    return sb.ToString();
                                }
                            }

                            output = $"{fieldName} IN ({FormatSQLValue(value)})";
                        }
                        break;

                    case ComparisonOperator.Contains: output = $"{fieldName} LIKE {FormatSQLValue("%" + value + "%")}"; break;
                    case ComparisonOperator.NotContains: output = $"NOT {fieldName} LIKE {FormatSQLValue("%" + value + "%")}"; break;
                    case ComparisonOperator.StartsWith: output = $"{fieldName} LIKE {FormatSQLValue(value + "%")}"; break;
                    case ComparisonOperator.EndsWith: output = $"{fieldName} LIKE {FormatSQLValue("%" + value)}"; break;
                    case ComparisonOperator.HasFlag: output = $"{fieldName} & {value} <> 0"; break;
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
                        case ComparisonOperator.EqualTo: output = $"{fieldName} IS NULL"; break;
                        case ComparisonOperator.NotEqualTo: output = $"{fieldName} IS NOT NULL"; break;
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
                var collectionType = GetCollectionType(collection, type);
                if (collectionType == null)
                {
                    formattedValue = "NULL";
                    return formattedValue;
                }

                switch (collectionType.Name)
                {
                    case "String":
                        formattedValue = string.Join("','", collection.OfType<string>()).EnquoteSingle();
                        break;

                    case "DateTime":
                        formattedValue = string.Join("','", collection.OfType<DateTime>().Select(x => x.ToString("yyyy-MM-dd HH:mm:ss"))).EnquoteSingle();
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

        private Type GetCollectionType(IEnumerable collection, Type parentType)
        {
            var typeInfo = parentType.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                return typeInfo.GetGenericArguments().Single();
            }
            else
            {
                var firstItem = collection.OfType<object>().FirstOrDefault();
                if (firstItem != null)
                {
                    return firstItem.GetType();
                }
            }

            return null;
        }

        private WhereClause GetDateRangesForInOperator(string tableName, string columnName, IEnumerable<DateTime> values)
        {
            var outerContainer = WhereClause.CreateContainer(LogicOperator.And);

            int count = values.Count();
            for (int i = 0; i < count; i++)
            {
                var dateTime = values.ElementAt(i);

                var innerContainer = WhereClause.CreateContainer(i == 0 ? LogicOperator.And : LogicOperator.Or);

                var clauseFrom = new WhereClause(LogicOperator.And, tableName, columnName, ComparisonOperator.GreaterThanOrEqualTo, dateTime.Date);
                var clauseTo = new WhereClause(LogicOperator.And, tableName, columnName, ComparisonOperator.LessThan, dateTime.Date.AddDays(1));

                innerContainer.AddSubClause(clauseFrom);
                innerContainer.AddSubClause(clauseTo);

                outerContainer.AddSubClause(innerContainer);
            }

            return outerContainer;
        }

        #endregion Non-Public Methods
    }
}