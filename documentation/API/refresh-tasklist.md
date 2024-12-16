## `RefreshTaskList`

Schedules added tasks.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the tasklist was refresh.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match RefreshTaskList controller options with
| Ok ok -> printfn "refresh-tasklist: ok %A" ok
| Error err -> printfn "refresh-tasklist: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = RefreshTaskList(controller, options);

if (result.IsOk)
{
    Console.WriteLine($"refresh-tasklist: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"refresh-tasklist: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = RefreshTaskList(controller, options)

If result.IsOk Then
    Console.WriteLine($"refresh-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"refresh-tasklist: error {result.ErrorValue}")
End If
```

### Notes
