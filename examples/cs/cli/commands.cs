using Microsoft.FSharp.Collections;

using static System.Console;
using static uhppoted.Uhppoted;

class Commands
{

    public static void GetControllers()
    {
        try
        {
            FSharpList<uhppoted.GetControllerResponse> controllers = get_controllers();

            WriteLine("get-controllers: {0}", controllers.Length);
            foreach (var controller in controllers)
            {
                WriteLine("  controller {0}", controller.controller);
                WriteLine("    address  {0}", controller.address);
                WriteLine("    netmask  {0}", controller.netmask);
                WriteLine("    gateway  {0}", controller.gateway);
                WriteLine("    MAC      {0}", controller.MAC);
                WriteLine("    version  {0}", controller.version);
                WriteLine("    date     {0}", controller.date);
                WriteLine();
            }
        }
        catch (Exception err)
        {
            WriteLine("** ERROR  {0}", err.Message);
            WriteLine("** STACKTRACE  {0}" + err.StackTrace);
        }
    }
}

