using Extenso.Data.QueryBuilder.Npgsql;

namespace Extenso.Data.QueryBuilder.Tests
{
    public class NpgsqlSelectQueryBuilderTests
    {
        [Fact]
        public void SampleQuery()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products")
                .Join(JoinType.InnerJoin, "Categories", "Id", ComparisonOperator.EqualTo, "Products", "CategoryId")
                .Where("Products", "Name", ComparisonOperator.StartsWith, "A")
                .OrderBy("Products", "Name", SortDirection.Ascending)
                .Take(25);

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\" INNER JOIN public.\"Categories\" ON public.\"Products\".\"CategoryId\" = public.\"Categories\".\"Id\" WHERE public.\"Products\".\"Name\" LIKE 'A%' ORDER BY public.\"Products\".\"Name\" ASC LIMIT 25";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Distinct()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .From("Products")
                .Distinct();

            string expected = "SELECT DISTINCT public.\"Products\".\"Name\" FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromOneTable()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products");

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FromMultipleTables()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products", "Categories");

            string expected = "SELECT * FROM public.\"Products\",public.\"Categories\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GroupBy_Simple()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy("Products", "Name")
                .OrderBy("COUNT(\"Name\") DESC");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GroupBy_Advanced()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy(new[] { new TableColumnPair("Products", "Name") })
                .OrderBy("COUNT(\"Name\") DESC");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Having()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy("Products", "Name")
                .OrderBy("COUNT(\"Name\") DESC")
                .Having("Products", "Name", ComparisonOperator.StartsWith, "M");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" HAVING public.\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Having_WhereClause()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy("Products", "Name")
                .OrderBy("COUNT(\"Name\") DESC")
                .Having(new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M"));

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" HAVING public.\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Having_WhereStatement()
        {
            var havingStatement = new WhereStatement
            {
                new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M")
            };

            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy("Products", "Name")
                .OrderBy("COUNT(\"Name\") DESC")
                .Having(havingStatement);

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" HAVING public.\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Having_Literal()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .GroupBy("Products", "Name")
                .OrderBy("COUNT(\"Name\") DESC")
                .Having("COUNT(\"Name\") > 3");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" GROUP BY public.\"Products\".\"Name\" HAVING COUNT(\"Name\") > 3 ORDER BY COUNT(\"Name\") DESC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Join()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products")
                .Join(JoinType.InnerJoin, "Categories", "Id", ComparisonOperator.EqualTo, "Products", "CategoryId");

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\" INNER JOIN public.\"Categories\" ON public.\"Products\".\"CategoryId\" = public.\"Categories\".\"Id\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrderBy()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products")
                .OrderBy("Name", SortDirection.Ascending);

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\" ORDER BY \"Name\" ASC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrderBy_WithTable()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products")
                .OrderBy("Products", "Name", SortDirection.Ascending);

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\" ORDER BY public.\"Products\".\"Name\" ASC";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Select_Simple()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .From("Products");

            string expected = "SELECT public.\"Products\".\"Name\" FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Select_Advanced()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select(new[] { new TableColumnPair("Products", "Name") })
                .From("Products");

            string expected = "SELECT public.\"Products\".\"Name\" FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Select_Literal()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select(new SqlLiteral("\"Name\", GETDATE() AS \"Today\""))
                .From("Products");

            string expected = "SELECT \"Name\", GETDATE() AS \"Today\" FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectAll()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products");

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectAs()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAs("Products", "Name", "ProductName")
                .From("Products");

            string expected = "SELECT public.\"Products\".\"Name\" AS \"ProductName\" FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectCountAll()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectCountAll()
                .From("Products");

            string expected = "SELECT COUNT(*) FROM public.\"Products\"";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Skip()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Log")
                .OrderBy("Log", "DateCreatedUtc", SortDirection.Descending)
                .Skip(100)
                .Take(25);

            string expected = "SELECT public.\"Log\".* FROM public.\"Log\" ORDER BY public.\"Log\".\"DateCreatedUtc\" DESC LIMIT 25 OFFSET 100";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Take()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .SelectAll()
                .From("Products")
                .Take(25);

            string expected = "SELECT public.\"Products\".* FROM public.\"Products\" LIMIT 25";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .Where("Products", "Name", ComparisonOperator.StartsWith, "M");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" WHERE public.\"Products\".\"Name\" LIKE 'M%'";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where_WhereClause()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .Where(new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M"));

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" WHERE public.\"Products\".\"Name\" LIKE 'M%'";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where_WhereStatement()
        {
            var whereStatement = new WhereStatement
            {
                new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M")
            };

            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .Where(whereStatement);

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" WHERE public.\"Products\".\"Name\" LIKE 'M%'";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where_Literal()
        {
            var query = new NpgsqlSelectQueryBuilder("public")
                .Select("Products", "Name")
                .Select(new SqlLiteral("COUNT(\"Name\")"))
                .From("Products")
                .Where("\"Name\" LIKE 'M%'");

            string expected = "SELECT public.\"Products\".\"Name\", COUNT(\"Name\") FROM public.\"Products\" WHERE \"Name\" LIKE 'M%'";
            string actual = query.BuildQuery();

            Assert.Equal(expected, actual);
        }
    }
}