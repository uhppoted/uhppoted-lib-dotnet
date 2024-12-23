namespace Uhppoted.Tests

open System
open System.Net
open System.Threading
open System.Globalization

open NUnit.Framework
open uhppoted

[<TestFixture>]
type TestInternationalisation() =
    [<Test>]
    member this.TestTranslateEventType_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let events =
            [ {| event = 0uy
                 expected = "not found" |}
              {| event = 1uy
                 expected = "card swipe" |}
              {| event = 2uy; expected = "door" |}
              {| event = 3uy; expected = "alarm" |}
              {| event = 254uy
                 expected = "unknown (254)" |}
              {| event = 255uy
                 expected = "overwritten" |} ]

        events
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateEventType t.event

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateEventReason_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let reasons =
            [ {| reason = 1uy; expected = "swipe" |}
              {| reason = 2uy
                 expected = "swipe open" |}
              {| reason = 3uy
                 expected = "swipe closed" |}
              {| reason = 5uy
                 expected = "swipe:denied (system)" |}
              {| reason = 6uy
                 expected = "no access rights" |}
              {| reason = 7uy
                 expected = "incorrect password" |}
              {| reason = 8uy
                 expected = "anti-passback" |}
              {| reason = 9uy
                 expected = "more cards" |}
              {| reason = 10uy
                 expected = "first card open" |}
              {| reason = 11uy
                 expected = "door is normally closed" |}
              {| reason = 12uy
                 expected = "interlock" |}
              {| reason = 13uy
                 expected = "not allowed in time period" |}
              {| reason = 15uy
                 expected = "invalid timezone" |}
              {| reason = 18uy
                 expected = "access denied" |}
              {| reason = 20uy
                 expected = "pushbutton ok" |}
              {| reason = 23uy
                 expected = "door opened" |}
              {| reason = 24uy
                 expected = "door closed" |}
              {| reason = 25uy
                 expected = "door opened (supervisor password)" |}
              {| reason = 28uy
                 expected = "controller power on" |}
              {| reason = 29uy
                 expected = "controller reset" |}
              {| reason = 31uy
                 expected = "pushbutton denied (door locked)" |}
              {| reason = 32uy
                 expected = "pushbutton denied (offline)" |}
              {| reason = 33uy
                 expected = "pushbutton denied (interlock)" |}
              {| reason = 34uy
                 expected = "pushbutton denied (threat)" |}
              {| reason = 37uy
                 expected = "door open too long" |}
              {| reason = 38uy
                 expected = "door forced open" |}
              {| reason = 39uy; expected = "fire" |}
              {| reason = 40uy
                 expected = "door forced closed" |}
              {| reason = 41uy
                 expected = "tamper detect" |}
              {| reason = 42uy
                 expected = "24x7 zone" |}
              {| reason = 43uy
                 expected = "emergency" |}
              {| reason = 44uy
                 expected = "remote open door" |}
              {| reason = 45uy
                 expected = "remote open door (USB reader)" |}
              {| reason = 255uy
                 expected = "unknown (255)" |} ]

        reasons
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateEventReason t.reason

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateDoorDirection_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let directions =
            [ {| direction = 0uy
                 expected = "unknown (0)" |}
              {| direction = 1uy; expected = "in" |}
              {| direction = 2uy; expected = "out" |}
              {| direction = 255uy
                 expected = "unknown (255)" |} ]

        directions
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateDoorDirection t.direction

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateDoorMode_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let modes =
            [ {| mode = 0uy
                 expected = "unknown (0)" |}
              {| mode = 1uy
                 expected = "normally open" |}
              {| mode = 2uy
                 expected = "normally closed" |}
              {| mode = 3uy
                 expected = "controlled" |}
              {| mode = 255uy
                 expected = "unknown (255)" |} ]

        modes
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateDoorMode t.mode

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateDoorInterlock_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let interlocks =
            [ {| interlock = 0uy
                 expected = "no interlock" |}
              {| interlock = 1uy
                 expected = "doors 1&2" |}
              {| interlock = 2uy
                 expected = "doors 3&4" |}
              {| interlock = 3uy
                 expected = "doors 1&2 and doors 3&4" |}
              {| interlock = 4uy
                 expected = "doors 1&2&3" |}
              {| interlock = 8uy
                 expected = "doors 1&2&3&4" |}
              {| interlock = 255uy
                 expected = "unknown (255)" |} ]

        interlocks
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateDoorInterlock t.interlock

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateTaskCodes_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let tasks =
            [ {| task = 0uy
                 expected = "unknown (0)" |}
              {| task = 1uy
                 expected = "control door" |}
              {| task = 2uy
                 expected = "unlock door" |}
              {| task = 3uy; expected = "lock door" |}
              {| task = 4uy
                 expected = "disable time profile" |}
              {| task = 5uy
                 expected = "enable time profile" |}
              {| task = 6uy
                 expected = "enable card, no password" |}
              {| task = 7uy
                 expected = "enable card+IN password" |}
              {| task = 8uy
                 expected = "enable card+password" |}
              {| task = 9uy
                 expected = "enable more cards" |}
              {| task = 10uy
                 expected = "disable more cards" |}
              {| task = 11uy
                 expected = "trigger once" |}
              {| task = 12uy
                 expected = "disable pushbutton" |}
              {| task = 13uy
                 expected = "enable pushbutton" |}
              {| task = 255uy
                 expected = "unknown (255)" |} ]

        tasks
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateTaskCode t.task

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateRelayState_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let relays =
            [ {| relay = 0uy
                 expected = "unknown (0)" |}
              {| relay = 1uy; expected = "locked" |}
              {| relay = 2uy; expected = "unlocked" |}
              {| relay = 255uy
                 expected = "unknown (255)" |} ]

        relays
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateRelayState t.relay

            Assert.That(message, Is.EqualTo(t.expected)))

    [<Test>]
    member this.TestTranslateInputState_en_US() =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let inputs =
            [ {| input = 0uy
                 expected = "unknown (0)" |}
              {| input = 1uy; expected = "off" |}
              {| input = 2uy; expected = "on" |}
              {| input = 255uy
                 expected = "unknown (255)" |} ]

        inputs
        |> List.iter (fun t ->
            let message = Uhppoted.TranslateInputState t.input

            Assert.That(message, Is.EqualTo(t.expected)))
