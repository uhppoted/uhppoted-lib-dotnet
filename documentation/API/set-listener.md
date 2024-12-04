## `SetListener`

Sets the controller event listener endpoint and auto-send interval:
- The listener endpoint is the IPv4 _address:port_ to which events are sent
- The auto-send interval is the interval (in seconds) at which the controller sends the current status and most 
  recent event to the listener. Events are always dispatched as they occur and a zero interval disables auto-send.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`listener` (`IPEndPoint`)**: Event listener IPv4 endpoint.
- **`interval` (`uint8`)**: Auto-send interval (seconds).
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with `true` if the controller event listener was set.
- `Error` if the request failed

### Examples

```fsharp
let controller = 405419896u
let listener = IPEndPoint.Parse("192.168.1.100:60001")
let interval = 30uy
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match SetListener controller listener interval timeout options with
| Ok ok -> printfn "set-listener: ok %A" ok
| Error err -> printfn "set-listener: error %A" err
```

```csharp
var controller = 405419896u;
var listener = IPEndPoint.Parse("192.168.1.100:60001");
var interval = 30uy;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = SetListener(controller, listener, interval, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"set-listener: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-listener: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim listener = IPEndPoint.Parse("192.168.1.100:60001")
Dim interval = 30uy
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetListener(controller, listener, interval, timeout, options)

If result.IsOk Then
    Console.WriteLine($"set-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-listener: error {result.ErrorValue}")
End If
```

### Notes

