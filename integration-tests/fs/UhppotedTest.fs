namespace FSharp.Tests

open System
open System.Net
open System.Net.NetworkInformation
open System.Threading
open NUnit.Framework

open uhppoted
open stub
open TestCases

[<TestFixture("uint32")>]
type TestClass(tt: string) =
    [<DefaultValue>]
    val mutable emulator: Emulator

    let controller = controllers[tt]

    let CONTROLLER = 405419896u
    let CONTROLLER_NO_EVENT = 405419897u
    let ENDPOINT = IPEndPoint.Parse("127.0.0.1:59999")
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
          endpoint = None
          protocol = None
          debug = false }

    let options =
        [ { OPTIONS with endpoint = None }

          { OPTIONS with
              endpoint = Some(ENDPOINT) }

          { OPTIONS with
              endpoint = Some(ENDPOINT)
              protocol = Some("udp") }

          { OPTIONS with
              endpoint = Some(ENDPOINT)
              protocol = Some("tcp") } ]

    [<OneTimeSetUp>]
    member this.Initialise() =
        this.emulator <- Stub.initialise TestContext.Error

    [<OneTimeTearDown>]
    member this.Terminate() =
        Stub.terminate this.emulator TestContext.Error

    [<SetUp>]
    member this.Setup() = ()

    [<TearDown>]
    member this.TearDown() = ()

    [<Test>]
    member this.TestFindControllers() =
        printfn ">>>>>>>>>>>>>>>>>>>>>>>>> %A" controller

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
        | Error err -> Assert.Fail(err)

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetController(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetIPv4() =
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetIPv4(CONTROLLER, address, netmask, gateway, OPTIONS) with
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetListener() =
        let expected: Listener =
            { Endpoint = IPEndPoint.Parse("192.168.1.100:60001")
              Interval = 13uy }

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetListener(CONTROLLER, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetListener() =
        let expected: bool = true
        let listener = IPEndPoint.Parse("192.168.1.100:60001")
        let interval = 17uy

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetListener(CONTROLLER, listener, interval, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetTime() =
        let expected: Nullable<DateTime> =
            Nullable(DateTime.ParseExact("2024-11-01 12:34:56", "yyyy-MM-dd HH:mm:ss", null))

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetTime(CONTROLLER, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetTime() =
        let expected: Nullable<DateTime> =
            Nullable(DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null))

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetTime(CONTROLLER, datetime, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetDoor() =
        let expected: Door =
            { mode = DoorMode.Controlled
              delay = 7uy }

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetDoor(CONTROLLER, DOOR, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetDoorNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetDoor(CONTROLLER, DOOR_NOT_FOUND, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetDoor() =
        let expected: Door =
            { mode = DoorMode.NormallyClosed
              delay = 17uy }

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetDoor(CONTROLLER, DOOR, MODE, DELAY, OPTIONS) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetDoorNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.SetDoor(CONTROLLER, DOOR_NOT_FOUND, MODE, DELAY, opts) with
            | Ok result when result.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetDoorPasscodes() =
        let expected = true
        let passcodes = [| 12345u; 54321u; 999999u |]

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetDoorPasscodes(CONTROLLER, DOOR, passcodes, opts) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestOpenDoor() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.OpenDoor(CONTROLLER, DOOR, opts) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

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
              SequenceNumber = 21987u
              SpecialInfo = 154uy
              Relay1 = Relay.Closed
              Relay2 = Relay.Closed
              Relay3 = Relay.Closed
              Relay4 = Relay.Open
              Input1 = Input.Closed
              Input2 = Input.Open
              Input3 = Input.Open
              Input4 = Input.Closed }

        let event: Event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-10 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 75312u
              Event = 19uy
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason = 6uy }

        let expected = (status, Nullable(event))

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetStatus(CONTROLLER, opts) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

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
              SequenceNumber = 21987u
              SpecialInfo = 154uy
              Relay1 = Relay.Closed
              Relay2 = Relay.Closed
              Relay3 = Relay.Closed
              Relay4 = Relay.Open
              Input1 = Input.Closed
              Input2 = Input.Open
              Input3 = Input.Open
              Input4 = Input.Closed }

        let expected = (status, Nullable())

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetStatus(CONTROLLER_NO_EVENT, opts) with
            | Ok result -> Assert.That(result, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCards() =
        let expected: uint32 = 13579u

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCards(CONTROLLER, opts) with
            | Ok cards -> Assert.That(cards, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCard(CONTROLLER, CARD, opts) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))


    [<Test>]
    member this.TestGetCardNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCard(CONTROLLER, MISSING_CARD, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX, opts) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardAtIndexNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX_NOT_FOUND, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardAtIndexDeleted() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX_DELETED, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.PutCard(CONTROLLER, card, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestDeleteCard() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.DeleteCard(CONTROLLER, CARD, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestDeleteAllCards() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.DeleteAllCards(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetEvent() =
        let expected: Event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-17 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 13579u
              Event = 2uy
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason = 21uy }

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetEvent(CONTROLLER, EVENT_INDEX, opts) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetEventNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetEvent(CONTROLLER, EVENT_INDEX_NOT_FOUND, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetEventOverwritten() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetEvent(CONTROLLER, EVENT_INDEX_OVERWRITTEN, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetEventIndex() =
        let expected: uint32 = 13579u

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetEventIndex(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetEventIndex() =
        let expected: bool = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetEventIndex(CONTROLLER, EVENT_INDEX, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestRecordSpecialEvents() =
        let expected: bool = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.RecordSpecialEvents(CONTROLLER, true, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetTimeProfile(CONTROLLER, TIME_PROFILE_ID, opts) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetTimeProfileNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetTimeProfile(CONTROLLER, TIME_PROFILE_ID_NOT_FOUND, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

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

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetTimeProfile(CONTROLLER, profile, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestClearTimeProfiles() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.ClearTimeProfiles(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestAddTask() =
        let expected: bool = true

        let task: Task =
            { Task = 4uy
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

        options
        |> List.iter (fun opts ->
            match Uhppoted.AddTask(CONTROLLER, task, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestClearTaskList() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.ClearTaskList(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestRefreshTaskList() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.RefreshTaskList(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetPCControl() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetPCControl(CONTROLLER, true, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetInterlock() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.SetInterlock(CONTROLLER, Interlock.Doors1234, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestActivateKeypads() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.ActivateKeypads(CONTROLLER, true, true, false, true, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestRestoreDefaultParameters() =
        let expected = true

        options
        |> List.iter (fun opts ->
            match Uhppoted.RestoreDefaultParameters(CONTROLLER, opts) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

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
              SequenceNumber = 21987u
              SpecialInfo = 154uy
              Relay1 = Relay.Closed
              Relay2 = Relay.Closed
              Relay3 = Relay.Closed
              Relay4 = Relay.Open
              Input1 = Input.Closed
              Input2 = Input.Open
              Input3 = Input.Open
              Input4 = Input.Closed }

        let event =
            { Timestamp = Nullable(DateTime.ParseExact("2024-11-10 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              Index = 75312u
              Event = 19uy
              AccessGranted = true
              Door = 4uy
              Direction = Direction.Out
              Card = 10058400u
              Reason = 6uy }

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
        Assert.That(errors, Is.EqualTo([ "invalid listen-event packet" ]))
