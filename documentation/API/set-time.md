## `SetTime`

Sets the controller date and time.

### Parameters
- **`controller`**: Controller ID.
- **`datetime`**: Date and time to set.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let datetime = DateTime.Now
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match SetTime controller datetie timeout options with
| Ok datetime -> printfn "set-time: ok %A" datetime
| Error err -> printfn "set-time: error %A" err
```

```csharp
var controller = 405419896u;
var datetime = DateTime.Now;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = SetTime(controller, datetime, timeout, options);

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
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetTime(controller, datetime, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"set-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-time: error '{result.ErrorValue}'")
End If
```
