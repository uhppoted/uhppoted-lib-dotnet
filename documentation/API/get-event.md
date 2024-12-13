## `GetEvent`

Retrieves the event record (if any) at the index from the controller.

### Parameters
- **`controller` (`T`)**: Controller ID or struct with controller ID, endpoint and protocol.
- **`index` (`uint32`)**: Event index.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `Event` record if the request was processed or an `Error` 

The `Ok` value is:
- An `Event` record if an event was found at the index.
- `null` if there was no record at the index.
- `null` if the record at the index was deleted.

The `Event` record has the following fields:
  - `Timestamp` (`DateTime`): Timestamp of event.
  - `Index` (`uint32`): Event index.
  - `Event` (`uint8`): Event type.
  - `AccessGranted` (`bool`): `true` if access to the door was granted.
  - `Door` (`uint8`): Door [1.4] for event.
  - `Direction` (`Direction`): `In` or `Out`.
  - `Card` (`uint32`): Card number.
  - `Reason (`uint8`): Reason code for access granted/denied.


### Examples

```fsharp
let controller = 405419896u
let index = 13579u
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match GetEvent controller index options with
| Ok response when response.HasValue -> printfn "get-event: ok %A" response.Value
| Ok _ -> printfn "get-event: not found"
| Error err -> printfn "get-event: error %A" err
```

```csharp
var controller = 405419896u;
var index = 13579u;
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = GetEvent(controller, index, options);

if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-event: error 'not found'");
}
else
{
    Console.WriteLine($"get-event: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
Dim index = 13579
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = GetEvent(controller, index, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-event: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-event: error 'not found'")
Else
    Console.WriteLine($"get-event: error '{result.ErrorValue}'")
End If
```

### Notes
