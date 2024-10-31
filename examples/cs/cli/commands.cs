using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

using System.Net;

using static System.Console;
using static uhppoted.Uhppoted;

class Commands
{
    const uint CONTROLLER = 405419896u;
    const int TIMEOUT = 1000;
    const bool DEBUG = true;

    private static string YYYYMMDD(DateOnly? date)
    {
        return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "---";
    }

    public static void GetControllers(string[] args)
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

    public static void GetController(string[] args)
    {
        try
        {
            var addr = IPEndPoint.Parse("192.168.1.100:60000");
            var controller = new uhppoted.Controller(
                                 controller: CONTROLLER,
                                 address: FSharpOption<IPEndPoint>.Some(addr),
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
        }
    }

    public static void SetIPv4(string[] args)
    {
        try
        {
            var addr = IPEndPoint.Parse("192.168.1.100:60000");
            var controller = new uhppoted.Controller(
                                 controller: CONTROLLER,
                                 address: FSharpOption<IPEndPoint>.Some(addr),
                                 protocol: FSharpOption<string>.None);

            var address = IPAddress.Parse("192.168.1.100");
            var netmask = IPAddress.Parse("255.255.255.0");
            var gateway = IPAddress.Parse("192.168.1.1");
            var result = set_IPv4(controller, address, netmask, gateway, TIMEOUT, DEBUG);

            if (result.IsOk)
            {
                WriteLine("set-IPv4");
                WriteLine("  ok");
            }
            else if (result.IsError)
            {
                throw new Exception(result.ErrorValue);
            }
        }
        catch (Exception err)
        {
            WriteLine("** ERROR  {0}", err.Message);
        }
    }

    public static void GetListener(string[] args)
    {
        try
        {
            var addr = IPEndPoint.Parse("192.168.1.100:60000");
            var controller = new uhppoted.Controller(
                                 controller: CONTROLLER,
                                 address: FSharpOption<IPEndPoint>.Some(addr),
                                 protocol: FSharpOption<string>.None);

            var result = get_listener(controller, TIMEOUT, DEBUG);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-listener");
                WriteLine("  controller {0}", response.controller);
                WriteLine("    endpoint {0}", response.endpoint);
                WriteLine("    interval {0}s", response.interval);
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
        }
    }

    public static void SetListener(string[] args)
    {
        try
        {
            var addr = IPEndPoint.Parse("192.168.1.100:60000");
            var controller = new uhppoted.Controller(
                                 controller: CONTROLLER,
                                 address: FSharpOption<IPEndPoint>.Some(addr),
                                 protocol: FSharpOption<string>.None);

            var endpoint = IPEndPoint.Parse("192.168.1.100:60001");
            var interval = (byte)30;
            var result = set_listener(controller, endpoint, interval, TIMEOUT, DEBUG);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("set-listener");
                WriteLine("  controller {0}", response.controller);
                WriteLine("          ok {0}", response.ok);
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
        }
    }

}

