using System.Net;

using static System.Console;
using uhppoted;

public readonly struct Command
{
    public readonly string command;
    public readonly string description;
    public readonly Action<string[]> f;

    public Command(string command, string description, Action<string[]> f)
    {
        this.command = command;
        this.description = description;
        this.f = f;
    }
}

class Commands
{
    const uint CONTROLLER = 405419896u;
    const uint CARD = 10058400u;
    const uint CARD_INDEX = 1u;
    const int TIMEOUT = 1000;
    const string PROTOCOL = "udp";
    static readonly IPEndPoint ADDRESS = IPEndPoint.Parse("192.168.1.100:60000");

    static readonly uhppoted.Options OPTIONS = new uhppoted.OptionsBuilder()
                                                           .WithDestination(ADDRESS)
                                                           .WithProtocol(PROTOCOL)
                                                           .WithDebug(true)
                                                           .build();

    public static List<Command> commands = new List<Command>
    {
          new Command ( "get-all-controllers","Retrieves a list of controllers accessible on the local LAN", GetControllers),
          new Command ( "get-controller","Retrieves the controller information for a specific controller", GetController),
          new Command ( "set-IPv4","Sets the controller IPv4 address, netmask and gateway", SetIPv4),
          new Command ( "get-listener","Retrieves the controller event listener address:port and auto-send interval", GetListener),
          new Command ( "set-listener","Sets the controller event listener address:port and auto-send interval", SetListener),
          new Command ( "get-time","Retrieves the controller system date and time", GetTime),
          new Command ( "set-time","Sets the controller system date and time",SetTime),
          new Command ( "get-door","Retrieves a controller door mode and delay settings", GetDoor),
          new Command ( "set-door","Sets a controller door mode and delay",SetDoor),
          new Command ( "set-door-passcodes","Sets the supervisor passcodes for a controller door",SetDoorPasscodes),
          new Command ( "open-door","Unlocks a door controlled by a controller",OpenDoor),
          new Command ( "get-status","Retrieves the current status of the controller",GetStatus),
          new Command ( "get-cards","Retrieves the number of cards stored on the controller",GetCards),
          new Command ( "get-card","Retrieves a card record from the controller",GetCard),
          new Command ( "get-card-at-index","Retrieves the card record stored at the index from the controller",GetCardAtIndex),
          new Command ( "put-card","Adds or updates a card record on controller",PutCard),
    };

    public static void GetControllers(string[] args)
    {
        try
        {
            var result = Uhppoted.get_all_controllers(TIMEOUT, OPTIONS);

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

            var result = Uhppoted.get_controller(controller, TIMEOUT, OPTIONS);

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
            var result = Uhppoted.set_IPv4(controller, address, netmask, gateway, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.get_listener(controller, TIMEOUT, OPTIONS);

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
            var result = Uhppoted.set_listener(controller, endpoint, interval, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.get_time(controller, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.set_time(controller, datetime, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.get_door(controller, door, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.set_door(controller, door, mode, delay, TIMEOUT, OPTIONS);

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

            var result = Uhppoted.set_door_passcodes(controller, door, passcodes[0], passcodes[1], passcodes[2], passcodes[3], TIMEOUT, OPTIONS);

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

            var result = Uhppoted.open_door(controller, door, TIMEOUT, OPTIONS);

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

    public static void GetStatus(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            var result = Uhppoted.get_status(controller, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-status");
                WriteLine("         controller {0}", response.controller);
                WriteLine("        door 1 open {0}", response.door1_open);
                WriteLine("        door 2 open {0}", response.door2_open);
                WriteLine("        door 3 open {0}", response.door3_open);
                WriteLine("        door 4 open {0}", response.door3_open);
                WriteLine("   button 1 pressed {0}", response.door1_button);
                WriteLine("   button 2 pressed {0}", response.door1_button);
                WriteLine("   button 3 pressed {0}", response.door1_button);
                WriteLine("   button 4 pressed {0}", response.door1_button);
                WriteLine("       system error {0}", response.system_error);
                WriteLine("   system date/time {0}", YYYYMMDDHHmmss(response.system_datetime));
                WriteLine("       sequence no. {0}", response.sequence_number);
                WriteLine("       special info {0}", response.special_info);
                WriteLine("             relays {0:X}", response.relays);
                WriteLine("             inputs {0:X}", response.inputs);
                WriteLine();
                WriteLine("    event index     {0}", response.evt.index);
                WriteLine("          event     {0}", response.evt.event_type);
                WriteLine("          granted   {0}", response.evt.granted);
                WriteLine("          door      {0}", response.evt.door);
                WriteLine("          direction {0}", response.evt.direction);
                WriteLine("          card      {0}", response.evt.card);
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(response.evt.timestamp));
                WriteLine("          reason    {0}", response.evt.reason);
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

    public static void GetCards(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            var result = Uhppoted.get_cards(controller, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-cards");
                WriteLine("  controller {0}", response.controller);
                WriteLine("       cards {0}", response.cards);
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

    public static void GetCard(string[] args)
    {
        try
        {
            var controller = new uhppoted.ControllerBuilder(CONTROLLER)
                                         .With(IPEndPoint.Parse("192.168.1.100:60000"))
                                         .With("udp")
                                         .build();
            var card = CARD;
            var result = Uhppoted.GetCard(controller, card, TIMEOUT, OPTIONS);

            if (result.IsOk)
            {
                var response = result.ResultValue;

                WriteLine("get-card");
                WriteLine("  controller {0}", response.controller);
                WriteLine("        card {0}", response.card);
                WriteLine("  start date {0}", (YYYYMMDD(response.startdate)));
                WriteLine("    end date {0}", (YYYYMMDD(response.enddate)));
                WriteLine("      door 1 {0}", response.door1);
                WriteLine("      door 2 {0}", response.door2);
                WriteLine("      door 3 {0}", response.door3);
                WriteLine("      door 4 {0}", response.door4);
                WriteLine("         PIN {0}", response.PIN);
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

    public static void GetCardAtIndex(string[] args)
    {
        var controller = CONTROLLER;
        var index = CARD_INDEX;
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetCardAtIndex(controller, index, timeout, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var card = result.ResultValue.Value;

            WriteLine("get-card-at-index");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", card.card);
            WriteLine("  start date {0}", (YYYYMMDD(card.startdate)));
            WriteLine("    end date {0}", (YYYYMMDD(card.enddate)));
            WriteLine("      door 1 {0}", card.door1);
            WriteLine("      door 2 {0}", card.door2);
            WriteLine("      door 3 {0}", card.door3);
            WriteLine("      door 4 {0}", card.door4);
            WriteLine("         PIN {0}", card.PIN);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("card not found");
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void PutCard(string[] args)
    {
        var controller = CONTROLLER;
        var card = CARD;
        var startdate = new DateOnly(2024, 1, 1);
        var enddate = new DateOnly(2024, 12, 31);
        byte door1 = 1;
        byte door2 = 0;
        byte door3 = 17;
        byte door4 = 1;
        var PIN = 7531u;

        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.PutCard(controller, card, startdate, enddate, door1, door2, door3, door4, PIN, timeout, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("put-card");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", card);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
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
