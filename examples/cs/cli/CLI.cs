using static System.Console;

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
                    command.f(args[1..]);
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
            WriteLine("  - {0,-19}  {1}", command.command, command.description);
        }

        WriteLine();
    }
}
