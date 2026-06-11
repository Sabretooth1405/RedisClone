namespace RedisCloneTest;

using RedisClone;



public class RespParserTests
{
    [Fact]
    public void Parse_SimpleString_ReturnsCorrectValue()
    {
        var result = Parser.Parse("+OK\r\n") as RespSimpleString;
        Assert.NotNull(result);
        Assert.Equal("OK", result.Value);
    }

    [Fact]
    public void Parse_SimpleString_HelloWorld()
    {
        var result = Parser.Parse("+hello world\r\n") as RespSimpleString;
        Assert.NotNull(result);
        Assert.Equal("hello world", result.Value);
    }

    // Error
    [Fact]
    public void Parse_Error_ReturnsCorrectMessage()
    {
        var result = Parser.Parse("-Error message\r\n") as RespError;
        Assert.NotNull(result);
        Assert.Equal("Error message", result.Value);
    }

    // Integer
    [Fact]
    public void Parse_Integer_ReturnsCorrectValue()
    {
        var result = Parser.Parse(":42\r\n") as RespInteger;
        Assert.NotNull(result);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Parse_Integer_NegativeValue()
    {
        var result = Parser.Parse(":-1\r\n") as RespInteger;
        Assert.NotNull(result);
        Assert.Equal(-1, result.Value);
    }

    // Bulk String
    [Fact]
    public void Parse_BulkString_ReturnsCorrectValue()
    {
        var result = Parser.Parse("$5\r\nhello\r\n") as RespBulkString;
        Assert.NotNull(result);
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void Parse_BulkString_EmptyString()
    {
        var result = Parser.Parse("$0\r\n\r\n") as RespBulkString;
        Assert.NotNull(result);
        Assert.Equal("", result.Value);
    }

    [Fact]
    public void Parse_BulkString_Null()
    {
        var result = Parser.Parse("$-1\r\n") as RespBulkString;
        Assert.NotNull(result);
        Assert.Null(result.Value);
    }

    // Array
    [Fact]
    public void Parse_Array_ReturnsCorrectElements()
    {
        var result = Parser.Parse("*2\r\n$3\r\nGET\r\n$3\r\nkey\r\n") as RespArray;
        Assert.NotNull(result);
        Assert.Equal(2, result.Values.Count);
    }

    [Fact]
    public void Parse_Array_NullArray()
    {
        var result = Parser.Parse("*-1\r\n") as RespArray;
        Assert.NotNull(result);
        Assert.Null(result.Values);
    }

    [Fact]
    public void Parse_Array_PingCommand()
    {
        var result = Parser.Parse("*1\r\n$4\r\nping\r\n") as RespArray;
        Assert.NotNull(result);
        Assert.Single(result.Values);
    }

    // Serialize roundtrip
    [Fact]
    public void Serialize_SimpleString_RoundTrip()
    {
        var original = "+OK\r\n";
        var result = Parser.Parse(original);
        Assert.Equal(original, result.Serialize());
    }

    [Fact]
    public void Serialize_Error_RoundTrip()
    {
        var original = "-Error message\r\n";
        var result = Parser.Parse(original);
        Assert.Equal(original, result.Serialize());
    }

    [Fact]
    public void Serialize_BulkString_RoundTrip()
    {
        var original = "$5\r\nhello\r\n";
        var result = Parser.Parse(original);
        Assert.Equal(original, result.Serialize());
    }

    // Invalid inputs
    [Fact]
    public void Parse_InvalidPrefix_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => Parser.Parse("invalid\r\n"));
    }

    [Fact]
    public void Parse_EmptyString_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => Parser.Parse(""));
    }
}