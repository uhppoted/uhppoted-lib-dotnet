## `SetAntiPassback`

Sets the anti-passback mode for an access controller:
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
- **`controller` (`uint32`)**: Controller ID.
- **`antipassback` (`AntiPassback`)**: Anti-passback mode.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

The `antipassback` parameter may take one of the following values:
- `AntiPassback.Disabled`: no anti-passback
- `AntiPassback.Doors12_34`: doors 1 and 2 are interlocked, doors 3 and 4 are interlocked
- `AntiPassback.Doors13_24`: doors 1 and 3 are interlocked with doors 2 and 4
- `AntiPassback.Doors1_23`: door 1 is interlocked with doors 2 and 3
- `AntiPassback.Doors1_234`: door 1 is interlocked with doors 2,3 and 4

### Returns

Returns:
- `Ok` with a `true` if the access controller set the anti-passback mode
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let antipassback = AntiPassback.Doors13_24
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match SetAntiPassback 405419896u antipassback options with
| Ok ok -> printfn "set-antipassback: ok %A" ok
| Error err -> printfn "set-antipassback: error %A" err

match SetAntiPassback controller antipassback options with
| Ok ok -> printfn "set-antipassback: ok %A" ok
| Error err -> printfn "set-antipassback: error %A" err
```

#### C#
```csharp
var antipassback = AntiPassback.Doors13_24
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = SetAntiPassback(405419896u, antipassback, options);
if (result.IsOk)
{
    Console.WriteLine($"set-antipassback: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-antipassback: error {result.ErrorValue}");
}

var result = SetAntiPassback(controller, antipassback, options);
if (result.IsOk)
{
    Console.WriteLine($"set-antipassback: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-antipassback: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim antipassback = AntiPassback.Doors13_24
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetAntiPassback(405419896UI, antipassback, options)
If result.IsOk Then
    Console.WriteLine($"set-antipassback: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-antipassback: error {result.ErrorValue}")
End If

Dim result = SetAntiPassback(controller, antipassback, options)
If result.IsOk Then
    Console.WriteLine($"set-antipassback: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-antipassback: error {result.ErrorValue}")
End If
```
