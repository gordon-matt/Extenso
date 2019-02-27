namespace Extenso.Data.QueryBuilder.MySql
{
    using System;
    using System.Linq;
    using System.Text;
    using Extenso.Collections;
    using Extenso.Data.QueryBuilder;

    public class MySqlSelectQueryBuilder : BaseSelectQueryBuilder
    {
        public MySqlSelectQueryBuilder()
            : base()
        {
        }

        public override ISelectQueryBuilder SelectCount()
        {
            selectedColumns.Clear();
            selectedColumns.Add("COUNT(1)", null);
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
                    query.Append("."); // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.
                }

                query.Append("*");
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

                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
                query.Append(' ');
            }
            // Output table names
            if (selectedTables.Count > 0)
            {
                query.Append(" FROM ");
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

                    string fromField = CreateFieldName(clause.FromTable, clause.FromColumn);
                    string toField = CreateFieldName(clause.ToTable, clause.ToColumn);

                    joinString += CreateComparisonClause(fromField, clause.ComparisonOperator, new SqlLiteral(toField));
                    query.Append(joinString);
                    query.Append(' ');
                }
            }

            // Output where statement
            if (!whereStatement.Clauses.IsNullOrEmpty())
            {
                query.Append(" ");
                query.Append(CreateWhereStatement(whereStatement));

                //query.Append(" WHERE ");
                //query.Append(whereStatement.BuildWhereStatement());
            }

            // Output GroupBy statement
            if (groupByColumns.Count > 0)
            {
                query.Append(" GROUP BY ");
                foreach (string column in groupByColumns)
                {
                    query.Append(column);
                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1);
                query.Append(' ');
            }

            // Output having statement
            if (!havingStatement.Clauses.IsNullOrEmpty())
            {
                // Check if a Group By Clause was set
                if (groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                query.Append(" ");
                query.Append(CreateWhereStatement(havingStatement));
                //query.Append(" HAVING ");
                //query.Append(havingStatement.BuildWhereStatement());
            }

            // Output OrderBy statement
            if (orderByStatement.Count > 0)
            {
                query.Append(" ORDER BY ");
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
                query.Append(skipCount);
                query.Append(",");
                query.Append(takeCount);
            }

            return query.ToString();
        }

        protected override string EncloseIdentifier(string identifier)
        {
            return string.Concat('`', identifier, '`');
        }
    }
}