## `DeleteCard`

Deletes a card record from a controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`card` (`uint32`)**: Card number.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the card was deleted.
- `Ok` with `false` if the card was not deleted (typically because the card is not stored on the controller).
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let card = 10058400u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match DeleteCard controller card options with
| Ok ok -> printfn "delete-card: ok %A" ok
| Error err -> printfn "delete-card: error %A" err
```

```csharp
var controller = 405419896u;
var card = 10058400u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = DeleteCard(controller, card, options);

if (result.IsOk)
{
    Console.WriteLine($"delete-card: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"delete-card: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim card = 10058400

Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = DeleteCard(controller, card, options)

If result.IsOk Then
    Console.WriteLine($"delete-card: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-card: error {result.ErrorValue}")
End If
```

### Notes
