## `Listen`

Listens for access controller events.

### Parameters
- **`onevent` (`OnEvent`)**: Handler for received events.
- **`onerror` (`OnError`)**: Handler for non-fatal errors.
- **`stop` (`CancellationToken`)**: Cancellation token to stop listening.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

`OnEvent` is a defined as a delegate that accepts a `ListenEvent` struct:
  - `Controller` (`uint32`): Originating access controller.
  - `Status` (`Status`): Controller status record.
  - `Event` (`Event`): Event record - may be null in the rare case if the controller does not have an event
                       (e.g. if the listener interval is non-zero and the controller is fresh out of the box).

`OnError` is a defined as a delegate that accepts a string.

### Returns

Returns:
- `Ok`.
- `Error` if the 'listen' function failed on setup.

### Examples

#### F#
```fsharp
    let cancel = new CancellationTokenSource()

    Console.CancelKeyPress.Add(fun args ->
        args.Cancel <- true
        cancel.Cancel())

    let eventHandler evt =
        printfn "-- EVENT"
        printfn "   controller %u" evt.Controller
        printfn "       status %A" evt.Status
        printfn "       status %A" evt.Eevent

    let errorHandler err =
        printfn "** ERROR %A" err

    let onevent: OnEvent = new OnEvent(eventHandler)
    let onerror: OnError = new OnError(errorHandler)

    Uhppoted.Listen(onevent, onerror, cancel.Token, options)
```

#### C#
```csharp
    var cancel = new CancellationTokenSource();

    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        cancel.Cancel();
    };

    void onEvent(uhppoted.ListenerEvent e)
    {
        WriteLine("-- EVENT");
        WriteLine($"         controller {e.Controller}");
        WriteLine($"             status {e.Status}");
        WriteLine($"             status {e.Event}");
    }

    var onevent = new uhppoted.OnEvent(onEvent);
    var onerror = new uhppoted.OnError(onError);

    Uhppoted.Listen(onevent, onerror, cancel.Token, options);
```

#### VB.NET
```vb
    Sub Listen(args As String())
        Dim cancel = New CancellationTokenSource()

        AddHandler Console.CancelKeyPress, Sub(sender, e)
                                               e.Cancel = True
                                               cancel.Cancel()
                                           End Sub

        Dim onevent As New uhppoted.OnEvent(AddressOf eventHandler)
        Dim onerror As New uhppoted.OnError(AddressOf errorHandler)

        UHPPOTE.Listen(onevent, onerror, cancel.Token, OPTIONS)
    End Sub

    Private Sub eventHandler(e As uhppoted.ListenerEvent)
        WriteLine("-- EVENT")
        WriteLine($"   controller {e.Controller}")
        WriteLine($"       status {e.Status}")
        WriteLine($"        event {e.Event}")
    End Sub

    Private Sub errorHandler(err)
        WriteLine("** ERROR {err}")
    End Sub

```
