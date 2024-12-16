## `GetListener`

Retrieves the controller event listener endpoint and auto-send interval:
- The listener endpoint is the IPv4 _address:port_ to which events are sent
- The auto-send interval is the interval (in seconds) at which the controller sends the current status and most 
  recent event to the listener. Events are always dispatched as they occur and a zero interval disables auto-send.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `Listener` record if the controller responded.
- `Error` if the request failed

The `Listener` record comprises the following fields:
  - `endpoint` (`IPEndPoint`): The configured controller event listener endpoint.
  - `interval` (`uint8`): Auto-send interval (seconds), `0` if auto-send is not enabled.

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetListener controller options with
| Ok record -> printfn "get-listener: ok %A" record
| Error err -> printfn "get-listener: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
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

```vb
Dim controller = 405419896
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetListener(controller, options)

If result.IsOk Then
    Console.WriteLine($"get-listener: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-listener: error {result.ErrorValue}")
End If
```

### Notes

