## `ActivateKeypads`

Activates/deactivates the access reader keypads attached to an access controller.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`reader1` (`bool`)**: Activate the keypad for reader 1 if `true`.
- **`reader2` (`bool`)**: Activate the keypad for reader 2 if `true`.
- **`reader3` (`bool`)**: Activate the keypad for reader 3 if `true`.
- **`reader4` (`bool`)**: Activate the keypad for reader 4 if `true`.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the keypads were activated
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let reader1 = true
let reader2 = true
let reader3 = false
let reader4 = true
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match ActivateKeypads controller reader1 reader2 reader3 reader4 timeout options with
| Ok ok -> printfn "activate-keypads: ok %A" ok
| Error err -> printfn "activate-keypads: error %A" err
```

```csharp
var controller = 405419896u;
var reader1 = true
var reader2 = true
var reader3 = false
var reader4 = true
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = ActivateKeypads(controller, reader1, reader2, reader3, reader4, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim reader1 = True
Dim reader2 = True
Dim reader3 = False
Dim reader4 = True
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = ActivateKeypads(controller, reader1, reader2, reader3, reader4, timeout, options)

If result.IsOk Then
    Console.WriteLine($"activate-keypads: ok {result.ResultValue}")
Else
    Console.WriteLine($"activate-keypads: error {result.ErrorValue}")
End If
```

### Notes
