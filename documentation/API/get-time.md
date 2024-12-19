## `GetTime`

Retrieves the controller date and time.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetTime 405419896u options with
| Ok datetime -> printfn "get-time: ok %A" datetime
| Error err -> printfn "get-time: error %A" err

match GetTime controller options with
| Ok datetime -> printfn "get-time: ok %A" datetime
| Error err -> printfn "get-time: error %A" err
```

```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetTime(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"get-time: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-time: error '{result.ErrorValue}'");
}

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
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetTime(405419896UI, options)
If (result.IsOk) Then
    Console.WriteLine($"get-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-time: error '{result.ErrorValue}'")
End If

Dim result = GetTime(controller, options)
If (result.IsOk) Then
    Console.WriteLine($"get-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-time: error '{result.ErrorValue}'")
End If
```
