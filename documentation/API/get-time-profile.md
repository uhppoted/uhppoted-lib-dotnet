## `GetTimeProfile`

Retrieves an access time profile from the controller.

### Parameters
- **`controller` (`T`)**: Controller ID (`uint32`) or `struct` with controller ID, endpoint and protocol.
- **`profile` (`uint8`)**: Time profile ID [2.254].
- **`options` (`Options`)**: Bind, broadcast, and listen addresses.

### Returns
Returns `Ok` with a Nullable `TimeProfile` record if the request was processed or an `Error` 

The `Ok` value is:
- A `TimeProfile` record if an access time profile exists for the requested profile ID.
- `null` if there was no record at the index.

The `TimeProfile` record has the following fields:
  - `profile` (`uint8`): profile ID ([2.254])
  - `start_date` (`DateOnly`): date from which the access time profile is activated
  - `end_date` (`DateOnly`): date after which the access time profile is no longer activated
  - `monday` (`bool`): `true` if the access time profile is activatated on Monday
  - `tuesday` (`bool`):  `true` if the access time profile is activatated on Tuesday
  - `wednesday` (`bool`):  `true` if the access time profile is activatated on Wednesday
  - `thursday` (`bool`):  `true` if the access time profile is activatated on Thursday
  - `friday` (`bool`):  `true` if the access time profile is activatated on Friday
  - `saturday` (`bool`):  `true` if the access time profile is activatated on Saturday
  - `sunday` (`bool`):  `true` if the access time profile is activatated on Sunday
  - `segment1_start` (`TimeOnly Nullable`):  start time of first time segment
  - `segment1_end` (`TimeOnly Nullable`): end time of first time segment
  - `segment2_start` (`TimeOnly Nullable`): start time of second time segment
  - `segment2_end` (`TimeOnly Nullable`): end time of second time segment
  - `segment3_start` (`TimeOnly Nullable`): start time of third time segment
  - `segment3_end` (`TimeOnly Nullable`): end time of third time segment
  - `linked_profile` (`uint8`): linked time profile ID (0 if none)


### Examples

#### F#
```fsharp
let profile = 29uy
let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol:Some("tcp") }

match GetTimeProfile 405419896u profile options with
| Ok response when response.HasValue -> printfn "get-time-profile: ok %A" response.Value
| Ok _ -> printfn "get-time-profile: not found"
| Error err -> printfn "get-time-profile: error %A" err

match GetTimeProfile controller profile options with
| Ok response when response.HasValue -> printfn "get-time-profile: ok %A" response.Value
| Ok _ -> printfn "get-time-profile: not found"
| Error err -> printfn "get-time-profile: error %A" err
```

#### C#
```csharp
var profile = 29u;
var options = new OptionsBuilder().WithTimeout(1250).build();

var controller = new uhppoted.CBuilder(405419896u)
                              .WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000"))
                              .WithProtocol("udp")
                              .Build()

var result = GetTimeProfile(405419896u, profile, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-time-profile: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-time-profile: error 'not found'");
}
else
{
    Console.WriteLine($"get-time-profile: error '{result.ErrorValue}'");
}

var result = GetTimeProfile(controller, profile, options);
if (result.IsOk && result.ResultValue.HasValue)
{
    Console.WriteLine($"get-time-profile: ok {result.ResultValue.Value}");
}
else if (result.IsOk)
{
    Console.WriteLine($"get-time-profile: error 'not found'");
}
else
{
    Console.WriteLine($"get-time-profile: error '{result.ErrorValue}'");
}
```

#### VB.NET
```vb
Dim controller As New CBuilder(405419896UI).
                      WithEndPoint(IPEndPoint.Parse("192.168.1.100:60000")).
                      WithProtocol("udp").
                      Build()

Dim profile = 29
Dim options As New OptionsBuilder().WithTimeout(1250).build()

Dim result = GetTimeProfile(405419896UI, profile, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-time-profile: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-time-profile: error 'not found'")
Else
    Console.WriteLine($"get-time-profile: error '{result.ErrorValue}'")
End If

Dim result = GetTimeProfile(controller, profile, options)
If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-time-profile: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-time-profile: error 'not found'")
Else
    Console.WriteLine($"get-time-profile: error '{result.ErrorValue}'")
End If
```

### Notes
