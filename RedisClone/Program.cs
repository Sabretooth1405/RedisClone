using System.Net.Sockets;
using System.Text;

namespace RedisClone;


public class Program
{
    public static void Main(string[] args)
    {
      TcpListener server = new TcpListener(System.Net.IPAddress.Any, 6380);
      server.Start();
      while (true)
      {
          TcpClient client = server.AcceptTcpClient();
          Thread thread = new Thread(() => HandleClient(client));
          thread.Start();
      }
        

    }
    static List<string> SplitCommands(string input)
    {
        var commands = new List<string>();
        int i = 0;
        while (i < input.Length)
        {
            if (input[i] == '*')
            {
                int next = input.IndexOf('*', i + 1);
                if (next == -1)
                    commands.Add(input.Substring(i));
                else
                    commands.Add(input.Substring(i, next - i));
                i = next == -1 ? input.Length : next;
            }
            else i++;
        }
        return commands;
    }

    static void HandleClient(TcpClient client)
    { NetworkStream stream = client.GetStream();
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var commands = SplitCommands(message);

                // Console.WriteLine(message.Replace("\r\n", "\\r\\n"));
                foreach (string msg in commands)
                {


                    RespArray cmd;
                    try
                    {
                        cmd = Parser.Parse(msg) as RespArray;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }

                    if (cmd == null || cmd.Values == null || cmd.Values.Count == 0)
                    {
                        Console.WriteLine("Invalid RESP message type");
                    }

                    Console.WriteLine($"Received command with {cmd.Values.Count} arguments");
                    string response = Handler.Handle(cmd);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                }
            }


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            client.Close();
        }
    }
}

