## `DeleteAllCards`

Deletes all card records from a controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the cards were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match DeleteAllCards 405419896u options with
| Ok ok -> printfn "delete-all-cards: ok %A" ok
| Error err -> printfn "delete-all-cards: error %A" err

match DeleteAllCards controller options with
| Ok ok -> printfn "delete-all-cards: ok %A" ok
| Error err -> printfn "delete-all-cards: error %A" err
```

#### C#
```csharp
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = DeleteAllCards(405419896u, options);
if (result.IsOk)
{
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}");
}

var result = DeleteAllCards(controller, options);
if (result.IsOk)
{
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = DeleteAllCards(405419896UI, options)
If result.IsOk Then
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}")
End If

Dim result = DeleteAllCards(controller, options)
If result.IsOk Then
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}")
End If
```

### Notes
