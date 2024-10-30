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

    let controllers =
        [ { controller = 405419896u
            address = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 59999))
            protocol = None }

          { controller = 405419896u
            address = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 59999))
            protocol = Some("udp") }

          { controller = 405419896u
            address = Some(IPEndPoint(IPAddress.Parse("192.168.1.100"), 59999))
            protocol = Some("tcp") } ]


    [<OneTimeSetUp>]
    member this.Initialise() =
        TestContext.Error.WriteLine("=========>OneTimeSetUp")
        this.emulator <- Stub.initialise TestContext.Error

    [<OneTimeTearDown>]
    member this.Terminate() =
        TestContext.Error.WriteLine("=========>OneTimeTearDown")
        Stub.terminate this.emulator

    [<SetUp>]
    member this.Setup() = printfn ">>> setup %A" this.emulator

    [<TearDown>]
    member this.TearDown() = printfn ">>> teardown %A" this.emulator

    [<Test>]
    member this.TestGetController() =
        let expected: GetControllerResponse =
            { controller = 405419896u
              address = IPAddress.Parse("192.168.1.100")
              netmask = IPAddress.Parse("255.255.255.0")
              gateway = IPAddress.Parse("192.168.1.1")
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Some(DateOnly.ParseExact("2018-08-16", "yyyy-MM-dd")) }

        controllers
        |> List.iter (fun controller ->
            match Uhppoted.get_controller (controller, 1000, false) with
            | Ok response -> Assert.That(response, Is.EqualTo(expected))
            | Error err -> Assert.Fail(err))
