using Microsoft.VisualBasic;

namespace RedisClone;

public class RespBulkString:RespMessage
{
    public string? Value { get; init; }
    
    
    public RespBulkString(string value)
    {
        if (value != null)
        {
            Value = Parse(value);
        }
       
    }

    public static string Parse(string msg)
    {
        if(msg[0]!='$')
        {
            throw new ArgumentException("Invalid RESP Bulk String");
        }

        var s=msg.Split("\r\n");
        int len=int.Parse(s[0].Substring(1));
        if (len == -1)
        {
            return null;
        } else if (s.Length != 3 || len != s[1].Length)
        {
            throw new ArgumentException("Invalid RESP Bulk String");
        }

        return s[1];
    }
    
    public override string Serialize()
    {
        if (Value == null)
        {
            return "$-1\r\n";
        }
        return '$'+Value.Length.ToString()+"\r\n"+Value+"\r\n";
    }
    
    public static string Serialize(string msg)
    {
        if (msg == null)
        {
            return "$-1\r\n";
        }
        return '$' + msg.Length.ToString() + "\r\n" + msg + "\r\n";
    }
    
}