## `AddTask`

Adds or updates a scheduled task stored on a controller. Added tasks are not scheduled until the 
tasklist is refreshed.

### Parameters
- **`controller`**: Controller ID.
- **`task`**: Task definition.
- **`options`**: Bind, broadcast, and listen addresses and (optionally) controller address and transport protocol.

The `Task` record has the following fields:
  - `task` (`uint8`): task ID
  - `door` (`uint8`): door ID [1..4]
  - `start_date` (`DateOnly`): date from which the task is active
  - `end_date` (`DateOnly`): date after which the task is no longer active
  - `start_time` (`TimeOnly Nullable`):  time at which task is run
  - `monday` (`bool`): `true` if the task is scheduled on Monday
  - `tuesday` (`bool`):  `true` if the task is scheduled on Tuesday
  - `wednesday` (`bool`):  `true` if the task is scheduled on Wednesday
  - `thursday` (`bool`):  `true` if the task is scheduled on Thursday
  - `friday` (`bool`):  `true` if the task is scheduled on Friday
  - `saturday` (`bool`):  `true` if the task is scheduled on Saturday
  - `sunday` (`bool`):  `true` if the task is schedule on Sunday
  - `more_cards` (`uint8`): number of 'more cards' allowed for the _more cards_ task (0 if not required)

Valid tasks:
- 0: set door controlled
- 1: set door normally open
- 2: set door normally closed
- 3: disable time profile
- 4: enable time profile
- 5: card - no PIN required
- 6: card - IN PIN required
- 7: card - both IN and OUT PIN required
- 8: enable more cards
- 9: disable more cards
- 10: trigger once
- 11: disable push button
- 12: enable push button


### Returns
Returns `Ok` with `true` if the task was added or updated updated or an `Error` 

### Examples

```fsharp
let controller = 405419896u
let task: Task =
    { task = 4uy
      door = 3
      start_date = Nullable(DateOnly(2024, 11, 26))
      end_date = Nullable(DateOnly(2024, 12, 29))
      start_time = Nullable(TimeOnly(8, 30))
      monday = true
      tuesday = true
      wednesday = false
      thursday = true
      friday = false
      saturday = true
      sunday = true
      more_cards = 7uy }

let options = { broadcast = IPAddress.Broadcast; timeout = 1250; debug = true }

match AddTask controller task options with
| Ok response -> printfn "add-task: ok %A" response.Value
| Error err -> printfn "add-task: error %A" err
```

```csharp
var controller = 405419896u;
var task = new TaskBuilder(4,3)
                  .WithStartDate(DateOnly.Parse("2024-01-01"))
                  .WithEndDate(DateOnly.Parse("2024-12-31"))
                  .WithStartTime(TimeOnly.Parse("08:30"))
                  .WithWeekdays(true,true,false,false,true,false,false)
                  .WithMoreCards(7)
                  .build();
var options = new OptionsBuilder().WithTimeout(1250).build();
var result = AddTask(controller, task, options);

if (result.IsOk)
{
    Console.WriteLine($"add-task: ok {result.ResultValue}");
}
else
{
    Console.WriteLine($"add-task: error '{result.ErrorValue}'");
}
```

```vb
Dim controller = 405419896
DIm task = New TaskBuilder(4,3).
               WithStartDate(DateOnly.Parse("2024-01-01")).
               WithEndDate(DateOnly.Parse("2024-12-31")).
               WithStartTime(TimeOnly.Parse("08:30")).
               WithWeekdays(true,true,false,false,true,false,false).
               WithMoreCards(7).
               build()
Dim options As New OptionsBuilder().WithTimeout(1250).build()
Dim result = AddTask(controller, task, options)

If (result.IsOk) Then
    Console.WriteLine($"add-task: ok {result.ResultValue}")
Else
    Console.WriteLine($"add-task: error '{result.ErrorValue}'")
End If
```

### Notes
