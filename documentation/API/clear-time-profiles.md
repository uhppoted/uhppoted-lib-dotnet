## `ClearTimeProfiles`

Clears all access time profiles stored on a controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the time profiles were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match ClearTimeProfiles controller options with
| Ok ok -> printfn "clear-time-profiles: ok %A" ok
| Error err -> printfn "clear-time-profiles: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = ClearTimeProfiles(controller, options);

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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = ClearTimeProfiles(controller, options)

If result.IsOk Then
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}")
End If
```

### Notes
