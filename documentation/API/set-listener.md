## `SetListener`

Sets the controller event listener endpoint and auto-send interval:
- The listener endpoint is the IPv4 _address:port_ to which events are sent
- The auto-send interval is the interval (in seconds) at which the controller sends the current status and most 
  recent event to the listener. Events are always dispatched as they occur and a zero interval disables auto-send.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`listener` (`IPEndPoint`)**: Event listener IPv4 endpoint.
- **`interval` (`uint8`)**: Auto-send interval (seconds).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with `true` if the controller event listener was set.
- `Error` if the request failed

### Examples

#### F#
```fsharp
let listener = IPEndPoint.Parse("192.168.1.100:60001")
let interval = 30uy
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match SetListener 405419896u listener interval options with
| Ok ok -> printfn "set-listener: ok %A" ok
| Error err -> printfn "set-listener: error %A" err

match SetListener controller listener interval options with
| Ok ok -> printfn "set-listener: ok %A" ok
| Error err -> printfn "set-listener: error %A" err
```

#### C#
```csharp
var listener = IPEndPoint.Parse("192.168.1.100:60001");
var interval = 30uy;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetListener(405419896u, listener, interval, options);
if (result.IsOk)
{
    Console.WriteLine($"set-listener: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-listener: error {result.ErrorValue}");
}

var result = SetListener(controller, listener, interval, options);
if (result.IsOk)
{
    Console.WriteLine($"set-listener: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-listener: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim listener = IPEndPoint.Parse("192.168.1.100:60001")
Dim interval = 30uy
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetListener(405419896UI, listener, interval, options)
If result.IsOk Then
    Console.WriteLine($"set-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-listener: error {result.ErrorValue}")
End If

Dim result = SetListener(controller, listener, interval, options)
If result.IsOk Then
    Console.WriteLine($"set-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-listener: error {result.ErrorValue}")
End If
```
