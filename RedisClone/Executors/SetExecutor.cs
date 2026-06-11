namespace RedisClone;

public class SetExecutor
{
    public RespMessage Execute(RespArray args)
    {
        if (args.Values.Count < 3)
        {
            return new RespError(RespError.Serialize("ERR wrong number of arguments for 'set' command"));
        }
        DateTime? expiry = null;

        if (args.Values.Count >= 5)
        { Console.WriteLine("Parsing options for SET command");
            var option = (args.Values[3] as RespBulkString).Value.ToUpper();
            var optionValue = long.Parse((args.Values[4] as RespBulkString).Value);
    
            expiry = option switch
            {
                "EX"   => DateTime.UtcNow.AddSeconds(optionValue),
                "PX"   => DateTime.UtcNow.AddMilliseconds(optionValue),
                "EXAT" => DateTimeOffset.FromUnixTimeSeconds(optionValue).UtcDateTime,
                "PXAT" => DateTimeOffset.FromUnixTimeMilliseconds(optionValue).UtcDateTime,
                _ => throw new ArgumentException($"Unknown option: {option}")
            };
        }

       
        var keyVal = args.Values[1] as RespBulkString;
        var valueVal = args.Values[2] as RespBulkString;
        var key = keyVal.Value;
        var value = valueVal.Value;
        
        if (int.TryParse(value, out int intValue))
        {  
            RedisStore.Instance.Set(key, intValue,intValue.GetType(),expiry);
        }
        else
        {
            RedisStore.Instance.Set(key,value,value.GetType(),expiry);
        }
        Console.WriteLine($"Setting key: {key} with value: {value} and expiry: {expiry}");
        Console.WriteLine("Value is of "+value.GetType());
        
        return new RespSimpleString(RespSimpleString.Serialize("OK"));
        
    }
    
}