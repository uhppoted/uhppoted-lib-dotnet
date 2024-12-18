## `SetTime`

Sets the controller date and time.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`datetime` (`DateTime`)**: Date and time to set.
- **`controller` (`uint32`)**: Controller ID.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let datetime = DateTime.Now
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match SetTime controller datetie options with
| Ok datetime -> printfn "set-time: ok %A" datetime
| Error err -> printfn "set-time: error %A" err
```

```csharp
var controller = 405419896u;
var datetime = DateTime.Now;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = SetTime(controller, datetime, options);

if (result.IsOk)
{
    Console.WriteLine($"set-time: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-time: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim datetime = DateTime.Now
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = SetTime(controller, datetime, options)

If (result.IsOk) Then
    Console.WriteLine($"set-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-time: error '{result.ErrorValue}'")
End If
```
