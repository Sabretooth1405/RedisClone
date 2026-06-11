namespace RedisClone;

public class Parser
{
        public static RespMessage Parse(string msg)
        {
            if (string.IsNullOrEmpty(msg) || msg.Length < 5)
            {
                throw new ArgumentException("Invalid RESP message");
            }
    
            char prefix = msg[0];
            return prefix switch
            {
                '+' => new RespSimpleString(msg),
                '-' => new RespError(msg),
                ':' => new RespInteger(msg),
                '*' => new RespArray(msg),
                '$' => new RespBulkString(msg),
                _ => throw new ArgumentException("Unknown RESP message type")
            };
        }
}