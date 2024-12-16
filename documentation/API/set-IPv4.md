## `SetIPv4`

Sets the controller IPv4 address, netmask and gateway address.

### Parameters
- **`controller` (`uint32`)**: Controller ID.
- **`address` (`IPAddress`)**: IPv4 address.
- **`netmask` (`IPAddress`)**: IPv4 netmask.
- **`gateway` (`IPAddress`)**: gateway IPv4 address.
- **`options` (`Options`)**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns

Returns:
- `Ok`
- `Error` if the request failed

### Examples

```fsharp
let controller = 405419896u
let address = IPAddress.Parse "192.168.1.100"
let netmask = IPAddress.Parse "255.255.255.0"
let gateway = IPAddress.Parse "192.168.1.1"
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match SetIPv4 controller address netmask gateway options with
| Ok record -> printfn "set-IPv4: ok"
| Error err -> printfn "set-IPv4: error %A" err
```

```csharp
var controller = 405419896u;
var address = IPAddress.Parse("192.168.1.100");
var netmask = IPAddress.Parse("255.255.255.0");
var gateway = IPAddress.Parse("192.168.1.1");
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = SetIPv4(controller, address, netmask, gateway, options);

if (result.IsOk)
{
    Console.WriteLine($"set-IPv4: ok");
}
else
{
    Console.WriteLine($"set-IPv4: error {result.ErrorValue}");
}
```

```vb
Dim controller = 405419896
Dim address = IPAddress.Parse("192.168.1.100")
Dim netmask = IPAddress.Parse("255.255.255.0")
Dim gateway = IPAddress.Parse("192.168.1.1")
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = SetIPv4(controller, address, netmask, gateway, options)

If result.IsOk Then
    Console.WriteLine($"set-IPv4: ok")
Else
    Console.WriteLine($"set-IPv4: error {result.ErrorValue}")
End If
```

### Notes

 The controller does not respond to the request so in the absence of other errors a _sent_ request is deemed successful.
 