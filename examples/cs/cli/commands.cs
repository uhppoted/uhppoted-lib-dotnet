using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

using System.Net;

using static System.Console;
using static uhppoted.Uhppoted;

class Commands
{
    const int TIMEOUT = 1000;
    const bool DEBUG = true;

    public static void GetControllers()
    {
        try
        {
            FSharpList<uhppoted.GetControllerResponse> controllers = get_all_controllers(TIMEOUT, DEBUG);

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
        }
    }

    public static void GetController()
    {
        try
        {
            var address = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 60000);
            var controller = new uhppoted.Controller(
                                 controller: 405419896u,
                                 address: FSharpOption<IPEndPoint>.Some(address),
                                 protocol: FSharpOption<string>.None);

            var result = get_controller(controller, TIMEOUT, DEBUG);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-controller");
                WriteLine("  controller {0}", response.controller);
                WriteLine("    address  {0}", response.address);
                WriteLine("    netmask  {0}", response.netmask);
                WriteLine("    gateway  {0}", response.gateway);
                WriteLine("    MAC      {0}", response.MAC);
                WriteLine("    version  {0}", response.version);
                WriteLine("    date     {0}", response.date);
                WriteLine();
            }
            else if (result.IsError)
            {
                throw new Exception(result.ErrorValue);
            }
        }
        catch (Exception err)
        {
            WriteLine("** ERROR  {0}", err.Message);
            WriteLine("** STACKTRACE  {0}" + err.StackTrace);
        }
    }
}

