## `GetCards`

Retrieves the number of card records stored on the controller.

### Parameters
- **`controller`**: Controller ID.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the number of cards stored on the controller or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetCards controller timeout options with
| Ok response -> printfn "get-cards: ok %A" response.Value
| Error err -> printfn "get-cards: error %A" err
```

```csharp
var controller = 405419896u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetCards(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"get-cards: ok {result.ResultValue.Value}");
}
else
{
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetCards(controller, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"get-cards: ok {result.ResultValue.Value}")
Else
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'")
End If
```
