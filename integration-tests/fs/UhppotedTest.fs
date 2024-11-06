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

    let OPTIONS: Options =
        { bind = IPEndPoint(IPAddress.Any, 0)
          broadcast = IPEndPoint(IPAddress.Broadcast, 59999)
          listen = IPEndPoint(IPAddress.Any, 60001)
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
    member this.TestGetAllController() =
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

        let result = Uhppoted.get_all_controllers (TIMEOUT, OPTIONS)

        Assert.That(result, Is.EqualTo(expected))

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
