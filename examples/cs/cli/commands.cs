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
    const int TIMEOUT = 1000;
    const string PROTOCOL = "udp";
    static readonly uhppoted.Options OPTIONS = new uhppoted.OptionsBuilder()
                                                           .WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000"))
                                                           .WithProtocol(PROTOCOL)
                                                           .WithDebug(true)
                                                           .build();

    const uint CONTROLLER = 1u;
    const byte INTERVAL = 0;
    const uint CARD = 1u;
    const uint CARD_INDEX = 1u;
    const uint EVENT_INDEX = 1u;
    const bool ENABLE = true;

    static readonly IPAddress ADDRESS = IPAddress.Parse("192.168.1.10");
    static readonly IPAddress NETMASK = IPAddress.Parse("255.255.255.0");
    static readonly IPAddress GATEWAY = IPAddress.Parse("192.168.1.1");
    static readonly IPEndPoint LISTENER = IPEndPoint.Parse("192.168.1.250:60001");

    public static List<Command> commands = new List<Command>
    {
          new Command ( "find-controllers","Retrieves a list of controllers accessible on the local LAN", FindControllers),
          new Command ( "get-controller","Retrieves the controller information from a controller", GetController),
          new Command ( "set-IPv4","Sets a controller IPv4 address, netmask and gateway", SetIPv4),
          new Command ( "get-listener","Retrieves a controller event listener address:port and auto-send interval", GetListener),
          new Command ( "set-listener","Sets a controller event listener address:port and auto-send interval", SetListener),
          new Command ( "get-time","Retrieves a controller system date and time", GetTime),
          new Command ( "set-time","Sets a controller system date and time",SetTime),
          new Command ( "get-door","Retrieves a controller door mode and delay settings", GetDoor),
          new Command ( "set-door","Sets a controller door mode and delay",SetDoor),
          new Command ( "set-door-passcodes","Sets the supervisor passcodes for a controller door",SetDoorPasscodes),
          new Command ( "open-door","Unlocks a door controlled by a controller",OpenDoor),
          new Command ( "get-status","Retrieves the current status of a controller",GetStatus),
          new Command ( "get-cards","Retrieves the number of cards stored on a controller",GetCards),
          new Command ( "get-card","Retrieves a card record from a controller",GetCard),
          new Command ( "get-card-at-index","Retrieves the card record stored at the index from a controller",GetCardAtIndex),
          new Command ( "put-card","Adds or updates a card record on controller",PutCard),
          new Command ( "delete-card", "Deletes a card record from a controller", DeleteCard),
          new Command ( "delete-all-cards", "Deletes all card records from a controller", DeleteAllCards),
          new Command ( "get-event","Retrieves the event record stored at the index from a controller",GetEvent),
          new Command ( "get-event-index","Retrieves the current event index from a controller",GetEventIndex),
          new Command ( "set-event-index","Sets a controller event index",SetEventIndex),
          new Command ( "record-special-events","Enables events for door open/close, button press, etc",RecordSpecialEvents),
    };

    public static void FindControllers(string[] args)
    {
        var result = Uhppoted.FindControllers(TIMEOUT, OPTIONS);

        if (result.IsOk)
        {
            var controllers = result.ResultValue;

            WriteLine("find-controllers: {0}", controllers.Length);
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

    public static void GetController(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetController(controller, timeout, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-controller");
            WriteLine("  controller {0}", record.controller);
            WriteLine("    address  {0}", record.address);
            WriteLine("    netmask  {0}", record.netmask);
            WriteLine("    gateway  {0}", record.gateway);
            WriteLine("    MAC      {0}", record.MAC);
            WriteLine("    version  {0}", record.version);
            WriteLine("    date     {0}", YYYYMMDD(record.date));
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetIPv4(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var address = ArgParse.Parse(args, "--address", ADDRESS);
        var netmask = ArgParse.Parse(args, "--netmask", NETMASK);
        var gateway = ArgParse.Parse(args, "--gateway", GATEWAY);
        var timeout = TIMEOUT;
        var options = OPTIONS;

        var result = Uhppoted.SetIPv4(controller, address, netmask, gateway, timeout, options);

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

    public static void GetListener(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetListener(controller, timeout, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-listener");
            WriteLine("  controller {0}", controller);
            WriteLine("    endpoint {0}", record.endpoint);
            WriteLine("    interval {0}s", record.interval);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetListener(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var listener = ArgParse.Parse(args, "--listener", LISTENER);
        var interval = ArgParse.Parse(args, "--interval", INTERVAL);
        var result = Uhppoted.SetListener(controller, listener, interval, TIMEOUT, OPTIONS);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-listener");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetTime(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var result = Uhppoted.GetTime(controller, TIMEOUT, OPTIONS);

        if (result.IsOk)
        {
            var datetime = result.ResultValue;

            WriteLine("get-time");
            WriteLine("  controller {0}", controller);
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime));
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetTime(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var now = ArgParse.Parse(args, "--datetime", DateTime.Now);

        var result = Uhppoted.SetTime(controller, now, TIMEOUT, OPTIONS);

        if (result.IsOk)
        {
            var datetime = result.ResultValue;

            WriteLine("set-time");
            WriteLine("  controller {0}", controller);
            WriteLine("    datetime {0}", YYYYMMDDHHmmss(datetime));
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
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
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetCards(controller, timeout, options);

        if (result.IsOk)
        {
            var cards = result.ResultValue;

            WriteLine("get-cards");
            WriteLine("  controller {0}", controller);
            WriteLine("       cards {0}", cards);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetCard(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetCard(controller, card, timeout, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-card");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", record.card);
            WriteLine("  start date {0}", (YYYYMMDD(record.startdate)));
            WriteLine("    end date {0}", (YYYYMMDD(record.enddate)));
            WriteLine("      door 1 {0}", record.door1);
            WriteLine("      door 2 {0}", record.door2);
            WriteLine("      door 3 {0}", record.door3);
            WriteLine("      door 4 {0}", record.door4);
            WriteLine("         PIN {0}", record.PIN);
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

    public static void GetCardAtIndex(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = CARD_INDEX;
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetCardAtIndex(controller, index, timeout, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-card-at-index");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", record.card);
            WriteLine("  start date {0}", (YYYYMMDD(record.startdate)));
            WriteLine("    end date {0}", (YYYYMMDD(record.enddate)));
            WriteLine("      door 1 {0}", record.door1);
            WriteLine("      door 2 {0}", record.door2);
            WriteLine("      door 3 {0}", record.door3);
            WriteLine("      door 4 {0}", record.door4);
            WriteLine("         PIN {0}", record.PIN);
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
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);
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

    public static void DeleteCard(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);

        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.DeleteCard(controller, card, timeout, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("delete-card");
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

    public static void DeleteAllCards(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.DeleteAllCards(controller, timeout, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("delete-all-cards");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetEvent(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = ArgParse.Parse(args, "--index", EVENT_INDEX);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetEvent(controller, index, timeout, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-event");
            WriteLine("  controller {0}", controller);
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.timestamp)));
            WriteLine("       index {0}", record.index);
            WriteLine("       event {0}", record.event_type);
            WriteLine("     granted {0}", record.access_granted);
            WriteLine("        door {0}", record.door);
            WriteLine("   direction {0}", record.direction);
            WriteLine("        card {0}", record.card);
            WriteLine("      reason {0}", record.reason);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("event not found");
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetEventIndex(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.GetEventIndex(controller, timeout, options);

        if (result.IsOk)
        {
            var index = result.ResultValue;

            WriteLine("get-event-index");
            WriteLine("  controller {0}", controller);
            WriteLine("       index {0}", index);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetEventIndex(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = ArgParse.Parse(args, "--index", EVENT_INDEX);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.SetEventIndex(controller, index, timeout, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-event-index");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void RecordSpecialEvents(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var enable = ArgParse.Parse(args, "--enable", ENABLE);
        var timeout = TIMEOUT;
        var options = OPTIONS;
        var result = Uhppoted.RecordSpecialEvents(controller, enable, timeout, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("record-special-events");
            WriteLine("  controller {0}", controller);
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
