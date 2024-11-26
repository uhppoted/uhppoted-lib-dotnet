## `GetTimeProfile`

Retrieves an access time profile from the controller.

### Parameters
- **`controller`**: Controller ID.
- **`profile`**: Time profile ID [2..254].
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

### Returns
Returns `Ok` with a Nullable `TimeProfile` record if the request was processed or an `Error` 

The `Ok` value is:
- A `TimeProfile` record if an access time profile exists for the requested profile ID.
- `null` if there was no record at the index.

The `TimeProfile` record has the following fields:
  - `profile` (`uint8`): profile ID ([2..254])
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

```fsharp
let controller = 405419896u
let profile = 29uy
let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match GetTimeProfile controller profile timeout options with
| Ok response when response.HasValue -> printfn "get-time-profile: ok %A" response.Value
| Ok _ -> printfn "get-time-profile: not found"
| Error err -> printfn "get-time-profile: error %A" err
```

```csharp
var controller = 405419896u;
var profile = 29u;
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = GetTimeProfile(controller, profile, timeout, options);

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

```vb
Dim controller = 405419896
Dim profile = 29
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = GetTimeProfile(controller, profile, timeout, options)

If (result.IsOk And result.Value.HasValue) Then
    Console.WriteLine($"get-time-profile: ok {result.ResultValue.Value}")
Els If (result.IsOk) Then
    Console.WriteLine($"get-time-profile: error 'not found'")
Else
    Console.WriteLine($"get-time-profile: error '{result.ErrorValue}'")
End If
```

### Notes
