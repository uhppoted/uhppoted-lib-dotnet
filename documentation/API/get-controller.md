## `GetController`

Retrieves the IPv4 configuration, MAC address and version information for an access controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options`**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `Controller` record if the controller responded.
- `Error` if the request failed

The `Controller` record contains the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `address` (`IPAddress option`): The IP address of the controller, or `None` if not available.
  - `netmask` (`IPAddress`): The netmask associated with the controller.
  - `gateway` (`IPAddress`): The gateway associated with the controller.
  - `MAC` (`PhysicalAddress`): The MAC address of the controller.
  - `version` (`string`): The version of the controller firmware.
  - `date` (`Nullable<DateOnly>`): The date associated with the controller, if available.

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetController 405419896u options with
| Ok record -> printfn "get-controller: ok %A" record
| Error err -> printfn "get-controller: error %A" err

match GetController controller options with
| Ok record -> printfn "get-controller: ok %A" record
| Error err -> printfn "get-controller: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetController(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"get-controller: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-controller: error {result.ErrorValue}");
}

var result = GetController(controller, options);
if (result.IsOk)
{
    Console.WriteLine($"get-controller: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-controller: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetController(405419896UI, options)
If result.IsOk Then
    Console.WriteLine($"get-controller: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-controller: error {result.ErrorValue}")
End If

Dim result = GetController(controller, options)
If result.IsOk Then
    Console.WriteLine($"get-controller: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-controller: error {result.ErrorValue}")
End If
```

### Notes

