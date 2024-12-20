## `SetDoor`

Retrieves a door control mode and unlock delay from a controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`door` (`uint8`)**: Door ID [1.4].
- **`mode` (`DoorMode`)**: Door control mode (controlled, normally-open or normally-closed)
- **`delay` (`uint8`)**: Door unlock delay (seconds).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `Door` record if the door was updated or an `Error` 

The `Ok` value is:
- An `Door` record if the controller returned a matching response.
- `null` if there was no door matching the request door ID.

The `Door` record has the following fields:
  - `mode` (`DoorMode`): Door control mode (NormallyOpen, NormallyClosed, Controlled).
  - `delay` (`uint8`): Duration (seconds, [0.255]) for which the door remains unlocked after access is granted.
  - `event_type` (`uint8`): Event type.


### Examples

#### F#
```fsharp
let door = 3uy
let mode = DoorMode.NormallyClosed
let delay = 5
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match SetDoor 405419896u door mode delay options with
| Ok response when response.HasValue -> printfn "set-door: ok %A" response.Value
| Ok _ -> printfn "set-door: not found"

match SetDoor controller door mode delay options with
| Ok response when response.HasValue -> printfn "set-door: ok %A" response.Value
| Ok _ -> printfn "set-door: not found"
| Error err -> printfn "set-door: error %A" err
```

#### C#
```csharp
var door = 3u;
var mode = DoorMode.NormallyClosed;
var delay = 5;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetDoor(405419896u, door, mode, delay, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"set-door: error 'not found'");
}
else
{
    Console.WriteLine($"set-door: error '{result.ErrorValue}'");
}

var result = SetDoor(controller, door, mode, delay, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"set-door: error 'not found'");
}
else
{
    Console.WriteLine($"set-door: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim door = 3
Dim mode = DoorMode.NormallyClosed
Dim delay = 5
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetDoor(405419896UI, door, mode, delay, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"set-door: error 'not found'")
Else
    Console.WriteLine($"set-door: error '{result.ErrorValue}'")
End If

Dim result = SetDoor(controller, door, mode, delay, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"set-door: error 'not found'")
Else
    Console.WriteLine($"set-door: error '{result.ErrorValue}'")
End If
```

### Notes
