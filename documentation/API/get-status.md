## **`GetStatus`**

Retrieves a controller status record (and most recent event, if any).

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
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
- `Relays.Door1` (`Relay`): door 1 relay state (`Active` or `Inactive`).
- `Relays.Door2` (`Relay`): door 2 relay state (`Active` or `Inactive`).
- `Relays.Door3` (`Relay`): door 3 relay state (`Active` or `Inactive`).
- `Relays.Door4` (`Relay`): door 4 relay state (`Active` or `Inactive`).
- `Inputs.LockForced` (`Input`): input 1 contact state (`Set` or `Clear`).
- `Inputs.FireAlarm` (`Input`): input 2 contact state (`Set` or `Clear`).

An `Event` record includes the following fields:
- `Index` (`uint32`) Index of most recent event (0 if none).
- `Event` (`uint8`) Event type.
- `AccessGranted` (`bool`) Event access granted.
- `Door` (`uint8`) Event door.
- `Direction` (`uint8`) Event direction (`In` or `Out`).
- `Card` (`uint32`) Event card (0 if none).
- `Timestamp` (`DateTime Nullable`) Event timestamp.
- `Reason` (`uint8`) Event reason code.


### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match GetStatus 405419896u options with
| Ok response -> printfn "get-status: ok %A" response
| Error e -> printfn "get-status: error %A" e

match GetStatus controller options with
| Ok response -> printfn "get-status: ok %A" response
| Error e -> printfn "get-status: error %A" e
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetStatus(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine("get-status: ok {0}", result.Value);
}
else
{
    Console.WriteLine("get-status: error {0}", result.Error);
}

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

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetStatus(405419896UI, options)
If result.IsOk Then
    Console.WriteLine("get-status: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-status: error {0}", result.Error)
End If

Dim result = GetStatus(controller, options)
If result.IsOk Then
    Console.WriteLine("get-status: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-status: error {0}", result.Error)
End If
```
