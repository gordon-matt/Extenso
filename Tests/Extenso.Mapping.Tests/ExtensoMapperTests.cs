using System.Linq.Expressions;
using Xunit.Abstractions;

namespace Extenso.Mapping.Tests;

public class ExtensoMapperTests
{
    private readonly ITestOutputHelper _output;

    public ExtensoMapperTests(ITestOutputHelper output)
    {
        _output = output;

        ExtensoMapper.Register<CategoryEntity, CategoryModel>(x => x.ToModel());
        ExtensoMapper.Register<CategoryModel, CategoryEntity>(x => x.ToEntity());

        ExtensoMapper.Register<TestEntity, TestModel>(x => x.ToModel());
        ExtensoMapper.Register<TestModel, TestEntity>(x => x.ToEntity());
    }

    [Fact]
    public void Map()
    {
        var entity = new CategoryEntity
        {
            Id = 1,
            Name = "Source"
        };

        var model = ExtensoMapper.Map<CategoryEntity, CategoryModel>(entity);

        Assert.Equal(entity.Id, model.Id);
        Assert.Equal(entity.Name, model.Name);
    }

    #region Work in Progress

    [Fact]
    public void MapExpression_Should_Map_Simple_Predicate()
    {
        // Arrange
        Expression<Func<TestModel, bool>> predicate = m => m.Id == 123 && m.FirstName == "Test";

        // Act
        var mappedPredicate = ExtensoMapper.MapPredicate<TestModel, TestEntity>(predicate);
        _output.WriteLine(mappedPredicate.ToString());

        // Assert - now accounts for the FirstName → FullName.Split mapping
        Assert.Contains("x.Id == 123", mappedPredicate.ToString());
        Assert.Contains("FirstName == \"Test\"", mappedPredicate.ToString());
    }

    [Fact]
    public void MapExpression_Should_Map_Nested_Properties()
    {
        // Arrange
        Expression<Func<TestModel, bool>> predicate = m => m.Category.Name == "Electronics";

        // Act
        var mappedPredicate = ExtensoMapper.MapPredicate<TestModel, TestEntity>(predicate);
        _output.WriteLine(mappedPredicate.ToString());

        // Assert
        Assert.Equal(
            "x => (x.Category.Name == \"Electronics\")",
            mappedPredicate.ToString());
    }

    [Fact]
    public void MapUpdateExpression_Should_Map_Property_Assignments()
    {
        // Arrange
        Expression<Func<TestModel, TestModel>> update = m => new TestModel
        {
            Email = m.Email.Replace("@old.com", "@new.com"),
            FirstName = "Updated_" + m.FirstName
        };

        // Act
        var mappedUpdate = ExtensoMapper.MapUpdateExpression<TestModel, TestEntity>(update);
        _output.WriteLine(mappedUpdate.ToString());

        // Assert
        Assert.Contains("Email = x.Email.Replace(\"@old.com\", \"@new.com\")", mappedUpdate.ToString());
        Assert.Contains("FirstName = (\"Updated_\" + x.FirstName)", mappedUpdate.ToString());
    }

    [Fact]
    public void MapQueryable_Should_Compose_With_Other_LINQ_Methods()
    {
        // Arrange
        var models = new List<TestModel>
        {
            new()
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "a@test.com",
                Category = new CategoryModel { Name = "A" }
            },
            new()
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Jones",
                Email = "b@test.com",
                Category = new CategoryModel { Name = "B" }
            }
        }.AsQueryable();

        // Act
        var query = models
            .Where(m => m.Id > 0)
            .MapQuery<TestModel, TestEntity>()
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName);

        var results = query.ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("Alice", results[0].FirstName);
        Assert.Equal("Smith", results[0].LastName);
        Assert.Equal("Bob", results[1].FirstName);
        Assert.Equal("Jones", results[1].LastName);
    }

    [Fact]
    public void Should_Handle_Null_In_Update_Expressions()
    {
        // Arrange
        Expression<Func<TestModel, TestModel>> update = m => new TestModel
        {
            Email = m.Email ?? "default@email.com"
        };

        // Act
        var mappedUpdate = ExtensoMapper.MapUpdateExpression<TestModel, TestEntity>(update);

        // Assert
        Assert.Contains("x.Email ?? \"default@email.com\"", mappedUpdate.ToString());
    }

    #endregion
}

#region Models

public class CategoryEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class TestEntity
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CategoryEntity Category { get; set; }
}

public class TestModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CategoryModel Category { get; set; }
}

#endregion Models

public static class Extensions
{
    public static CategoryEntity ToEntity(this CategoryModel model) => new()
    {
        Id = model.Id,
        Name = model.Name
    };

    public static CategoryModel ToModel(this CategoryEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name
    };

    public static TestEntity ToEntity(this TestModel model) => new()
    {
        Id = model.Id,
        Email = model.Email,
        FirstName = model.FirstName,
        LastName = model.LastName,
        Category = model.Category.ToEntity()
    };

    public static TestModel ToModel(this TestEntity entity) => new()
    {
        Id = entity.Id,
        Email = entity.Email,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Category = entity.Category.ToModel()
    };
}