using static System.Console;
using System.Net;

using uhppoted;

class CLI
{
    static void Main(string[] args)
    {
        WriteLine("**uhppoted-Lib-dotnet C# CLI v0.9.0");
        WriteLine();

        var (options, remaining) = parse(args);

        if (remaining.Length > 0)
        {
            var cmd = remaining[0];

            foreach (var command in Commands.commands)
            {
                if (command.command == cmd)
                {
                    try
                    {
                        command.f(options, remaining[1..]);
                    }
                    catch (Exception err)
                    {
                        WriteLine("** ERROR  {0}", err.Message);
                    }

                    Environment.Exit(0);
                }
            }
        }

        usage();
        Environment.Exit(1);
    }

    static void usage()
    {
        WriteLine("Usage: dotnet run <command>");
        WriteLine();
        WriteLine("  Supported commands:");

        foreach (var command in Commands.commands)
        {
            WriteLine("  - {0,-26}  {1}", command.command, command.description);
        }

        WriteLine();
    }

    static (uhppoted.Options options, string[] args) parse(string[] args)
    {
        var builder = new uhppoted.OptionsBuilder()
            .WithBind(new IPEndPoint(IPAddress.Any, 0))
            .WithBroadcast(new IPEndPoint(IPAddress.Broadcast, 60000))
            .WithListen(new IPEndPoint(IPAddress.Any, 60001))
            .WithTimeout(1000)
            .WithDebug(true);

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--bind" when i + 1 < args.Length:
                    builder.WithBind(IPEndPoint.Parse(args[++i]));
                    break;

                case "--broadcast" when i + 1 < args.Length:
                    builder.WithBroadcast(IPEndPoint.Parse(args[++i]));
                    break;

                case "--listen" when i + 1 < args.Length:
                    builder.WithListen(IPEndPoint.Parse(args[++i]));
                    break;

                case "--timeout" when i + 1 < args.Length:
                    int timeoutMs = (int)(double.Parse(args[++i]) * 1000.0);
                    builder.WithTimeout(timeoutMs);
                    break;

                case "--debug":
                    builder.WithDebug(true);
                    break;

                default:
                    return (builder.Build(), args[i..]);
            }
        }

        return (builder.Build(), args);
    }

}


