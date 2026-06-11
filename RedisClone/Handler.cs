namespace RedisClone;

public class Handler
{
    public static string Handle(RespArray command)
    {
        if (command.Values==null||command.Values.Count == 0)
        {
            return new RespError("Empty command").Serialize();
        }

        int i = 0;
        while (i < command.Values.Count)
        {
            var cmdRespMessage = command.Values[i];
            string cmd;
            if(cmdRespMessage.GetType()==typeof(RespBulkString))
            {
                RespBulkString respBulkString = (RespBulkString) cmdRespMessage;
                cmd = respBulkString.Value.ToString().ToUpper();
            }
            else
            {
                throw new ArgumentException("Invalid command type, expected RESP Bulk String");
            }
            Console.WriteLine(cmdRespMessage.GetType());
            Console.WriteLine(cmd);
            return cmd switch
            {
                "PING" => new PingExecutor().Execute().Serialize(),
                "ECHO"=> new EchoExecutor().Execute(command.Values[i+1] as RespBulkString).Serialize(),
                "SET" => new SetExecutor().Execute(command).Serialize(),
                "GET" => new GetExecutor().Execute(command).Serialize(),
                _ => RespError.Serialize($"ERR unknown command '{cmd}'")
            };
            
        }

        return "";

    }
}