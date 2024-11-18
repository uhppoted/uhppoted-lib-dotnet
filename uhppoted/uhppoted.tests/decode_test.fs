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
              date = Nullable(DateOnly(2018, 8, 16)) }

        match Decode.get_controller_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetControllerResponseWithInvalidDate() =
        let packet = TestResponses.get_controller_with_invalid_date

        let expected =
            { controller = 405419896u
              address = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x64uy |])
              netmask = IPAddress([| 0xffuy; 0xffuy; 0xffuy; 0x00uy |])
              gateway = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x01uy |])
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Nullable() }

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

        let expected: SetListenerResponse = { controller = 405419896u; ok = true }

        match Decode.set_listener_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetTimeResponse() =
        let packet = TestResponses.get_time

        let expected: GetTimeResponse =
            { controller = 405419896u
              datetime = Nullable(DateTime.ParseExact("2024-11-01 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        match Decode.get_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetTimeResponseWithInvalidDateTime() =
        let packet = TestResponses.get_time_with_invalid_datetime

        let expected: GetTimeResponse =
            { controller = 405419896u
              datetime = Nullable() }

        match Decode.get_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetTimeResponse() =
        let packet = TestResponses.set_time

        let expected: SetTimeResponse =
            { controller = 405419896u
              datetime = Nullable(DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)) }

        match Decode.set_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetTimeResponseWithInvalidDateTime() =
        let packet = TestResponses.set_time_with_invalid_datetime

        let expected: SetTimeResponse =
            { controller = 405419896u
              datetime = Nullable() }

        match Decode.set_time_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetDoorResponse() =
        let packet = TestResponses.get_door

        let expected: GetDoorResponse =
            { controller = 405419896u
              door = 03uy
              mode = 01uy
              delay = 05uy }

        match Decode.get_door_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetDoorResponse() =
        let packet = TestResponses.set_door

        let expected: SetDoorResponse =
            { controller = 405419896u
              door = 03uy
              mode = 02uy
              delay = 17uy }

        match Decode.set_door_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetDoorPasscodesResponse() =
        let packet = TestResponses.set_door_passcodes

        let expected: SetDoorPasscodesResponse = { controller = 405419896u; ok = true }

        match Decode.set_door_passcodes_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeOpenDoorResponse() =
        let packet = TestResponses.open_door

        let expected: OpenDoorResponse = { controller = 405419896u; ok = true }

        match Decode.open_door_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetStatusResponse() =
        let packet = TestResponses.get_status

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

        match Decode.get_status_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardsResponse() =
        let packet = TestResponses.get_cards

        let expected: GetCardsResponse =
            { controller = 405419896u
              cards = 13579u }

        match Decode.get_cards_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardResponse() =
        let packet = TestResponses.get_card

        let expected: GetCardResponse =
            { controller = 405419896u
              card = 10058400u
              startdate = Nullable(DateOnly(2024, 1, 1))
              enddate = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 999999u }

        match Decode.get_card_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardAtIndexResponse() =
        let packet = TestResponses.get_card_at_index

        let expected: GetCardAtIndexResponse =
            { controller = 405419896u
              card = 10058400u
              startdate = Nullable(DateOnly(2024, 1, 1))
              enddate = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 999999u }

        match Decode.get_card_at_index_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodePutCardResponse() =
        let packet = TestResponses.put_card

        let expected: PutCardResponse = { controller = 405419896u; ok = true }

        match Decode.put_card_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeDeleteCardResponse() =
        let packet = TestResponses.delete_card

        let expected: DeleteCardResponse = { controller = 405419896u; ok = true }

        match Decode.delete_card_response packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)
