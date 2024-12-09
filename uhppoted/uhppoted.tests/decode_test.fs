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
        let packet = TestResponses.getController

        let expected =
            { controller = 405419896u
              address = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x64uy |])
              netmask = IPAddress([| 0xffuy; 0xffuy; 0xffuy; 0x00uy |])
              gateway = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x01uy |])
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Nullable(DateOnly(2018, 8, 16)) }

        match Decode.getControllerResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetControllerResponseWithInvalidDate() =
        let packet = TestResponses.getControllerWithInvalidDate

        let expected =
            { controller = 405419896u
              address = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x64uy |])
              netmask = IPAddress([| 0xffuy; 0xffuy; 0xffuy; 0x00uy |])
              gateway = IPAddress([| 0xc0uy; 0xa8uy; 0x01uy; 0x01uy |])
              MAC = PhysicalAddress([| 0x00uy; 0x66uy; 0x19uy; 0x39uy; 0x55uy; 0x2duy |])
              version = "v8.92"
              date = Nullable() }

        match Decode.getControllerResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetListenerResponse() =
        let packet = TestResponses.getListener

        let expected =
            { controller = 405419896u
              endpoint = IPEndPoint.Parse("192.168.1.100:60001")
              interval = 13uy }

        match Decode.getListenerResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetListenerResponse() =
        let packet = TestResponses.setListener

        let expected: SetListenerResponse = { controller = 405419896u; ok = true }

        match Decode.setListenerResponse packet with
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
        let packet = TestResponses.openDoor

        let expected: OpenDoorResponse = { controller = 405419896u; ok = true }

        match Decode.openDoorResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetStatusResponse() =
        let packet = TestResponses.getStatus

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

        match Decode.getStatusResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardsResponse() =
        let packet = TestResponses.getCards

        let expected: GetCardsResponse =
            { controller = 405419896u
              cards = 13579u }

        match Decode.getCardsResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardResponse() =
        let packet = TestResponses.getCard

        let expected: GetCardResponse =
            { controller = 405419896u
              card = 10058400u
              start_date = Nullable(DateOnly(2024, 1, 1))
              end_date = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 999999u }

        match Decode.getCardResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetCardAtIndexResponse() =
        let packet = TestResponses.getCardAtIndex

        let expected: GetCardAtIndexResponse =
            { controller = 405419896u
              card = 10058400u
              start_date = Nullable(DateOnly(2024, 1, 1))
              end_date = Nullable(DateOnly(2024, 12, 31))
              door1 = 1uy
              door2 = 0uy
              door3 = 17uy
              door4 = 1uy
              PIN = 999999u }

        match Decode.getCardAtIndexResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodePutCardResponse() =
        let packet = TestResponses.putCard

        let expected: PutCardResponse = { controller = 405419896u; ok = true }

        match Decode.putCardResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeDeleteCardResponse() =
        let packet = TestResponses.deleteCard

        let expected: DeleteCardResponse = { controller = 405419896u; ok = true }

        match Decode.deleteCardResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeDeleteAllCardsResponse() =
        let packet = TestResponses.deleteAllCards

        let expected: DeleteAllCardsResponse = { controller = 405419896u; ok = true }

        match Decode.deleteAllCardsResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetEventResponse() =
        let packet = TestResponses.getEvent

        let expected: GetEventResponse =
            { controller = 405419896u
              index = 13579u
              timestamp = Nullable(DateTime.ParseExact("2024-11-17 12:34:56", "yyyy-MM-dd HH:mm:ss", null))
              event = 2uy
              granted = true
              door = 4uy
              direction = 2uy
              card = 10058400u
              reason = 21uy }

        match Decode.getEventResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetEventIndexResponse() =
        let packet = TestResponses.getEventIndex

        let expected: GetEventIndexResponse =
            { controller = 405419896u
              index = 13579u }

        match Decode.getEventIndexResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetEventIndexResponse() =
        let packet = TestResponses.setEventIndex

        let expected: SetEventIndexResponse = { controller = 405419896u; ok = true }

        match Decode.setEventIndexResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeRecordSpecialEventsResponse() =
        let packet = TestResponses.recordSpecialEvents

        let expected: RecordSpecialEventsResponse = { controller = 405419896u; ok = true }

        match Decode.recordSpecialEventsResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeGetTimeProfileResponse() =
        let packet = TestResponses.getTimeProfile

        let expected: GetTimeProfileResponse =
            { controller = 405419896u
              profile = 37uy
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

        match Decode.getTimeProfileResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetTimeProfileResponse() =
        let packet = TestResponses.setTimeProfile

        let expected: SetTimeProfileResponse = { controller = 405419896u; ok = true }

        match Decode.setTimeProfileResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeClearTimeProfilesResponse() =
        let packet = TestResponses.clearTimeProfiles

        let expected: ClearTimeProfilesResponse = { controller = 405419896u; ok = true }

        match Decode.clearTimeProfilesResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeAddTaskResponse() =
        let packet = TestResponses.addTask

        let expected: AddTaskResponse = { controller = 405419896u; ok = true }

        match Decode.addTaskResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeClearTasklistResponse() =
        let packet = TestResponses.clearTaskList

        let expected: ClearTaskListResponse = { controller = 405419896u; ok = true }

        match Decode.clearTaskListResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeRefreshTasklistResponse() =
        let packet = TestResponses.refreshTaskList

        let expected: RefreshTaskListResponse = { controller = 405419896u; ok = true }

        match Decode.refreshTaskListResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetPCControlResponse() =
        let packet = TestResponses.setPCControl

        let expected: SetPCControlResponse = { controller = 405419896u; ok = true }

        match Decode.setPCControlResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeSetInterlockResponse() =
        let packet = TestResponses.setInterlock

        let expected: SetInterlockResponse = { controller = 405419896u; ok = true }

        match Decode.setInterlockResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeActivateKeypadsResponse() =
        let packet = TestResponses.activateKeypads

        let expected: ActivateKeypadsResponse = { controller = 405419896u; ok = true }

        match Decode.activateKeypadsResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)

    [<Test>]
    member this.TestDecodeRestoreDefaultParametersResponse() =
        let packet = TestResponses.restoreDefaultParameters

        let expected: RestoreDefaultParametersResponse =
            { controller = 405419896u; ok = true }

        match Decode.restoreDefaultParametersResponse packet with
        | Ok response -> Assert.That(response, Is.EqualTo(expected))
        | Error err -> Assert.Fail(err)
