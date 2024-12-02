## `SetDoorPasscodes`

Sets up to 4 passcodes for a controller door.

### Parameters
- **`controller`**: Controller ID.
- **`door`**: Door number `[1..4]`.
- **`passcode1`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode2`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode3`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode4`**: Passcode `[0..999999]` (0 is 'none').
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a `true` if the passcodes were updated, `Error` if the request failed.


### Examples
```fsharp
let controller = 405419896u
let door = 4uy
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match SetDoorPasscodes controller door 12345u 54321u 0u 999999u timeout options with
| Ok ok -> printfn "set-door-passcodes: ok %A" ok
| Error err -> printfn "set-door-passcodes: error %A" err
```
```csharp
var controller = 405419896u
var door = 4u
var timeout = 5000
var options = new OptionsBuilder().build();
var result = SetDoorPasscodes(controller, door, 12345, 54321, 0, 999999, timeout, options);
if (result.IsOk)
{
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
}
```
```vb
Dim controller = 405419896
Dim door As Byte = 4
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetDoorPasscodes(controller, door, 12345UI, 54321UI, 0UI, 999999UI, timeout, options)

If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If
```

