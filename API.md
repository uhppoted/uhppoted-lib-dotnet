# API

- [`get_all_controllers`](#get_all_controllers)


### **`get_all_controllers`**

**Description**
This function sends a broadcast request to a network and processes the received UDP responses. For each valid response, it decodes the controller data and returns it as an array of `GetControllerResponse` records.

**Parameters**
- `timeout` (`int`): The timeout duration in milliseconds for the UDP request. If the response takes longer than this value, it will be discarded.
- `options` (`Options`): A configuration object containing the following fields:
  - `broadcast` (`IPAddress`): The target IP address for broadcasting the request.
  - `debug` (`bool`): A flag to indicate whether to log debug information during the request/response process.

**Returns**
`GetControllerResponse array`: An array of `GetControllerResponse` records that represent the valid decoded responses. Each record includes data about a network controller, including its address, MAC, version, and other relevant details.

**Example Usage**

```fsharp
let timeout = 5000
let options = { broadcast = IPAddress.Parse("255.255.255.255"); debug = true }

let controllers = get_all_controllers(timeout, options)

controllers |> Array.iter (fun controller ->
    printfn "controller: %u, version: %s" controller.controller controller.version
)
```
```csharp
var timeout = 5000;
var options = new Options { broadcast = IPAddress.Parse("255.255.255.255"), debug = true };
var controllers = get_all_controllers(timeout, options);
foreach (var controller in controllers)
{
    Console.WriteLine($"controller: {controller.controller}, version: {controller.version}");
}
```
```vb
Dim timeout As Integer = 3000
Dim options As New Options With { .broadcast = IPAddress.Parse("255.255.255.255"), .debug = True }
Dim controllers = get_all_controllers(timeout, options)
For Each controller In controllers
    Console.WriteLine($"Controller ID: {controller.controller}, Version: {controller.version}")
Next
```

**Errors**
- If the UDP request fails or times out, the response will be excluded from the results.
- Invalid responses that cannot be decoded into `GetControllerResponse` records are discarded.

**Notes**
- The `GetControllerResponse` record includes the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `address` (`IPAddress option`): The IP address of the controller, or `None` if not available.
  - `netmask` (`IPAddress`): The netmask associated with the controller.
  - `gateway` (`IPAddress`): The gateway associated with the controller.
  - `MAC` (`PhysicalAddress`): The MAC address of the controller.
  - `version` (`string`): The version of the controller firmware.
  - `date` (`Nullable<DateOnly>`): The date associated with the controller, if available.

