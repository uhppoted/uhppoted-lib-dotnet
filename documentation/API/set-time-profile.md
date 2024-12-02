## `SetTimeProfile`

Adds or updates an access time profile on a controller.

### Parameters
- **`controller`**: Controller ID.
- **`profile`**: Time profile.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

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


### Returns
Returns `Ok` with `true` if the time profile was updated or an `Error` 

### Examples

```fsharp
let controller = 405419896u
let profile: TimeProfile =
    { profile = 37uy
      start_date = Nullable(DateOnly(2024, 11, 26))
      end_date = Nullable(DateOnly(2024, 12, 29))
      monday = true
      tuesday = true
      wednesday = false
      thursday = true
      friday = false
      saturday = true
      sunday = true
      segment1_start = Nullable(TimeOnly(8, 30))
      segment1_end = Nullable(TimeOnly(09, 45))
      segment2_start = Nullable(TimeOnly(11, 35))
      segment2_end = Nullable(TimeOnly(13, 15))
      segment3_start = Nullable(TimeOnly(14, 01))
      segment3_end = Nullable(TimeOnly(17, 59))
      linked_profile = 19uy }

let timeout = 5000
let options = { broadcast = IPAddress.Broadcast; destination=None; protocol=None; debug = true }

match SetTimeProfile controller profile timeout options with
| Ok response -> printfn "set-time-profile: ok %A" response.Value
| Error err -> printfn "set-time-profile: error %A" err
```

```csharp
var controller = 405419896u;
var profile = new TimeProfileBuilder(29)
                  .WithStartDate(DateOnly.Parse("2024-01-01"))
                  .WithEndDate(DateOnly.Parse("2024-12-31"))
                  .WithWeekdays(true,true,false,false,true,false,false)
                  .WithSegment1(TimeOnly.Parse("08:30"),TimeOnly.Parse("11:30"))
                  .WithSegment2(TimeOnly.Parse("13:30"),TimeOnly.Parse("17:30"))
                  .WithSegment2(TimeOnly.Parse("19:15"),TimeOnly.Parse("21:45"))
                  .WithLinkedProfile(37)
                  .build();
var timeout = 5000;
var options = new OptionsBuilder().build();
var result = SetTimeProfile(controller, profile, timeout, options);

if (result.IsOk)
{
    Console.WriteLine($"set-time-profile: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"set-time-profile: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
DIm profile = New TimeProfileBuilder(29).
                  WithStartDate(DateOnly.Parse("2024-01-01")).
                  WithEndDate(DateOnly.Parse("2024-12-31")).
                  WithWeekdays(true,true,false,false,true,false,false).
                  WithSegment1(TimeOnly.Parse("08:30"),TimeOnly.Parse("11:30")).
                  WithSegment2(TimeOnly.Parse("13:30"),TimeOnly.Parse("17:30")).
                  WithSegment2(TimeOnly.Parse("19:15"),TimeOnly.Parse("21:45")).
                  WithLinkedProfile(37).
                  build()
Dim timeout = 5000
Dim options As New OptionsBuilder().build()
Dim result = SetTimeProfile(controller, profile, timeout, options)

If (result.IsOk) Then
    Console.WriteLine($"set-time-profile: ok {result.ResultValue}")
Else
    Console.WriteLine($"set-time-profile: error '{result.ErrorValue}'")
End If
```

### Notes