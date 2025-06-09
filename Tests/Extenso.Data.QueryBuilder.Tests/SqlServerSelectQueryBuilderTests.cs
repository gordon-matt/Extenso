namespace Extenso.Data.QueryBuilder.Tests;

public class SqlServerSelectQueryBuilderTests
{
    [Fact]
    public void SampleQuery()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products")
            .Join(JoinType.InnerJoin, "Categories", "Id", ComparisonOperator.EqualTo, "Products", "CategoryId")
            .Where("Products", "Name", ComparisonOperator.StartsWith, "A")
            .OrderBy("Products", "Name", SortDirection.Ascending)
            .Take(25);

        string expected = "SELECT TOP 25 [Products].* FROM [Products] INNER JOIN [Categories] ON [Products].[CategoryId] = [Categories].[Id] WHERE [Products].[Name] LIKE 'A%' ORDER BY [Products].[Name] ASC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Distinct()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("dbo.Products", "Name")
            .From("dbo.Products")
            .Distinct();

        string expected = "SELECT DISTINCT [dbo].[Products].[Name] FROM [dbo].[Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromOneTable()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products");

        string expected = "SELECT [Products].* FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromMultipleTables()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products", "Categories");

        string expected = "SELECT * FROM [Products],[Categories]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupBy_Simple()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy("Products", "Name")
            .OrderBy("COUNT([Name]) DESC");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] ORDER BY COUNT([Name]) DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GroupBy_Advanced()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy(new[] { new TableColumnPair("Products", "Name") })
            .OrderBy("COUNT([Name]) DESC");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] ORDER BY COUNT([Name]) DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy("Products", "Name")
            .OrderBy("COUNT([Name]) DESC")
            .Having("Products", "Name", ComparisonOperator.StartsWith, "M");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] HAVING [Products].[Name] LIKE 'M%' ORDER BY COUNT([Name]) DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having_WhereClause()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy("Products", "Name")
            .OrderBy("COUNT([Name]) DESC")
            .Having(new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M"));

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] HAVING [Products].[Name] LIKE 'M%' ORDER BY COUNT([Name]) DESC";
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

        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy("Products", "Name")
            .OrderBy("COUNT([Name]) DESC")
            .Having(havingStatement);

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] HAVING [Products].[Name] LIKE 'M%' ORDER BY COUNT([Name]) DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Having_Literal()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .GroupBy("Products", "Name")
            .OrderBy("COUNT([Name]) DESC")
            .Having("COUNT([Name]) > 3");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] GROUP BY [Products].[Name] HAVING COUNT([Name]) > 3 ORDER BY COUNT([Name]) DESC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Join()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products")
            .Join(JoinType.InnerJoin, "Categories", "Id", ComparisonOperator.EqualTo, "Products", "CategoryId");

        string expected = "SELECT [Products].* FROM [Products] INNER JOIN [Categories] ON [Products].[CategoryId] = [Categories].[Id]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OrderBy()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products")
            .OrderBy("Name", SortDirection.Ascending);

        string expected = "SELECT [Products].* FROM [Products] ORDER BY [Name] ASC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OrderBy_WithTable()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products")
            .OrderBy("Products", "Name", SortDirection.Ascending);

        string expected = "SELECT [Products].* FROM [Products] ORDER BY [Products].[Name] ASC";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Simple()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .From("Products");

        string expected = "SELECT [Products].[Name] FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Advanced()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select(new[] { new TableColumnPair("Products", "Name") })
            .From("Products");

        string expected = "SELECT [Products].[Name] FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Select_Literal()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select(new SqlLiteral("[Name], GETDATE() AS [Today]"))
            .From("Products");

        string expected = "SELECT [Name], GETDATE() AS [Today] FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectAll()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products");

        string expected = "SELECT [Products].* FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectAs()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAs("Products", "Name", "ProductName")
            .From("Products");

        string expected = "SELECT [Products].[Name] AS [ProductName] FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectCountAll()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectCountAll()
            .From("Products");

        string expected = "SELECT COUNT(*) FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Skip()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Log")
            .OrderBy("Log", "DateCreatedUtc", SortDirection.Descending)
            .Skip(100)
            .Take(25);

        string expected = "SELECT [Log].* FROM [Log] ORDER BY [Log].[DateCreatedUtc] DESC OFFSET 100 ROWS FETCH NEXT 25 ROWS ONLY";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Take()
    {
        var query = new SqlServerSelectQueryBuilder()
            .SelectAll()
            .From("Products")
            .Take(25);

        string expected = "SELECT TOP 25 [Products].* FROM [Products]";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .Where("Products", "Name", ComparisonOperator.StartsWith, "M");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] WHERE [Products].[Name] LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where_WhereClause()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .Where(new WhereClause(LogicOperator.And, "Products", "Name", ComparisonOperator.StartsWith, "M"));

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] WHERE [Products].[Name] LIKE 'M%'";
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

        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .Where(whereStatement);

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] WHERE [Products].[Name] LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Where_Literal()
    {
        var query = new SqlServerSelectQueryBuilder()
            .Select("Products", "Name")
            .Select(new SqlLiteral("COUNT([Name])"))
            .From("Products")
            .Where("[Name] LIKE 'M%'");

        string expected = "SELECT [Products].[Name], COUNT([Name]) FROM [Products] WHERE [Name] LIKE 'M%'";
        string actual = query.BuildQuery();

        Assert.Equal(expected, actual);
    }
}