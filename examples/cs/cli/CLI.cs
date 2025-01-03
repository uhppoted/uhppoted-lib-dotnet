﻿using static System.Console;

class CLI
{
    static void Main(string[] args)
    {
        WriteLine("**uhppoted-Lib-dotnet C# CLI v0.8.10");
        WriteLine();

        if (args.Length > 0)
        {
            var cmd = args[0];

            foreach (var command in Commands.commands)
            {
                if (command.command == cmd)
                {
                    try
                    {
                        command.f(args[1..]);
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
}
