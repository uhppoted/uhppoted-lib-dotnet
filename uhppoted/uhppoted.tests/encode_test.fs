namespace Uhppoted.Tests

open System
open System.Net
open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestEncoder() =
    [<Test>]
    member this.TestEncodeGetControllerRequestWithZeroControllerID() =
        let expected = TestRequests.get_all_controllers
        let packet = Encode.get_controller_request 0u

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetControllerRequestWithValidControllerID() =
        let expected = TestRequests.get_controller

        let controller = 405419896u
        let packet = Encode.get_controller_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetIPv4Request() =
        let expected = TestRequests.set_IPv4

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")
        let packet = Encode.set_IPv4_request controller address netmask gateway

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetListenerRequest() =
        let expected = TestRequests.get_listener

        let controller = 405419896u
        let packet = Encode.get_listener_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetListenerRequest() =
        let expected = TestRequests.set_listener

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let port = 60001us
        let interval = 17uy
        let packet = Encode.set_listener_request controller address port interval

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetTimeRequest() =
        let expected = TestRequests.get_time

        let controller = 405419896u
        let packet = Encode.get_time_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetTimeRequest() =
        let expected = TestRequests.set_time

        let controller = 405419896u

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        let packet = Encode.set_time_request controller datetime

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetDoorRequest() =
        let expected = TestRequests.get_door

        let controller = 405419896u
        let door = 3uy
        let packet = Encode.get_door_request controller door

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorRequest() =
        let expected = TestRequests.set_door

        let controller = 405419896u
        let door = 3uy
        let mode = 2uy
        let delay = 17uy
        let packet = Encode.set_door_request controller door mode delay

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorPasscodesRequest() =
        let expected = TestRequests.set_door_passcodes

        let controller = 405419896u
        let door = 3uy
        let passcodes = [| 123456u; 234567u; 345678u; 456789u |]

        let packet =
            Encode.set_door_passcodes_request controller door passcodes[0] passcodes[1] passcodes[2] passcodes[3]

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorPasscodesRequestWithInvalidPasscode() =
        let expected = TestRequests.set_door_passcodes_with_invalid_passcode

        let controller = 405419896u
        let door = 3uy
        let passcodes = [| 123456u; 1234567u; 345678u; 456789u |]

        let packet =
            Encode.set_door_passcodes_request controller door passcodes[0] passcodes[1] passcodes[2] passcodes[3]

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeOpenDoorRequest() =
        let expected = TestRequests.open_door

        let controller = 405419896u
        let door = 3uy

        let packet = Encode.open_door_request controller door

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetStatusRequest() =
        let expected = TestRequests.get_status

        let controller = 405419896u
        let packet = Encode.get_status_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardsRequest() =
        let expected = TestRequests.get_cards

        let controller = 405419896u
        let packet = Encode.get_cards_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardRequest() =
        let expected = TestRequests.get_card

        let controller = 405419896u
        let card = 10058400u
        let packet = Encode.get_card_request controller card

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardAtIndexRequest() =
        let expected = TestRequests.get_card_at_index

        let controller = 405419896u
        let index = 135u
        let packet = Encode.get_card_at_index_request controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodePutCardRequest() =
        let expected = TestRequests.put_card

        let controller = 405419896u
        let card = 10058400u
        let startdate = DateOnly(2024, 1, 1)
        let enddate = DateOnly(2024, 12, 31)
        let door1 = 1uy
        let door2 = 0uy
        let door3 = 17uy
        let door4 = 1uy
        let PIN = 999999u

        let packet =
            Encode.put_card_request controller card startdate enddate door1 door2 door3 door4 PIN

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeDeleteCardRequest() =
        let expected = TestRequests.delete_card

        let controller = 405419896u
        let card = 10058400u
        let packet = Encode.delete_card_request controller card

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeDeleteAllCardsRequest() =
        let expected = TestRequests.delete_all_cards

        let controller = 405419896u
        let packet = Encode.delete_all_cards_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetEventRequest() =
        let expected = TestRequests.get_event

        let controller = 405419896u
        let index = 13579u
        let packet = Encode.get_event_request controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetEventIndexRequest() =
        let expected = TestRequests.get_event_index

        let controller = 405419896u
        let packet = Encode.get_event_index_request controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetEventIndexRequest() =
        let expected = TestRequests.set_event_index

        let controller = 405419896u
        let index = 13579u
        let packet = Encode.set_event_index_request controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestRecordSpecialEventsRequest() =
        let expected = TestRequests.record_special_events

        let controller = 405419896u
        let enabled = true
        let packet = Encode.record_special_events_request controller enabled

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestGetTimeProfileRequest() =
        let expected = TestRequests.get_time_profile

        let controller = 405419896u
        let profile = 37uy
        let packet = Encode.get_time_profile_request controller profile

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestSetTimeProfileRequest() =
        let expected = TestRequests.set_time_profile

        let controller = 405419896u

        let profile: TimeProfile =
            { profile = 37uy
              start_date = Nullable(DateOnly(2024, 11, 26))
              end_date = Nullable(DateOnly(2024, 12, 29))
              monday = true
              tuesday = true
              wednesday = false
              thursday = true
              friday = false
              saturday = true
              sunday = true
              segment1_start = Nullable(TimeOnly(8, 30))
              segment1_end = Nullable(TimeOnly(09, 45))
              segment2_start = Nullable(TimeOnly(11, 35))
              segment2_end = Nullable(TimeOnly(13, 15))
              segment3_start = Nullable(TimeOnly(14, 01))
              segment3_end = Nullable(TimeOnly(17, 59))
              linked_profile = 19uy }

        let packet = Encode.set_time_profile_request controller profile

        Assert.That(packet, Is.EqualTo(expected))
