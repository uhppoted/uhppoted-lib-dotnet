## `GetTime`

Retrieves the controller date and time.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetTime controller timeout options with
| Ok datetime -> printfn "get-time: ok %A" datetime
| Error err -> printfn "get-time: error %A" err
```

```csharp
var controller = 405419896u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetTime(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"get-time: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-time: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetTime(controller, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"get-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-time: error '{result.ErrorValue}'")
End If
```
