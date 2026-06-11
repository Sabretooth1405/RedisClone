namespace RedisCloneTest;
using RedisClone;

public class ExecutorTests : IDisposable
{
    public ExecutorTests()
    {
        RedisStore.Instance.Clear();
    }

    public void Dispose() { }

    private RespArray MakeArgs(params string[] values)
    {
        var arr = new RespArray();
        foreach (var v in values)
            arr.Values.Add(new RespBulkString(RespBulkString.Serialize(v)));
        return arr;
    }

    [Fact]
    public void Set_And_Get_ReturnsCorrectValue()
    {
        var setResult = new SetExecutor().Execute(MakeArgs("SET", "mykey", "myvalue")) as RespSimpleString;
        Assert.NotNull(setResult);
        Assert.Equal("OK", setResult.Value);

        var getResult = new GetExecutor().Execute(MakeArgs("GET", "mykey")) as RespBulkString;
        Assert.NotNull(getResult);
        Assert.Equal("myvalue", getResult.Value);
    }

    [Fact]
    public void Get_NonExistentKey_ReturnsNull()
    {
        var getResult = new GetExecutor().Execute(MakeArgs("GET", "nonexistent")) as RespBulkString;
        Assert.NotNull(getResult);
        Assert.Null(getResult.Value);
    }

    [Fact]
    public void Set_WithExpiry_KeyExpiresCorrectly()
    {
        var setResult = new SetExecutor().Execute(MakeArgs("SET", "tempkey", "tempvalue", "EX", "1")) as RespSimpleString;
        Assert.NotNull(setResult);
        Assert.Equal("OK", setResult.Value);

        Thread.Sleep(2000);

        var getResult = new GetExecutor().Execute(MakeArgs("GET", "tempkey")) as RespBulkString;
        Assert.NotNull(getResult);
        Assert.Null(getResult.Value);
    }
}