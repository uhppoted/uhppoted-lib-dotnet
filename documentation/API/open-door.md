## **`OpenDoor`**

Unlocks a door controlled by a controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`door`**: Door number `[1.4]`.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a `true` if the door was unlocked, `Error` if the request failed.

### Examples
```fsharp
let controller = 405419896u
let door = 4uy
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match OpenDoor controller door options with
| Ok ok -> printfn "open-door: ok %A" ok
| Error err -> printfn "open-door: error %A" err
```

```csharp
var controller = 405419896u;
var door = 4u;
var options = new OptionsBuilder().WithTimeout(1250).build();
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

```vb
Dim controller = 405419896
Dim door As Byte = 4
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = OpenDoor(controller, door, options)

If result.IsOk Then
    Console.WriteLine($"open-door: ok {result.ResultValue}")
Else
    Console.WriteLine($"open-door: error {result.ErrorValue}")
End If
```

