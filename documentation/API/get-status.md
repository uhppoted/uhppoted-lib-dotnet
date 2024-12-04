## **`GetStatus`**

Retrieves a controller status record (and most recent event, if any).

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.


### Returns
Returns `Ok` with the controller status record if the request was processed, or `Error`.

A `Status` record includes the following fields:
- `Door1Open` (`bool`): `true`if the door 1 open contact is set.
- `Door2Open` (`bool`): `true`if the door 2 open contact is set.
- `Door3Open` (`bool`): `true`if the door 3 open contact is set.
- `Door4Open` (`bool`): `true`if the door 4 open contact is set.
- `Button1Pressed` (`bool`): `true`if the door 1 button is pressed.
- `Button2Pressed` (`bool`): `true`if the door 2 button is pressed.
- `Button3Pressed` (`bool`): `true`if the door 3 button is pressed.
- `Button4Pressed` (`bool`): `true`if the door 4 button is pressed.
- `SystemError` (`uint8`): System error code (0 for none).
- `SystemDateTime` (`Nullable<DateOnly>`): Current controller date/time.
- `SequenceNumber` (`uint32`): Message sequence number.
- `SpecialInfo` (`uint8`): Special info code.
- `Relays` (`uint8`) Bitmask for door relays 1 to 4.
- `Inputs` (`uint8`) Bitmask for door inputs 1 to 4.
- `EventIndex` (`uint32`) Index of most recent event (0 if none).
- `EventType (`uint8`) Event type.
- `EventAccessGranted (`bool`) Event access granted.
- `EventDoor (`uint8`) Event door.
- `EventDirection (`uint8`) Event direction (IN/OUT).
- `EventCard (`uint32`) Event card (0 if none).
- `EventTimestamp (`DateTime Nullable`) Event timestamp.
- `EventReason (`uint8`) Event reason code.


### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }
let result = GetStatus controller timeout options
match result with
| Ok response -> printfn "get-status: ok %A" response
| Error e -> printfn "get-status: error %A" e
```

```csharp
var controller = 405419896u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetStatus(controller, timeout, options);
if (result.IsOk)
{
    Console.WriteLine("get-status: ok {0}", result.Value);
}
else
{
    Console.WriteLine("get-status: error {0}", result.Error);
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetStatus(controller, timeout, options)
If result.IsOk Then
    Console.WriteLine("get-status: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-status: error {0}", result.Error)
End If
```

### Notes
