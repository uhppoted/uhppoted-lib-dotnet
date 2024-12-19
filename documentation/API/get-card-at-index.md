## `GetCardAtIndex`

Retrieves the card record (if any) at the index from the cards list stored on the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`index` (`uint32`)**: Card record index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.
- **`controller`**: Controller ID.

### Returns
Returns `Ok` with a Nullable `Card` record if the request was processed or an error otherwise. 

The `Ok` value is:
- A card record if a card record was found at the index.
- `null` if there was no record at the index.
- `null` if the record at the index was deleted.

A `Card` record has the following fields:
  - `Card` (`uint32`): Card number.
  - `StartDate` (`DateOnly`): Date from which card is valid.
  - `EndDate` (`DateOnly`): Date after which card is no longer valid.
  - `Door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `PIN` (`uint32`): Optional card PIN (0 for _none_).

### Examples
```fsharp
let index = 135u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetCardAtIndex 405419896u index options with
| Ok response when response.HasValue -> printfn "get-card-at-index: ok %A" response.Value
| Ok _ -> printfn "get-card-at-index: not found"

match GetCardAtIndex controller index options with
| Ok response when response.HasValue -> printfn "get-card-at-index: ok %A" response.Value
| Ok _ -> printfn "get-card-at-index: not found"
| Error err -> printfn "get-card-at-index: error %A" err
```

```csharp
var index = 135u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetCardAtIndex(405419896u, index, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-card-at-index: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-card-at-index: error 'not found'");
}
else
{
    Console.WriteLine($"get-card-at-index: error '{result.ErrorValue}'");
}

var result = GetCardAtIndex(controller, index, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-card-at-index: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-card-at-index: error 'not found'");
}
else
{
    Console.WriteLine($"get-card-at-index: error '{result.ErrorValue}'");
}
```

```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim index = 135
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetCardAtIndex(405419896UI, index, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-card-at-index: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-card-at-index: error 'not found'")
Else
    Console.WriteLine($"get-card-at-index: error '{result.ErrorValue}'")
End If

Dim result = GetCardAtIndex(controller, index, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-card-at-index: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-card-at-index: error 'not found'")
Else
    Console.WriteLine($"get-card-at-index: error '{result.ErrorValue}'")
End If
```

### Notes
