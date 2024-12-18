## `PutCard`

Adds or updates a card record on a controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`card` (`Card`)**: Card record.
- **`startdate`**: Date from which card is valid.
- **`enddate`**: Date after which card is no longer valid.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

A `Card` record has the following fields:
  - `Card` (`uint32`): Card number.
  - `StartDate` (`DateOnly`): Date from which card is valid.
  - `EndDate` (`DateOnly`): Date after which card is no longer valid.
  - `Door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `Door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2.254]: time profile).
  - `PIN (`uint32`): Optional card PIN (0 for _none_).

#### CardBuilder

`CardBuilder` is a utility class to simplify construction of a `Card` struct when using C# or VB.NET.

- **`WithStartDate(date: DateOnly)`: Sets the card _StartDate_.
- **`WithStartDate(date: DateOnly)`: Sets the card _EndDate_.
- **`WithDoor1(profile: uint8)`**: Sets the access profile for door 1.
- **`WithDoor2(profile: uint8)`**: Sets the access profile for door 2.
- **`WithDoor3(profile: uint8)`**: Sets the access profile for door 3.
- **`WithDoor4(profile: uint8)`**: Sets the access profile for door 4.
- **`WithPIN(pin: uint32)`**: Sets the optional card keypad PIN code.
- **`Build()`**: Builds the `Card` struct.

```csharp
static readonly Card card = new CardBuilder(10058400u)
                                .WithStartDate(DateOnly.Parse("2024-01-01"))
                                .WithEndDate(DateOnly.Parse("2024-12-31"))
                                .WithDoor1(1)
                                .WithDoor2(0)
                                .WithDoor3(17)
                                .WithDoor4(1)
                                .Build();
```

```vb
Private ReadOnly Dim card = New CardBuilder().
                                WithStartDate(DateOnly.Parse("2024-01-01")).
                                WithEndDate(DateOnly.Parse("2024-12-31")).
                                WithDoor1(1).
                                WithDoor2(0).
                                WithDoor3(17).
                                WithDoor4(1).
                                Build()
```


### Returns

Returns:
- `Ok` with the `true` if the card was added or updated
- `Ok` with `false` if the card was not added or updated
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let card: Card = {
    Card = 10058400u
    StartDate = Nullable(DateOnly(2024, 1, 1))
    EndDate = Nullable(DateOnly(2024, 12, 31))
    Door1 = 1uy
    Door2 = 0uy
    Door3 = 17uy
    Door4 = 1uy
    PIN = 7531u }
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match PutCard controller card options with
| Ok ok -> printfn "put-card: ok %A" ok
| Error err -> printfn "put-card: error %A" err
```

```csharp
var controller = 405419896u;
var card = new CardBuilder(10058400u)
               .WithStartDate(DateOnly.Parse("2024-01-01"))
               .WithEndDate(DateOnly.Parse("2024-12-31"))
               .WithDoor1(1)
               .WithDoor2(0)
               .WithDoor3(17)
               .WithDoor4(1)
               .Build();

var options = new OptionsBuilder().WithTimeout(1250).build();
var result = PutCard(controller, card, options);

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
Dim card = New CardBuilder().
               WithStartDate(DateOnly.Parse("2024-01-01")).
               WithEndDate(DateOnly.Parse("2024-12-31")).
               WithDoor1(1).
               WithDoor2(0).
               WithDoor3(17).
               WithDoor4(1).
               Build()

Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = PutCard(controller, card, options)

If result.IsOk Then
    Console.WriteLine($"put-card: ok {result.ResultValue}")
Else
    Console.WriteLine($"put-card: error {result.ErrorValue}")
End If
```

### Notes
