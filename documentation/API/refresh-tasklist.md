## `RefreshTaskList`

Schedules added tasks.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the tasklist was refresh.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match RefreshTaskList controller timeout options with
| Ok ok -> printfn "refresh-tasklist: ok %A" ok
| Error err -> printfn "refresh-tasklist: error %A" err
```

```csharp
var controller = 405419896u
var timeout = 5000
var options = new OptionsBuilder().build();
var result = RefreshTaskList(controller, timeout, options);

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
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = RefreshTaskList(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"refresh-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"refresh-tasklist: error {result.ErrorValue}")
End If
```

### Notes
