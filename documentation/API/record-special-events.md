## `RecordSpecialEvents`

Enables (or disables) events for door open/close, button press, etc.

### Parameters
- **`controller`**: Controller ID.
- **`enable`**: Enables _special events_ if `true`.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns:
- `Ok` with `true` if the _special events_ mode was updated
- `Error` 

### Examples

```fsharp
let controller = 405419896u
let enable = true
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match RecordSpecialEvents controller enable timeout options with
| Ok ok  -> printfn "record-special-events: ok %A" ok
| Error err -> printfn "record-special-events: error %A" err
```

```csharp
var controller = 405419896u;
var enable = true;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = RecordSpecialEvents(controller, enable, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"record-special-events: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim enable = true
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = RecordSpecialEvents(controller, enable, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"record-special-events: ok {result.ResultValue}")
Else
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'")
End If
```

### Notes
