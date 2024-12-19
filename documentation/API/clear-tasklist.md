## `ClearTaskList`

Clears all scheduled tasks from the controller task list.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the tasklist was cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match ClearTaskList 405419896u options with
| Ok ok -> printfn "clear-tasklist: ok %A" ok
| Error err -> printfn "clear-tasklist: error %A" err

match ClearTaskList controller options with
| Ok ok -> printfn "clear-tasklist: ok %A" ok
| Error err -> printfn "clear-tasklist: error %A" err
```

```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = ClearTaskList(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}");
}

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
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = ClearTaskList(405419896UI, options)
If result.IsOk Then
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}")
End If

Dim result = ClearTaskList(controller, options)
If result.IsOk Then
    Console.WriteLine($"clear-tasklist: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-tasklist: error {result.ErrorValue}")
End If
```

### Notes
