using Extenso.Data.QueryBuilder.Npgsql;

namespace Extenso.Data.QueryBuilder.Tests;

public class NpgsqlSelectQueryBuilderTests
{
    [Fact]
    public void SampleQuery()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products")
            .Join(JoinType.InnerJoin, "public.Categories", "Id", ComparisonOperator.EqualTo, "public.Products", "CategoryId")
            .Where("public.Products", "Name", ComparisonOperator.StartsWith, "A")
            .OrderBy("public.Products", "Name", SortDirection.Ascending)
            .Take(25);

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\" INNER JOIN \"public\".\"Categories\" ON \"public\".\"Products\".\"CategoryId\" = \"public\".\"Categories\".\"Id\" WHERE \"public\".\"Products\".\"Name\" LIKE 'A%' ORDER BY \"public\".\"Products\".\"Name\" ASC LIMIT 25";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Distinct()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .From("public.Products")
            .Distinct();

        string expected = "SELECT DISTINCT \"public\".\"Products\".\"Name\" FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromOneTable()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products");

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromMultipleTables()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products", "public.Categories");

        string expected = "SELECT * FROM \"public\".\"Products\",\"public\".\"Categories\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupBy_Simple()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy("public.Products", "Name")
            .OrderBy("COUNT(\"Name\") DESC");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupBy_Advanced()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy([new TableColumnPair("public.Products", "Name")])
            .OrderBy("COUNT(\"Name\") DESC");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy("public.Products", "Name")
            .OrderBy("COUNT(\"Name\") DESC")
            .Having("public.Products", "Name", ComparisonOperator.StartsWith, "M");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" HAVING \"public\".\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having_WhereClause()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy("public.Products", "Name")
            .OrderBy("COUNT(\"Name\") DESC")
            .Having(new WhereClause(LogicOperator.And, "public.Products", "Name", ComparisonOperator.StartsWith, "M"));

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" HAVING \"public\".\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having_WhereStatement()
    {
        var havingStatement = new WhereStatement
        {
            new WhereClause(LogicOperator.And, "public.Products", "Name", ComparisonOperator.StartsWith, "M")
        };

        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy("public.Products", "Name")
            .OrderBy("COUNT(\"Name\") DESC")
            .Having(havingStatement);

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" HAVING \"public\".\"Products\".\"Name\" LIKE 'M%' ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having_Literal()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .GroupBy("public.Products", "Name")
            .OrderBy("COUNT(\"Name\") DESC")
            .Having("COUNT(\"Name\") > 3");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" GROUP BY \"public\".\"Products\".\"Name\" HAVING COUNT(\"Name\") > 3 ORDER BY COUNT(\"Name\") DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Join()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products")
            .Join(JoinType.InnerJoin, "public.Categories", "Id", ComparisonOperator.EqualTo, "public.Products", "CategoryId");

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\" INNER JOIN \"public\".\"Categories\" ON \"public\".\"Products\".\"CategoryId\" = \"public\".\"Categories\".\"Id\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OrderBy()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products")
            .OrderBy("Name", SortDirection.Ascending);

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\" ORDER BY \"Name\" ASC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OrderBy_WithTable()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products")
            .OrderBy("public.Products", "Name", SortDirection.Ascending);

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\" ORDER BY \"public\".\"Products\".\"Name\" ASC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Simple()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .From("public.Products");

        string expected = "SELECT \"public\".\"Products\".\"Name\" FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Advanced()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select([new TableColumnPair("public.Products", "Name")])
            .From("public.Products");

        string expected = "SELECT \"public\".\"Products\".\"Name\" FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Literal()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select(new SqlLiteral("\"Name\", GETDATE() AS \"Today\""))
            .From("public.Products");

        string expected = "SELECT \"Name\", GETDATE() AS \"Today\" FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectAll()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products");

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectAs()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAs("public.Products", "Name", "ProductName")
            .From("public.Products");

        string expected = "SELECT \"public\".\"Products\".\"Name\" AS \"ProductName\" FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectCountAll()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectCountAll()
            .From("public.Products");

        string expected = "SELECT COUNT(*) FROM \"public\".\"Products\"";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Skip()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Log")
            .OrderBy("public.Log", "DateCreatedUtc", SortDirection.Descending)
            .Skip(100)
            .Take(25);

        string expected = "SELECT \"public\".\"Log\".* FROM \"public\".\"Log\" ORDER BY \"public\".\"Log\".\"DateCreatedUtc\" DESC LIMIT 25 OFFSET 100";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Take()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .SelectAll()
            .From("public.Products")
            .Take(25);

        string expected = "SELECT \"public\".\"Products\".* FROM \"public\".\"Products\" LIMIT 25";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .Where("public.Products", "Name", ComparisonOperator.StartsWith, "M");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" WHERE \"public\".\"Products\".\"Name\" LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where_WhereClause()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .Where(new WhereClause(LogicOperator.And, "public.Products", "Name", ComparisonOperator.StartsWith, "M"));

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" WHERE \"public\".\"Products\".\"Name\" LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where_WhereStatement()
    {
        var whereStatement = new WhereStatement
        {
            new WhereClause(LogicOperator.And, "public.Products", "Name", ComparisonOperator.StartsWith, "M")
        };

        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .Where(whereStatement);

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" WHERE \"public\".\"Products\".\"Name\" LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where_Literal()
    {
        var query = new NpgsqlSelectQueryBuilder()
            .Select("public.Products", "Name")
            .Select(new SqlLiteral("COUNT(\"Name\")"))
            .From("public.Products")
            .Where("\"Name\" LIKE 'M%'");

        string expected = "SELECT \"public\".\"Products\".\"Name\", COUNT(\"Name\") FROM \"public\".\"Products\" WHERE \"Name\" LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }
}