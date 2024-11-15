namespace FSharp.Tests

open System
open System.Net
open System.Net.NetworkInformation
open NUnit.Framework
open uhppoted
open stub

[<TestFixture>]
type TestClass() =
    [<DefaultValue>]
    val mutable emulator: Emulator

    let CONTROLLER = 405419896u
    let ENDPOINT = IPEndPoint.Parse("127.0.0.1:59999")
    let TIMEOUT = 500
    let DOOR = 4uy
    let MODE = 2uy
    let DELAY = 17uy
    let CARD = 10058400u
    let CARD_INDEX = 135u
    let CARD_INDEX_NOT_FOUND = 136u
    let CARD_INDEX_DELETED = 137u

    let OPTIONS: Options =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 59999)
          listen = IPEndPoint(IPAddress.Any, 60001)
          destination = None
          protocol = None
          debug = false }

    let controllers =
        [ { controller = CONTROLLER
            address = None
            protocol = None }

          { controller = CONTROLLER
            address = Some(ENDPOINT)
            protocol = None }

          { controller = CONTROLLER
            address = Some(ENDPOINT)
            protocol = Some("udp") }

          { controller = CONTROLLER
            address = Some(ENDPOINT)
            protocol = Some("tcp") } ]

    let options =
        [ { OPTIONS with destination = None }
          { OPTIONS with
              destination = Some(ENDPOINT) }
          { OPTIONS with
              destination = Some(ENDPOINT)
              protocol = Some("udp") }
          { OPTIONS with
              destination = Some(ENDPOINT)
              protocol = Some("tcp") } ]

    [<OneTimeSetUp>]
    member this.Initialise() =
        this.emulator <- Stub.initialise TestContext.Error

    [<OneTimeTearDown>]
    member this.Terminate() = Stub.terminate this.emulator

    [<SetUp>]
    member this.Setup() = ()

    [<TearDown>]
    member this.TearDown() = ()

    [<Test>]
    member this.TestFindControllers() =
        let expected: GetControllerResponse array =
            [| { controller = 405419896u
                 address = IPAddress.Parse("192.168.1.100")
                 netmask = IPAddress.Parse("255.255.255.0")
                 gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x00uy; 0x12uy; 0x23uy; 0x34uy; 0x45uy; 0x56uy |])
                 version = "v8.92"
                 date = Nullable(DateOnly.ParseExact("2018-11-05", "yyyy-MM-dd")) }

               { controller = 303986753u
                 address = IPAddress.Parse("192.168.1.100")
                 netmask = IPAddress.Parse("255.255.255.0")
                 gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x52uy; 0xfduy; 0xfcuy; 0x07uy; 0x21uy; 0x82uy |])
                 version = "v8.92"
                 date = Nullable(DateOnly.ParseExact("2019-08-15", "yyyy-MM-dd")) }

               { controller = 201020304u
                 address = IPAddress.Parse("192.168.1.101")
                 netmask = IPAddress.Parse("255.255.255.0")
                 gateway = IPAddress.Parse("192.168.1.1")
                 MAC = PhysicalAddress([| 0x52uy; 0xfduy; 0xfcuy; 0x07uy; 0x21uy; 0x82uy |])
                 version = "v6.62"
                 date = Nullable(DateOnly.ParseExact("2020-01-01", "yyyy-MM-dd")) } |]

        match Uhppoted.FindControllers(TIMEOUT, OPTIONS) with
        | Ok controllers -> Assert.That(controllers, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestGetController() =
        let expected: GetControllerResponse =
            { controller = 405419896u
              address = IPAddress.Parse("192.168.1.100")
              netmask = IPAddress.Parse("255.255.255.0")
              gateway = IPAddress.Parse("192.168.1.1")
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Nullable(DateOnly.ParseExact("2018-08-16", "yyyy-MM-dd")) }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_controller (controller, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetIPv4() =
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.set_IPv4 (controller, address, netmask, gateway, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetListener() =
        let expected: GetListenerResponse =
            { controller = 405419896u
              endpoint = IPEndPoint.Parse("192.168.1.100:60001")
              interval = 13uy }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_listener (controller, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetListener() =
        let expected: SetListenerResponse = { controller = 405419896u; ok = true }

        let endpoint = IPEndPoint.Parse("192.168.1.100:60001")
        let interval = 17uy

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.set_listener (controller, endpoint, interval, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetTime() =
        let expected: GetTimeResponse =
            { controller = 405419896u
              datetime = Nullable(DateTime.ParseExact("2024-11-01 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_time (controller, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetTime() =
        let expected: SetTimeResponse =
            { controller = 405419896u
              datetime = Nullable(DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.set_time (controller, datetime, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetDoor() =
        let expected: GetDoorResponse =
            { controller = 405419896u
              door = 4uy
              mode = 3uy
              delay = 7uy }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_door (controller, DOOR, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetDoor() =
        let expected: SetDoorResponse =
            { controller = 405419896u
              door = 4uy
              mode = 2uy
              delay = 17uy }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.set_door (controller, DOOR, MODE, DELAY, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestSetDoorPasscodes() =
        let expected: SetDoorPasscodesResponse = { controller = 405419896u; ok = true }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.set_door_passcodes (controller, DOOR, 12345u, 54321u, 0u, 999999u, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestOpenDoor() =
        let expected: OpenDoorResponse = { controller = 405419896u; ok = true }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.open_door (controller, DOOR, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetStatus() =
        let expected: GetStatusResponse =
            { controller = 405419896u
              door1_open = true
              door2_open = false
              door3_open = true
              door4_open = true
              door1_button = true
              door2_button = true
              door3_button = false
              door4_button = true
              system_error = 27uy
              system_datetime = Nullable(DateTime.ParseExact("2024-11-13 14:37:53", "yyyy-MM-dd HH:mm:ss", null))
              sequence_number = 21987u
              special_info = 154uy
              relays = 7uy
              inputs = 9uy
              evt =
                {| index = 75312u
                   event_type = 19uy
                   granted = true
                   door = 4uy
                   direction = 2uy
                   card = 10058400u
                   timestamp = Nullable(DateTime.ParseExact("2024-11-10 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
                   reason = 6uy |} }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_status (controller, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCards() =
        let expected: GetCardsResponse =
            { controller = 405419896u
              cards = 13579u }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_cards (controller, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCard() =
        let expected: GetCardResponse =
            { controller = 405419896u
              card = 10058400u
              startdate = Nullable(DateOnly(2024, 1, 1))
              enddate = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 7531u }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.GetCard(controller, CARD, TIMEOUT, OPTIONS) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardNotFound() =
        let expected: GetCardResponse =
            { controller = 405419896u
              card = 10058400u
              startdate = Nullable(DateOnly(2024, 1, 1))
              enddate = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 7531u }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.GetCard(controller, 10058399u, TIMEOUT, OPTIONS) with
            | Ok _ -> Assert.Fail("expected 'card not found' error")
            | Error "card not found" -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardAtIndex() =
        let expected: Card =
            { card = 10058400u
              startdate = Nullable(DateOnly(2024, 1, 1))
              enddate = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 7531u }

        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX, TIMEOUT, opts) with
            | Ok response -> Assert.That(response.Value, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardAtIndexNotFound() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX_NOT_FOUND, TIMEOUT, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestGetCardAtIndexDeleted() =
        options
        |> List.iter (fun opts ->
            match Uhppoted.GetCardAtIndex(CONTROLLER, CARD_INDEX_DELETED, TIMEOUT, opts) with
            | Ok response when response.HasValue -> Assert.Fail("expected 'null'")
            | Ok _ -> Assert.Pass()
            | Error err -> Assert.Fail(err))

    [<Test>]
    member this.TestPutCard() =
        let expected = true

        let startdate = DateOnly(2024, 1, 1)
        let enddate = DateOnly(2024, 12, 31)
        let door1 = 1uy
        let door2 = 0uy
        let door3 = 17uy
        let door4 = 1uy
        let PIN = 7531u

        options
        |> List.iter (fun opts ->
            match
                Uhppoted.PutCard(CONTROLLER, CARD, startdate, enddate, door1, door2, door3, door4, PIN, TIMEOUT, opts)
            with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))
