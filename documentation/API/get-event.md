## `GetEvent`

Retrieves the event record (if any) at the index from the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with an `Event` record if the event exists or an `Error`:

- an `Event` record has nthe following fields:
  - `Timestamp` (`DateTime`): Timestamp of event.
  - `Index` (`uint32`): Event index.
  - `Event` (`uint8`): Event type.
  - `AccessGranted` (`bool`): `true` if access to the door was granted.
  - `Door` (`uint8`): Door [1.4] for event.
  - `Direction` (`Direction`): `In` or `Out`.
  - `Card` (`uint32`): Card number.
  - `Reason` (`uint8`): Reason code for access granted/denied.

- `Error EventNotFound` if the event at the index does not exist
- `Error EventOverwritten` if the event at the index has been overwritten
- `Error <error>` if the request failed.

### Examples

#### F#
```fsharp
let index = 13579u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetEvent 405419896u index options with
| Ok response -> printfn "get-event: ok %A" response
| Error EventNotFound -> printfn "get-event: not found"
| Error EventOverwritten -> printfn "get-event: overwritten"
| Error err -> printfn "get-event: error %A" err

match GetEvent controller index options with
| Ok response -> printfn "get-event: ok %A" response
| Error EventNotFound -> printfn "get-event: not found"
| Error EventOverwritten -> printfn "get-event: overwritten"
| Error err -> printfn "get-event: error %A" err
```

#### C#
```csharp
var index = 13579u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetEvent(405419896u, index, options);
if (result.IsOk)
{
    Console.WriteLine($"get-event: ok {result.ResultValue}");
}
else if (result.IsError && result.ErrorValue == uhppoted.Err.EventNotFound)
{
    Console.WriteLine($"get-event: error 'not found'");
}
else if (result.IsError && result.ErrorValue == uhppoted.Err.EventOverwritten)
{
    Console.WriteLine($"get-event: error 'overwritten'");
}
else
{
    Console.WriteLine($"get-event: error '{result.ErrorValue}'");
}

var result = GetEvent(controller, index, options);
if (result.IsOk)
{
    Console.WriteLine($"get-event: ok {result.ResultValue}");
}
else if (result.IsError && result.ErrorValue == uhppoted.Err.EventNotFound)
{
    Console.WriteLine($"get-event: error 'not found'");
}
else if (result.IsError && result.ErrorValue == uhppoted.Err.EventOverwritten)
{
    Console.WriteLine($"get-event: error 'overwritten'");
}
else
{
    Console.WriteLine($"get-event: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim index = 13579
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetEvent(405419896UI, index, options)
If (result.IsOk) Then
    Console.WriteLine($"get-event: ok {result.ResultValue}")
Else If (result.IsError And result.ErrorValue is uhppoted.Err.EventNotFound) Then
    Console.WriteLine($"get-event: error 'not found'")
Else If (result.IsError And result.ErrorValue is uhppoted.Err.EventOverwritten) Then
    Console.WriteLine($"get-event: error 'overwritten'")
Else
    Console.WriteLine($"get-event: error '{result.ErrorValue}'")
End If

Dim result = GetEvent(controller, index, options)
If (result.IsOk) Then
    Console.WriteLine($"get-event: ok {result.ResultValue}")
Else If (result.IsError And result.ErrorValue is uhppoted.Err.EventNotFound) Then
    Console.WriteLine($"get-event: error 'not found'")
Else If (result.IsError And result.ErrorValue is uhppoted.Err.EventOverwritten) Then
    Console.WriteLine($"get-event: error 'overwritten'")
Else
    Console.WriteLine($"get-event: error '{result.ErrorValue}'")
End If
```

### Notes
