namespace FSharp.Tests

open System
open System.Net
open System.Net.NetworkInformation
open System.Threading
open NUnit.Framework

open uhppoted
open stub

type TestCase =
    | Id of uint32
    | Controller of C


[<TestFixture>]
type TestFindAPI() =
    [<DefaultValue>]
    val mutable emulator: Emulator

    let OPTIONS: Options =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 59999)
          listen = IPEndPoint(IPAddress.Any, 60001)
          timeout = 500
          debug = false }

    [<OneTimeSetUp>]
    member this.Initialise() =
        this.emulator <- Stub.initialise "broadcast" TestContext.Error

    [<OneTimeTearDown>]
    member this.Terminate() =
        Stub.terminate this.emulator TestContext.Error

    [<SetUp>]
    member this.Setup() = ()

    [<TearDown>]
    member this.TearDown() = ()

    [<Test>]
    member this.TestFindControllers() =
        let expected: Controller array =
            [| { Controller = 405419896u
                 Address = IPAddress.Parse("192.168.1.100")
                 Netmask = IPAddress.Parse("255.255.255.0")
                 Gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x00uy; 0x12uy; 0x23uy; 0x34uy; 0x45uy; 0x56uy |])
                 Version = "v8.92"
                 Date = Nullable(DateOnly.ParseExact("2018-11-05", "yyyy-MM-dd")) }

               { Controller = 303986753u
                 Address = IPAddress.Parse("192.168.1.100")
                 Netmask = IPAddress.Parse("255.255.255.0")
                 Gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x52uy; 0xfduy; 0xfcuy; 0x07uy; 0x21uy; 0x82uy |])
                 Version = "v8.92"
                 Date = Nullable(DateOnly.ParseExact("2019-08-15", "yyyy-MM-dd")) }

               { Controller = 201020304u
                 Address = IPAddress.Parse("192.168.1.101")
                 Netmask = IPAddress.Parse("255.255.255.0")
                 Gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x52uy; 0xfduy; 0xfcuy; 0x07uy; 0x21uy; 0x82uy |])
                 Version = "v6.62"
                 Date = Nullable(DateOnly.ParseExact("2020-01-01", "yyyy-MM-dd")) } |]

        match Uhppoted.FindControllers(OPTIONS) with
        | Ok controllers -> Assert.That(controllers, Is.EqualTo(expected))
        | Error err -> Assert.Fail($"{err}")


