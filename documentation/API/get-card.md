## `GetCard`

Retrieves the card record (if any) at the index in the cards list stored on the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`card` (`uint32`)**: Card number.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a`Card` record  or `Error`:

- A `Card` record has the following fields:
  - `Card` (`uint32`): Card number.
  - `StartDate` (`DateOnly`): Date from which card is valid.
  - `EndDate` (`DateOnly`): Date after which card is no longer valid.
  - `Door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `PIN` (`uint32`): Optional card PIN (0 for _none_).

- `Error CardNotFound` if the card does not exist
- `Error <error>` if the request failed.

### Examples

#### F#
```fsharp
let card = 10058400u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match GetCard 405419896u card options with
| Ok response when response.HasValue -> printfn "get-card: ok %A" response.Value
| Error CardNotFound -> printfn "get-card: not found"
| Error err -> printfn "get-card: error %A" err

match GetCard controller card options with
| Ok response when response.HasValue -> printfn "get-card: ok %A" response.Value
| Error CardNotFound -> printfn "get-card: not found"
| Error err -> printfn "get-card: error %A" err
```

#### C#
```csharp
var card = 10058400u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetCard(405419896u, card, options);
if (result.IsOk)
{
    Console.WriteLine($"get-card: ok {result.ResultValue}");
}
else if (result.IsError && result.ErrorValue == Err.CardNotFound)
{
    Console.WriteLine($"get-card: card not found");
}
else
{
    Console.WriteLine($"get-card: error '{result.ErrorValue}'");
}

var result = GetCard(controller, card, options);
if (result.IsOk)
{
    Console.WriteLine($"get-card: ok {result.ResultValue}");
}
else if (result.IsError && result.ErrorValue == Err.CardNotFound)
{
    Console.WriteLine($"get-card: card not found");
}
else
{
    Console.WriteLine($"get-card: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim card = 10058400
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetCard(405419896UI, card, options)
If (result.IsOk) Then
    Console.WriteLine($"get-card: ok {result.ResultValue}")
Else If (result.IsError And result.ErrorValue is Err.CardNotFound) Then
    Console.WriteLine($"get-card: card not found")
Else
    Console.WriteLine($"get-card: error '{result.ErrorValue}'")
End If

Dim result = GetCard(controller, card, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-card: ok {result.ResultValue}")
Else If (result.IsError And result.ErrorValue is Err.CardNotFound) Then
    Console.WriteLine($"get-card: card not found")
Else
    Console.WriteLine($"get-card: error '{result.ErrorValue}'")
End If
```
