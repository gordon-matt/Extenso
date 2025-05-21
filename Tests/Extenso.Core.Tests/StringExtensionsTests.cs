namespace Extenso.Core.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void Append_StringValues()
    {
        string source = "test";
        string[] values = ["_one", "_two"];

        string expected = "test_one_two";
        string actual = source.Append(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Append_ObjectValues()
    {
        string source = "test";
        object[] values = ["_one_", 2];

        string expected = "test_one_2";
        string actual = source.Append(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrEmpty_True_Empty()
    {
        string[] values = ["one", "two", string.Empty, "four"];

        bool expected = true;
        bool actual = StringExtensions.AreAnyNullOrEmpty(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrEmpty_True_Null()
    {
        string?[] values = ["one", "two", null, "four"];

        bool expected = true;
        bool actual = StringExtensions.AreAnyNullOrEmpty(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrEmpty_False()
    {
        string[] values = ["one", "two", "three", "four"];

        bool expected = false;
        bool actual = StringExtensions.AreAnyNullOrEmpty(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrWhiteSpace_True_Empty()
    {
        string[] values = ["one", "two", string.Empty, "four"];

        bool expected = true;
        bool actual = StringExtensions.AreAnyNullOrWhiteSpace(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrWhiteSpace_True_Null()
    {
        string?[] values = ["one", "two", null, "four"];

        bool expected = true;
        bool actual = StringExtensions.AreAnyNullOrWhiteSpace(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrWhiteSpace_True_WhiteSpace()
    {
        string[] values = ["one", "two", Environment.NewLine, "four"];

        bool expected = true;
        bool actual = StringExtensions.AreAnyNullOrWhiteSpace(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void AreAnyNullOrWhiteSpace_False()
    {
        string[] values = ["one", "two", "three", "four"];

        bool expected = false;
        bool actual = StringExtensions.AreAnyNullOrWhiteSpace(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Base64Deserialize_String()
    {
        string expected = "my test string";
        string encoded = expected.Base64Serialize();
        string actual = encoded.Base64Deserialize<string>();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Base64Deserialize_Object()
    {
        var expected = new TestObject
        {
            Value1 = 1,
            Value2 = "Test",
            Value3 = 4.5m
        };

        string encoded = expected.Base64Serialize();
        var actual = encoded.Base64Deserialize<TestObject>();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Between_DifferentChar()
    {
        string source = "test_one-test";
        string expected = "one";
        string actual = source.Between('_', '-');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Between_SameChar()
    {
        string source = "test_one_test";
        string expected = "one";
        string actual = source.Between('_', '_');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BrotliCompressAndDecompress()
    {
        string expected = "the quick brown fox";
        string compressed = expected.BrotliCompress();
        string decompressed = compressed.BrotliDecompress();

        Assert.Equal(expected, decompressed);
    }

    [Fact]
    public void CharacterCount()
    {
        string source = "some sample string";
        int expected = 3;
        int actual = source.CharacterCount('s');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Contains_True()
    {
        string source = "the QuICk brown fox";

        bool expected = true;
        bool actual = source.Contains("quick", StringComparison.InvariantCultureIgnoreCase);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Contains_False()
    {
        string source = "the QuICk brown fox";

        bool expected = false;
        bool actual = source.Contains("quack", StringComparison.InvariantCultureIgnoreCase);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAll_Strings_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.ContainsAll("quick", "fox");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAll_Strings_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.ContainsAll("quack", "fox");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAll_Chars_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.ContainsAll('u', 'x');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAll_Chars_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.ContainsAll('a', 'z');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Strings_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.ContainsAny("quick", "quack");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Strings_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.ContainsAny("blue", "green");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Chars_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.ContainsAny('u', 'z');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Chars_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.ContainsAny('a', 'z');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Enumerable_True()
    {
        string source = "the quick brown fox";
        var values = new List<string> { "quick", "quack" };

        bool expected = true;
        bool actual = source.ContainsAny(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ContainsAny_Enumerable_False()
    {
        string source = "the quick brown fox";
        var values = new List<string> { "green", "blue" };

        bool expected = false;
        bool actual = source.ContainsAny(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DeflateCompressAndDecompress()
    {
        string expected = "the quick brown fox";
        string compressed = expected.DeflateCompress();
        string decompressed = compressed.DeflateDecompress();

        Assert.Equal(expected, decompressed);
    }

    [Fact]
    public void EndsWithAny_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.EndsWithAny("fox", "fax");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void EndsWithAny_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.EndsWithAny("quick", "quack");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GZipCompressAndDecompress()
    {
        string expected = "the quick brown fox";
        string compressed = expected.GZipCompress();
        string decompressed = compressed.GZipDecompress();

        Assert.Equal(expected, decompressed);
    }

    [Fact]
    public void IsRightToLeft_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.IsRightToLeft();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsRightToLeft_True()
    {
        string source = "השועל החום המהיר";

        bool expected = true;
        bool actual = source.IsRightToLeft();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Left()
    {
        string source = "test_one_two";

        string expected = "test";
        string actual = source.Left(4);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LeftOf_Char()
    {
        string source = "test_one_two";

        string expected = "t";
        string actual = source.LeftOf('e');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LeftOf_Char_N()
    {
        string source = "test_one_two";

        string expected = "test_on";
        string actual = source.LeftOf('e', 2);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LeftOf_String()
    {
        string source = "test_one_two";

        string expected = "test_";
        string actual = source.LeftOf("one");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LeftOfLastIndexOf_Char()
    {
        string source = "test_one_two";

        string expected = "test_on";
        string actual = source.LeftOfLastIndexOf('e');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LeftOfLastIndexOf_String()
    {
        string source = "test_one_two";

        string expected = "test_";
        string actual = source.LeftOfLastIndexOf("one");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Prepend_StringValues()
    {
        string source = "test";
        string[] values = ["one_", "two_"];

        string expected = "one_two_test";
        string actual = source.Prepend(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Prepend_ObjectValues()
    {
        string source = "test";
        object[] values = [2, "_one_"];

        string expected = "2_one_test";
        string actual = source.Prepend(values);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Repeat()
    {
        string value = "=";

        string expected = "=====";
        string actual = value.Repeat(5);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Replace()
    {
        string source = "the quick brown fox jumped over the lazy dog";
        string expected = "the quick brown chicken jumped over the lazy frog";

        var replacements = new Dictionary<string, string>
        {
            { "fox", "chicken" },
            { "dog", "frog" }
        };

        string actual = source.Replace(replacements);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Right()
    {
        string source = "test_one_two";
        string expected = "two";
        string actual = source.Right(3);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RightOf_Char()
    {
        string source = "test_one_two";
        string expected = "one_two";
        string actual = source.RightOf('_');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RightOf_Char_N()
    {
        string source = "test_one_two";
        string expected = "_two";
        string actual = source.RightOf('e', 2);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RightOf_String()
    {
        string source = "test_one_two";
        string expected = "one_two";
        string actual = source.RightOf("_one");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RightOfLastIndexOf_Char()
    {
        string source = "test_one_two";
        string expected = "_two";
        string actual = source.RightOfLastIndexOf('e');

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RightOfLastIndexOf_String()
    {
        string source = "test_one_two";
        string expected = "one_two";
        string actual = source.RightOfLastIndexOf("_one");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SafeTrim_Null()
    {
        string? source = null;
        string? expected = null;
        string actual = source.SafeTrim();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SafeTrim_NotNull()
    {
        string? source = " something ";
        string? expected = "something";
        string actual = source.SafeTrim();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SplitPascal()
    {
        string source = "SomePascalValue";
        string expected = "Some Pascal Value";
        string actual = source.SplitPascal();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void StartsWithAny_True()
    {
        string source = "the quick brown fox";

        bool expected = true;
        bool actual = source.StartsWithAny("the", "a ");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void StartsWithAny_False()
    {
        string source = "the quick brown fox";

        bool expected = false;
        bool actual = source.StartsWithAny("foo", "bar");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToFile()
    {
        string expected = "Some Text";

        string tempFileName = Path.GetTempFileName();
        expected.ToFile(tempFileName);

        bool exists = File.Exists(tempFileName);
        Assert.True(exists);

        string actual = File.ReadAllText(tempFileName);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToLines()
    {
        string source =
@"Line 1
Line 2
Line 3";

        string[] expected = ["Line 1", "Line 2", "Line 3"];

        var actual = source.ToLines();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WordCount()
    {
        string source = "some sample string";
        int expected = 3;
        int actual = source.WordCount();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WordCount_SpecificWord()
    {
        string source = "the quick brown fox jumped over the lazy dog";
        int expected = 2;
        int actual = source.WordCount("the");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void XmlDeserialize()
    {
        var expected = new TestObject
        {
            Value1 = 1,
            Value2 = "Test",
            Value3 = 4.5m
        };

        string xml = expected.XmlSerialize();
        var actual = xml.XmlDeserialize<TestObject>();

        Assert.Equal(expected, actual);
    }
}