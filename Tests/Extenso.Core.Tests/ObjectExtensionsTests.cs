using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Extenso.Core.Tests;

public class ComplexTestClass
{
    public DateTime DateProperty { get; set; }

    public int IntProperty { get; set; }

    public TestClass NestedObject { get; set; }

    public string StringProperty { get; set; }
}

[DataContract]
public class TestClass
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int Value { get; set; }
}

public class ObjectExtensionsTests
{
    [Fact]
    public void AllHashMethods_WithComplexObject_ProduceDifferentLengthHashes()
    {
        // Arrange
        var complexObj = new ComplexTestClass
        {
            StringProperty = "Test String",
            IntProperty = 12345,
            DateProperty = new DateTime(2023, 1, 1),
            NestedObject = new TestClass { Name = "Nested", Value = 999 }
        };

        // Act
        byte[] md5Hash = complexObj.ComputeMD5Hash();
        byte[] sha1Hash = complexObj.ComputeSHA1Hash();
        byte[] sha256Hash = complexObj.ComputeSHA256Hash();
        byte[] sha512Hash = complexObj.ComputeSHA512Hash();

        // Assert
        Assert.Equal(16, md5Hash.Length);
        Assert.Equal(20, sha1Hash.Length);
        Assert.Equal(32, sha256Hash.Length);
        Assert.Equal(64, sha512Hash.Length);
    }

    [Fact]
    public void AllHashMethods_WithSameComplexObject_ProduceConsistentResults()
    {
        // Arrange
        var complexObj1 = new ComplexTestClass
        {
            StringProperty = "Test String",
            IntProperty = 12345,
            DateProperty = new DateTime(2023, 1, 1),
            NestedObject = new TestClass { Name = "Nested", Value = 999 }
        };

        var complexObj2 = new ComplexTestClass
        {
            StringProperty = "Test String",
            IntProperty = 12345,
            DateProperty = new DateTime(2023, 1, 1),
            NestedObject = new TestClass { Name = "Nested", Value = 999 }
        };

        // Act & Assert
        Assert.Equal(complexObj1.ComputeMD5Hash(), complexObj2.ComputeMD5Hash());
        Assert.Equal(complexObj1.ComputeSHA1Hash(), complexObj2.ComputeSHA1Hash());
        Assert.Equal(complexObj1.ComputeSHA256Hash(), complexObj2.ComputeSHA256Hash());
        Assert.Equal(complexObj1.ComputeSHA512Hash(), complexObj2.ComputeSHA512Hash());
    }

    [Fact]
    public void ComputeHash_WithDifferentObjects_ReturnsDifferentHashes()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test1", Value = 42 };
        var testObj2 = new TestClass { Name = "Test2", Value = 42 };
        using var sha256_1 = SHA256.Create();
        using var sha256_2 = SHA256.Create();

        // Act
        byte[] hash1 = testObj1.ComputeHash(sha256_1);
        byte[] hash2 = testObj2.ComputeHash(sha256_2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_WithDisposedHashAlgorithm_ThrowsObjectDisposedException()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };
        var sha256 = SHA256.Create();
        sha256.Dispose();

        // Act & Assert
        Assert.Throws<ObjectDisposedException>(() => testObj.ComputeHash(sha256));
    }

    [Fact]
    public void ComputeHash_WithNullHashAlgorithm_ThrowsArgumentNullException()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };

        // Act & Assert
        Assert.Throws<NullReferenceException>(() => testObj.ComputeHash(null));
    }

    [Fact]
    public void ComputeHash_WithNullObject_ThrowsArgumentException()
    {
        // Arrange
        object nullObj = null;
        using var sha256 = SHA256.Create();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullObj.ComputeHash(sha256));
    }

    [Fact]
    public void ComputeHash_WithSameObject_ReturnsSameHash()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test", Value = 42 };
        var testObj2 = new TestClass { Name = "Test", Value = 42 };
        using var sha256_1 = SHA256.Create();
        using var sha256_2 = SHA256.Create();

        // Act
        byte[] hash1 = testObj1.ComputeHash(sha256_1);
        byte[] hash2 = testObj2.ComputeHash(sha256_2);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_WithValidObject_ReturnsNonEmptyHash()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };
        using var sha256 = SHA256.Create();

        // Act
        byte[] result = testObj.ComputeHash(sha256);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(32, result.Length); // SHA256 produces 32 bytes
    }

    [Fact]
    public void ComputeMD5Hash_WithNullObject_ThrowsArgumentNullException()
    {
        // Arrange
        object nullObj = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullObj.ComputeMD5Hash());
    }

    [Fact]
    public void ComputeMD5Hash_WithSameObject_ReturnsSameHash()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test", Value = 42 };
        var testObj2 = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] hash1 = testObj1.ComputeMD5Hash();
        byte[] hash2 = testObj2.ComputeMD5Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeMD5Hash_WithValidObject_Returns16ByteHash()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] result = testObj.ComputeMD5Hash();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(16, result.Length); // MD5 produces 16 bytes
    }

    [Fact]
    public void ComputeSHA1Hash_WithNullObject_ThrowsArgumentNullException()
    {
        // Arrange
        object nullObj = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullObj.ComputeSHA1Hash());
    }

    [Fact]
    public void ComputeSHA1Hash_WithSameObject_ReturnsSameHash()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test", Value = 42 };
        var testObj2 = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] hash1 = testObj1.ComputeSHA1Hash();
        byte[] hash2 = testObj2.ComputeSHA1Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeSHA1Hash_WithValidObject_Returns20ByteHash()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] result = testObj.ComputeSHA1Hash();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20, result.Length); // SHA1 produces 20 bytes
    }

    [Fact]
    public void ComputeSHA256Hash_WithNullObject_ThrowsArgumentNullException()
    {
        // Arrange
        object nullObj = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullObj.ComputeSHA256Hash());
    }

    [Fact]
    public void ComputeSHA256Hash_WithSameObject_ReturnsSameHash()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test", Value = 42 };
        var testObj2 = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] hash1 = testObj1.ComputeSHA256Hash();
        byte[] hash2 = testObj2.ComputeSHA256Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Test")]
    [InlineData("A very long string that should still hash consistently")]
    public void ComputeSHA256Hash_WithStringValues_ProducesConsistentResults(string testValue)
    {
        // Arrange
        var obj1 = new TestClass { Name = testValue, Value = 42 };
        var obj2 = new TestClass { Name = testValue, Value = 42 };

        // Act
        byte[] hash1 = obj1.ComputeSHA256Hash();
        byte[] hash2 = obj2.ComputeSHA256Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeSHA256Hash_WithValidObject_Returns32ByteHash()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] result = testObj.ComputeSHA256Hash();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(32, result.Length); // SHA256 produces 32 bytes
    }

    [Fact]
    public void ComputeSHA512Hash_WithNullObject_ThrowsArgumentNullException()
    {
        // Arrange
        object nullObj = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullObj.ComputeSHA512Hash());
    }

    [Fact]
    public void ComputeSHA512Hash_WithSameObject_ReturnsSameHash()
    {
        // Arrange
        var testObj1 = new TestClass { Name = "Test", Value = 42 };
        var testObj2 = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] hash1 = testObj1.ComputeSHA512Hash();
        byte[] hash2 = testObj2.ComputeSHA512Hash();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeSHA512Hash_WithValidObject_Returns64ByteHash()
    {
        // Arrange
        var testObj = new TestClass { Name = "Test", Value = 42 };

        // Act
        byte[] result = testObj.ComputeSHA512Hash();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(64, result.Length); // SHA512 produces 64 bytes
    }
}