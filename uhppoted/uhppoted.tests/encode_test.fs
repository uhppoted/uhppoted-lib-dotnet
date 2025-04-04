namespace Uhppoted.Tests

open System
open System.Net
open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestEncoder() =
    [<Test>]
    member this.TestEncodeGetControllerRequestWithZeroControllerID() =
        let expected = TestRequests.getAllControllers
        let packet = Encode.getControllerRequest 0u

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetControllerRequestWithValidControllerID() =
        let expected = TestRequests.getController

        let controller = 405419896u
        let packet = Encode.getControllerRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetIPv4Request() =
        let expected = TestRequests.setIPv4

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let netmask = IPAddress.Parse("255.255.255.0")
        let gateway = IPAddress.Parse("192.168.1.1")
        let packet = Encode.setIPv4Request controller address netmask gateway

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetListenerRequest() =
        let expected = TestRequests.getListener

        let controller = 405419896u
        let packet = Encode.getListenerRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetListenerRequest() =
        let expected = TestRequests.setListener

        let controller = 405419896u
        let address = IPAddress.Parse("192.168.1.100")
        let port = 60001us
        let interval = 17uy
        let packet = Encode.setListenerRequest controller address port interval

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetTimeRequest() =
        let expected = TestRequests.getTime

        let controller = 405419896u
        let packet = Encode.getTimeRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetTimeRequest() =
        let expected = TestRequests.setTime

        let controller = 405419896u

        let datetime =
            DateTime.ParseExact("2024-11-04 12:34:56", "yyyy-MM-dd HH:mm:ss", null)

        let packet = Encode.setTimeRequest controller datetime

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetDoorRequest() =
        let expected = TestRequests.getDoor

        let controller = 405419896u
        let door = 3uy
        let packet = Encode.getDoorRequest controller door

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorRequest() =
        let expected = TestRequests.setDoor

        let controller = 405419896u
        let door = 3uy
        let mode = DoorMode.NormallyClosed
        let delay = 17uy
        let packet = Encode.setDoorRequest controller door mode delay

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorPasscodesRequest() =
        let expected = TestRequests.setDoorPasscodes

        let controller = 405419896u
        let door = 3uy
        let passcodes = [| 123456u; 234567u; 345678u; 456789u |]

        let packet =
            Encode.setDoorPasscodesRequest controller door passcodes[0] passcodes[1] passcodes[2] passcodes[3]

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetDoorPasscodesRequestWithInvalidPasscode() =
        let expected = TestRequests.setDoorPasscodesWithInvalidPasscode

        let controller = 405419896u
        let door = 3uy
        let passcodes = [| 123456u; 1234567u; 345678u; 456789u |]

        let packet =
            Encode.setDoorPasscodesRequest controller door passcodes[0] passcodes[1] passcodes[2] passcodes[3]

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeOpenDoorRequest() =
        let expected = TestRequests.openDoor

        let controller = 405419896u
        let door = 3uy

        let packet = Encode.openDoorRequest controller door

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetStatusRequest() =
        let expected = TestRequests.getStatus

        let controller = 405419896u
        let packet = Encode.getStatusRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardsRequest() =
        let expected = TestRequests.getCards

        let controller = 405419896u
        let packet = Encode.getCardsRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardRequest() =
        let expected = TestRequests.getCard

        let controller = 405419896u
        let card = 10058400u
        let packet = Encode.getCardRequest controller card

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetCardAtIndexRequest() =
        let expected = TestRequests.getCardAtIndex

        let controller = 405419896u
        let index = 135u
        let packet = Encode.getCardAtIndexRequest controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodePutCardRequest() =
        let expected = TestRequests.putCard

        let controller = 405419896u

        let card: Card =
            { Card = 10058400u
              StartDate = Nullable(DateOnly(2024, 1, 1))
              EndDate = Nullable(DateOnly(2024, 12, 31))
              Door1 = 1uy
              Door2 = 0uy
              Door3 = 17uy
              Door4 = 1uy
              PIN = 999999u }

        let packet = Encode.putCardRequest controller card

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeDeleteCardRequest() =
        let expected = TestRequests.deleteCard

        let controller = 405419896u
        let card = 10058400u
        let packet = Encode.deleteCardRequest controller card

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeDeleteAllCardsRequest() =
        let expected = TestRequests.deleteAllCards

        let controller = 405419896u
        let packet = Encode.deleteAllCardsRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetEventRequest() =
        let expected = TestRequests.getEvent

        let controller = 405419896u
        let index = 13579u
        let packet = Encode.getEventRequest controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetEventIndexRequest() =
        let expected = TestRequests.getEventIndex

        let controller = 405419896u
        let packet = Encode.getEventIndexRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetEventIndexRequest() =
        let expected = TestRequests.setEventIndex

        let controller = 405419896u
        let index = 13579u
        let packet = Encode.setEventIndexRequest controller index

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestRecordSpecialEventsRequest() =
        let expected = TestRequests.recordSpecialEvents

        let controller = 405419896u
        let enabled = true
        let packet = Encode.recordSpecialEventsRequest controller enabled

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestGetTimeProfileRequest() =
        let expected = TestRequests.getTimeProfile

        let controller = 405419896u
        let profile = 37uy
        let packet = Encode.getTimeProfileRequest controller profile

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestSetTimeProfileRequest() =
        let expected = TestRequests.setTimeProfile

        let controller = 405419896u

        let profile: TimeProfile =
            { Profile = 37uy
              StartDate = Nullable(DateOnly(2024, 11, 26))
              EndDate = Nullable(DateOnly(2024, 12, 29))
              Monday = true
              Tuesday = true
              Wednesday = false
              Thursday = true
              Friday = false
              Saturday = true
              Sunday = true
              Segment1Start = Nullable(TimeOnly(8, 30))
              Segment1End = Nullable(TimeOnly(9, 45))
              Segment2Start = Nullable(TimeOnly(11, 35))
              Segment2End = Nullable(TimeOnly(13, 15))
              Segment3Start = Nullable(TimeOnly(14, 01))
              Segment3End = Nullable(TimeOnly(17, 59))
              LinkedProfile = 19uy }

        let packet = Encode.setTimeProfileRequest controller profile

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestClearTimeProfilesRequest() =
        let expected = TestRequests.clearTimeProfiles

        let controller = 405419896u
        let packet = Encode.clearTimeProfilesRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestAddTaskRequest() =
        let expected = TestRequests.addTask

        let controller = 405419896u

        let task: Task =
            { Task = TaskCode.DisableTimeProfiles
              Door = 3uy
              StartDate = Nullable(DateOnly(2024, 11, 26))
              EndDate = Nullable(DateOnly(2024, 12, 29))
              Monday = true
              Tuesday = true
              Wednesday = false
              Thursday = true
              Friday = false
              Saturday = true
              Sunday = true
              StartTime = Nullable(TimeOnly(8, 45))
              MoreCards = 7uy }

        let packet = Encode.addTaskRequest controller task

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestClearTasklistRequest() =
        let expected = TestRequests.clearTaskList

        let controller = 405419896u
        let packet = Encode.clearTaskListRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestRefreshTasklistRequest() =
        let expected = TestRequests.refreshTaskList

        let controller = 405419896u
        let packet = Encode.refreshTaskListRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestSetPCControlRequest() =
        let expected = TestRequests.setPCControl

        let controller = 405419896u
        let enable = true
        let packet = Encode.setPCControlRequest controller true

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestSetInterlockRequest() =
        let expected = TestRequests.setInterlock

        let controller = 405419896u
        let interlock = Interlock.Doors1234
        let packet = Encode.setInterlockRequest controller interlock

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestActivateKeypadsRequest() =
        let expected = TestRequests.activateKeypads

        let controller = 405419896u
        let packet = Encode.activateKeypadsRequest controller true true false true

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeSetAntiPassback() =
        let expected = TestRequests.setAntiPassback

        let controller = 405419896u
        let antipassback = AntiPassback.Doors13_24
        let packet = Encode.setAntiPassbackRequest controller antipassback

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestEncodeGetAntiPassback() =
        let expected = TestRequests.getAntiPassback

        let controller = 405419896u
        let packet = Encode.getAntiPassbackRequest controller

        Assert.That(packet, Is.EqualTo(expected))

    [<Test>]
    member this.TestRestoreDefaultParametersRequest() =
        let expected = TestRequests.restoreDefaultParameters

        let controller = 405419896u
        let packet = Encode.restoreDefaultParametersRequest controller

        Assert.That(packet, Is.EqualTo(expected))
