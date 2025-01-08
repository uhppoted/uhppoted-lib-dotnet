## `SetEventIndex`

Sets the controller event index.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns:
- `Ok` with `true` if the controller event index was updated
- `Ok` with `false` if the controller event index was unchanged
- `Error` 

### Examples

#### F#
```fsharp
let index = 13579u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match SetEventIndex 405419896u index options with
| Ok ok  -> printfn "set-event-index: ok %A" ok
| Error err -> printfn "set-event-index: error %A" err

match SetEventIndex controller index options with
| Ok ok  -> printfn "set-event-index: ok %A" ok
| Error err -> printfn "set-event-index: error %A" err
```

#### C#
```csharp
var index = 13579u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetEventIndex(405419896u, index, options);
if (result.IsOk)
{
    Console.WriteLine($"set-event-index: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'");
}

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

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim index = 13579
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetEventIndex(405419896UI, index, options)
If (result.IsOk) Then
    Console.WriteLine($"set-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'")
End If

Dim result = SetEventIndex(controller, index, options)
If (result.IsOk) Then
    Console.WriteLine($"set-event-index: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-event-index: error '{result.ErrorValue}'")
End If
```
