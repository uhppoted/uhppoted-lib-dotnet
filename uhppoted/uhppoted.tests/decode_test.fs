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

        let response = Decode.get_controller_response packet
        Assert.That(response, Is.EqualTo(expected))

    [<Test>]
    member this.TestDecodeGetListenerResponse() =
        let packet = TestResponses.get_listener

        let expected =
            { controller = 405419896u
              endpoint = IPEndPoint(IPAddress.Parse("192.168.1.100"), 60001)
              interval = 13uy }

        let response = Decode.get_listener_response packet
        Assert.That(response, Is.EqualTo(expected))

    [<Test>]
    member this.TestDecodeSetListenerResponse() =
        let packet = TestResponses.set_listener

        let expected = { controller = 405419896u; ok = true }

        let response = Decode.set_listener_response packet
        Assert.That(response, Is.EqualTo(expected))
