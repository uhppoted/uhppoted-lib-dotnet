## `RecordSpecialEvents`

Enables (or disables) events for door open/close, button press, etc.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`enable` (`bool`)**: Enables _special events_ if `true`.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns:
- `Ok` with `true` if the _special events_ mode was updated
- `Error` 

### Examples

```fsharp
let controller = 405419896u
let enable = true
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match RecordSpecialEvents controller enable options with
| Ok ok  -> printfn "record-special-events: ok %A" ok
| Error err -> printfn "record-special-events: error %A" err
```

```csharp
var controller = 405419896u;
var enable = true;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = RecordSpecialEvents(controller, enable, options);

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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = RecordSpecialEvents(controller, enable, options)

If (result.IsOk) Then
    Console.WriteLine($"record-special-events: ok {result.ResultValue}")
Else
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'")
End If
```

### Notes
