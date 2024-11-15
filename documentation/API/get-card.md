## `GetCard`

Retrieves a card record from a controller by card number.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`card`**: Card number.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a `GetCardResponse` if the request was processed, an error otherwise. Returns
a "card not found" error if a record matching the card number does not exist on the controller.

### Examples

```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let card = 10058400u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; debug = true }

match GetCard controller card timeout options with
| Ok response -> printfn "get-card: ok %A" response
| Error err -> printfn "get-card: error %A" err
```

```csharp
var controller = new ControllerBuilder(405419896).build();
var card = 10058400u
var timeout = 5000
var options = new OptionsBuilder().build();

var result = GetCard(controller, card, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"get-card: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-card: error {result.ErrorValue}");
}
```

```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim card = 10058400u
Dim timeout = 5000
Dim options As New OptionsBuilder().build()

Dim result = GetCard(controller, card, timeout, options)

If result.IsOk Then
    Console.WriteLine($"get-card: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-card: error {result.ErrorValue}")
End If
```

### Notes
- The `GetCardResponse` record includes the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `card` (`uint32`): Card number.
  - `startdate` (`DateOnly`): Date from which card is valid.
  - `enddate` (`DateOnly`): Date after which card is no longer valid.
  - `door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `PIN (`uint32`): Optional card PIN (0 for _none_).

