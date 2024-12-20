## **`FindControllers`**

'Discovers' all controllers accessible via a UDP broadcast on the local LAN. The returned list of 
controllers includes all controllers that responded within the `timeout` value set in the options.


### Parameters
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Array of `Controller` structs that represent the valid decoded responses. Each item includes data about an access controller, 
including its address, MAC, version, and other relevant details.

### Examples
#### F#
```fsharp
let options = { 
    bind = IPEndPoint.Parse(IPAddress.Any, 0)
    broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
    listen = IPEndPoint(IPAddress.Any, 60001)
    destination = None
    protocol = None
    debug = true }

match find_controllers(options) with
| Ok controllers -> controllers |> Array.iter (fun controller -> printfn "controller: %u, version: %s" controller.controller controller.version
| Error err -> printfn "find-controlelrs %A" err
)
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = FindControllers(options);

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

#### VB.NET
```vb
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = FindControllers(options)

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
