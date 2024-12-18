## `GetTime`

Retrieves the controller date and time.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetTime controller options with
| Ok datetime -> printfn "get-time: ok %A" datetime
| Error err -> printfn "get-time: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetTime(controller, options);

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
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetTime(controller, options)

If (result.IsOk) Then
    Console.WriteLine($"get-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-time: error '{result.ErrorValue}'")
End If
```
