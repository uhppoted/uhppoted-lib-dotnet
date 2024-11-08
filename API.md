# API

- [`get_all_controllers`](#get_all_controllers)
- [`set_door_passcodes`](#set_door_passcodes)
- [`open_door`](#open_door)


## **`get_all_controllers`**

'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### Parameters
- `timeout` (`int`): The timeout duration in milliseconds for the UDP request. If the response takes longer than this value, it will be discarded.
- `options` (`Options`): A configuration object containing the following fields:
  - `broadcast` (`IPAddress`): The target IP address for broadcasting the request.
  - `debug` (`bool`): A flag to indicate whether to log debug information during the request/response process.

### Returns
`GetControllerResponse array`: An array of `GetControllerResponse` records that represent the valid decoded responses. Each record includes data about a network controller, including its address, MAC, version, and other relevant details.

### Examples
```fsharp
let timeout = 5000
let options = { broadcast = IPAddress.Parse("255.255.255.255"); debug = true }
match get_all_controllers(timeout, options) with
| Ok controllers -> controllers |> Array.iter (fun controller -> printfn "controller: %u, version: %s" controller.controller controller.version
| Error err -> printfn "get-all-controllers %A" err
)
```
```csharp
var timeout = 5000;
var options = new Options { broadcast = IPAddress.Parse("255.255.255.255"), debug = true };
var result = get_all_controllers(timeout, options);
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
Dim options As New Options With { .broadcast = IPAddress.Parse("255.255.255.255"), .debug = True }
Dim result = get_all_controllers(timeout, options)
If (result.IsOk)
    Dim controllers = result.ResultValue
    For Each controller In controllers
        Console.WriteLine($"Controller ID: {controller.controller}, Version: {controller.version}")
    Next
Else If (result.IsError)
    Throw New Exception(result.ErrorValue)
End If
```

### Errors
- If the UDP request fails or times out, the response will be excluded from the results.
- Invalid responses that cannot be decoded into `GetControllerResponse` records are discarded.

### Notes
- The `GetControllerResponse` record includes the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `address` (`IPAddress option`): The IP address of the controller, or `None` if not available.
  - `netmask` (`IPAddress`): The netmask associated with the controller.
  - `gateway` (`IPAddress`): The gateway associated with the controller.
  - `MAC` (`PhysicalAddress`): The MAC address of the controller.
  - `version` (`string`): The version of the controller firmware.
  - `date` (`Nullable<DateOnly>`): The date associated with the controller, if available.


## `set_door_passcodes`

Sets up to 4 passcodes for a controller door.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`door`**: Door number `[1..4]`.
- **`passcode1`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode2`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode3`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode4`**: Passcode `[0..999999]` (0 is 'none').
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns `Ok` if the request was processed, or an error otherwise. The `Ok` response should be checked for `true`.

### Examples
```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
match set_door_passcodes controller 4uy 12345u 54321u 0u 999999u 5000 options with
| Ok response -> printfn "set door passcodes %A" response.ok
| Error err -> printfn "error setting door passcodes: %A" err
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = set_door_passcodes(controller, 4, 12345, 54321, 0, 999999, 5000, options);
if (result.IsOk)
{
    Console.WriteLine($"set-door-passcodes: {result.ResultValue.ok}");
}
else
{
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = set_door_passcodes(controller, 4, 12345UI, 54321UI, 0UI, 999999UI, 5000, options)
If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: {result.ResultValue.ok}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If
```

## **`open_door`**

Unlocks a door controlled by a controller.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`door`**: Door number `[1..4]`.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns Ok if the request was processed, error otherwise. The Ok response should be checked for 'true'.

### Examples

```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
let result = open_door controller 4uy 5000 options
match result with
| Ok response -> printfn "Door opened successfully: %A" response
| Error e -> printfn "Error: %s" e
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = open_door(controller, 1, 5000, options);
if (result.IsOk)
{
    Console.WriteLine("open-door: {0}", result.Value.ok);
}
else
{
    Console.WriteLine("open-door: error {0}", result.Error);
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = OpenDoor(controller, 1, 3000, options)
If result.IsOk Then
    Console.WriteLine("open-door: {0}", result.Value.ok)
Else
    Console.WriteLine("open-door: error {0}", result.Error)
End If
```
