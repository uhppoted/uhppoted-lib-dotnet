## `GetEventIndex`

Retrieves the current event index from the controller.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the current event index (`uint32`) or an `Error` 

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetEventIndex controller timeout options with
| Ok index  -> printfn "get-event-index: ok %A" index
| Error err -> printfn "get-event-index: error %A" err
```

```csharp
var controller = 405419896u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetEventIndex(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"get-event-index: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetEventIndex(controller, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"get-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'")
End If
```

### Notes
