## `ActivateKeypads`

Activates/deactivates the access reader keypads attached to an access controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`reader1` (`bool`)**: Activate the keypad for reader 1 if `true`.
- **`reader2` (`bool`)**: Activate the keypad for reader 2 if `true`.
- **`reader3` (`bool`)**: Activate the keypad for reader 3 if `true`.
- **`reader4` (`bool`)**: Activate the keypad for reader 4 if `true`.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the keypads were activated
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let reader1 = true
let reader2 = true
let reader3 = false
let reader4 = true
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }
let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match ActivateKeypads 405419896u reader1 reader2 reader3 reader4 options with
| Ok ok -> printfn "activate-keypads: ok %A" ok
| Error err -> printfn "activate-keypads: error %A" err

match ActivateKeypads controller reader1 reader2 reader3 reader4 options with
| Ok ok -> printfn "activate-keypads: ok %A" ok
| Error err -> printfn "activate-keypads: error %A" err
```

#### C#
```csharp
var reader1 = true
var reader2 = true
var reader3 = false
var reader4 = true
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = ActivateKeypads(405419896u, reader1, reader2, reader3, reader4, options);
if (result.IsOk)
{
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}");
}

var result = ActivateKeypads(controller, reader1, reader2, reader3, reader4, options);
if (result.IsOk)
{
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim reader1 = True
Dim reader2 = True
Dim reader3 = False
Dim reader4 = True
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = ActivateKeypads(405419896UI, reader1, reader2, reader3, reader4, options)
If result.IsOk Then
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}")
Else
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}")
End If

Dim result = ActivateKeypads(controller, reader1, reader2, reader3, reader4, options)
If result.IsOk Then
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}")
Else
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}")
End If
```

