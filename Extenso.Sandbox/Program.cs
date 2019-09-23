using System;
using Extenso.Data.QueryBuilder;
using Extenso.Data.QueryBuilder.Npgsql;

namespace Extenso.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string tableName = "EnginePackageView";

            //var query = new NpgsqlSelectQueryBuilder("dbo")
            //    .SelectAs(tableName, "BookingDate", "Booking Date")
            //    .SelectAs(tableName, "BookingConfirmRef", "Confirm Ref")
            //    .Select(new SqlLiteral(@"GetProductTypeNameFull(""ProductType"") AS ""Product Type"""))
            //    .From(tableName)
            //    .Where(tableName, "BookingDate", ComparisonOperator.In, new DateTime[]
            //    {
            //        new DateTime(2019, 4, 4),
            //        new DateTime(2019, 4, 6)
            //    })
            //    .Where(tableName, "ProductType", ComparisonOperator.HasFlag, 32)
            //    .Where(WhereClause.CreateContainer(LogicOperator.And)
            //        .AddSubClause(WhereClause.CreateContainer(LogicOperator.And)
            //            .AddSubClause(new WhereClause(LogicOperator.And, tableName, "BookingConfirmRef", ComparisonOperator.StartsWith, "ODH"))
            //            .AddSubClause(new WhereClause(LogicOperator.And, tableName, "BookingConfirmRef", ComparisonOperator.EndsWith, "1")))
            //        .AddSubClause(WhereClause.CreateContainer(LogicOperator.Or)
            //            .AddSubClause(new WhereClause(LogicOperator.And, tableName, "BookingConfirmRef", ComparisonOperator.StartsWith, "ODH"))
            //            .AddSubClause(new WhereClause(LogicOperator.And, tableName, "BookingConfirmRef", ComparisonOperator.EndsWith, "2")))
            //    )
            //    .OrderBy(tableName, "BookingDate", SortDirection.Descending)
            //    .Take(25);

            var query = new NpgsqlSelectQueryBuilder("dbo")
                .SelectAs(tableName, "BookingDate", "Booking Date")
                .SelectAs(tableName, "BookingConfirmRef", "Confirm Ref")
                .Select(new SqlLiteral(@"GetProductTypeNameFull(""ProductType"") AS ""Product Type"""))
                .From(tableName)
                // Added new Where() overload, in case you want to build your own filters with something like jQuery QueryBuilder (https://querybuilder.js.org/)
                .Where($@"""BookingDate"" >= '2019-01-01' AND ""BookingDate"" < '2019-02-01' AND ""ProductType"" & 32 <> 0")
                .OrderBy(tableName, "BookingDate", SortDirection.Descending)
                .Take(25);

            string queryText = query.BuildQuery();
            Console.WriteLine(queryText);

            Console.ReadLine();
        }
    }
}