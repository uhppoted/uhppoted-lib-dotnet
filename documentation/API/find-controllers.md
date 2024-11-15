## **`FindControllers`**

'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### Parameters
- `timeout` (`int`): The timeout duration in milliseconds for the UDP request. If the response takes longer than this value, it will be discarded.
- `options` (`Options`): A configuration object containing the following fields:
  - `broadcast` (`IPAddress`): The target IP address for broadcasting the request.
  - `debug` (`bool`): A flag to indicate whether to log debug information during the request/response process.

### Returns
Array of `Controller` structs that represent the valid decoded responses. Each item includes data about an access controller, 
including its address, MAC, version, and other relevant details.

### Examples
```fsharp
let timeout = 5000
let options = { 
    bind = IPEndPoint.Parse(IPAddress.Any, 0)
    broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
    listen = IPEndPoint(IPAddress.Any, 60001)
    destination = None
    protocol = None
    debug = true }

match find_controllers(timeout, options) with
| Ok controllers -> controllers |> Array.iter (fun controller -> printfn "controller: %u, version: %s" controller.controller controller.version
| Error err -> printfn "find-controlelrs %A" err
)
```

```csharp
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = FindControllers(timeout, options);

if (result.IsOk)
{
  var controllers = result.ResultValue;
  foreach (var controller in controllers)
  {
    Console.WriteLine($"controller: {controller.controller}, version: {controller.version}");
  }
}
else if (result.IsError)
{
  throw new Exception(result.ErrorValue);
}
```

```vb
Dim timeout As Integer = 3000
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = get_all_controllers(timeout, options)

If (result.IsOk)
    Dim controllers = result.ResultValue
    For Each controller In controllers
        Console.WriteLine($"controller: {controller.controller}, version: {controller.version}")
    Next
Else If (result.IsError)
    Throw New Exception(result.ErrorValue)
End If
```

### Errors
- If the UDP request fails or times out, the response will be excluded from the results.
- Invalid responses that cannot be decoded into `GetControllerResponse` records are discarded.

### Notes
- The `Controller` struct defines the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `address` (`IPAddress option`): The IP address of the controller, or `None` if not available.
  - `netmask` (`IPAddress`): The netmask associated with the controller.
  - `gateway` (`IPAddress`): The gateway associated with the controller.
  - `MAC` (`PhysicalAddress`): The MAC address of the controller.
  - `version` (`string`): The version of the controller firmware.
  - `date` (`Nullable<DateOnly>`): The date associated with the controller, if available.