[<TestFixture("uint32")>]
[<TestFixture("controller")>]
[<TestFixture("controller+endpoint")>]
[<TestFixture("controller+endpoint+udp")>]
[<TestFixture("controller+endpoint+tcp")>]
type TestAPI(tt: string) =
    [<DefaultValue>]
    val mutable emulator: Emulator

    let controllers =
        Map.ofList
            [ ("uint32", Id 405419896u)

              ("controller",
               Controller
                   { controller = 405419896u
                     endpoint = None
                     protocol = None })

              ("controller+endpoint",
               Controller
                   { controller = 405419896u
                     endpoint = Some(IPEndPoint.Parse("127.0.0.1:59998"))
                     protocol = None })

              ("controller+endpoint+udp",
               Controller
                   { controller = 405419896u
                     endpoint = Some(IPEndPoint.Parse("127.0.0.1:59998"))
                     protocol = Some("udp") })

              ("controller+endpoint+tcp",
               Controller
                   { controller = 405419896u
                     endpoint = Some(IPEndPoint.Parse("127.0.0.1:59997"))
                     protocol = Some("tcp") }) ]

    let controller = controllers[tt]

    let CONTROLLER_NO_EVENT = 405419897u
    let DOOR = 4uy
    let DOOR_NOT_FOUND = 5uy
    let MODE = DoorMode.NormallyClosed
    let DELAY = 17uy
    let CARD = 10058400u
    let MISSING_CARD = 10058399u
    let CARD_INDEX = 135u
    let CARD_INDEX_NOT_FOUND = 136u
    let CARD_INDEX_DELETED = 137u
    let EVENT_INDEX = 13579u
    let EVENT_INDEX_NOT_FOUND = 24680u
    let EVENT_INDEX_OVERWRITTEN = 98765u
    let TIME_PROFILE_ID = 37uy
    let TIME_PROFILE_ID_NOT_FOUND = 80uy

    let OPTIONS: Options =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 59999)
          listen = IPEndPoint(IPAddress.Any, 60001)
          timeout = 500
          debug = false }

    [<OneTimeSetUp>]
    member this.Initialise() =
        match tt with
        | "uint32" -> this.emulator <- Stub.initialise "broadcast" TestContext.Error
        | "controller" -> this.emulator <- Stub.initialise "broadcast" TestContext.Error
        | "controller+endpoint" -> this.emulator <- Stub.initialise "connected udp" TestContext.Error
        | "controller+endpoint+udp" -> this.emulator <- Stub.initialise "connected udp" TestContext.Error
        | "controller+endpoint+tcp" -> this.emulator <- Stub.initialise "tcp" TestContext.Error
        | _ -> failwith "unknown test case"

        let nodelay = Environment.GetEnvironmentVariable("NODELAY")

        match Boolean.TryParse nodelay with
        | true, v -> ()
        | _ -> Thread.Sleep 500

    [<OneTimeTearDown>]
    member this.Terminate() =
        Stub.terminate this.emulator TestContext.Error

        let nodelay = Environment.GetEnvironmentVariable("NODELAY")

        match Boolean.TryParse nodelay with
        | true, v -> ()
        | _ -> Thread.Sleep 500

    [<SetUp>]
    member this.Setup() = ()

    [<TearDown>]
    member this.TearDown() = ()

    [<Test>]
    member this.TestGetController() =
        let expected: Controller =
            { Controller = 405419896u
              Address = IPAddress.Parse("192.168.1.100")
              Netmask = IPAddress.Parse("255.255.255.0")
              Gateway = IPAddress.Parse("192.168.1.1")
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              Version = "v8.92"
              Date = Nullable(DateOnly.ParseExact("2018-08-16", "yyyy-MM-dd")) }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetController(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetController(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetIPv4() =
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetIPv4(controller, address, netmask, gateway, OPTIONS) with
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetIPv4(controller, address, netmask, gateway, OPTIONS) with
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetListener() =
        let expected: Listener =
            { Endpoint = IPEndPoint.Parse("192.168.1.100:60001")
              Interval = 13uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetListener(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetListener(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetListener() =
        let expected: bool = true
        let listener = IPEndPoint.Parse("192.168.1.100:60001")
        let interval = 17uy

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetListener(controller, listener, interval, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetListener(controller, listener, interval, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetTime() =
        let expected: Nullable<DateTime> =
            Nullable(DateTime.ParseExact("2024-11-01 12:34:56", "yyyy-MM-dd HH:mm:ss", null))

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetTime(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetTime(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetTime() =
        let expected: Nullable<DateTime> =
            Nullable(DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null))

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetTime(controller, datetime, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetTime(controller, datetime, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetDoor() =
        let expected: Door =
            { Mode = DoorMode.Controlled
              Delay = 7uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetDoor(controller, DOOR, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetDoor(controller, DOOR, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetDoorNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetDoor(controller, DOOR_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetDoor(controller, DOOR_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetDoor() =
        let expected: Door =
            { Mode = DoorMode.NormallyClosed
              Delay = 17uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetDoor(controller, DOOR, MODE, DELAY, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetDoor(controller, DOOR, MODE, DELAY, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetDoorNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetDoor(controller, DOOR_NOT_FOUND, MODE, DELAY, OPTIONS) with
            | Ok result when result.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetDoor(controller, DOOR_NOT_FOUND, MODE, DELAY, OPTIONS) with
            | Ok result when result.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetDoorPasscodes() =
        let expected = true
        let passcodes = [| 12345u; 54321u; 999999u |]

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetDoorPasscodes(controller, DOOR, passcodes, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetDoorPasscodes(controller, DOOR, passcodes, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestOpenDoor() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.OpenDoor(controller, DOOR, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.OpenDoor(controller, DOOR, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetStatus() =
        let status: Status =
            { Door1Open = true
              Door2Open = false
              Door3Open = true
              Door4Open = true
              Button1Pressed = true
              Button2Pressed = true
              Button3Pressed = false
              Button4Pressed = true
              SystemError = 27uy
              SystemDateTime = Nullable(DateTime.ParseExact("2024-11-13 14:37:53", "yyyy-MM-dd HH:mm:ss", null))
              SpecialInfo = 154uy
              Relays =
                { Door1 = Relay.Active
                  Door2 = Relay.Active
                  Door3 = Relay.Active
                  Door4 = Relay.Inactive }
              Inputs =
                { LockForced = Input.Set
                  FireAlarm = Input.Clear
                  Input3 = Input.Clear
                  Input4 = Input.Set
                  Input5 = Input.Clear
                  Input6 = Input.Clear
                  Input7 = Input.Clear
                  Input8 = Input.Clear } }

        let event: Event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-10 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 75312u
              Event = { Code = 3uy; Text = "alarm" }
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason =
                { Code = 6uy
                  Text = "no access rights" } }

        let expected = (status, Nullable(event))

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetStatus(controller, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetStatus(controller, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetStatusNoEvent() =
        let status: Status =
            { Door1Open = true
              Door2Open = false
              Door3Open = true
              Door4Open = true
              Button1Pressed = true
              Button2Pressed = true
              Button3Pressed = false
              Button4Pressed = true
              SystemError = 27uy
              SystemDateTime = Nullable(DateTime.ParseExact("2024-11-13 14:37:53", "yyyy-MM-dd HH:mm:ss", null))
              SpecialInfo = 154uy
              Relays =
                { Door1 = Relay.Active
                  Door2 = Relay.Active
                  Door3 = Relay.Active
                  Door4 = Relay.Inactive }
              Inputs =
                { LockForced = Input.Set
                  FireAlarm = Input.Clear
                  Input3 = Input.Clear
                  Input4 = Input.Set
                  Input5 = Input.Clear
                  Input6 = Input.Clear
                  Input7 = Input.Clear
                  Input8 = Input.Clear } }

        let expected = (status, Nullable())

        match controllers[tt] with
        | Id c ->
            let controller = CONTROLLER_NO_EVENT

            match Uhppoted.GetStatus(controller, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller c ->
            let controller =
                { controller = CONTROLLER_NO_EVENT
                  endpoint = c.endpoint
                  protocol = c.protocol }

            match Uhppoted.GetStatus(controller, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCards() =
        let expected: uint32 = 13579u

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCards(controller, OPTIONS) with
            | Ok cards -> Assert.That(cards, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCards(controller, OPTIONS) with
            | Ok cards -> Assert.That(cards, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCard() =
        let expected: Card =
            { Card = 10058400u
              StartDate = Nullable(DateOnly(2024, 1, 1))
              EndDate = Nullable(DateOnly(2024, 12, 31))
              Door1 = 1uy
              Door2 = 0uy
              Door3 = 17uy
              Door4 = 1uy
              PIN = 7531u }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCard(controller, CARD, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCard(controller, CARD, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCardNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCard(controller, MISSING_CARD, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCard(controller, MISSING_CARD, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCardAtIndex() =
        let expected: Card =
            { Card = 10058400u
              StartDate = Nullable(DateOnly(2024, 1, 1))
              EndDate = Nullable(DateOnly(2024, 12, 31))
              Door1 = 1uy
              Door2 = 0uy
              Door3 = 17uy
              Door4 = 1uy
              PIN = 7531u }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCardAtIndexNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetCardAtIndexDeleted() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX_DELETED, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetCardAtIndex(controller, CARD_INDEX_DELETED, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestPutCard() =
        let expected = true

        let card: Card =
            { Card = CARD
              StartDate = Nullable(DateOnly(2024, 1, 1))
              EndDate = Nullable(DateOnly(2024, 12, 31))
              Door1 = 1uy
              Door2 = 0uy
              Door3 = 17uy
              Door4 = 1uy
              PIN = 7531u }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.PutCard(controller, card, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.PutCard(controller, card, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestDeleteCard() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.DeleteCard(controller, CARD, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.DeleteCard(controller, CARD, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestDeleteAllCards() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.DeleteAllCards(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.DeleteAllCards(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetEvent() =
        let expected: Event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-17 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 13579u
              Event = { Code = 2uy; Text = "door" }
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason = { Code = 18uy; Text = "access denied" } }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetEventNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetEventOverwritten() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX_OVERWRITTEN, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetEvent(controller, EVENT_INDEX_OVERWRITTEN, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetEventIndex() =
        let expected: uint32 = 13579u

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetEventIndex(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetEventIndex(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetEventIndex() =
        let expected: bool = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetEventIndex(controller, EVENT_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetEventIndex(controller, EVENT_INDEX, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestRecordSpecialEvents() =
        let expected: bool = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.RecordSpecialEvents(controller, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.RecordSpecialEvents(controller, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetTimeProfile() =
        let expected: TimeProfile =
            { Profile = 37uy
              StartDate = Nullable(DateOnly(2024, 11, 26))
              EndDate = Nullable(DateOnly(2024, 12, 29))
              Monday = true
              Tuesday = true
              Wednesday = false
              Thursday = true
              Friday = false
              Saturday = true
              Sunday = true
              Segment1Start = Nullable(TimeOnly(8, 30))
              Segment1End = Nullable(TimeOnly(09, 45))
              Segment2Start = Nullable(TimeOnly(11, 35))
              Segment2End = Nullable(TimeOnly(13, 15))
              Segment3Start = Nullable(TimeOnly(14, 01))
              Segment3End = Nullable(TimeOnly(17, 59))
              LinkedProfile = 19uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetTimeProfile(controller, TIME_PROFILE_ID, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetTimeProfile(controller, TIME_PROFILE_ID, OPTIONS) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestGetTimeProfileNotFound() =
        match controllers[tt] with
        | Id controller ->
            match Uhppoted.GetTimeProfile(controller, TIME_PROFILE_ID_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.GetTimeProfile(controller, TIME_PROFILE_ID_NOT_FOUND, OPTIONS) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetTimeProfile() =
        let expected: bool = true

        let profile: TimeProfile =
            { Profile = 37uy
              StartDate = Nullable(DateOnly(2024, 11, 26))
              EndDate = Nullable(DateOnly(2024, 12, 29))
              Monday = true
              Tuesday = true
              Wednesday = false
              Thursday = true
              Friday = false
              Saturday = true
              Sunday = true
              Segment1Start = Nullable(TimeOnly(8, 30))
              Segment1End = Nullable(TimeOnly(09, 45))
              Segment2Start = Nullable(TimeOnly(11, 35))
              Segment2End = Nullable(TimeOnly(13, 15))
              Segment3Start = Nullable(TimeOnly(14, 01))
              Segment3End = Nullable(TimeOnly(17, 59))
              LinkedProfile = 19uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetTimeProfile(controller, profile, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetTimeProfile(controller, profile, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestClearTimeProfiles() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.ClearTimeProfiles(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.ClearTimeProfiles(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestAddTask() =
        let expected: bool = true

        let task: Task =
            { Task = TaskCode.DisableTimeProfiles
              Door = 3uy
              StartDate = Nullable(DateOnly(2024, 11, 26))
              EndDate = Nullable(DateOnly(2024, 12, 29))
              Monday = true
              Tuesday = true
              Wednesday = false
              Thursday = true
              Friday = false
              Saturday = true
              Sunday = true
              StartTime = Nullable(TimeOnly(8, 45))
              MoreCards = 7uy }

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.AddTask(controller, task, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.AddTask(controller, task, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestClearTaskList() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.ClearTaskList(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.ClearTaskList(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestRefreshTaskList() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.RefreshTaskList(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.RefreshTaskList(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetPCControl() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetPCControl(controller, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetPCControl(controller, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestSetInterlock() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.SetInterlock(controller, Interlock.Doors1234, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.SetInterlock(controller, Interlock.Doors1234, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestActivateKeypads() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.ActivateKeypads(controller, true, true, false, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.ActivateKeypads(controller, true, true, false, true, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestRestoreDefaultParameters() =
        let expected = true

        match controllers[tt] with
        | Id controller ->
            match Uhppoted.RestoreDefaultParameters(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

        | Controller controller ->
            match Uhppoted.RestoreDefaultParameters(controller, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail($"{err}")

    [<Test>]
    member this.TestListen() =
        let cancel = new CancellationTokenSource()

        Console.CancelKeyPress.Add(fun args ->
            args.Cancel <- true
            cancel.Cancel())

        let mutable events = []
        let mutable errors = []

        let eventHandler event = events <- events @ [ event ]
        let errorHandler err = errors <- errors @ [ err ]

        let onevent: OnEvent = new OnEvent(eventHandler)
        let onerror: OnError = new OnError(errorHandler)

        let listen =
            Async.StartAsTask(async { Uhppoted.Listen(onevent, onerror, cancel.Token, OPTIONS) |> ignore })

        async {
            do! Async.Sleep(1000)
            cancel.Cancel()
        }
        |> Async.Start

        async {
            do! Async.Sleep(100)
            Stub.event this.emulator Events.normalEvent TestContext.Error
            do! Async.Sleep(100)
            Stub.event this.emulator Events.v6_62_Event TestContext.Error
            do! Async.Sleep(100)
            Stub.event this.emulator Events.eventWithoutEvent TestContext.Error
            do! Async.Sleep(100)
            Stub.event this.emulator Events.errorEvent TestContext.Error
        }
        |> Async.Start

        listen.Wait()

        let status =
            { Door1Open = true
              Door2Open = false
              Door3Open = true
              Door4Open = true
              Button1Pressed = true
              Button2Pressed = true
              Button3Pressed = false
              Button4Pressed = true
              SystemError = 27uy
              SystemDateTime = Nullable(DateTime.ParseExact("2024-11-13 14:37:53", "yyyy-MM-dd HH:mm:ss", null))
              SpecialInfo = 154uy
              Relays =
                { Door1 = Relay.Active
                  Door2 = Relay.Active
                  Door3 = Relay.Active
                  Door4 = Relay.Inactive }
              Inputs =
                { LockForced = Input.Set
                  FireAlarm = Input.Clear
                  Input3 = Input.Clear
                  Input4 = Input.Set
                  Input5 = Input.Clear
                  Input6 = Input.Clear
                  Input7 = Input.Clear
                  Input8 = Input.Clear } }

        let event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-10 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 75312u
              Event = { Code = 3uy; Text = "alarm" }
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason =
                { Code = 6uy
                  Text = "no access rights" } }

        let expected =
            [ { Controller = 405419896u
                Status = status
                Event = Nullable(event) }

              { Controller = 303986753u
                Status = status
                Event = Nullable(event) }

              { Controller = 405419897u
                Status = status
                Event = Nullable() } ]

        Assert.That(events, Is.EqualTo(expected))
        Assert.That(errors, Is.EqualTo([ PacketError "invalid listen-event packet" ]))
