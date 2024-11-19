## `DeleteAllCards`

Deletes all card records from a controller.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the cards were cleared.
- `Ok` with `false` if request was declined.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match DeleteAllCards controller timeout options with
| Ok ok -> printfn "delete-all-cards: ok %A" ok
| Error err -> printfn "delete-all-cards: error %A" err
```

```csharp
var controller = 405419896u
var timeout = 5000
var options = new OptionsBuilder().build();
var result = DeleteAllCards(controller, timeout, options);

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
Dim timeout = 5000
Dim options As New OptionsBuilder().build()

Dim result = DeleteAllCards(controller, timeout, options)

If result.IsOk Then
    Console.WriteLine($"delete-all-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-all-cards: error {result.ErrorValue}")
End If
```

### Notes
