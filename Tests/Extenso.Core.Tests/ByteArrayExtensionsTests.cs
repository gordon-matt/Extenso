using System.Text;

namespace Extenso.Core.Tests;

public class ByteArrayExtensionsTests
{
    [Fact]
    public void Base64Serialize()
    {
        string source = "the quick brown fox jumped over the lazy dog";
        byte[] bytes = Encoding.UTF8.GetBytes(source);
        string base64 = bytes.Base64Serialize();

        byte[] deserializedBytes = base64.Base64Deserialize();
        string deserializedString = Encoding.UTF8.GetString(deserializedBytes);

        Assert.Equal(source, deserializedString);
    }

    [Fact]
    public void BinaryDeserialize()
    {
        var expected = new TestObject
        {
            Value1 = 1,
            Value2 = "Test",
            Value3 = 4.5m
        };
        byte[] serialized = expected.BinarySerialize();
        var deserialized = serialized.BinaryDeserialize<TestObject>();

        Assert.Equal(expected, deserialized);
    }

    [Fact]
    public void XmlDeserialize()
    {
        var source = new TestObject
        {
            Value1 = 1,
            Value2 = "Test",
            Value3 = 4.5m
        };

        string xml = source.XmlSerialize();
        byte[] bytes = Encoding.UTF8.GetBytes(xml);
        var deserialized = bytes.XmlDeserialize<TestObject>(Encoding.UTF8);

        Assert.Equal(source, deserialized);
    }
}