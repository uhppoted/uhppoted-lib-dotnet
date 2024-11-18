# API

- [`FindControllers`](find-controllers.md)
- [`GetController`](get-controller.md)
- [`set_door_passcodes`](#set_door_passcodes)
- [`open_door`](#open_door)
- [`get_status`](#get_status)
- [`get_cards`](#get_cards)
- [`GetCard`](get-card.md)
- [`GetCardAtIndex`](get-card-at-index.md)
- [`PutCard`](put-card.md)
- [`DeleteCard`](delete-card.md)


## `set_door_passcodes`

Sets up to 4 passcodes for a controller door.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`door`**: Door number `[1..4]`.
- **`passcode1`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode2`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode3`**: Passcode `[0..999999]` (0 is 'none').
- **`passcode4`**: Passcode `[0..999999]` (0 is 'none').
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns `Ok` if the request was processed, or an error otherwise. The `Ok` response should be checked for `true`.

### Examples
```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
match set_door_passcodes controller 4uy 12345u 54321u 0u 999999u 5000 options with
| Ok response -> printfn "set door passcodes %A" response.ok
| Error err -> printfn "error setting door passcodes: %A" err
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = set_door_passcodes(controller, 4, 12345, 54321, 0, 999999, 5000, options);
if (result.IsOk)
{
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue.ok}");
}
else
{
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}");
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = set_door_passcodes(controller, 4, 12345UI, 54321UI, 0UI, 999999UI, 5000, options)
If result.IsOk Then
    Console.WriteLine($"set-door-passcodes: ok {result.ResultValue.ok}")
Else
    Console.WriteLine($"set-door-passcodes: error {result.ErrorValue}")
End If
```

## **`open_door`**

Unlocks a door controlled by a controller.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`door`**: Door number `[1..4]`.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns Ok if the request was processed, error otherwise. The Ok response should be checked for 'true'.

### Examples

```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
let result = open_door controller 4uy 5000 options
match result with
| Ok response -> printfn "open-door: ok %A" response
| Error e -> printfn "open-door: error %A" e
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = open_door(controller, 1, 5000, options);
if (result.IsOk)
{
    Console.WriteLine("open-door: ok {0}", result.Value.ok);
}
else
{
    Console.WriteLine("open-door: error {0}", result.Error);
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = open_door(controller, 1, 5000, options)
If result.IsOk Then
    Console.WriteLine("open-door: ok {0}", result.Value.ok)
Else
    Console.WriteLine("open-door: error {0}", result.Error)
End If
```

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

## **`get_cards`**

Retrieves the number of cards stored on a controller.

### Parameters
- **`controller`**: Controller ID and (optionally) address and transport protocol.
- **`timeout`**: Operation timeout (ms).
- **`options`**: Optional bind, broadcast, and listen addresses.

### Returns
Returns Ok and the number of cards stored on the controller if the request was processed, error otherwise.

### Examples

```fsharp
let controller = { controller = 405419896u; address = None; protocol = None }
let options = { broadcast = IPAddress.Broadcast; debug = true }
let result = get-cards controller 5000 options
match result with
| Ok response -> printfn "get-cards: ok %A" response
| Error e -> printfn "get-cards: error %A" e
```
```csharp
var controller = new ControllerBuilder(405419896).build();
var options = new OptionsBuilder().build();
var result = get_cards(controller, 5000, options);
if (result.IsOk)
{
    Console.WriteLine("get-cards: ok {0}", result.Value.ok);
}
else
{
    Console.WriteLine("get-cards: error {0}", result.Error);
}
```
```vb
Dim controller As New ControllerBuilder(405419896u).build()
Dim options As New OptionsBuilder().build()
Dim result = get_cards(controller, 5000, options)
If result.IsOk Then
    Console.WriteLine("get-cards: ok {0}", result.Value.ok)
Else
    Console.WriteLine("get-cards: error {0}", result.Error)
End If
```

### Notes
- The `GetCardsResponse` record includes the following fields:
  - `controller` (`uint32`): The controller identifier.
  - `cards` (`uint32`): Number of cards stored on the controller.
