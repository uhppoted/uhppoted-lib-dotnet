using System.Net;

using static System.Console;

using uhppoted;
using static uhppoted.Err;

public readonly struct Command
{
    public readonly string command;
    public readonly string description;
    public readonly Action<uhppoted.Options, string[]> f;

    public Command(string command, string description, Action<uhppoted.Options, string[]> f)
    {
        this.command = command;
        this.description = description;
        this.f = f;
    }
}

class Commands
{
    static readonly Dictionary<uint, uhppoted.C> CONTROLLERS = new Dictionary<uint, uhppoted.C>()
   {
    { 303986753u, new uhppoted.CBuilder(303986753u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.125:60000"))
                              .WithProtocol("udp")
                              .Build()
    },
    { 201020304u, new uhppoted.CBuilder(201020304u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.125:60000"))
                              .WithProtocol("tcp")
                              .Build()
    },
   };

    const uint CONTROLLER = 1u;

    const byte INTERVAL = 0;
    const byte DOOR = 1;
    const DoorMode MODE = DoorMode.Controlled;
    const byte DELAY = 5;
    const uint CARD = 1u;
    const uint CARD_INDEX = 1u;
    const uint EVENT_INDEX = 1u;
    const bool ENABLE = true;
    const byte TIME_PROFILE_ID = 0;
    const TaskCode TASK_CODE = TaskCode.Unknown;

    static readonly IPAddress ADDRESS = IPAddress.Parse("192.168.1.10");
    static readonly IPAddress NETMASK = IPAddress.Parse("255.255.255.0");
    static readonly IPAddress GATEWAY = IPAddress.Parse("192.168.1.1");
    static readonly IPEndPoint LISTENER = IPEndPoint.Parse("192.168.1.125:60001");
    static readonly DateOnly START_DATE = new DateOnly(2026, 1, 1);
    static readonly DateOnly END_DATE = new DateOnly(2026, 12, 31);

    public static List<Command> commands = new List<Command>
    {
          new Command ("find-controllers","Retrieves a list of controllers accessible on the local LAN", FindControllers),
          new Command ("get-controller","Retrieves the controller information from a controller", GetController),
          new Command ("set-IPv4","Sets a controller IPv4 address, netmask and gateway", SetIPv4),
          new Command ("get-listener","Retrieves a controller event listener address:port and auto-send interval", GetListener),
          new Command ("set-listener","Sets a controller event listener address:port and auto-send interval", SetListener),
          new Command ("get-time","Retrieves a controller system date and time", GetTime),
          new Command ("set-time","Sets a controller system date and time",SetTime),
          new Command ("get-door","Retrieves a controller door mode and delay settings", GetDoor),
          new Command ("set-door","Sets a controller door mode and delay",SetDoor),
          new Command ("set-door-passcodes","Sets the supervisor passcodes for a controller door",SetDoorPasscodes),
          new Command ("open-door","Unlocks a door controlled by a controller",OpenDoor),
          new Command ("get-status","Retrieves the current status of a controller",GetStatus),
          new Command ("get-cards","Retrieves the number of cards stored on a controller",GetCards),
          new Command ("get-card","Retrieves a card record from a controller",GetCard),
          new Command ("get-card-at-index","Retrieves the card record stored at the index from a controller",GetCardAtIndex),
          new Command ("put-card","Adds or updates a card record on controller",PutCard),
          new Command ("delete-card", "Deletes a card record from a controller", DeleteCard),
          new Command ("delete-all-cards", "Deletes all card records from a controller", DeleteAllCards),
          new Command ("get-event","Retrieves the event record stored at the index from a controller", GetEvent),
          new Command ("get-event-index","Retrieves the current event index from a controller", GetEventIndex),
          new Command ("set-event-index","Sets a controller event index", SetEventIndex),
          new Command ("record-special-events","Enables events for door open/close, button press, etc", RecordSpecialEvents),
          new Command ("get-time-profile","Retrieves an access time profile from a controller", GetTimeProfile),
          new Command ("set-time-profile","Adds or updates an access time profile on a controller", SetTimeProfile),
          new Command ("clear-time-profiles", "Clears all access time profiles stored on a controller", ClearTimeProfiles),
          new Command ("add-task", "Adds or updates a scheduled task stored on a controller", AddTask),
          new Command ("clear-tasklist", "Clears all scheduled tasks from the controller task list", ClearTaskList),
          new Command ("refresh-tasklist", "Schedules added tasks", RefreshTaskList),
          new Command ("set-pc-control", "Enables (or disables) remote access control management", SetPCControl),
          new Command ("set-interlock", "Sets the door interlock mode for a controller", SetInterlock),
          new Command ("activate-keypads", "Activates the access reader keypads attached to a controller", ActivateKeypads),
          new Command ("get-antipassback","Retrieves the controller anti-passback mode", GetAntiPassback),
          new Command ("set-antipassback","Sets the anti-passback mode for a controller",SetAntiPassback),
          new Command ("set-firstcard", "Sets the first card configuration for a door", SetFirstCard),
          new Command ("restore-default-parameters", "Restores the manufacturer defaults", RestoreDefaultParameters),
          new Command ("listen", "Listens for access controller events", Listen),
    };

    public static void FindControllers(uhppoted.Options options, string[] args)
    {
        var result = Uhppoted.FindControllers(options);

        if (result.IsOk)
        {
            var controllers = result.ResultValue;

            WriteLine("find-controllers: {0}", controllers.Length);
            foreach (var controller in controllers)
            {
                WriteLine("  controller {0}", controller.Controller);
                WriteLine("    address  {0}", controller.Address);
                WriteLine("    netmask  {0}", controller.Netmask);
                WriteLine("    gateway  {0}", controller.Gateway);
                WriteLine("    MAC      {0}", controller.MAC);
                WriteLine("    version  {0}", controller.Version);
                WriteLine("    date     {0}", YYYYMMDD(controller.Date));
                WriteLine();
            }
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetController(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetController(c, options)
                     : Uhppoted.GetController(controller, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-controller");
            WriteLine("  controller {0}", record.Controller);
            WriteLine("    address  {0}", record.Address);
            WriteLine("    netmask  {0}", record.Netmask);
            WriteLine("    gateway  {0}", record.Gateway);
            WriteLine("    MAC      {0}", record.MAC);
            WriteLine("    version  {0}", record.Version);
            WriteLine("    date     {0}", YYYYMMDD(record.Date));
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetIPv4(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var address = ArgParse.Parse(args, "--address", ADDRESS);
        var netmask = ArgParse.Parse(args, "--netmask", NETMASK);
        var gateway = ArgParse.Parse(args, "--gateway", GATEWAY);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetIPv4(c, address, netmask, gateway, options)
                     : Uhppoted.SetIPv4(controller, address, netmask, gateway, options);

        if (result.IsOk)
        {
            WriteLine("set-IPv4");
            WriteLine("  ok");
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetListener(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetListener(c, options)
                     : Uhppoted.GetListener(controller, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-listener");
            WriteLine("  controller {0}", controller);
            WriteLine("    endpoint {0}", record.Endpoint);
            WriteLine("    interval {0}s", record.Interval);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetListener(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var listener = ArgParse.Parse(args, "--listener", LISTENER);
        var interval = ArgParse.Parse(args, "--interval", INTERVAL);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetListener(c, listener, interval, options)
                     : Uhppoted.SetListener(controller, listener, interval, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetTime(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetTime(c, options)
                     : Uhppoted.GetTime(controller, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetTime(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var now = ArgParse.Parse(args, "--datetime", DateTime.Now);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetTime(c, now, options)
                     : Uhppoted.SetTime(controller, now, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetDoor(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetDoor(c, door, options)
                     : Uhppoted.GetDoor(controller, door, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-door");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("        mode {0}", translate(record.Mode));
            WriteLine("       delay {0}s", record.Delay);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetDoor(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var mode = ArgParse.Parse(args, "--mode", MODE);
        var delay = ArgParse.Parse(args, "--delay", DELAY);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetDoor(c, door, mode, delay, options)
                     : Uhppoted.SetDoor(controller, door, mode, delay, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("set-door");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("        mode {0}", translate(record.Mode));
            WriteLine("       delay {0}s", record.Delay);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetDoorPasscodes(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var mode = ArgParse.Parse(args, "--mode", MODE);
        var passcodes = ArgParse.Parse(args, "--passcodes", new uint[] { });

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetDoorPasscodes(c, door, passcodes, options)
                     : Uhppoted.SetDoorPasscodes(controller, door, passcodes, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-door-passcodes");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void OpenDoor(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.OpenDoor(c, door, options)
                     : Uhppoted.OpenDoor(controller, door, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("open-door");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetStatus(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetStatus(c, options)
                     : Uhppoted.GetStatus(controller, options);

        if (result.IsOk)
        {
            var status = result.ResultValue.Item1;
            var evt = result.ResultValue.Item2;

            WriteLine("get-status");
            WriteLine("         controller {0}", controller);
            WriteLine("             door 1 {1},{0}", status.Door1Open ? "open" : "closed", translate(status.Relays.Door1));
            WriteLine("             door 2 {1},{0}", status.Door2Open ? "open" : "closed", translate(status.Relays.Door2));
            WriteLine("             door 3 {1},{0}", status.Door3Open ? "open" : "closed", translate(status.Relays.Door3));
            WriteLine("             door 4 {1},{0}", status.Door4Open ? "open" : "closed", translate(status.Relays.Door4));
            WriteLine("   button 1 pressed {0}", status.Button1Pressed);
            WriteLine("   button 2 pressed {0}", status.Button2Pressed);
            WriteLine("   button 3 pressed {0}", status.Button3Pressed);
            WriteLine("   button 4 pressed {0}", status.Button4Pressed);
            WriteLine("       system error {0}", status.SystemError);
            WriteLine("   system date/time {0}", YYYYMMDDHHmmss(status.SystemDateTime));
            WriteLine("       special info {0}", status.SpecialInfo);
            WriteLine("        lock forced {0}", translate(status.Inputs.LockForced));
            WriteLine("         fire alarm {0}", translate(status.Inputs.FireAlarm));
            WriteLine();

            if (evt.HasValue)
            {
                WriteLine("    event index     {0}", evt.Value.Index);
                WriteLine("          event     {0} ({1})", evt.Value.Event.Text, evt.Value.Event.Code);
                WriteLine("          granted   {0}", evt.Value.AccessGranted);
                WriteLine("          door      {0}", evt.Value.Door);
                WriteLine("          direction {0}", translate(evt.Value.Direction));
                WriteLine("          card      {0}", evt.Value.Card);
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(evt.Value.Timestamp));
                WriteLine("          reason    {0} ({1})", evt.Value.Reason.Text, evt.Value.Reason.Code);
                WriteLine();
            }
            else
            {
                WriteLine("    (no event)");
                WriteLine();
            }
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetCards(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetCards(c, options)
                     : Uhppoted.GetCards(controller, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetCard(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetCard(c, card, options)
                     : Uhppoted.GetCard(controller, card, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-card");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", record.Card);
            WriteLine("  start date {0}", (YYYYMMDD(record.StartDate)));
            WriteLine("    end date {0}", (YYYYMMDD(record.EndDate)));
            WriteLine("      door 1 {0}", record.Door1);
            WriteLine("      door 2 {0}", record.Door2);
            WriteLine("      door 3 {0}", record.Door3);
            WriteLine("      door 4 {0}", record.Door4);
            WriteLine("         PIN {0}", record.PIN);
            WriteLine();
        }
        else if (result.IsError && result.ErrorValue == CardNotFound)
        {
            throw new Exception("card not found");
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetCardAtIndex(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = ArgParse.Parse(args, "--index", CARD_INDEX);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetCardAtIndex(c, index, options)
                     : Uhppoted.GetCardAtIndex(controller, index, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-card-at-index");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", record.Card);
            WriteLine("  start date {0}", (YYYYMMDD(record.StartDate)));
            WriteLine("    end date {0}", (YYYYMMDD(record.EndDate)));
            WriteLine("      door 1 {0}", record.Door1);
            WriteLine("      door 2 {0}", record.Door2);
            WriteLine("      door 3 {0}", record.Door3);
            WriteLine("      door 4 {0}", record.Door4);
            WriteLine("         PIN {0}", record.PIN);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("card not found");
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void PutCard(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var cardNumber = ArgParse.Parse(args, "--card", CARD);
        var startDate = ArgParse.Parse(args, "--start-date", START_DATE);
        var endDate = ArgParse.Parse(args, "--end-date", END_DATE);
        var permissions = ArgParse.Parse(args, "--permissions", new Dictionary<int, byte>());
        var PIN = ArgParse.Parse(args, "--PIN", 0u);

        byte u8;
        byte door1 = permissions.TryGetValue(1, out u8) ? u8 : (byte)0;
        byte door2 = permissions.TryGetValue(2, out u8) ? u8 : (byte)0;
        byte door3 = permissions.TryGetValue(3, out u8) ? u8 : (byte)0;
        byte door4 = permissions.TryGetValue(4, out u8) ? u8 : (byte)0;

        var card = new uhppoted.CardBuilder(cardNumber)
                               .WithStartDate(startDate)
                               .WithEndDate(endDate)
                               .WithDoor1(door1)
                               .WithDoor2(door2)
                               .WithDoor3(door3)
                               .WithDoor4(door4)
                               .WithPIN(PIN)
                               .Build();

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.PutCard(c, card, options)
                     : Uhppoted.PutCard(controller, card, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("put-card");
            WriteLine("  controller {0}", controller);
            WriteLine("        card {0}", card.Card);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void DeleteCard(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.DeleteCard(c, card, options)
                     : Uhppoted.DeleteCard(controller, card, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void DeleteAllCards(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.DeleteAllCards(c, options)
                     : Uhppoted.DeleteAllCards(controller, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetEvent(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = ArgParse.Parse(args, "--index", EVENT_INDEX);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetEvent(c, index, options)
                     : Uhppoted.GetEvent(controller, index, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-event");
            WriteLine("  controller {0}", controller);
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.Timestamp)));
            WriteLine("       index {0}", record.Index);
            WriteLine("       event {0} (1)", record.Event.Text, record.Event.Code);
            WriteLine("     granted {0}", record.AccessGranted);
            WriteLine("        door {0}", record.Door);
            WriteLine("   direction {0}", translate(record.Direction));
            WriteLine("        card {0}", record.Card);
            WriteLine("      reason {0} ({1})", record.Reason.Text, record.Reason.Code);
            WriteLine();
        }
        else if (result.IsError && result.ErrorValue == EventNotFound)
        {
            throw new Exception("event not found");
        }
        else if (result.IsError && result.ErrorValue == EventOverwritten)
        {
            throw new Exception("event overwritten");
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetEventIndex(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetEventIndex(c, options)
                     : Uhppoted.GetEventIndex(controller, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetEventIndex(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var index = ArgParse.Parse(args, "--index", EVENT_INDEX);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetEventIndex(c, index, options)
                     : Uhppoted.SetEventIndex(controller, index, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void RecordSpecialEvents(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var enable = ArgParse.Parse(args, "--enable", ENABLE);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.RecordSpecialEvents(c, enable, options)
                     : Uhppoted.RecordSpecialEvents(controller, enable, options);

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
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetTimeProfile(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var profile = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetTimeProfile(c, profile, options)
                     : Uhppoted.GetTimeProfile(controller, profile, options);

        if (result.IsOk)
        {
            var record = result.ResultValue;

            WriteLine("get-time-profile");
            WriteLine("          controller {0}", controller);
            WriteLine("             profile {0}", record.Profile);
            WriteLine("          start date {0}", YYYYMMDD(record.StartDate));
            WriteLine("            end date {0}", YYYYMMDD(record.EndDate));
            WriteLine("              monday {0}", record.Monday);
            WriteLine("             tuesday {0}", record.Tuesday);
            WriteLine("           wednesday {0}", record.Wednesday);
            WriteLine("            thursday {0}", record.Thursday);
            WriteLine("              friday {0}", record.Friday);
            WriteLine("            saturday {0}", record.Saturday);
            WriteLine("              sunday {0}", record.Sunday);
            WriteLine("   segment 1 - start {0}", HHmm(record.Segment1Start));
            WriteLine("                 end {0}", HHmm(record.Segment1End));
            WriteLine("   segment 2 - start {0}", HHmm(record.Segment2Start));
            WriteLine("                 end {0}", HHmm(record.Segment2End));
            WriteLine("   segment 3 - start {0}", HHmm(record.Segment3Start));
            WriteLine("                 end {0}", HHmm(record.Segment3End));
            WriteLine("      linked profile {0}", record.LinkedProfile);
            WriteLine();
        }
        else if (result.IsError && result.ErrorValue == TimeProfileNotFound)
        {
            throw new Exception("time profile does not exist");
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetTimeProfile(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var profile_id = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID);
        var linked = ArgParse.Parse(args, "--linked", (byte)0);
        var start_date = ArgParse.Parse(args, "--start_date", START_DATE);
        var end_date = ArgParse.Parse(args, "--end_date", END_DATE);
        var weekdays = ArgParse.Parse(args, "--weekdays", new string[] { });
        var segments = ArgParse.Parse(args, "--segments", new TimeOnly[6]);

        bool monday = Array.Exists(weekdays, v => v == "monday");
        bool tuesday = Array.Exists(weekdays, v => v == "tuesday");
        bool wednesday = Array.Exists(weekdays, v => v == "wednesday");
        bool thursday = Array.Exists(weekdays, v => v == "thursday");
        bool friday = Array.Exists(weekdays, v => v == "friday");
        bool saturday = Array.Exists(weekdays, v => v == "saturday");
        bool sunday = Array.Exists(weekdays, v => v == "sunday");

        var profile = new uhppoted.TimeProfileBuilder(profile_id)
                                  .WithStartDate(start_date)
                                  .WithEndDate(end_date)
                                  .WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday)
                                  .WithSegment1(segments[0], segments[1])
                                  .WithSegment2(segments[2], segments[3])
                                  .WithSegment3(segments[4], segments[5])
                                  .WithLinkedProfile(linked)
                                  .Build();

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetTimeProfile(c, profile, options)
                     : Uhppoted.SetTimeProfile(controller, profile, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-time-profile");
            WriteLine("  controller {0}", controller);
            WriteLine("     profile {0}", profile.Profile);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void ClearTimeProfiles(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.ClearTimeProfiles(c, options)
                     : Uhppoted.ClearTimeProfiles(controller, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("clear-time-profiles");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void AddTask(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var task_code = ArgParse.Parse(args, "--task", TASK_CODE);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var start_date = ArgParse.Parse(args, "--start_date", START_DATE);
        var end_date = ArgParse.Parse(args, "--end_date", END_DATE);
        var start_time = ArgParse.Parse(args, "--start-time", TimeOnly.Parse("00:00"));
        var weekdays = ArgParse.Parse(args, "--weekdays", new string[] { });
        var more_cards = ArgParse.Parse(args, "--more-cards", (byte)0);

        bool monday = Array.Exists(weekdays, v => v == "monday");
        bool tuesday = Array.Exists(weekdays, v => v == "tuesday");
        bool wednesday = Array.Exists(weekdays, v => v == "wednesday");
        bool thursday = Array.Exists(weekdays, v => v == "thursday");
        bool friday = Array.Exists(weekdays, v => v == "friday");
        bool saturday = Array.Exists(weekdays, v => v == "saturday");
        bool sunday = Array.Exists(weekdays, v => v == "sunday");

        var task = new uhppoted.TaskBuilder(task_code, door)
                                  .WithStartDate(start_date)
                                  .WithEndDate(end_date)
                                  .WithStartTime(start_time)
                                  .WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday)
                                  .WithMoreCards(more_cards)
                                  .Build();


        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.AddTask(c, task, options)
                     : Uhppoted.AddTask(controller, task, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("add-task");
            WriteLine("  controller {0}", controller);
            WriteLine("        task {0}", translate(task.Task));
            WriteLine("        door {0}", task.Door);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void ClearTaskList(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.ClearTaskList(c, options)
                     : Uhppoted.ClearTaskList(controller, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("clear-tasklist");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void RefreshTaskList(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.RefreshTaskList(c, options)
                     : Uhppoted.RefreshTaskList(controller, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("refresh-tasklist");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetPCControl(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var enable = ArgParse.Parse(args, "--enable", false);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetPCControl(c, enable, options)
                     : Uhppoted.SetPCControl(controller, enable, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-pc-control");
            WriteLine("  controller {0}", controller);
            WriteLine("      enable {0}", enable);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetInterlock(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var interlock = ArgParse.Parse(args, "--interlock", Interlock.None);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetInterlock(c, interlock, options)
                     : Uhppoted.SetInterlock(controller, interlock, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-interlock");
            WriteLine("  controller {0}", controller);
            WriteLine("   interlock {0}", translate(interlock));
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void ActivateKeypads(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var keypads = ArgParse.Parse(args, "--keypads", new byte[] { });

        var reader1 = keypads.Contains((byte)1);
        var reader2 = keypads.Contains((byte)2);
        var reader3 = keypads.Contains((byte)3);
        var reader4 = keypads.Contains((byte)4);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.ActivateKeypads(c, reader1, reader2, reader3, reader4, options)
                     : Uhppoted.ActivateKeypads(controller, reader1, reader2, reader3, reader4, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("activate-keypads");
            WriteLine("  controller {0}", controller);
            WriteLine("    reader 1 {0}", reader1);
            WriteLine("    reader 2 {0}", reader2);
            WriteLine("    reader 3 {0}", reader3);
            WriteLine("    reader 4 {0}", reader4);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void GetAntiPassback(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.GetAntiPassback(c, options)
                     : Uhppoted.GetAntiPassback(controller, options);

        if (result.IsOk)
        {
            var antipassback = result.ResultValue;

            WriteLine("get-antipassback");
            WriteLine("  controller      {0}", controller);
            WriteLine("    anti-passback {0}", translate(antipassback));
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetAntiPassback(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var antipassback = ArgParse.Parse(args, "--antipassback", AntiPassback.Disabled);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetAntiPassback(c, antipassback, options)
                     : Uhppoted.SetAntiPassback(controller, antipassback, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-antipassback");
            WriteLine("  controller      {0}", controller);
            WriteLine("    anti-passback {0}", translate(antipassback));
            WriteLine("               ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void SetFirstCard(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var firstcard = ArgParse.Parse(args, "--firstcard", new FirstCard());

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.SetFirstCard(c, door, firstcard, options)
                     : Uhppoted.SetFirstCard(controller, door, firstcard, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-firstcard");
            WriteLine("  controller  {0}", controller);
            WriteLine("        door  {0}", door);
            WriteLine("   firstcard  {0}", firstcard);
            WriteLine("          ok  {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void RestoreDefaultParameters(uhppoted.Options options, string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);

        var result = CONTROLLERS.TryGetValue(controller, out var c)
                     ? Uhppoted.RestoreDefaultParameters(c, options)
                     : Uhppoted.RestoreDefaultParameters(controller, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("restore-default-parameters");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(translate(result.ErrorValue));
        }
    }

    public static void Listen(uhppoted.Options options, string[] args)
    {
        var cancel = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            cancel.Cancel();
        };

        void onEvent(uhppoted.ListenerEvent e)
        {
            var controller = e.Controller;
            var status = e.Status;
            var evt = e.Event;

            WriteLine("-- EVENT");
            WriteLine($"         controller {controller}");
            WriteLine("");

            WriteLine("              door 1 {1},{0}", status.Door1Open ? "open" : "closed", translate(status.Relays.Door1));
            WriteLine("              door 2 {1},{0}", status.Door2Open ? "open" : "closed", translate(status.Relays.Door2));
            WriteLine("              door 3 {1},{0}", status.Door3Open ? "open" : "closed", translate(status.Relays.Door3));
            WriteLine("              door 4 {1},{0}", status.Door4Open ? "open" : "closed", translate(status.Relays.Door4));
            WriteLine($"   button 1 pressed {status.Button1Pressed}");
            WriteLine($"   button 2 pressed {status.Button2Pressed}");
            WriteLine($"   button 3 pressed {status.Button3Pressed}");
            WriteLine($"   button 4 pressed {status.Button4Pressed}");
            WriteLine($"       system error {status.SystemError}");
            WriteLine($"   system date/time {YYYYMMDDHHmmss(status.SystemDateTime)}");
            WriteLine($"       special info {status.SpecialInfo}");
            WriteLine($"        lock forced {translate(status.Inputs.LockForced)}");
            WriteLine($"         fire alarm {translate(status.Inputs.FireAlarm)}");
            WriteLine("");

            if (evt.HasValue)
            {
                var v = evt.Value;

                WriteLine($"    event timestamp {YYYYMMDDHHmmss(v.Timestamp)}");
                WriteLine($"              index {v.Index}");
                WriteLine($"              event {v.Event.Text} ({v.Event.Code})");
                WriteLine($"            granted {v.AccessGranted}");
                WriteLine($"               door {v.Door}");
                WriteLine($"          direction {translate(v.Direction)}");
                WriteLine($"               card {v.Card}");
                WriteLine($"             reason {v.Reason.Text} ({v.Reason.Code})");
                WriteLine("");
            }
            else
            {
                WriteLine("   (no event)");
                WriteLine("");
            }
        }

        void onError(object err)
        {
            WriteLine("** ERROR {0}", translate(err));
        }

        var onevent = new uhppoted.OnEvent(onEvent);
        var onerror = new uhppoted.OnError(onError);

        Uhppoted.Listen(onevent, onerror, cancel.Token, options);
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

    public static string HHmm(TimeOnly? time)
    {
        return time.HasValue
            ? time.Value.ToString("HH:mm")
            : "---";
    }

    public static string translate<T>(T val)
    {
        return Uhppoted.Translate(val);
    }
}
