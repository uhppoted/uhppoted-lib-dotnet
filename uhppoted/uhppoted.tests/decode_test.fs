namespace Uhppoted.Tests

open System
open System.Net
open System.Net.NetworkInformation

open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestDecoder() =
    [<Test>]
    member this.TestDecodeGetControllerResponse() =
        let packet = TestResponses.get_controller

        let expected =
            { controller = 405419896u
              address = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x64uy |])
              netmask = IPAddress([| 0xffuy; 0xffuy; 0xffuy; 0x00uy |])
              gateway = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x01uy |])
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Some(DateOnly(2018, 8, 16)) }

        match Decode.get_controller_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetListenerResponse() =
        let packet = TestResponses.get_listener

        let expected =
            { controller = 405419896u
              endpoint = IPEndPoint.Parse("192.168.1.100:60001")
              interval = 13uy }

        match Decode.get_listener_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetListenerResponse() =
        let packet = TestResponses.set_listener

        let expected = { controller = 405419896u; ok = true }

        match Decode.set_listener_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetTimeResponse() =
        let packet = TestResponses.get_time

        let expected =
            { GetTimeResponse.controller = 405419896u
              GetTimeResponse.datetime =
                Nullable(DateTime.ParseExact("2024-11-01 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        match Decode.get_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetTimeResponseWithInvalidDateTime() =
        let packet = TestResponses.get_time_with_invalid_datetime

        let expected =
            { GetTimeResponse.controller = 405419896u
              GetTimeResponse.datetime = Nullable() }

        match Decode.get_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetTimeResponse() =
        let packet = TestResponses.set_time

        let expected =
            { SetTimeResponse.controller = 405419896u
              SetTimeResponse.datetime =
                Nullable(DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        match Decode.set_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetTimeResponseWithInvalidDateTime() =
        let packet = TestResponses.set_time_with_invalid_datetime

        let expected =
            { SetTimeResponse.controller = 405419896u
              SetTimeResponse.datetime = Nullable() }

        match Decode.set_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetDoorSettingsResponse() =
        let packet = TestResponses.get_door_settings

        let expected =
            { controller = 405419896u
              door = 03uy
              mode = 01uy
              delay = 05uy }

        match Decode.get_door_settings_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)
