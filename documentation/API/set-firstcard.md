## `SetFirstCard`

Sets the _first card_ configuration for a controller managed door. The _first card_ configuration will only take effect when the 
tasklist is refreshed.

### Parameters
- **`controller`**: Controller ID.
- **`door`**: Door ID [1..4].
- **`firstcard`**: FirstCard configuration.
- **`options`**: Bind, broadcast, and listen addresses.

The `FirstCard` record has the following fields:
  - `startTime` (`TimeOnly Nullable`):  time of day from which _first card_ can be activated
  - `endTime` (`TimeOnly Nullable`):  time of day after which _first card_ mode is deactivated
  - `activeMode` (`DoorMode`):  door control mode after a valid _first card_ swipe (_Controlled_, _NormallyOpen_ or _NormallyClosed_)
  - `inactiveMode` (`DoorMode`):  door control mode when _first card_ mode is deactivated (_Controlled_, _NormallyOpen_, _NormallyClosed_ or _FirstCardOnly_)
  - `monday` (`bool`): `true` if _first card_ mode can be activated on a Monday
  - `tuesday` (`bool`):  `true` if _first card_ mode can be activated on a Tuesday
  - `wednesday` (`bool`):  `true` if _first card_ mode can be activated on a Wednesday
  - `thursday` (`bool`):  `true` if _first card_ mode can be activated on a Thursday
  - `friday` (`bool`):  `true` if _first card_ mode can be activated on a Friday
  - `saturday` (`bool`):  `true` if _first card_ mode can be activated on a Saturday
  - `sunday` (`bool`):  `true` if _first card_ mode can be activated on a Sunday
  

### Returns
Returns `Ok` with `true` if the _first card_ configuration was accepted, or an `Error` 

### Examples

#### F#
```fsharp
let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

let door = 3uy;

let firstcard: FirstCard =
    { startTime = Nullable(TimeOnly(8, 30))
      endTime = Nullable(TimeOnly(16, 45))
      activeMode = DoorMode.NormallyOpen
      inactiveMode = DoorMode.FirstCardOnly
      monday = true
      tuesday = true
      wednesday = false
      thursday = true
      friday = false
      saturday = true
      sunday = true }

let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match SetFirstCard 405419896u door firstcard options with
| Ok response -> printfn "set-firstcard: ok %A" response.Value
| Error err -> printfn "set-firstcard: error %A" err

match SetFirstCard controller door firstcard options with
| Ok response -> printfn "set-firstcard: ok %A" response.Value
| Error err -> printfn "set-firstcard: error %A" err
```

#### C#
```csharp
var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var door = 3u;

var firstcard = new FirstCardBuilder()
                    .WithStartTime(TimeOnly.Parse("08:30"))
                    .WithEndTime(TimeOnly.Parse("16:45"))
                    .WithActiveMode(DoorMode.NormallyOpen)
                    .WithInactiveMode(DoorMode.FirstCardOnly)
                    .WithWeekdays(true,true,false,false,true,false,false)
                    .build();

var options = new OptionsBuilder().WithTimeout(1250).build();

var result = SetFirstCard(405419896u, door, firstcard, options);
if (result.IsOk)
{
    Console.WriteLine($"set-firstcard: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-firstcard: error '{result.ErrorValue}'");
}

var result = SetFirstCard(controller, door, firstcard, options);
if (result.IsOk)
{
    Console.WriteLine($"set-firstcard: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-firstcard: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim firstcard = New FirstCardBuilder().
                    WithStartTime(TimeOnly.Parse("08:30")).
                    WithEndTime(TimeOnly.Parse("16:45")).
                    WithActiveMode(DoorMode.NormallyOpen).
                    WithInactiveMode(DoorMode.FirstCardOnly).
                    WithMonday().
                    WithTuesday().
                    WithFriday().
                    build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = SetFirstCard(405419896UI, door, firstcard, options)
If (result.IsOk) Then
    Console.WriteLine($"set-firstcard: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-firstcard: error '{result.ErrorValue}'")
End If

Dim result = SetFirstCard(controller, door, firstcard options)
If (result.IsOk) Then
    Console.WriteLine($"set-firstcard: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-firstcard: error '{result.ErrorValue}'")
End If
```
