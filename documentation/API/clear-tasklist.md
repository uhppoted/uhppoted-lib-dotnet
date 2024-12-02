## `ClearTaskList`

Clears all scheduled tasks from the controller task list.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the tasklist was cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match ClearTaskList controller timeout options with
| Ok ok -> printfn "clear-tasklist: ok %A" ok
| Error err -> printfn "clear-tasklist: error %A" err
```

```csharp
var controller = 405419896u
var timeout = 5000
var options = new OptionsBuilder().build();
var result = ClearTaskList(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = ClearTaskList(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}")
End If
```

### Notes
