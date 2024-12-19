## `GetDoor`

Retrieves a door control mode and unlock delay from a controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`door` (`uint8`)**: Door ID [1.4].
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `Door` record if the request was processed or an `Error` 

The `Ok` value is:
- An `Door` record if the controller returned a matching response.
- `null` if there was no door matching the request door ID.

The `Door` record has the following fields:
  - `mode` (`DoorMode`): Door control mode (NormallyOpen, NormallyClosed, Controlled).
  - `delay` (`uint8`): Duration (seconds) for which the door remains unlocked after access is granted.
  - `event_type` (`uint8`): Event type.


### Examples

```fsharp
let door = 3uy
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetDoor 405419896u door options with
| Ok response when response.HasValue -> printfn "get-door: ok %A" response.Value
| Ok _ -> printfn "get-door: not found"
| Error err -> printfn "get-door: error %A" err

match GetDoor controller door options with
| Ok response when response.HasValue -> printfn "get-door: ok %A" response.Value
| Ok _ -> printfn "get-door: not found"
| Error err -> printfn "get-door: error %A" err
```

```csharp
var door = 3u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetDoor(405419896u, door, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-door: error 'not found'");
}
else
{
    Console.WriteLine($"get-door: error '{result.ErrorValue}'");
}

var result = GetDoor(controller, door, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-door: error 'not found'");
}
else
{
    Console.WriteLine($"get-door: error '{result.ErrorValue}'");
}
```

```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim door = 3
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetDoor(405419896UI, door, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-door: error 'not found'")
Else
    Console.WriteLine($"get-door: error '{result.ErrorValue}'")
End If

Dim result = GetDoor(controller, door, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-door: error 'not found'")
Else
    Console.WriteLine($"get-door: error '{result.ErrorValue}'")
End If
```

### Notes
