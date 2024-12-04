## `SetPCControl`

Enables/disables remote access control management. The access controller will revert to standalone access
control managment if it does not receive a command from the 'PC' at least every 30 seconds.


### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`enable` (`bool`)**: Enables (or disables) remote access control managment.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok` with a `true` if the access controller has delegated access control to the remote system.
- `Ok` with `false`  if the access controller retains access control management.
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let enable = true
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match SetPCControl controller enable timeout options with
| Ok ok -> printfn "set-pc-control: ok %A" ok
| Error err -> printfn "set-pc-control: error %A" err
```

```csharp
var controller = 405419896u;
var enable = true;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = RefreshTaskList(controller, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim enable = True
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetPCControl(controller, enable, timeout, options)

If result.IsOk Then
    Console.WriteLine($"set-pc-control: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-pc-control: error {result.ErrorValue}")
End If
```

### Notes
