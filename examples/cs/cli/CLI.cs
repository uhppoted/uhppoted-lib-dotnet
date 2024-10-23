using static System.Console;

class CLI
{
    static void Main(string[] args)
    {
        WriteLine("**uhppoted-Lib-dotnet C# CLI v0.8.10");
        WriteLine();

        foreach (string arg in args)
        {
            if (arg == "get-all-controllers")
            {
                Commands.GetControllers();
                Environment.Exit(0);
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
        WriteLine();
    }
}
