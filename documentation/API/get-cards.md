## `GetCards`

Retrieves the number of card records stored on the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with the number of cards stored on the controller or `Error`. 

### Examples

#### F#
```fsharp
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match GetCards 405419896u options with
| Ok cards -> printfn "get-cards: ok %A" cards
| Error err -> printfn "get-cards: error %A" err

match GetCards controller options with
| Ok cards -> printfn "get-cards: ok %A" cards
| Error err -> printfn "get-cards: error %A" err
```

#### C#
```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetCards(controller, options);

if (result.IsOk)
{
    Console.WriteLine($"get-cards: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetCards(405419896UI, options)
If (result.IsOk) Then
    Console.WriteLine($"get-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'")
End If

Dim result = GetCards(controller, options)
If (result.IsOk) Then
    Console.WriteLine($"get-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'")
End If
```
