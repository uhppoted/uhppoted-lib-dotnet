## `GetEventIndex`

Retrieves the current event index from the controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the current event index (`uint32`) or an `Error` 

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetEventIndex controller options with
| Ok index  -> printfn "get-event-index: ok %A" index
| Error err -> printfn "get-event-index: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetEventIndex(controller, options);

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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetEventIndex(controller, options)

If (result.IsOk) Then
    Console.WriteLine($"get-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'")
End If
```

### Notes
