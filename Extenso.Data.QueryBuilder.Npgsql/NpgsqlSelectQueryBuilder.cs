using System;
using System.Linq;
using System.Text;
using Extenso.Collections;

namespace Extenso.Data.QueryBuilder.Npgsql;

public class NpgsqlSelectQueryBuilder : BaseSelectQueryBuilder
{
    public NpgsqlSelectQueryBuilder(string schema)
        : base()
    {
        base.schema = schema;
    }

    public override ISelectQueryBuilder SelectCountAll()
    {
        selectedColumns.Clear();
        selectedColumns.Add("COUNT(*)", null);
        orderByStatement.Clear();
        takeCount = 0;
        return this;
    }

    public override string BuildQuery()
    {
        var query = new StringBuilder();
        query.Append("SELECT ");

        // Output Distinct
        if (isDistinct)
        {
            query.Append("DISTINCT ");
        }

        // Output column names
        if (selectedColumns.Count == 0)
        {
            if (selectedTables.Count == 1)
            {
                query.Append(selectedTables.First());
                query.Append('.'); // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.
            }

            query.Append("* ");
        }
        else
        {
            foreach (var column in selectedColumns)
            {
                if (column.Value == null)
                {
                    query.Append(column.Key);
                }
                else
                {
                    query.Append(column.Key);
                    query.Append(" AS ");
                    query.Append(EncloseIdentifier(column.Value));
                }

                query.Append(", ");
            }
            query.Remove(query.Length - 2, 2); // Trim the last comma inserted by foreach loop
            query.Append(' ');
        }
        // Output table names
        if (selectedTables.Count > 0)
        {
            query.Append("FROM ");
            foreach (string tableName in selectedTables)
            {
                query.Append(tableName);
                query.Append(',');
            }
            query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
            query.Append(' ');
        }

        // Output joins
        if (joins.Count > 0)
        {
            foreach (var clause in joins)
            {
                string joinString = string.Empty;
                switch (clause.JoinType)
                {
                    case JoinType.InnerJoin: joinString = "INNER JOIN"; break;
                    case JoinType.OuterJoin: joinString = "OUTER JOIN"; break;
                    case JoinType.LeftJoin: joinString = "LEFT JOIN"; break;
                    case JoinType.RightJoin: joinString = "RIGHT JOIN"; break;
                }
                joinString += " " + EncloseTable(clause.ToTable) + " ON ";

                //string fromField = CreateFieldName(clause.FromTable, clause.FromColumn);
                string toField = CreateFieldName(clause.ToTable, clause.ToColumn);

                joinString += CreateComparisonClause(clause.FromTable, clause.FromColumn, clause.ComparisonOperator, new SqlLiteral(toField));

                query.Append(joinString);
                query.Append(' ');
            }
        }

        // Output where statement
        if (!whereStatement.IsNullOrEmpty() || !string.IsNullOrEmpty(whereStatement.Literal))
        {
            query.Append(CreateWhereStatement(whereStatement, false));

            //query.Append(" WHERE ");
            //query.Append(whereStatement.BuildWhereStatement());
        }

        // Output GroupBy statement
        if (groupByColumns.Count > 0)
        {
            query.Append("GROUP BY ");
            foreach (string column in groupByColumns)
            {
                query.Append(column);
                query.Append(',');
            }
            query.Remove(query.Length - 1, 1);
            query.Append(' ');
        }

        // Output having statement
        if (!havingStatement.IsNullOrEmpty() || !string.IsNullOrEmpty(havingStatement.Literal))
        {
            // Check if a Group By Clause was set
            if (groupByColumns.Count == 0)
            {
                throw new Exception("Having statement was set without Group By");
            }

            query.Append(CreateWhereStatement(havingStatement, true));
            //query.Append(" HAVING ");
            //query.Append(havingStatement.BuildWhereStatement());
        }

        // Output OrderBy statement
        bool hasOrderByLiteral = !string.IsNullOrWhiteSpace(orderByLiteral);
        if (hasOrderByLiteral)
        {
            query.Append($"ORDER BY {orderByLiteral}");
        }

        if (orderByStatement.Count > 0)
        {
            query.Append(hasOrderByLiteral ? "AND " : "ORDER BY ");
            foreach (var clause in orderByStatement)
            {
                string orderByClause = string.Empty;
                switch (clause.SortDirection)
                {
                    case SortDirection.Ascending: orderByClause = clause.FieldName + " ASC"; break;
                    case SortDirection.Descending: orderByClause = clause.FieldName + " DESC"; break;
                }
                query.Append(orderByClause);
                query.Append(',');
            }
            query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
            query.Append(' ');
        }

        // Output Top clause
        if (takeCount > 0)
        {
            query.Append("LIMIT ");
            query.Append(takeCount);

            if (skipCount > 0)
            {
                query.Append(" OFFSET ");
                query.Append(skipCount);
            }
        }

        return query.ToString().Trim();
    }

    protected override string EncloseIdentifier(string identifier) => string.Concat('"', identifier, '"');

    protected override string FormatSQLValue(object someValue)
    {
        var type = someValue.GetType();

        return type.Name == "Boolean" ? (bool)someValue ? "true" : "false" : base.FormatSQLValue(someValue);
    }
}