## `GetListener`

Retrieves the controller event listener endpoint and auto-send interval:
- The listener endpoint is the IPv4 _address:port_ to which events are sent
- The auto-send interval is the interval (in seconds) at which the controller sends the current status and most 
  recent event to the listener. Events are always dispatched as they occur and a zero interval disables auto-send.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `Listener` record if the controller responded.
- `Error` if the request failed

The `Listener` record comprises the following fields:
  - `endpoint` (`IPEndPoint`): The configured controller event listener endpoint.
  - `interval` (`uint8`): Auto-send interval (seconds), `0` if auto-send is not enabled.

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetListener 405419896u options with
| Ok record -> printfn "get-listener: ok %A" record
| Error err -> printfn "get-listener: error %A" err

match GetListener controller options with
| Ok record -> printfn "get-listener: ok %A" record
| Error err -> printfn "get-listener: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetListener(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"get-listener: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-listener: error {result.ErrorValue}");
}

var result = GetListener(controller, options);
if (result.IsOk)
{
    Console.WriteLine($"get-listener: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-listener: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetListener(405419896UI, options)
If result.IsOk Then
    Console.WriteLine($"get-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-listener: error {result.ErrorValue}")
End If

Dim result = GetListener(controller, options)
If result.IsOk Then
    Console.WriteLine($"get-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-listener: error {result.ErrorValue}")
End If
```

### Notes

