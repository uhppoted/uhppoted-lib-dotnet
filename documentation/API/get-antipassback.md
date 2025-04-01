## `GetAntiPassback`

Retrieves the controller anti-passback mode:
- _disabled_
- _(1:2);(3:4)_
- _(1,3):(2,4)_
- _1:(2,3)_
- _1:(2,3,4)_

| Anti-passback | Description                                                  |
|---------------|--------------------------------------------------------------|
| _disabled_    | No anti-passback                                             |
| _(1:2);(3:4)_ | Doors 1 and 2 are interlocked, doors 3 and 4 are interlocked |
| _(1,3):(2,4)_ | Doors 1 and 3 are interlocked with doors 2 and 4             |
| _1:(2,3)_     | Door 1 is interlocked with doors 2 and 3                     |
| _1:(2,3,4)_   | Door 1 is interlocked with doors 2,3 and 4                   |

where _interlocked_ means a card will be swiped through a second time on a door until it has 
been swiped through at the _interlocked_ door. e.g: if the anti-passback mode is _(1,3):(2,4),
a card swiped through at either of doors 1 or 3 will be denied access at doors 1 and 3 until 
it has been swiped through at either of doors 2 or 4. Likewise a card swiped through at either
of doors 2 or 4 will be denied access at doors 2 and 4 until is has been swiped through at 
either of doors 1 or 3.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the controller anti-passback mode or `Error`. 

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match GetAntiPassback 405419896u options with
| Ok antipassback -> printfn "get-antipassback: ok %A" antipassback
| Error err -> printfn "get-antipassback: error %A" err

match GetAntiPassback controller options with
| Ok antipassback -> printfn "get-antipassback: ok %A" antipassback
| Error err -> printfn "get-antipassback: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetAntiPassback(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"get-antipassback: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-antipassback: error '{result.ErrorValue}'");
}

var result = GetAntiPassback(controller, options);
if (result.IsOk)
{
    Console.WriteLine($"get-antipassback: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-antipassback: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetAntiPassback(405419896UI, options)
If (result.IsOk) Then
    Console.WriteLine($"get-antipassback: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-antipassback: error '{result.ErrorValue}'")
End If

Dim result = GetAntiPassback(controller, options)
If (result.IsOk) Then
    Console.WriteLine($"get-antipassback: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-antipassback: error '{result.ErrorValue}'")
End If
```
