## `SetTime`

Sets the controller date and time.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`datetime` (`DateTime`)**: Date and time to set.
- **`controller` (`uint32`)**: Controller ID.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the controller date and time or `Error`. 

### Examples

#### F#
```fsharp
let datetime = DateTime.Now
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match SetTime 405419896u datetie options with
| Ok datetime -> printfn "set-time: ok %A" datetime
| Error err -> printfn "set-time: error %A" err

match SetTime controller datetie options with
| Ok datetime -> printfn "set-time: ok %A" datetime
| Error err -> printfn "set-time: error %A" err
```

#### C#
```csharp
var datetime = DateTime.Now;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetTime(405419896u, datetime, options);
if (result.IsOk)
{
    Console.WriteLine($"set-time: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-time: error '{result.ErrorValue}'");
}

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

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim datetime = DateTime.Now
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetTime(405419896UI, datetime, options)
If (result.IsOk) Then
    Console.WriteLine($"set-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-time: error '{result.ErrorValue}'")
End If

Dim result = SetTime(controller, datetime, options)
If (result.IsOk) Then
    Console.WriteLine($"set-time: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-time: error '{result.ErrorValue}'")
End If
```
