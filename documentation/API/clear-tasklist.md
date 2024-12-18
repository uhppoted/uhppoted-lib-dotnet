## `ClearTaskList`

Clears all scheduled tasks from the controller task list.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the tasklist was cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match ClearTaskList controller options with
| Ok ok -> printfn "clear-tasklist: ok %A" ok
| Error err -> printfn "clear-tasklist: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = ClearTaskList(controller, options);

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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = ClearTaskList(controller, options)

If result.IsOk Then
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}")
End If
```

### Notes
