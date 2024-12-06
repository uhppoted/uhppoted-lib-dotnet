## `SetInterlock`

Sets the door interlocks for an access controller. Door interlocks prevent a door from being unlocked if one of
the other interlocked doors is already unlocked.


### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`interlock` (`Interlock`)**: Door interlock mode.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

The `interlock` parameter may take one of the following values:
- `Interlock.None`: no interlocks
- `Interlock.Doors12`: doors 1 & 2 are interlocked
- `Interlock.Doors34`: doors 3 & 4 are interlocked
- `Interlock.Doors12And34`: doors 1 &2 are interlocked and doors 3 & 4 are interlocked
- `Interlock.Doors123`: doors 1 & 2 & 3 are interlocked
- `Interlock.Doors1234`: doors 1 & 2 & 3 & 4 are interlocked

### Returns

Returns:
- `Ok` with a `true` if the access controller set the door interlock mode
- `Error` if the request failed.

### Examples

```fsharp
let controller = 405419896u
let interlock = Interlock.Doors123
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination = None; protoocol = None; debug = true }

match SetInterlock controller interlock timeout options with
| Ok ok -> printfn "set-interlock: ok %A" ok
| Error err -> printfn "set-interlock: error %A" err
```

```csharp
var controller = 405419896u;
var interlock = Interlock.Doors123;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = SetInterlock(controller, interlock, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"set-interlock: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-interlock: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim interlock = Interlock.Doors123
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetInterlock(controller, interlock, timeout, options)

If result.IsOk Then
    Console.WriteLine($"set-interlock: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-interlock: error {result.ErrorValue}")
End If
```

### Notes
