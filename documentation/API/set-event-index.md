## `SetEventIndex`

Sets the controller event index.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns:
- `Ok` with `true` if the controller event index was updated
- `Ok` with `false` if the controller event index was unchanged
- `Error` 

### Examples

```fsharp
let controller = 405419896u
let index = 13579u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match SetEventIndex controller index options with
| Ok ok  -> printfn "set-event-index: ok %A" ok
| Error err -> printfn "set-event-index: error %A" err
```

```csharp
var controller = 405419896u;
var index = 13579u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = SetEventIndex(controller, index, options);

if (result.IsOk)
{
    Console.WriteLine($"set-event-index: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim index = 13579
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = SetEventIndex(controller, index, options)

If (result.IsOk) Then
    Console.WriteLine($"set-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'")
End If
```

### Notes
