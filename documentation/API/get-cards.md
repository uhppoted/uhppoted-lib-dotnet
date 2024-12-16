## `GetCards`

Retrieves the number of card records stored on the controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with the number of cards stored on the controller or `Error`. 

### Examples

```fsharp
let controller = 405419896u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetCards controller options with
| Ok cards -> printfn "get-cards: ok %A" cards
| Error err -> printfn "get-cards: error %A" err
```

```csharp
var controller = 405419896u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetCards(controller, options);

if (result.IsOk)
{
    Console.WriteLine($"get-cards: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetCards(controller, options)

If (result.IsOk) Then
    Console.WriteLine($"get-cards: ok {result.ResultValue}")
Else
    Console.WriteLine($"get-cards: error '{result.ErrorValue}'")
End If
```
