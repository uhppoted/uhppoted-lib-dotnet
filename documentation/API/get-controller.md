## `GetController`

Retrieves the IPv4 configuration, MAC address and version information for an access controller.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `Controller` record if the controller responded.
- `Error` if the request failed

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match GetController controller timeout options with
| Ok record -> printfn "get-controller: ok %A" record
| Error err -> printfn "get-controller: error %A" err
```

```csharp
var controller = 405419896u;
var timeout = 5000
var options = new OptionsBuilder().build();
var result = GetController(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"get-controller: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-controller: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetController(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"get-controller: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-controller: error {result.ErrorValue}")
End If
```

### Notes

- The `Controller` record defines the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `address` (`IPAddress option`): The IP address of the controller, or `None` if not available.
  - `netmask` (`IPAddress`): The netmask associated with the controller.
  - `gateway` (`IPAddress`): The gateway associated with the controller.
  - `MAC` (`PhysicalAddress`): The MAC address of the controller.
  - `version` (`string`): The version of the controller firmware.
  - `date` (`Nullable<DateOnly>`): The date associated with the controller, if available.
