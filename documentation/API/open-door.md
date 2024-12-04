## **`OpenDoor`**

Unlocks a door controlled by a controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`door`**: Door number `[1..4]`.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a `true` if the door was unlocked, `Error` if the request failed.

### Examples
```fsharp
let controller = 405419896u
let door = 4uy
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match OpenDoor controller door timeout options with
| Ok ok -> printfn "open-door: ok %A" ok
| Error err -> printfn "open-door: error %A" err
```

```csharp
var controller = 405419896u;
var door = 4u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = OpenDoor(controller, door, timeout, options);
if (result.IsOk)
{
    Console.WriteLine($"open-door: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"open-door: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim door As Byte = 4
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = OpenDoor(controller, door, timeout, options)

If result.IsOk Then
    Console.WriteLine($"open-door: ok {result.ResultValue}")
Else
    Console.WriteLine($"open-door: error {result.ErrorValue}")
End If
```

