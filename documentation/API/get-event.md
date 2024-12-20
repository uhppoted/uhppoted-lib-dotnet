## `GetEvent`

Retrieves the event record (if any) at the index from the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `Event` record if the request was processed or an `Error` 

The `Ok` value is:
- An `Event` record if an event was found at the index.
- `null` if there was no record at the index.
- `null` if the record at the index was deleted.

The `Event` record has the following fields:
  - `Timestamp` (`DateTime`): Timestamp of event.
  - `Index` (`uint32`): Event index.
  - `Event` (`uint8`): Event type.
  - `AccessGranted` (`bool`): `true` if access to the door was granted.
  - `Door` (`uint8`): Door [1.4] for event.
  - `Direction` (`Direction`): `In` or `Out`.
  - `Card` (`uint32`): Card number.
  - `Reason` (`uint8`): Reason code for access granted/denied.


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
| Ok response when response.HasValue -> printfn "get-event: ok %A" response.Value
| Ok _ -> printfn "get-event: not found"
| Error err -> printfn "get-event: error %A" err

match GetEvent controller index options with
| Ok response when response.HasValue -> printfn "get-event: ok %A" response.Value
| Ok _ -> printfn "get-event: not found"
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
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-event: error 'not found'");
}
else
{
    Console.WriteLine($"get-event: error '{result.ErrorValue}'");
}

var result = GetEvent(controller, index, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-event: error 'not found'");
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
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-event: error 'not found'")
Else
    Console.WriteLine($"get-event: error '{result.ErrorValue}'")
End If

Dim result = GetEvent(controller, index, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-event: error 'not found'")
Else
    Console.WriteLine($"get-event: error '{result.ErrorValue}'")
End If
```

### Notes
