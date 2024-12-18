## `GetCard`

Retrieves the card record (if any) at the index in the cards list stored on the controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`card` (`uint32`)**: Card number.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `Card` record  or `Error`. 

The `Ok` value is:
- A `Card` record if a card record was found for the card number.
- `null` if there was no record for the card number.

A `Card` record has the following fields:
  - `Card` (`uint32`): Card number.
  - `StartDate` (`DateOnly`): Date from which card is valid.
  - `EndDate` (`DateOnly`): Date after which card is no longer valid.
  - `Door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `PIN (`uint32`): Optional card PIN (0 for _none_).

### Examples

```fsharp
let controller = 405419896u
let card = 10058400u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetCard controller card options with
| Ok response when response.HasValue -> printfn "get-card: ok %A" response.Value
| Ok _ -> printfn "get-card: not found"
| Error err -> printfn "get-card: error %A" err
```

```csharp
var controller = 405419896u;
var card = 10058400u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetCard(controller, card, options);

if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-card: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-card: error 'not found'");
}
else
{
    Console.WriteLine($"get-card: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim card = 10058400
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetCard(controller, card, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-card: ok {result.ResultValue.Value}")
Else If (result.IsOk) Then
    Console.WriteLine($"get-card: error 'not found'")
Else
    Console.WriteLine($"get-card: error '{result.ErrorValue}'")
End If
```

### Notes

