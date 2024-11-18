## `DeleteCard`

Deletes a card record from a controller.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`card`**: Card number.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with `true` if the card was deleted
- `Ok` with `false` if the card was not deleted (typically because the card is not stored on the controller)
- `Error` if the request failed

### Examples

```fsharp
let controller = 405419896u
let card = 10058400u

let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match DeleteCard controller card timeout options with
| Ok ok -> printfn "delete-card: ok %A" ok
| Error err -> printfn "delete-card: error %A" err
```

```csharp
var controller = new ControllerBuilder(405419896).build();
var card = 10058400u

var timeout = 5000
var options = new OptionsBuilder().build();
var result = DeleteCard(controller, card, timeout, options);

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
Dim controller As New ControllerBuilder(405419896u).build()
Dim card = 10058400

Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = DeleteCard(controller, card, PIN, timeout, options)

If result.IsOk Then
    Console.WriteLine($"delete-card: ok {result.ResultValue}")
Else
    Console.WriteLine($"delete-card: error {result.ErrorValue}")
End If
```

### Notes
