namespace RedisClone;

public class RespInteger:RespMessage
{
    public int Value { get; init; }

    public RespInteger(string value)
    {
        Value = Parse(value);
    }

    private static int Parse(string msg)
    {
        if (msg[0] != ':' || msg.Length < 5)
        {
            throw new ArgumentException("Invalid Resp Integer");
        }

        try
        {
            return int.Parse(msg.Substring(1, msg.Length - 3));
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Resp Integer");
        }
    }

    public override string Serialize()
    {
        return ':' + Value.ToString() + "\r\n";
    }
    
}