namespace Uhppoted.Tests

open System
open System.Net
open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestEncoder() =
    [<Test>]
    member this.TestEncodeGetControllerWithZeroControllerID() =
        let expected = TestRequests.get_all_controllers
        let packet = Encode.get_controller_request 0u

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetControllerWithValidControllerID() =
        let expected = TestRequests.get_controller

        let controller = 405419896u
        let packet = Encode.get_controller_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetIPv4() =
        let expected = TestRequests.set_IPv4

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")
        let packet = Encode.set_IPv4_request controller address netmask gateway

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetListener() =
        let expected = TestRequests.get_listener

        let controller = 405419896u
        let packet = Encode.get_listener_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetListener() =
        let expected = TestRequests.set_listener

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let port = 60001us
        let interval = 17uy
        let packet = Encode.set_listener_request controller address port interval

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetTime() =
        let expected = TestRequests.get_time

        let controller = 405419896u
        let packet = Encode.get_time_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetTime() =
        let expected = TestRequests.set_time

        let controller = 405419896u

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        let packet = Encode.set_time_request controller datetime

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetDoor() =
        let expected = TestRequests.get_door_settings

        let controller = 405419896u
        let door = 3uy
        let packet = Encode.get_door_settings_request controller door

        Assert.That(packet, Is.EqualTo(expected))
