namespace RedisClone;

public class GetExecutor
{
    public RespMessage Execute(RespArray args)
    {
        if (args.Values==null||args.Values.Count < 2)
        {
            return new RespError("ERR wrong number of arguments for 'get' command");
        }
        var keyVal = args.Values[1] as RespBulkString;
        var key = keyVal.Value;
        var obj=RedisStore.Instance.Get(key);
        
        if (obj == null)
        {
            return new RespBulkString(null);
        }else if(obj.Type == typeof(int))
        {   if(obj.Expiry != null && obj.Expiry < DateTime.UtcNow)
            {
                RedisStore.Instance.Delete(key); // delete the key
                return new RespBulkString(null);
            }
            return new RespBulkString(RespBulkString.Serialize(obj.Value.ToString()));
        }
        else if (obj.Type == typeof(string))
        {
            if(obj.Expiry != null && obj.Expiry < DateTime.UtcNow)
            {
                RedisStore.Instance.Delete(key); // delete the key
                return new RespBulkString(null);
            }
            
            return new RespBulkString(RespBulkString.Serialize(obj.Value.ToString()));
        }
        else
        {
            return new RespError("ERR unsupported value type");
        }
       
    }
}