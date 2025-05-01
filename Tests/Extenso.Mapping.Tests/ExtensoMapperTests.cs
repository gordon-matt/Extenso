using Xunit.Abstractions;

namespace Extenso.Mapping.Tests;

public class ExtensoMapperTests
{
    private readonly ITestOutputHelper _output;

    public ExtensoMapperTests(ITestOutputHelper output)
    {
        _output = output;

        ExtensoMapper.Register<Source, Destination>(x => x.ToDestination());

        ExtensoMapper.Register<Destination, Source>(x => x.ToSource());

        #region Work in Progress

        //ExtensoMapper.Register<TestModel, TestEntity>(model => new TestEntity
        //{
        //    Id = model.Id,
        //    FullName = $"{model.FirstName} {model.LastName}",
        //    Email = model.ContactEmail,
        //    Category = new CategoryEntity
        //    {
        //        Id = model.Category.Id,
        //        Name = model.Category.Name // Explicit mapping
        //    }
        //});

        //// Add reverse mapping if needed
        //ExtensoMapper.Register<TestEntity, TestModel>(entity => new TestModel
        //{
        //    Id = entity.Id,
        //    ContactEmail = entity.Email,
        //    FirstName = entity.FullName.Split(' ')[0],
        //    LastName = entity.FullName.Split(' ')[1],
        //    Category = new CategoryModel
        //    {
        //        Id = entity.Category.Id,
        //        Name = entity.Category.Name
        //    }
        //});

        #endregion
    }

    [Fact]
    public void Map()
    {
        var source = new Source
        {
            Id = 1,
            Name = "Source"
        };

        var destination = ExtensoMapper.Map<Source, Destination>(source);

        Assert.Equal(source.Id, destination.Id);
        Assert.Equal(source.Name, destination.Name);
    }

    #region Work in Progress

    //[Fact]
    //public void MapExpression_Should_Map_Simple_Predicate()
    //{
    //    // Arrange
    //    Expression<Func<TestModel, bool>> predicate = m => m.Id == 123 && m.FirstName == "Test";

    //    // Act
    //    var mappedPredicate = ExtensoMapper.MapExpression<TestModel, TestEntity>(predicate);
    //    _output.WriteLine(mappedPredicate.ToString());

    //    // Assert - now accounts for the FirstName → FullName.Split mapping
    //    Assert.Contains("x.Id == 123", mappedPredicate.ToString());
    //    Assert.Contains("FullName.Split(' ')[0] == \"Test\"", mappedPredicate.ToString());
    //}

    //[Fact]
    //public void MapExpression_Should_Map_Nested_Properties()
    //{
    //    // Arrange
    //    Expression<Func<TestModel, bool>> predicate = m => m.Category.Name == "Electronics";

    //    // Act
    //    var mappedPredicate = ExtensoMapper.MapExpression<TestModel, TestEntity>(predicate);
    //    _output.WriteLine(mappedPredicate.ToString());

    //    // Assert
    //    Assert.Equal(
    //        "x => (x.Category.Name == \"Electronics\")",
    //        mappedPredicate.ToString().Replace(" ", string.Empty));
    //}

    //[Fact]
    //public void MapUpdateExpression_Should_Map_Property_Assignments()
    //{
    //    // Arrange
    //    Expression<Func<TestModel, TestModel>> update = m => new TestModel
    //    {
    //        ContactEmail = m.ContactEmail.Replace("@old.com", "@new.com"),
    //        FirstName = "Updated_" + m.FirstName
    //    };

    //    // Act
    //    var mappedUpdate = ExtensoMapper.MapUpdateExpression<TestModel, TestEntity>(update);
    //    _output.WriteLine(mappedUpdate.ToString());

    //    // Assert
    //    Assert.Contains("Email = x.Email.Replace(\"@old.com\", \"@new.com\")", mappedUpdate.ToString());
    //    Assert.Contains("FullName = (\"Updated_\" + x.FullName.Split(' ')[0])", mappedUpdate.ToString());
    //}

    //[Fact]
    //public void MapQueryable_Should_Compose_With_Other_LINQ_Methods()
    //{
    //    // Arrange
    //    var models = new List<TestModel>
    //    {
    //        new()
    //        {
    //            Id = 1,
    //            FirstName = "Alice",
    //            LastName = "Smith",
    //            ContactEmail = "a@test.com",
    //            Category = new CategoryModel { Name = "A" }
    //        },
    //        new()
    //        {
    //            Id = 2,
    //            FirstName = "Bob",
    //            LastName = "Jones",
    //            ContactEmail = "b@test.com",
    //            Category = new CategoryModel { Name = "B" }
    //        }
    //    }.AsQueryable();

    //    // Act
    //    var query = models
    //        .Where(m => m.Id > 0)
    //        .MapQueryable<TestModel, TestEntity>()
    //        .OrderBy(e => e.FullName);

    //    var results = query.ToList();

    //    // Assert
    //    Assert.Equal(2, results.Count);
    //    Assert.Equal("Alice Smith", results[0].FullName);
    //    Assert.Equal("Bob Jones", results[1].FullName);
    //}

    //[Fact]
    //public void Should_Handle_Null_In_Update_Expressions()
    //{
    //    // Arrange
    //    Expression<Func<TestModel, TestModel>> update = m => new TestModel
    //    {
    //        ContactEmail = m.ContactEmail ?? "default@email.com"
    //    };

    //    // Act
    //    var mappedUpdate = ExtensoMapper.MapUpdateExpression<TestModel, TestEntity>(update);

    //    // Assert
    //    Assert.Contains("x.Email ?? \"default@email.com\"", mappedUpdate.ToString());
    //}

    #endregion
}

#region Models

public class Source
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Destination
{
    public int Id { get; set; }
    public string Name { get; set; }
}

#region Work in Progress

//public class CategoryEntity
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//}

//public class CategoryModel
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//}

//public class TestEntity
//{
//    public CategoryEntity Category { get; set; }
//    public string Email { get; set; }
//    public string FullName { get; set; }
//    public int Id { get; set; }
//}

//public class TestModel
//{
//    public CategoryModel Category { get; set; }
//    public string ContactEmail { get; set; }
//    public string FirstName { get; set; }
//    public int Id { get; set; }
//    public string LastName { get; set; }
//}

#endregion

#endregion Models

public static class Extensions
{
    public static Destination ToDestination(this Source source) =>
        new()
        {
            Id = source.Id,
            Name = source.Name
        };

    public static Source ToSource(this Destination destination) =>
        new()
        {
            Id = destination.Id,
            Name = destination.Name
        };
}