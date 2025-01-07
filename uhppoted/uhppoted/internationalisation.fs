namespace uhppoted

open System
open System.Resources
open System.Threading
open System.Globalization
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal internationalisation =
    let translate (e: string) : string =
        let rm =
            ResourceManager("uhppoted.strings", System.Reflection.Assembly.GetExecutingAssembly())

        rm.GetString(e)

    /// <summary>
    /// Translates an event type into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="event">Event type.</param>
    /// <returns>
    /// Human readable event type string or "unknown (<code>)".
    /// </returns>
    let TranslateEventType (event: uint8) =
        let message = translate ($"event.type.{event}")
        let unknown = translate ("event.type.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown}"
        else
            $"#{event}"

    /// <summary>
    /// Translates an event reason code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="reason">Event reason code.</param>
    /// <returns>
    /// Human readable reason string or "unknown (<code>)".
    /// </returns>
    let TranslateEventReason (reason: uint8) =
        let message = translate ($"event.reason.{reason}")
        let unknown = translate ("event.reason.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown}"
        else
            $"#{reason}"

    /// <summary>
    /// Translates an event door direction code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="direction">Door direction code.</param>
    /// <returns>
    /// Human readable direction string or "unknown (<direction>)"
    /// </returns>
    let TranslateDoorDirection (direction: uint8) =
        let message = translate ($"door.direction.{direction}")
        let unknown = translate ("door.direction.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({direction})"
        else
            $"#{direction}"

    /// <summary>
    /// Translates a door control mode code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="mode">Door control mode.</param>
    /// <returns>
    /// Human readable door control mode string or "unknown (<code>)".
    /// </returns>
    let TranslateDoorMode (mode: uint8) =
        let message = translate ($"door.mode.{mode}")
        let unknown = translate ("door.mode.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({mode})"
        else
            $"#{mode}"

    /// <summary>
    /// Translates a door interlock code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="interlock">Door interlock code.</param>
    /// <returns>
    /// Human readable door interlock string or "unknown (<code>)".
    /// </returns>
    let TranslateDoorInterlock (interlock: uint8) =
        let message = translate ($"doors.interlock.{interlock}")
        let unknown = translate ("doors.interlock.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({interlock})"
        else
            $"#{interlock}"

    /// <summary>
    /// Translates a task code into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="task">Task code.</param>
    /// <returns>
    /// Human readable task description string or "unknown (<code>)".
    /// </returns>
    let TranslateTaskCode (task: uint8) =
        let message = translate ($"task.{task}")
        let unknown = translate ("task.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({task})"
        else
            $"#{task}"

    /// <summary>
    /// Translates a relay open/closed state into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="relay">Relay state.</param>
    /// <returns>
    /// Human readable relay state or "unknown (<code>)".
    /// </returns>
    let TranslateRelayState (relay: uint8) =
        let message = translate ($"relay.{relay}")
        let unknown = translate ("relay.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({relay})"
        else
            $"#{relay}"

    /// <summary>
    /// Translates an input on/off state into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="input">Input state.</param>
    /// <returns>
    /// Human readable input state or "unknown (<code>)".
    /// </returns>
    let TranslateInputState (input: uint8) =
        let message = translate ($"input.{input}")
        let unknown = translate ("input.unknown")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown} ({input})"
        else
            $"#{input}"

    /// <summary>
    /// Translates an error into a human readable string using the active 'CurrentCulture' setting.
    /// (e.g. Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US"))
    /// </summary>
    /// <param name="err">Error value.</param>
    /// <returns>
    /// Human readable error string or the type".
    /// </returns>
    let TranslateError (err: Err) =
        let unknown = translate ("error.unknown")

        let message =
            match err with
            | Timeout -> translate ("error.timeout")
            | ReceiveError err -> String.Format(translate ("error.receive"), err)
            | ListenError err -> String.Format(translate ("error.listen"), err)
            | PacketError err -> String.Format(translate ("error.packet"), err)
            | InvalidPacket -> translate ("error.invalid-packet")
            | InvalidResponse -> translate ("error.invalid-response")
            | InvalidControllerType err -> String.Format(translate ("error.invalid-controller-type"), err)
            | CardNotFound -> translate ("error.card-not-found")

        if not <| String.IsNullOrEmpty(message) then
            message
        else if not <| String.IsNullOrEmpty(unknown) then
            $"{unknown}"
        else
            $"{err}"
