## `SetPCControl`

Enables/disables remote access control management. The access controller will revert to standalone access
control managment if it does not receive a command from the 'PC' at least every 30 seconds.


### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`enable` (`bool`)**: Enables (or disables) remote access control managment.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the access controller has delegated access control to the remote system.
- `Ok` with `false`  if the access controller retains access control management.
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let enable = true
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match SetPCControl 405419896u enable options with
| Ok ok -> printfn "set-pc-control: ok %A" ok
| Error err -> printfn "set-pc-control: error %A" err

match SetPCControl controller enable options with
| Ok ok -> printfn "set-pc-control: ok %A" ok
| Error err -> printfn "set-pc-control: error %A" err
```

#### C#
```csharp
var enable = true;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetPCControl(405419896u, enable, options);
if (result.IsOk)
{
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}");
}

var result = SetPCControl(controller, enable, options);
if (result.IsOk)
{
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim enable = True
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetPCControl(405419896UI, enable, options)
If result.IsOk Then
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}")
End If

Dim result = SetPCControl(controller, enable, options)
If result.IsOk Then
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}")
End If
```

### Notes
