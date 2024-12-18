## **`GetStatus`**

Retrieves a controller status record (and most recent event, if any).

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.


### Returns
Returns `Ok` with a (Status,Nuillable<Event>) tuple if the request was processed, or `Error`. The second tuple field is 
`null` if the controller does not have an event.

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
- `Relay1` (`Relay`): Relay 1 state (`Open` or `Closed`).
- `Relay2` (`Relay`): Relay 2 state (`Open` or `Closed`).
- `Relay3` (`Relay`): Relay 3 state (`Open` or `Closed`).
- `Relay4` (`Relay`): Relay 4 state (`Open` or `Closed`).
- `Input1` (`Input`): Input 1 contact state (`Open` or `Closed`).
- `Input2` (`Input`): Input 2 contact state (`Open` or `Closed`).
- `Input3` (`Input`): Input 3 contact state (`Open` or `Closed`).
- `Input4` (`Input`): Input 4 contact state (`Open` or `Closed`).

An `Event` record includes the following fields:
- `Index` (`uint32`) Index of most recent event (0 if none).
- `Event (`uint8`) Event type.
- `AccessGranted (`bool`) Event access granted.
- `Door (`uint8`) Event door.
- `Direction (`uint8`) Event direction (`In` or `Out`).
- `Card (`uint32`) Event card (0 if none).
- `Timestamp (`DateTime Nullable`) Event timestamp.
- `Reason (`uint8`) Event reason code.


### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }
let result = GetStatus controller options
match result with
| Ok response -> printfn "get-status: ok %A" response
| Error e -> printfn "get-status: error %A" e
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetStatus(controller, options);
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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetStatus(controller, options)
If result.IsOk Then
    Console.WriteLine("get-status: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-status: error {0}", result.Error)
End If
```

### Notes
