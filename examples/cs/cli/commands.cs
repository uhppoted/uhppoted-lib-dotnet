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

public readonly struct Weekdays
{
    public readonly bool monday;
    public readonly bool tuesday;
    public readonly bool wednesday;
    public readonly bool thursday;
    public readonly bool friday;
    public readonly bool saturday;
    public readonly bool sunday;

    public Weekdays(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
    {
        this.monday = monday;
        this.tuesday = tuesday;
        this.wednesday = wednesday;
        this.thursday = thursday;
        this.friday = friday;
        this.saturday = saturday;
        this.sunday = sunday;
    }
}

public readonly struct TimeSegment
{
    public readonly TimeOnly start;
    public readonly TimeOnly end;

    public TimeSegment(TimeOnly start, TimeOnly end)
    {
        this.start = start;
        this.end = end;
    }
}

class Commands
{
    static readonly uhppoted.Options OPTIONS = new uhppoted.OptionsBuilder()
                                                           .WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000"))
                                                           .WithProtocol("udp")
                                                           .WithTimeout(1000)
                                                           .WithDebug(true)
                                                           .Build();

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
    const byte TASK_ID = 0;

    static readonly IPAddress ADDRESS = IPAddress.Parse("192.168.1.10");
    static readonly IPAddress NETMASK = IPAddress.Parse("255.255.255.0");
    static readonly IPAddress GATEWAY = IPAddress.Parse("192.168.1.1");
    static readonly IPEndPoint LISTENER = IPEndPoint.Parse("192.168.1.250:60001");
    static readonly DateOnly START_DATE = new DateOnly(2024, 1, 1);
    static readonly DateOnly END_DATE = new DateOnly(2024, 12, 31);

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
          new Command ("restore-default-parameters", "Restores the manufacturer defaults", RestoreDefaultParameters),
          new Command ("listen", "Listens for access controller events", Listen),
    };

    public static void FindControllers(string[] args)
    {
        var result = Uhppoted.FindControllers(OPTIONS);

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
        var options = OPTIONS;
        var result = Uhppoted.GetController(controller, options);

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
        var options = OPTIONS;

        var result = Uhppoted.SetIPv4(controller, address, netmask, gateway, options);

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
        var options = OPTIONS;
        var result = Uhppoted.GetListener(controller, options);

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
        var result = Uhppoted.SetListener(controller, listener, interval, OPTIONS);

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
        var result = Uhppoted.GetTime(controller, OPTIONS);

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

        var result = Uhppoted.SetTime(controller, now, OPTIONS);

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
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var result = Uhppoted.GetDoor(controller, door, OPTIONS);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-door");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("        mode {0}", record.mode);
            WriteLine("       delay {0}s", record.delay);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("door does not exist");
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetDoor(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var mode = ArgParse.Parse(args, "--mode", MODE);
        var delay = ArgParse.Parse(args, "--delay", DELAY);

        var result = Uhppoted.SetDoor(controller, door, mode, delay, OPTIONS);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("set-door");
            WriteLine("  controller {0}", controller);
            WriteLine("        door {0}", door);
            WriteLine("        mode {0}", record.mode);
            WriteLine("       delay {0}s", record.delay);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("door does not updated");
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetDoorPasscodes(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var mode = ArgParse.Parse(args, "--mode", MODE);
        var passcodes = ArgParse.Parse(args, "--passcodes", new uint[] { });
        var result = Uhppoted.SetDoorPasscodes(controller, door, passcodes, OPTIONS);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void OpenDoor(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var result = Uhppoted.OpenDoor(controller, door, OPTIONS);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("open-door");
            WriteLine("  controller {0}", controller);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetStatus(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var result = Uhppoted.GetStatus(controller, OPTIONS);

        if (result.IsOk)
        {
            var status = result.ResultValue.Item1;
            var evt = result.ResultValue.Item2;

            WriteLine("get-status");
            WriteLine("         controller {0}", controller);
            WriteLine("        door 1 open {0}", status.Door1Open);
            WriteLine("        door 2 open {0}", status.Door2Open);
            WriteLine("        door 3 open {0}", status.Door3Open);
            WriteLine("        door 4 open {0}", status.Door4Open);
            WriteLine("   button 1 pressed {0}", status.Button1Pressed);
            WriteLine("   button 2 pressed {0}", status.Button2Pressed);
            WriteLine("   button 3 pressed {0}", status.Button3Pressed);
            WriteLine("   button 4 pressed {0}", status.Button4Pressed);
            WriteLine("       system error {0}", status.SystemError);
            WriteLine("   system date/time {0}", YYYYMMDDHHmmss(status.SystemDateTime));
            WriteLine("       sequence no. {0}", status.SequenceNumber);
            WriteLine("       special info {0}", status.SpecialInfo);
            WriteLine("            relay 1 {0}", status.Relay1);
            WriteLine("            relay 2 {0}", status.Relay2);
            WriteLine("            relay 3 {0}", status.Relay3);
            WriteLine("            relay 4 {0}", status.Relay4);
            WriteLine("            input 1 {0}", status.Input1);
            WriteLine("            input 2 {0}", status.Input2);
            WriteLine("            input 3 {0}", status.Input3);
            WriteLine("            input 4 {0}", status.Input4);
            WriteLine();

            if (evt.HasValue)
            {
                WriteLine("    event index     {0}", evt.Value.Index);
                WriteLine("          event     {0}", evt.Value.Event);
                WriteLine("          granted   {0}", evt.Value.AccessGranted);
                WriteLine("          door      {0}", evt.Value.Door);
                WriteLine("          direction {0}", evt.Value.Direction);
                WriteLine("          card      {0}", evt.Value.Card);
                WriteLine("          timestamp {0}", YYYYMMDDHHmmss(evt.Value.Timestamp));
                WriteLine("          reason    {0}", evt.Value.Reason);
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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void GetCards(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);
        var options = OPTIONS;
        var result = Uhppoted.GetCards(controller, options);

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
        var options = OPTIONS;
        var result = Uhppoted.GetCard(controller, card, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

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
        var index = ArgParse.Parse(args, "--index", CARD_INDEX);
        var options = OPTIONS;
        var result = Uhppoted.GetCardAtIndex(controller, index, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void PutCard(string[] args)
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

        var options = OPTIONS;
        var result = Uhppoted.PutCard(controller, card, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void DeleteCard(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var card = ArgParse.Parse(args, "--card", CARD);

        var options = OPTIONS;
        var result = Uhppoted.DeleteCard(controller, card, options);

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
        var options = OPTIONS;
        var result = Uhppoted.DeleteAllCards(controller, options);

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
        var options = OPTIONS;
        var result = Uhppoted.GetEvent(controller, index, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-event");
            WriteLine("  controller {0}", controller);
            WriteLine("   timestamp {0}", (YYYYMMDDHHmmss(record.Timestamp)));
            WriteLine("       index {0}", record.Index);
            WriteLine("       event {0}", record.Event);
            WriteLine("     granted {0}", record.AccessGranted);
            WriteLine("        door {0}", record.Door);
            WriteLine("   direction {0}", record.Direction);
            WriteLine("        card {0}", record.Card);
            WriteLine("      reason {0}", record.Reason);
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
        var options = OPTIONS;
        var result = Uhppoted.GetEventIndex(controller, options);

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
        var options = OPTIONS;
        var result = Uhppoted.SetEventIndex(controller, index, options);

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
        var options = OPTIONS;
        var result = Uhppoted.RecordSpecialEvents(controller, enable, options);

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

    public static void GetTimeProfile(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var profile = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID);
        var options = OPTIONS;
        var result = Uhppoted.GetTimeProfile(controller, profile, options);

        if (result.IsOk && result.ResultValue.HasValue)
        {
            var record = result.ResultValue.Value;

            WriteLine("get-time-profile");
            WriteLine("          controller {0}", controller);
            WriteLine("             profile {0}", record.profile);
            WriteLine("          start date {0}", YYYYMMDD(record.start_date));
            WriteLine("            end date {0}", YYYYMMDD(record.end_date));
            WriteLine("              monday {0}", record.monday);
            WriteLine("             tuesday {0}", record.tuesday);
            WriteLine("           wednesday {0}", record.wednesday);
            WriteLine("            thursday {0}", record.thursday);
            WriteLine("              friday {0}", record.friday);
            WriteLine("            saturday {0}", record.saturday);
            WriteLine("              sunday {0}", record.sunday);
            WriteLine("   segment 1 - start {0}", HHmm(record.segment1_start));
            WriteLine("                 end {0}", HHmm(record.segment1_end));
            WriteLine("   segment 2 - start {0}", HHmm(record.segment2_start));
            WriteLine("                 end {0}", HHmm(record.segment2_end));
            WriteLine("   segment 3 - start {0}", HHmm(record.segment3_start));
            WriteLine("                 end {0}", HHmm(record.segment3_end));
            WriteLine("      linked profile {0}", record.linked_profile);
            WriteLine();
        }
        else if (result.IsOk)
        {
            throw new Exception("time profile does not exist");
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetTimeProfile(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var profile_id = ArgParse.Parse(args, "--profile", TIME_PROFILE_ID);
        var linked = ArgParse.Parse(args, "--linked", (byte)0);
        var start_date = ArgParse.Parse(args, "--start_date", START_DATE);
        var end_date = ArgParse.Parse(args, "--end_date", END_DATE);
        var weekdays = ArgParse.Parse(args, "--weekdays", new Weekdays(true, true, false, false, true, false, false));
        var segments = ArgParse.Parse(args, "--segments", new TimeSegment[] {
            new TimeSegment(TimeOnly.Parse("08:30"), TimeOnly.Parse("09:45")),
            new TimeSegment(TimeOnly.Parse("12:15"), TimeOnly.Parse("13:15")),
            new TimeSegment(TimeOnly.Parse("14:00"), TimeOnly.Parse("18:00")),
            });

        bool monday = weekdays.monday;
        bool tuesday = weekdays.tuesday;
        bool wednesday = weekdays.wednesday;
        bool thursday = weekdays.thursday;
        bool friday = weekdays.friday;
        bool saturday = weekdays.saturday;
        bool sunday = weekdays.sunday;

        var profile = new uhppoted.TimeProfileBuilder(profile_id)
                                  .WithStartDate(start_date)
                                  .WithEndDate(end_date)
                                  .WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday)
                                  .WithSegment1(segments[0].start, segments[0].end)
                                  .WithSegment2(segments[1].start, segments[1].end)
                                  .WithSegment3(segments[2].start, segments[2].end)
                                  .WithLinkedProfile(linked)
                                  .Build();

        var options = OPTIONS;
        var result = Uhppoted.SetTimeProfile(controller, profile, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-time-profile");
            WriteLine("  controller {0}", controller);
            WriteLine("     profile {0}", profile.profile);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void ClearTimeProfiles(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var options = OPTIONS;
        var result = Uhppoted.ClearTimeProfiles(controller, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void AddTask(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var task_id = ArgParse.Parse(args, "--task", TASK_ID);
        var door = ArgParse.Parse(args, "--door", DOOR);
        var start_date = ArgParse.Parse(args, "--start_date", START_DATE);
        var end_date = ArgParse.Parse(args, "--end_date", END_DATE);
        var start_time = ArgParse.Parse(args, "--start-time", TimeOnly.Parse("00:00"));
        var weekdays = ArgParse.Parse(args, "--weekdays", new Weekdays(true, true, false, false, true, false, false));
        var more_cards = ArgParse.Parse(args, "--more-cards", (byte)0);

        bool monday = weekdays.monday;
        bool tuesday = weekdays.tuesday;
        bool wednesday = weekdays.wednesday;
        bool thursday = weekdays.thursday;
        bool friday = weekdays.friday;
        bool saturday = weekdays.saturday;
        bool sunday = weekdays.sunday;

        var task = new uhppoted.TaskBuilder(task_id, door)
                                  .WithStartDate(start_date)
                                  .WithEndDate(end_date)
                                  .WithStartTime(start_time)
                                  .WithWeekdays(monday, tuesday, wednesday, thursday, friday, saturday, sunday)
                                  .WithMoreCards(more_cards)
                                  .Build();

        var options = OPTIONS;
        var result = Uhppoted.AddTask(controller, task, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("add-task");
            WriteLine("  controller {0}", controller);
            WriteLine("        task {0}", task.task);
            WriteLine("        door {0}", task.door);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void ClearTaskList(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var options = OPTIONS;
        var result = Uhppoted.ClearTaskList(controller, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void RefreshTaskList(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var options = OPTIONS;
        var result = Uhppoted.RefreshTaskList(controller, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetPCControl(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var enable = ArgParse.Parse(args, "--enable", false);
        var options = OPTIONS;
        var result = Uhppoted.SetPCControl(controller, enable, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void SetInterlock(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var interlock = ArgParse.Parse(args, "--interlock", Interlock.None);
        var options = OPTIONS;
        var result = Uhppoted.SetInterlock(controller, interlock, options);

        if (result.IsOk)
        {
            var ok = result.ResultValue;

            WriteLine("set-interlock");
            WriteLine("  controller {0}", controller);
            WriteLine("   interlock {0}", interlock);
            WriteLine("          ok {0}", ok);
            WriteLine();
        }
        else if (result.IsError)
        {
            throw new Exception(result.ErrorValue);
        }
    }

    public static void ActivateKeypads(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var keypads = ArgParse.Parse(args, "--keypads", new List<byte> { });
        var options = OPTIONS;

        var reader1 = keypads.Contains((byte)1);
        var reader2 = keypads.Contains((byte)2);
        var reader3 = keypads.Contains((byte)3);
        var reader4 = keypads.Contains((byte)4);

        var result = Uhppoted.ActivateKeypads(controller, reader1, reader2, reader3, reader4, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void RestoreDefaultParameters(string[] args)
    {
        var controller = ArgParse.Parse(args, "--controller", CONTROLLER);
        var options = OPTIONS;
        var result = Uhppoted.RestoreDefaultParameters(controller, options);

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
            throw new Exception(result.ErrorValue);
        }
    }

    public static void Listen(string[] args)
    {
        var options = OPTIONS;
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

            WriteLine($"        door 1 open {status.Door1Open}");
            WriteLine($"        door 2 open {status.Door2Open}");
            WriteLine($"        door 3 open {status.Door3Open}");
            WriteLine($"        door 4 open {status.Door4Open}");
            WriteLine($"   button 1 pressed {status.Button1Pressed}");
            WriteLine($"   button 2 pressed {status.Button2Pressed}");
            WriteLine($"   button 3 pressed {status.Button3Pressed}");
            WriteLine($"   button 4 pressed {status.Button4Pressed}");
            WriteLine($"       system error {status.SystemError}");
            WriteLine($"   system date/time {YYYYMMDDHHmmss(status.SystemDateTime)}");
            WriteLine($"       sequence no. {status.SequenceNumber}");
            WriteLine($"       special info {status.SpecialInfo}");
            WriteLine($"            relay 1 {status.Relay1}");
            WriteLine($"            relay 2 {status.Relay2}");
            WriteLine($"            relay 3 {status.Relay3}");
            WriteLine($"            relay 4 {status.Relay4}");
            WriteLine($"            input 1 {status.Input1}");
            WriteLine($"            input 2 {status.Input2}");
            WriteLine($"            input 3 {status.Input3}");
            WriteLine($"            input 4 {status.Input4}");
            WriteLine("");

            if (evt.HasValue)
            {
                var v = evt.Value;

                WriteLine($"    event timestamp {YYYYMMDDHHmmss(v.Timestamp)}");
                WriteLine($"              index {v.Index}");
                WriteLine($"              event {v.Event}");
                WriteLine($"            granted {v.AccessGranted}");
                WriteLine($"               door {v.Door}");
                WriteLine($"          direction {v.Direction}");
                WriteLine($"               card {v.Card}");
                WriteLine($"             reason {v.Reason}");
                WriteLine("");
            }
            else
            {
                WriteLine("   (no event)");
                WriteLine("");
            }
        }

        void onError(string err)
        {
            WriteLine("** ERROR {0}", err);
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
}
