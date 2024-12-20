## **`OpenDoor`**

Unlocks a door controlled by a controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`door`**: Door number `[1.4]`.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a `true` if the door was unlocked, `Error` if the request failed.

### Examples
#### F#
```fsharp
let door = 4uy
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match OpenDoor 405419896u door options with
| Ok ok -> printfn "open-door: ok %A" ok
| Error err -> printfn "open-door: error %A" err

match OpenDoor controller door options with
| Ok ok -> printfn "open-door: ok %A" ok
| Error err -> printfn "open-door: error %A" err
```

#### C#
```csharp
var door = 4u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = OpenDoor(405419896u, door, options);
if (result.IsOk)
{
    Console.WriteLine($"open-door: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"open-door: error {result.ErrorValue}");
}

var result = OpenDoor(controller, door, options);
if (result.IsOk)
{
    Console.WriteLine($"open-door: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"open-door: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim door As Byte = 4
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = OpenDoor(405419896UI, door, options)
If result.IsOk Then
    Console.WriteLine($"open-door: ok {result.ResultValue}")
Else
    Console.WriteLine($"open-door: error {result.ErrorValue}")
End If

Dim result = OpenDoor(controller, door, options)
If result.IsOk Then
    Console.WriteLine($"open-door: ok {result.ResultValue}")
Else
    Console.WriteLine($"open-door: error {result.ErrorValue}")
End If
```

