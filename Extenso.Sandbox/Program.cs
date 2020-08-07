using System;
using System.Collections.Generic;
using Extenso.Collections;
using Extenso.Data;
using Extenso.Data.QueryBuilder;
using Extenso.Data.QueryBuilder.Npgsql;

namespace Extenso.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //QueryBuilder();

            DataTableExtensions();

            Console.ReadLine();
        }

        private static void DataTableExtensions()
        {
            var people = new List<Person>
            {
                new Person
                {
                    FamilyName = "Anderson	",
                    GivenNames = "James",
                    Notes = @"Some
notes
with 	new
lines"
                },
                new Person
                {
                    FamilyName = "Anderson",
                    GivenNames = "Jane",
                    Notes = @"Some
notes
with new
lines 	2"
                }
            };

            var table = people.ToDataTable();

            string csv = table.ToDelimited(delimiter: "|", alwaysEnquote: false);

            string csv2 = people.ToCsv();

            Console.WriteLine("CSV: " + csv);
        }

        private static void QueryBuilder()
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
                .OrderBy("Product Type", SortDirection.Descending)
                .Take(25);

            string queryText = query.BuildQuery();
            Console.WriteLine(queryText);
        }
    }

    public class Person
    {
        public string FamilyName { get; set; }

        public string GivenNames { get; set; }

        public string Notes { get; set; }
    }
}