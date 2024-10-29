﻿using static System.Console;

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
                    Commands.GetControllers();
                    Environment.Exit(0);
                    break;

                case "get-controller":
                    Commands.GetController();
                    Environment.Exit(0);
                    break;

                case "set-IPv4":
                    Commands.SetIPv4();
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
        WriteLine();
    }
}
