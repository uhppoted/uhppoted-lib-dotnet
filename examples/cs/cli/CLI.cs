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
        WriteLine("  - get-all-controllers  Retrieves a list of controllers accessible on the local LAN");
        WriteLine("  - get-controller       Retrieves the controller information for a specific controller");
        WriteLine("  - set-IPv4             Sets the controller IPv4 address, netmask and gateway");
        WriteLine("  - get-listener         Retrieves the controller event listener address:port and auto-send interval");
        WriteLine("  - set-listener         Sets the controller event listener address:port and auto-send interval");
        WriteLine("  - get-time             Retrieves the controller system date and time");
        WriteLine("  - set-time             Sets the controller system date and time");
        WriteLine("  - get-door             Retrieves a controller door mode and delay settings");
        WriteLine("  - set-door             Sets a controller door mode and delay");
        WriteLine();
    }
}
