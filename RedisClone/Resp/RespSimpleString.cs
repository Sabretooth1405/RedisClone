namespace RedisClone;

public class RespSimpleString:RespMessage
{
    public string Value { get; init; }

    public RespSimpleString(string value)
    {
        
        Value = Parse(value);
    }

    private static string Parse(string msg)
    {
        if (msg[0] != '+'|| msg.Length<5)
        {
            throw new ArgumentException("Invalid RESP Simple String");
        }
        return msg.Substring(1, msg.Length - 3);
    }

    public override string Serialize()
    {
        return "+" + Value+"\r\n";
    }

    public static string Serialize(string msg)
    {
        return "+" + msg + "\r\n";
    }
}