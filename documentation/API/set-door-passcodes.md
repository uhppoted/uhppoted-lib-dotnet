## `SetDoorPasscodes`

Sets up to 4 passcodes for a controller door.

### Parameters
- **`controller`**: Controller ID.
- **`door`**: Door number `[1..4]`.
- **`passcodes`**: Array of up to 4 passcodes in the range [0..999999], defaulting to 0 ('none') if the list contains less than 4 entries.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a `true` if the passcodes were updated, `Error` if the request failed.


### Examples
```fsharp
let controller = 405419896u
let door = 4uy
let passcodes = [| 12345u; 54321u; 999999u |]
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match SetDoorPasscodes controller door passcodes timeout options with
| Ok ok -> printfn "set-door-passcodes: ok %A" ok
| Error err -> printfn "set-door-passcodes: error %A" err
```

```csharp
var controller = 405419896u
var door = 4u
var passcodes = new uint[] {12345u, 54321u, 999999u };
var timeout = 5000
var options = new OptionsBuilder().build();
var result = SetDoorPasscodes(controller, door, passcodes, timeout, options);
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
Dim passcodes As Uinteger() = new UInteger() {12345, 54321, 999999}
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetDoorPasscodes(controller, door, passcodes , timeout, options)

If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If
```

