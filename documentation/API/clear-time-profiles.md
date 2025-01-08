## `ClearTimeProfiles`

Clears all access time profiles stored on a controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the time profiles were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match ClearTimeProfiles 405419896uoptions with
| Ok ok -> printfn "clear-time-profiles: ok %A" ok
| Error err -> printfn "clear-time-profiles: error %A" err

match ClearTimeProfiles controller options with
| Ok ok -> printfn "clear-time-profiles: ok %A" ok
| Error err -> printfn "clear-time-profiles: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = ClearTimeProfiles(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}");
}

var result = ClearTimeProfiles(controller, options);
if (result.IsOk)
{
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = ClearTimeProfiles(405419896UI, options)
If result.IsOk Then
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}")
End If

Dim result = ClearTimeProfiles(controller, options)
If result.IsOk Then
    Console.WriteLine($"clear-time-profiles: ok {result.ResultValue}")
Else
    Console.WriteLine($"clear-time-profiles: error {result.ErrorValue}")
End If
```

