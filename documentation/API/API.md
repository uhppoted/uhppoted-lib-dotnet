# API

- [`FindControllers`](find-controllers.md)
- [`GetController`](get-controller.md)
- [`SetIPv4`](set-IPv4.md)
- [`GetListener`](get-listener.md)
- [`SetListener`](set-listener.md)
- [`GetTime`](get-time.md)
- [`SetTime`](set-time.md)
- [`GetDoor`](get-door.md)
- [`SetDoor`](set-door.md)
- [`SetDoorPasscodes`](set-door-passcodes.md)
- [`OpenDoor`](open_door.md)
- [`get_status`](#get_status)
- [`GetCards`](get-cards.md)
- [`GetCard`](get-card.md)
- [`GetCardAtIndex`](get-card-at-index.md)
- [`PutCard`](put-card.md)
- [`DeleteCard`](delete-card.md)
- [`DeleteAllCards`](delete-all-cards.md)
- [`GetEvent`](get-event.md)
- [`GetEventIndex`](get-event-index.md)
- [`SetEventIndex`](set-event-index.md)
- [`RecordSpecialEvents`](record-special-events.md)
- [`GetTimeProfile`](get-time-profile.md)
- [`SetTimeProfile`](set-time-profile.md)
- [`ClearTimeProfiles`](clear-time-profiles.md)
- [`AddTask`](add-task.md)
- [`ClearTaskList`](clear-tasklist.md)
- [`RefreshTaskList`](refresh-tasklist.md)


## **`get_status`**

Retrieves a controller status record.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns Ok and the controller status record if the request was processed, error otherwise.

### Examples

```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
let result = get-status controller 5000 options
match result with
| Ok response -> printfn "get-status: ok %A" response
| Error e -> printfn "get-status: error %A" e
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = get_status(controller, 5000, options);
if (result.IsOk)
{
    Console.WriteLine("get-status: ok {0}", result.Value.ok);
}
else
{
    Console.WriteLine("get-status: error {0}", result.Error);
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = get_status(controller, 5000, options)
If result.IsOk Then
    Console.WriteLine("get-status: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-status: error {0}", result.Error)
End If
```

### Notes
- The `GetStatusResponse` record includes the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `door1_open` (`bool`): `true`if the door 1 open contact is set.
  - `door2_open` (`bool`): `true`if the door 2 open contact is set.
  - `door3_open` (`bool`): `true`if the door 3 open contact is set.
  - `door4_open` (`bool`): `true`if the door 4 open contact is set.
  - `door1_button` (`bool`): `true`if the door 1 button is pressed.
  - `door2_button` (`bool`): `true`if the door 2 button is pressed.
  - `door3_button` (`bool`): `true`if the door 3 button is pressed.
  - `door4_button` (`bool`): `true`if the door 4 button is pressed.
  - `system_error` (`uint8`): System error code (0 for none).
  - `system_datetime` (`Nullable<DateOnly>`): Current controller date/time.
  - `sequence_number` (`uint32`): Message sequence number.
  - `special_info` (`uint8`): Special info code.
  - `relays` (`uint8`) Bitmask for door relays 1 to 4.
  - `inputs` (`uint8`) Bitmask for door inputs 1 to 4.
  - `evt.index` (`uint32`) Index of most recent event (0 if none).
  - `evt.event_type (`uint8`) Event type.
  - `evt.granted (`bool`) Event access granted.
  - `evt.door (`uint8`) Event door.
  - `evt.direction (`uint8`) Event direction (IN/OUT).
  - `evt.card (`uint32`) Event card (0 if none).
  - `evt.timestamp (`DateTime Nullable`) Event timestamp.
  - `evt.reason (`uint8`) Event reason code.

