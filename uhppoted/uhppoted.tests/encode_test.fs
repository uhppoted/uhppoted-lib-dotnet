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
        let packet = Encode.get_controller_request 405419896u
        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetIPv4() =
        let expected = TestRequests.set_IPv4

        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")
        let packet = Encode.set_IPv4_request 405419896u address netmask gateway

        Assert.That(packet, Is.EqualTo(expected))
