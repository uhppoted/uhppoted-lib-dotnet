## `GetCardAtIndex`

Retrieves the card record (if any) at the index in the cards list stored on the controller.

### Parameters
- **`controller`**: Controller ID.
- **`index`**: Card index.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a Nullable `Card` if the request was processed or an error otherwise. 

The `Ok` value is a card record if a card record was found at the index
- `null` if there was no record at the index
- `null` if the record at the index was deleted

### Examples

```fsharp
let controller = 405419896u
let index = 1u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetCardAtIndex controller index timeout options with
| Ok response when response.HasValue -> printfn "get-card-at-index: ok %A" response.Value
| Ok _ -> printfn "get-card-at-index: not found"
| Error err -> printfn "get-card-at-index: error %A" err
```

```csharp
var controller = 405419896u;
var index = 1u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetCardAtIndex(controller, index, timeout, options);

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
Dim controller 405419896
Dim index = 1
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetCardAtIndex(controller, index, timeout, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-card-at-index: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-card-at-index: error 'not found'")
Else
    Console.WriteLine($"get-card-at-index: error '{result.ErrorValue}'")
End If
```

### Notes
- The `Card` struct is defined as:
  - `card` (`uint32`): Card number.
  - `startdate` (`DateOnly`): Date from which card is valid.
  - `enddate` (`DateOnly`): Date after which card is no longer valid.
  - `door1` (`uint8`): Door 1 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door2` (`uint8`): Door 2 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door3` (`uint8`): Door 3 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `door4` (`uint8`): Door 4 access permission (0: NONE, 1: ALWAYS, [2..254]: time profile).
  - `PIN (`uint32`): Optional card PIN (0 for _none_).

