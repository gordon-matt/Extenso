using System;
using Extenso.Data.QueryBuilder;

namespace Demo.Data.QueryBuilder
{
    internal class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Main method.")]
        private static void Main(string[] args)
        {
            var query = new SqlServerSelectQueryBuilder()
                .SelectAll()
                .From("Products")
                .Join(JoinType.InnerJoin, "Categories", "Id", ComparisonOperator.EqualTo, "Products", "CategoryId")
                .Where("Products", "Name", ComparisonOperator.StartsWith, "A")
                .OrderBy("Products", "Name", SortDirection.Ascending)
                .Take(25);

            Console.WriteLine(query.BuildQuery());

            Console.ReadLine();
        }
    }
}