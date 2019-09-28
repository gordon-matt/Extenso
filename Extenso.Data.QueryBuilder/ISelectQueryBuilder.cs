using System.Collections.Generic;

namespace Extenso.Data.QueryBuilder
{
    public interface ISelectQueryBuilder : IQueryBuilder
    {
        ISelectQueryBuilder SelectAll();

        ISelectQueryBuilder SelectAs(string tableName, string column, string alias = null);

        ISelectQueryBuilder Select(string tableName, params string[] columns);

        ISelectQueryBuilder Select(IEnumerable<TableColumnPair> columns);

        ISelectQueryBuilder Select(SqlLiteral literal);

        ISelectQueryBuilder SelectCount();

        ISelectQueryBuilder Distinct(bool isDistinct = true);

        ISelectQueryBuilder From(string tableName);

        ISelectQueryBuilder From(params string[] tableNames);

        ISelectQueryBuilder Join(JoinType joinType, string toTableName, string toColumnName, ComparisonOperator comparisonOperator, string fromTableName, string fromColumnName);

        ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And);

        ISelectQueryBuilder Where(WhereClause whereClause);

        ISelectQueryBuilder Where(WhereStatement whereStatement);

        ISelectQueryBuilder Where(string literal);

        ISelectQueryBuilder OrderBy(string column, SortDirection order);

        ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection sortDirection);

        ISelectQueryBuilder GroupBy(string tableName, params string[] columns);

        ISelectQueryBuilder GroupBy(IEnumerable<TableColumnPair> columns);

        ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, LogicOperator logicOperator = LogicOperator.And);

        ISelectQueryBuilder Having(WhereClause whereClause);

        ISelectQueryBuilder Having(WhereStatement havingStatement);

        ISelectQueryBuilder Having(string literal);

        ISelectQueryBuilder Skip(int count);

        ISelectQueryBuilder Take(int count);
    }

    public struct TableColumnPair
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }
    }
}