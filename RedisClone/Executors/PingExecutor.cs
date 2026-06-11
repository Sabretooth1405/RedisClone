namespace RedisClone;

public class PingExecutor:Executable
{
    
    public  RespMessage Execute()
    {
        return new RespSimpleString(RespSimpleString.Serialize("PONG"));
    }
    
}