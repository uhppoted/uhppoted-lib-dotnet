## `PutCard`

Adds or updates a card record on a controller.

### Parameters
- **`controller`**: Controller ID.
- **`card`**: Card number.
- **`startdate`**: Date from which card is valid.
- **`enddate`**: Date after which card is no longer valid.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with the `true` if the card was added or updated
- `Ok` with `false` if the card was not added or updated
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let card = 10058400u
let startdate = DateOnly(2024, 1, 1)
let enddate = DateOnly(2024, 12, 31)
let door1 = 1uy
let door2 = 0uy
let door3 = 17uy
let door4 = 1uy
let PIN = 7531u

let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match PutCard controller card startdate enddate door1 door2 door3 door4 PIN timeout options with
| Ok ok -> printfn "put-card: ok %A" ok
| Error err -> printfn "put-card: error %A" err
```

```csharp
var controller = 405419896u;
var card = 10058400u
var startdate = new DateOnly(2024, 1, 1);
var enddate = new DateOnly(2024, 12, 31);
byte door1 = 1;
byte door2 = 0;
byte door3 = 17;
byte door4 = 1;
var PIN = 7531u;

var timeout = 5000
var options = new OptionsBuilder().build();
var result = PutCard(controller, card, startdate, enddate, door1, door2, door3, door4, PIN, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"put-card: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"put-card: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim card = 10058400
Dim startdate = New DateOnly(2024, 1, 1)
Dim enddate = New DateOnly(2024, 12, 31)
Dim door1 = 1
Dim door2 = 0
Dim door3 = 17
Dim door4 = 1
Dim PIN = 7531

Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = PutCard(controller, card, startdate, enddate, door1, door2, door3, door4, PIN, timeout, options)

If result.IsOk Then
    Console.WriteLine($"put-card: ok {result.ResultValue}")
Else
    Console.WriteLine($"put-card: error {result.ErrorValue}")
End If
```

### Notes
