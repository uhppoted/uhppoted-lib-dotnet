using System.Net;

using static System.Console;
using static uhppoted.Uhppoted;

public readonly struct Command
{
    public readonly string command;
    public readonly string description;

    public Command(string command, string description)
    {
        this.command = command;
        this.description = description;
    }
}

class Commands
{
    const uint CONTROLLER = 405419896u;
    const int TIMEOUT = 1000;
    static readonly uhppoted.Options OPTIONS = new uhppoted.OptionsBuilder()
                                                           .WithDebug(true)
                                                           .build();

    public static List<Command> commands = new List<Command>
        {
    new Command ( "get-all-controllers","Retrieves a list of controllers accessible on the local LAN"),
    new Command ( "get-controller","Retrieves the controller information for a specific controller"),
    new Command ( "set-IPv4","Sets the controller IPv4 address, netmask and gateway"),
    new Command ( "get-listener","Retrieves the controller event listener address:port and auto-send interval"),
    new Command ( "set-listener","Sets the controller event listener address:port and auto-send interval"),
    new Command ( "get-time","Retrieves the controller system date and time"),
    new Command ( "set-time","Sets the controller system date and time"),
    new Command ( "get-door","Retrieves a controller door mode and delay settings"),
    new Command ( "set-door","Sets a controller door mode and delay"),
    new Command ( "set-door-passcodes","Sets the supervisor passcodes for a controller door"),
    new Command ( "open-door","Unlocks a door controlled by a controller"),
};

    public static void GetControllers(string[] args)
    {
        try
        {
            var result = get_all_controllers(TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var controllers = result.ResultValue;

                WriteLine("get-controllers: {0}", controllers.Length);
                foreach (var controller in controllers)
                {
                    WriteLine("  controller {0}", controller.controller);
                    WriteLine("    address  {0}", controller.address);
                    WriteLine("    netmask  {0}", controller.netmask);
                    WriteLine("    gateway  {0}", controller.gateway);
                    WriteLine("    MAC      {0}", controller.MAC);
                    WriteLine("    version  {0}", controller.version);
                    WriteLine("    date     {0}", YYYYMMDD(controller.date));
                    WriteLine();
                }
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

    public static void GetController(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();

            var result = get_controller(controller, TIMEOUT, OPTIONS);

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
                WriteLine("    date     {0}", YYYYMMDD(response.date));
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
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();

            var address = IPAddress.Parse("192.168.1.100");
            var netmask = IPAddress.Parse("255.255.255.0");
            var gateway = IPAddress.Parse("192.168.1.1");
            var result = set_IPv4(controller, address, netmask, gateway, TIMEOUT, OPTIONS);

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
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();

            var result = get_listener(controller, TIMEOUT, OPTIONS);

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
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();

            var endpoint = IPEndPoint.Parse("192.168.1.100:60001");
            var interval = (byte)30;
            var result = set_listener(controller, endpoint, interval, TIMEOUT, OPTIONS);

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

    public static void GetTime(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();

            var result = get_time(controller, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-time");
                WriteLine("  controller {0}", response.controller);
                WriteLine("    datetime {0}", YYYYMMDDHHmmss(response.datetime));
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

    public static void SetTime(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            var datetime = DateTime.Now;

            var result = set_time(controller, datetime, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("set-time");
                WriteLine("  controller {0}", response.controller);
                WriteLine("    datetime {0}", YYYYMMDDHHmmss(response.datetime));
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

    public static void GetDoor(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            byte door = 4;

            var result = get_door(controller, door, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-door");
                WriteLine("  controller {0}", response.controller);
                WriteLine("        door {0}", response.door);
                WriteLine("        mode {0}", response.mode);
                WriteLine("       delay {0}s", response.delay);
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

    public static void SetDoor(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            byte door = 4;
            byte mode = 2;
            byte delay = 7;

            var result = set_door(controller, door, mode, delay, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("set-door");
                WriteLine("  controller {0}", response.controller);
                WriteLine("        door {0}", response.door);
                WriteLine("        mode {0}", response.mode);
                WriteLine("       delay {0}s", response.delay);
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

    public static void SetDoorPasscodes(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            byte door = 4;
            uint[] passcodes = { 12345, 54321, 0, 999999 };

            var result = set_door_passcodes(controller, door, passcodes[0], passcodes[1], passcodes[2], passcodes[3], TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("set-door-passcodes");
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

    public static void OpenDoor(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            byte door = 4;

            var result = open_door(controller, door, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("open-door");
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

    private static string YYYYMMDD(DateOnly? date)
    {
        return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "---";
    }

    public static string YYYYMMDDHHmmss(DateTime? datetime)
    {
        return datetime.HasValue
            ? datetime.Value.ToString("yyyy-MM-dd HH:mm:ss")
            : "---";
    }
}
