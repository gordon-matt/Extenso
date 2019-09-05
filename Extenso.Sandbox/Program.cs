using System;
using Extenso.Data.QueryBuilder;
using Extenso.Data.QueryBuilder.Npgsql;

namespace Extenso.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string viewName = "EnginePackageView";
            
            var clause1 = new WhereClause(LogicOperator.And, viewName, "BookingDate", ComparisonOperator.GreaterThanOrEqualTo, new DateTime(2019, 1, 1));
            clause1.SubClauses.Add(new WhereClause(LogicOperator.And, viewName, "BookingDate", ComparisonOperator.LessThan, new DateTime(2019, 1, 2)));

            var clause2 = new WhereClause(LogicOperator.Or, viewName, "BookingDate", ComparisonOperator.GreaterThanOrEqualTo, new DateTime(2019, 1, 3));
            clause2.SubClauses.Add(new WhereClause(LogicOperator.And, viewName, "BookingDate", ComparisonOperator.LessThan, new DateTime(2019, 1, 4)));

            var query = new NpgsqlSelectQueryBuilder("dbo")
                .SelectAll()
                .From(viewName)
                .Where(new WhereStatement()
                    .AddClause(clause1)
                    .AddClause(clause2)
                )
                .OrderBy(viewName, "BookingDate", SortDirection.Descending)
                .Take(25);

            string queryText = query.BuildQuery();
            Console.WriteLine(queryText);

            Console.ReadLine();
        }
    }
}