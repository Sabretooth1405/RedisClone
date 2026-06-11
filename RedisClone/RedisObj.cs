namespace RedisClone;

public class RedisObj
{
        public Type Type { get; set; }
        public object Value { get; set; }
        
        public DateTime? Expiry { get; set; }
        
        public RedisObj(Type type, object value, DateTime? expiry)
        {
                Type = type;
                Value = value;
                Expiry = expiry;
        }
        
}