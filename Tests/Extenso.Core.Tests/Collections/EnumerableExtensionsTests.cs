using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extenso.Collections;

namespace Extenso.Core.Tests.Collections;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Descendants()
    {
        // Arrange
        var root = new Node
        {
            Value = 1,
            Children =
            [
                new() { Value = 2 },
                new() { Value = 3, Children = [new() { Value = 4 }] }
            ]
        };

        // Act
        var result = new List<Node>(new[] { root }.Descendants(n => n.Children));

        // Assert
        Assert.Equal(4, result.Count); // root + 3 descendants
        Assert.Contains(result, n => n.Value == 1);
        Assert.Contains(result, n => n.Value == 2);
        Assert.Contains(result, n => n.Value == 3);
        Assert.Contains(result, n => n.Value == 4);
    }

    [Fact]
    public void Flatten()
    {
        // Arrange
        var root = new Node
        {
            Value = 1,
            Children =
            [
                new() { Value = 2 },
                new() { Value = 3, Children = [new() { Value = 4 }] }
            ]
        };

        // Act
        var result = new List<Node>(new[] { root }.Flatten(n => n.Children));

        // Assert
        Assert.Equal(4, result.Count); // root + 3 children
        Assert.Equal(1, result[0].Value);
        Assert.Equal(2, result[1].Value);
        Assert.Equal(3, result[2].Value);
        Assert.Equal(4, result[3].Value);
    }

    [Fact]
    public void SafeUnion_UnionsSequencesCorrectly()
    {
        // Arrange
        var first = new List<int> { 1, 2, 3 };
        var second = new List<int> { 3, 4, 5 };

        // Act
        var result = first.SafeUnion(second).ToList();

        // Assert
        Assert.Equal(5, result.Count);  // Union of both lists without duplicates: 1, 2, 3, 4, 5
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void SafeUnion_WithEmptyFirstSequence_ReturnsSecondSequence()
    {
        // Arrange
        var first = new List<int>();
        var second = new List<int> { 3, 4, 5 };

        // Act
        var result = first.SafeUnion(second).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void SafeUnion_WithEmptySecondSequence_ReturnsFirstSequence()
    {
        // Arrange
        var first = new List<int> { 1, 2, 3 };
        var second = new List<int>();

        // Act
        var result = first.SafeUnion(second).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
    }

    [Fact]
    public void SafeUnion_WithNullSequence_ReturnsOtherSequence()
    {
        // Arrange
        List<int> first = null;
        var second = new List<int> { 3, 4, 5 };

        // Act
        var result = first.SafeUnion(second).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
    }

    [Fact]
    public void ToChunks_SplitsIntoChunksOfGivenSize()
    {
        // Arrange
        var source = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        var result = source.ToChunks(3).ToList();

        // Assert
        Assert.Equal(4, result.Count); // Should have 4 chunks
        Assert.Contains(new HashSet<int> { 1, 2, 3 }, result);
        Assert.Contains(new HashSet<int> { 4, 5, 6 }, result);
        Assert.Contains(new HashSet<int> { 7, 8, 9 }, result);
        Assert.Contains(new HashSet<int> { 10 }, result);
    }

    [Fact]
    public void ToChunks_WithEmptySource_ReturnsEmptyChunks()
    {
        // Arrange
        var source = new List<int>();

        // Act
        var result = source.ToChunks(3).ToList();

        // Assert
        Assert.Empty(result); // No chunks
    }

    [Fact]
    public void ToChunks_WithSingleElementReturnsSingleChunk()
    {
        // Arrange
        var source = new List<int> { 5 };

        // Act
        var result = source.ToChunks(3).ToList();

        // Assert
        Assert.Single(result); // Only one chunk
        Assert.Contains(new HashSet<int> { 5 }, result);
    }

    [Fact]
    public void ToChunks_WithSmallerSourceThanChunkSize_CreatesSingleChunk()
    {
        // Arrange
        var source = new List<int> { 1, 2 };

        // Act
        var result = source.ToChunks(3).ToList();

        // Assert
        Assert.Single(result); // Only one chunk
        Assert.Contains(new HashSet<int> { 1, 2 }, result);
    }

    // Test for ToPivotTable with 1 row selector
    [Fact]
    public void ToPivotTable_WithSingleRowSelector_CreatesCorrectPivotTable()
    {
        // Arrange
        var data = new List<TestData>
        {
            new() { Category = "Fruit", Region = "East", Sales = 100 },
            new() { Category = "Fruit", Region = "West", Sales = 150 },
            new() { Category = "Vegetable", Region = "East", Sales = 200 },
            new() { Category = "Vegetable", Region = "West", Sales = 250 }
        };

        // Act
        var table = data.ToPivotTable(
            x => x.Category,
            x => x.Region,
            group => group.Sum(g => g.Sales)
        );

        // Assert
        Assert.Equal(3, table.Columns.Count);
        Assert.Equal("Region", table.Columns[0].ColumnName);
        Assert.Equal("Fruit", table.Columns[1].ColumnName);
        Assert.Equal("Vegetable", table.Columns[2].ColumnName);

        Assert.Equal("East", table.Rows[0][0]);
        Assert.Equal(100, table.Rows[0][1]);
        Assert.Equal(200, table.Rows[0][2]);

        Assert.Equal("West", table.Rows[1][0]);
        Assert.Equal(150, table.Rows[1][1]);
        Assert.Equal(250, table.Rows[1][2]);
    }

    // Test for ToPivotTable with 2 row selectors
    [Fact]
    public void ToPivotTable_WithTwoRowSelectors_CreatesCorrectPivotTable()
    {
        // Arrange
        var data = new List<TestData>
        {
            new() { Category = "Fruit", SubCategory = "Citrus", Region = "East", Sales = 100 },
            new() { Category = "Fruit", SubCategory = "Citrus", Region = "West", Sales = 150 },
            new() { Category = "Fruit", SubCategory = "Berry", Region = "East", Sales = 200 },
            new() { Category = "Vegetable", SubCategory = "Leafy", Region = "West", Sales = 250 }
        };

        // Act
        var table = data.ToPivotTable(
            x => x.Category,
            x => x.Region,
            x => x.SubCategory,
            group => group.Sum(g => g.Sales)
        );

        // Assert
        Assert.Equal(4, table.Columns.Count);
        Assert.Equal("Region", table.Columns[0].ColumnName);
        Assert.Equal("SubCategory", table.Columns[1].ColumnName);
        Assert.Equal("Fruit", table.Columns[2].ColumnName);
        Assert.Equal("Vegetable", table.Columns[3].ColumnName);

        Assert.Equal("East", table.Rows[0][0]);
        Assert.Equal("Citrus", table.Rows[0][1]);
        Assert.Equal(100, table.Rows[0][2]);
        Assert.Equal(0, table.Rows[0][3]);

        Assert.Equal("West", table.Rows[1][0]);
        Assert.Equal("Citrus", table.Rows[1][1]);
        Assert.Equal(150, table.Rows[1][2]);
        Assert.Equal(0, table.Rows[1][3]);

        Assert.Equal("East", table.Rows[2][0]);
        Assert.Equal("Berry", table.Rows[2][1]);
        Assert.Equal(200, table.Rows[2][2]);
        Assert.Equal(0, table.Rows[2][3]);

        Assert.Equal("West", table.Rows[3][0]);
        Assert.Equal("Leafy", table.Rows[3][1]);
        Assert.Equal(0, table.Rows[3][2]);
        Assert.Equal(250, table.Rows[3][3]);
    }

    // Test for ToPivotTable with 3 row selectors
    [Fact]
    public void ToPivotTable_WithThreeRowSelectors_CreatesCorrectPivotTable()
    {
        // Arrange
        var data = new List<TestData>
        {
            new() { Category = "Fruit", SubCategory = "Citrus", Region = "East", Type = "Orange", Sales = 100 },
            new() { Category = "Fruit", SubCategory = "Berry", Region = "West", Type = "Strawberry", Sales = 150 },
            new() { Category = "Vegetable", SubCategory = "Leafy", Region = "East", Type = "Spinach", Sales = 200 },
            new() { Category = "Vegetable", SubCategory = "Root", Region = "West", Type = "Carrot", Sales = 250 }
        };

        // Act
        var table = data.ToPivotTable(
            x => x.Category,
            x => x.Region,
            x => x.SubCategory,
            x => x.Type,
            group => group.Sum(g => g.Sales)
        );

        // Assert
        Assert.Equal(5, table.Columns.Count);
        Assert.Equal("Region", table.Columns[0].ColumnName);
        Assert.Equal("SubCategory", table.Columns[1].ColumnName);
        Assert.Equal("Type", table.Columns[2].ColumnName);
        Assert.Equal("Fruit", table.Columns[3].ColumnName);
        Assert.Equal("Vegetable", table.Columns[4].ColumnName);

        Assert.Equal("East", table.Rows[0][0]);
        Assert.Equal("Citrus", table.Rows[0][1]);
        Assert.Equal("Orange", table.Rows[0][2]);
        Assert.Equal(100, table.Rows[0][3]);
        Assert.Equal(0, table.Rows[0][4]);

        Assert.Equal("West", table.Rows[1][0]);
        Assert.Equal("Berry", table.Rows[1][1]);
        Assert.Equal("Strawberry", table.Rows[1][2]);
        Assert.Equal(150, table.Rows[1][3]);
        Assert.Equal(0, table.Rows[1][4]);

        Assert.Equal("East", table.Rows[2][0]);
        Assert.Equal("Leafy", table.Rows[2][1]);
        Assert.Equal("Spinach", table.Rows[2][2]);
        Assert.Equal(0, table.Rows[2][3]);
        Assert.Equal(200, table.Rows[2][4]);

        Assert.Equal("West", table.Rows[3][0]);
        Assert.Equal("Root", table.Rows[3][1]);
        Assert.Equal("Carrot", table.Rows[3][2]);
        Assert.Equal(0, table.Rows[3][3]);
        Assert.Equal(250, table.Rows[3][4]);
    }
}

public class TestData
{
    public string Category { get; set; }

    public string Region { get; set; }

    public int Sales { get; set; }

    public string SubCategory { get; set; }

    public string Type { get; set; }
}

public class Node
{
    public List<Node> Children { get; set; } = [];

    public int Value { get; set; }
}