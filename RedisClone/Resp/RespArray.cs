using System.Collections;

namespace RedisClone;

public class RespArray:RespMessage
{
    public List<RespMessage>?Values { get; init; }

    public RespArray(string value)
    {
        Values= Parse(value);
    }

    public RespArray()
    {
        Values = new List<RespMessage>();
    }
    
    public static List<RespMessage>? Parse(string msg)
    {
        if (msg[0] != '*' || msg.Length < 5)
        {
            throw new ArgumentException("Invalid RESP Array");
        }
        var ls=msg.Split("\r\n");
        int len=int.Parse(ls[0].Substring(1));
        if (len == -1)        {
            return null;
        }
        var val = new List<RespMessage>();
        for (int i = 1; i <ls.Length-1; i+=2)
        {
            // Console.WriteLine($"Parsing RESP Array element: {ls[i]} and {ls[i+1]}");
            // RespBulkString bs = Parser.Parse(ls[i] + "\r\n" + ls[i + 1] + "\r\n") as RespBulkString;
            // Console.WriteLine(bs.Value);
            val.Add(Parser.Parse(ls[i]+"\r\n"+ls[i+1]+"\r\n"));
        }
        // Console.WriteLine($"Parsed RESP Array with {val.Count} elements");
        
        return val;
    }

    public override string Serialize()
    
    {
        if (Values == null)
        {
            return "*-1\r\n";
        }
        return '*'+Values.Count.ToString()+"\r\n"+string.Join("",Values.Select(v=>v.Serialize()));  
    }
}

