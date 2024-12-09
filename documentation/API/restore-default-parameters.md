## `RestoreDefaultParameters`

Restores manufacturer default settings.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the default settings were restored.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match RestoreDefaultSettings controller timeout options with
| Ok ok -> printfn "restore-default-settings: ok %A" ok
| Error err -> printfn "restore-default-settings: error %A" err
```

```csharp
var controller = 405419896u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = RestoreDefaultSettings(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"restore-default-settings: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"restore-default-settings: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = RestoreDefaultSettings(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"restore-default-settings: ok {result.ResultValue}")
Else
    Console.WriteLine($"restore-default-settings: error {result.ErrorValue}")
End If
```

### Notes
