## `DeleteAllCards`

Deletes all card records from a controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns

Returns:
- `Ok` with a `true` if the cards were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match DeleteAllCards controller options with
| Ok ok -> printfn "delete-all-cards: ok %A" ok
| Error err -> printfn "delete-all-cards: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
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

```vb
Dim controller = 405419896
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = DeleteAllCards(controller, options)

If result.IsOk Then
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}")
End If
```

### Notes
