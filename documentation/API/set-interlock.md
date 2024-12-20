## `SetInterlock`

Sets the door interlocks for an access controller. Door interlocks prevent a door from being unlocked if one of
the other interlocked doors is already unlocked.


### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`interlock` (`Interlock`)**: Door interlock mode.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

The `interlock` parameter may take one of the following values:
- `Interlock.None`: no interlocks
- `Interlock.Doors12`: doors 1 & 2 are interlocked
- `Interlock.Doors34`: doors 3 & 4 are interlocked
- `Interlock.Doors12And34`: doors 1 &2 are interlocked and doors 3 & 4 are interlocked
- `Interlock.Doors123`: doors 1 & 2 & 3 are interlocked
- `Interlock.Doors1234`: doors 1 & 2 & 3 & 4 are interlocked

### Returns

Returns:
- `Ok` with a `true` if the access controller set the door interlock mode
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let interlock = Interlock.Doors123
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match SetInterlock 405419896u interlock options with
| Ok ok -> printfn "set-interlock: ok %A" ok
| Error err -> printfn "set-interlock: error %A" err

match SetInterlock controller interlock options with
| Ok ok -> printfn "set-interlock: ok %A" ok
| Error err -> printfn "set-interlock: error %A" err
```

#### C#
```csharp
var interlock = Interlock.Doors123;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetInterlock(405419896u, interlock, options);
if (result.IsOk)
{
    Console.WriteLine($"set-interlock: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-interlock: error {result.ErrorValue}");
}

var result = SetInterlock(controller, interlock, options);
if (result.IsOk)
{
    Console.WriteLine($"set-interlock: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-interlock: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim interlock = Interlock.Doors123
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetInterlock(405419896UI, interlock, options)
If result.IsOk Then
    Console.WriteLine($"set-interlock: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-interlock: error {result.ErrorValue}")
End If

Dim result = SetInterlock(controller, interlock, options)
If result.IsOk Then
    Console.WriteLine($"set-interlock: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-interlock: error {result.ErrorValue}")
End If
```

### Notes
