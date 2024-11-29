## `ClearTimeProfiles`

Clears all access time profiles stored on a controller.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the time profiles were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match ClearTimeProfiles controller timeout options with
| Ok ok -> printfn "clear-time-profiles: ok %A" ok
| Error err -> printfn "clear-time-profiles: error %A" err
```

```csharp
var controller = 405419896u
var timeout = 5000
var options = new OptionsBuilder().build();
var result = ClearTimeProfiles(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = ClearTimeProfiles(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}")
End If
```

### Notes
