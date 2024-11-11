using static System.Console;

class CLI
{
    static void Main(string[] args)
    {
        WriteLine("**uhppoted-Lib-dotnet C# CLI v0.8.10");
        WriteLine();

        if (args.Length > 0)
        {
            switch (args[0])
            {
                case "get-all-controllers":
                    Commands.GetControllers(args[1..]);
                    Environment.Exit(0);
                    break;

                case "get-controller":
                    Commands.GetController(args[1..]);
                    Environment.Exit(0);
                    break;

                case "set-IPv4":
                    Commands.SetIPv4(args[1..]);
                    Environment.Exit(0);
                    break;

                case "get-listener":
                    Commands.GetListener(args[1..]);
                    Environment.Exit(0);
                    break;

                case "set-listener":
                    Commands.SetListener(args[1..]);
                    Environment.Exit(0);
                    break;

                case "get-time":
                    Commands.GetTime(args[1..]);
                    Environment.Exit(0);
                    break;

                case "set-time":
                    Commands.SetTime(args[1..]);
                    Environment.Exit(0);
                    break;

                case "get-door":
                    Commands.GetDoor(args[1..]);
                    Environment.Exit(0);
                    break;

                case "set-door":
                    Commands.SetDoor(args[1..]);
                    Environment.Exit(0);
                    break;

                case "set-door-passcodes":
                    Commands.SetDoorPasscodes(args[1..]);
                    Environment.Exit(0);
                    break;

                case "open-door":
                    Commands.OpenDoor(args[1..]);
                    Environment.Exit(0);
                    break;

                case "get-status":
                    Commands.GetStatus(args[1..]);
                    Environment.Exit(0);
                    break;

                default:
                    WriteLine("** ERROR invalid command {0}", args[0]);
                    WriteLine();
                    break;
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
            WriteLine("  - {0,-19}  {1}", command.command, command.description);
        }

        WriteLine();
    }
}
