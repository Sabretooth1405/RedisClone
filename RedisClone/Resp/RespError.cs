namespace RedisClone;

public class RespError:RespMessage
{
    public string Value { get; init; }

    public RespError(string value)
    {
        Value = Parse(value);
    }

    private static string Parse(string msg)
    {
        if (msg.Length < 5 || msg[0] != '-')
        {
            throw new ArgumentException("Invalid RESP Error");
        }
        return msg.Substring(1,msg.Length - 3);
    }

    public override string Serialize()
    {
        return '-'+Value+"\r\n";
    }
    public static string Serialize(string msg)
    {
        return '-' + msg + "\r\n";
    }
}