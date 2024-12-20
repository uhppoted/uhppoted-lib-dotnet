## `GetEventIndex`

Retrieves the current event index from the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the current event index (`uint32`) or an `Error` 

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetEventIndex 405419896u options with
| Ok index  -> printfn "get-event-index: ok %A" index
| Error err -> printfn "get-event-index: error %A" err

match GetEventIndex controller options with
| Ok index  -> printfn "get-event-index: ok %A" index
| Error err -> printfn "get-event-index: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetEventIndex(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"get-event-index: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'");
}

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

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetEventIndex(405419896UI, options)
If (result.IsOk) Then
    Console.WriteLine($"get-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'")
End If

Dim result = GetEventIndex(controller, options)
If (result.IsOk) Then
    Console.WriteLine($"get-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-event-index: error '{result.ErrorValue}'")
End If
```

### Notes
