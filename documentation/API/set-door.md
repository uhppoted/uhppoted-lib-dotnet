## `SetDoor`

Retrieves a door control mode and unlock delay from a controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`door` (`uint8`)**: Door ID [1..4].
- **`mode` (`DoorMode`)**: Door control mode (controlled, normally-open or normally-closed)
- **`delay` (`uint8`)**: Door unlock delay (seconds).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a Nullable `Door` record if the door was updated or an `Error` 

The `Ok` value is:
- An `Door` record if the controller returned a matching response.
- `null` if there was no door matching the request door ID.

The `Door` record has the following fields:
  - `mode` (`DoorMode`): Door control mode (NormallyOpen, NormallyClosed, Controlled).
  - `delay` (`uint8`): Duration (seconds, [0..255]) for which the door remains unlocked after access is granted.
  - `event_type` (`uint8`): Event type.


### Examples

```fsharp
let controller = 405419896u
let door = 3uy
let mode = DoorMode.NormallyClosed
let delay = 5
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match SetDoor controller door mode delay options with
| Ok response when response.HasValue -> printfn "set-door: ok %A" response.Value
| Ok _ -> printfn "set-door: not found"
| Error err -> printfn "set-door: error %A" err
```

```csharp
var controller = 405419896u;
var door = 3u;
var mode = DoorMode.NormallyClosed;
var delay = 5;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = SetDoor(controller, door, mode, delay, options);

if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"set-door: error 'not found'");
}
else
{
    Console.WriteLine($"set-door: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim door = 3
Dim mode = DoorMode.NormallyClosed
Dim delay = 5
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = SetDoor(controller, door, mode, delay, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"set-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"set-door: error 'not found'")
Else
    Console.WriteLine($"set-door: error '{result.ErrorValue}'")
End If
```

### Notes
