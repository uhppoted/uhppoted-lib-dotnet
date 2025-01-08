## `SetDoorPasscodes`

Sets up to 4 passcodes for a controller door.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`door` (`uint8`)**: Door number `[1.4]`.
- **`passcodes` (`uint32 array`)**: Array of up to 4 passcodes in the range [0.999999], defaulting to 
  0 ('none') if the list contains less than 4 entries.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a `true` if the passcodes were updated, `Error` if the request failed.


### Examples
#### F#
```fsharp
let door = 4uy
let passcodes = [| 12345u; 54321u; 999999u |]
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match SetDoorPasscodes 405419896u door passcodes options with
| Ok ok -> printfn "set-door-passcodes: ok %A" ok
| Error err -> printfn "set-door-passcodes: error %A" err

match SetDoorPasscodes controller door passcodes options with
| Ok ok -> printfn "set-door-passcodes: ok %A" ok
| Error err -> printfn "set-door-passcodes: error %A" err
```

#### C#
```csharp
var door = 4u;
var passcodes = new uint[] {12345u, 54321u, 999999u };
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetDoorPasscodes(405419896u, door, passcodes, options);
if (result.IsOk)
{
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
}

var result = SetDoorPasscodes(controller, door, passcodes, options);
if (result.IsOk)
{
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim door As Byte = 4
Dim passcodes As Uinteger() = new UInteger() {12345, 54321, 999999}
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetDoorPasscodes(405419896UI, door, passcodes , options)
If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If

Dim result = SetDoorPasscodes(controller, door, passcodes , options)
If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If
```
