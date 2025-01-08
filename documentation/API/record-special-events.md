## `RecordSpecialEvents`

Enables (or disables) events for door open/close, button press, etc.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`enable` (`bool`)**: Enables _special events_ if `true`.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns:
- `Ok` with `true` if the _special events_ mode was updated
- `Error` 

### Examples

#### F#
```fsharp
let enable = true
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match RecordSpecialEvents 405419896u enable options with
| Ok ok  -> printfn "record-special-events: ok %A" ok
| Error err -> printfn "record-special-events: error %A" err

match RecordSpecialEvents controller enable options with
| Ok ok  -> printfn "record-special-events: ok %A" ok
| Error err -> printfn "record-special-events: error %A" err
```

#### C#
```csharp
var enable = true;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = RecordSpecialEvents(405419896u, enable, options);
if (result.IsOk)
{
    Console.WriteLine($"record-special-events: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'");
}

var result = RecordSpecialEvents(controller, enable, options);
if (result.IsOk)
{
    Console.WriteLine($"record-special-events: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim enable = true
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = RecordSpecialEvents(405419896UI, enable, options)
If (result.IsOk) Then
    Console.WriteLine($"record-special-events: ok {result.ResultValue}")
Else
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'")
End If

Dim result = RecordSpecialEvents(controller, enable, options)
If (result.IsOk) Then
    Console.WriteLine($"record-special-events: ok {result.ResultValue}")
Else
    Console.WriteLine($"record-special-events: error '{result.ErrorValue}'")
End If
```
