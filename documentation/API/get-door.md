## `GetDoor`

Retrieves a door control mode and unlock delay from a controller.

### Parameters
- **`controller`**: Controller ID.
- **`door`**: Door ID [1..4].
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a Nullable `Door` record if the request was processed or an `Error` 

The `Ok` value is:
- An `Door` record if the controller returned a matching response.
- `null` if there was no door matching the request door ID.

The `Door` record has the following fields:
  - `mode` (`DoorMode`): Door control mode (NormallyOpen, NormallyClosed, Controlled).
  - `delay` (`uint8`): Duration (seconds) for which the door remains unlocked after access is granted.
  - `event_type` (`uint8`): Event type.


### Examples

```fsharp
let controller = 405419896u
let door = 3uy
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetDoor controller door timeout options with
| Ok response when response.HasValue -> printfn "get-door: ok %A" response.Value
| Ok _ -> printfn "get-door: not found"
| Error err -> printfn "get-door: error %A" err
```

```csharp
var controller = 405419896u;
var door = 3u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetDoor(controller, door, timeout, options);

if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-door: error 'not found'");
}
else
{
    Console.WriteLine($"get-door: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim door = 3
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetDoor(controller, door, timeout, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-door: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-door: error 'not found'")
Else
    Console.WriteLine($"get-door: error '{result.ErrorValue}'")
End If
```

### Notes
