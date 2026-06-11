using System.Collections.Concurrent;

namespace RedisClone;

public class RedisStore
{
    public static RedisStore Instance { get; } = new RedisStore();
    
    private ConcurrentDictionary<string, RedisObj> _data = new();
    
    
    private RedisStore() { }  // private constructor, nobody can do new RedisStore()
    
  
    public RedisObj? Get(string key)
    {
        _data.TryGetValue(key, out var value);
        return value;
    }
    public void Set(string key, Object value,Type? type, DateTime? expiry=null)
    {   type ??= value.GetType();
        _data[key] = new RedisObj(type, value,expiry);
    }
    public void Delete(string key)
    {
        _data.TryRemove(key, out _);
    }
    public void Clear()
    {
        _data.Clear();
    }
    
}