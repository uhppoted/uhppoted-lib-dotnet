## `SetEventIndex`

Sets the controller event index.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`index` (`uint32`)**: Event index.
- **`timeout` (`int`)**: Operation timeout (ms).
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
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match SetEventIndex controller index timeout options with
| Ok ok  -> printfn "set-event-index: ok %A" ok
| Error err -> printfn "set-event-index: error %A" err
```

```csharp
var controller = 405419896u;
var index = 13579u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = SetEventIndex(controller, index, timeout, options);

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
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetEventIndex(controller, index, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"set-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'")
End If
```

### Notes
