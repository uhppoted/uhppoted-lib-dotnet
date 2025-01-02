## Types

### `C`: controller information for requests.

Utility container struct for controller IPv4 endpoint and transport information for 'connected UDP' or TCP/IP
requests.

- **`controller` (`uint32`)**: Controller serial number.
- **`endpoint` (`Option<IPEndPoint>`)**: Optional IPv4 controller address:port. Required if the controller is not accessible via UDP broadcast.
- **`protocol` (`Option<string>`)**: Optional 'protocol' to connect to controller. Valid values are currently 'udp' or 'tcp', defaults to 'udp'.


### `Controller`

Controller system information.

- **`Controller` (`uint32`)**: Controller serial number.
- **`Address` (`IPAddress`)**: IPv4 address.
- **`Netmask` (`IPAddress`)**: IPv4 subnet mask.
- **`Gateway` (`IPAddress`)**: IPv4 gateway address.
- **`MAC` (`PhysicalAddress`)**: Controller MAC address.
- **`Version` (`string`)**: Firmware version.
- **`Date` (`DateOnly Nullable`)**: Firmware release date.


### `Door`

Controller door configuration information.

- **`Mode` (`DoorMode`)**: Door control mode (normally open, normally closed or controlled).
- **`Delay` (`uint8`)**: Door unlocked delay (seconds).


### `Status`
Controller current state information.

- **`Door1Open` (`bool`)**: Door 1 open state
- **`Door2Open` (`bool`)**: Door 2 open state.
- **`Door3Open` (`bool`)**: Door 3 open state.
- **`Door4Open` (`bool`)**: Door 4 open state.
- **`Button1Pressed` (`bool`)**: Door 1 button state.
- **`Button2Pressed` (`bool`)**: Door 2 button state.
- **`Button3Pressed` (`bool`)**: Door 3 button state.
- **`Button4Pressed` (`bool`)**: Door 4 button state.
- **`SystemError` (`uint8`)**: System error code.
- **`SystemDateTime` (`DateTime Nullable`)**: Controller system date/time
- **`SpecialInfo` (`uint8`)**: 'special info' code.
- **`Relays` (`Relays`)**: Door unlock relays state.
- **`Inputs` (`Inputs`)**: Extender inputs state.

`Relays`
- **`Door1` (`Relay`)**
- **`Door2` (`Relay`)**
- **`Door3` (`Relay`)**
- **`Door4` (`Relay`)**

`Inputs`
- **`LockForced` (`Input`)**: Door lock forced.
- **`FireAlarm` (`Input')**: Fire alarm.


### `Event`
Event information.

- **`Timestamp` (`DateTime Nullable`)**: Event timestamp.
- **`Index` (`uint32`)**: Event index.
- **`Event` (`EventType`)**: Event type.
- **`AccessGranted` (`bool`)**: Access granted/refused for 'swipe' events.
- **`Door` (`uint8`)**: Event door.
- **`Direction` (`Direction`)**: Direction (in/out) for access events.
- **`Card` (`uint32`)**: Card number for 'swipe' events.
- **`Reason` (`EventReason`)**: Event reason.

`EventType`
Container 'type' for event types.

- **`Code` (`uint8`)**: Event type code.
- **`Text` (`string`)**: Event type description.

`EventReason`
Container 'type' for event reason.

- **`Code` (`uint8`)**: Event reason code.
- **`Text` (`string`)**:Event reason description.


### `Card`
Access card information.

- **`Card` (`uint32`)**: Card number.
- **`StartDate` (`DateOnly Nullable`)**: Date from which card is valid.
- **`EndDate` (`DateOnly Nullable`)**: Date after which card is no longer valid.
- **`Door1` (`uint8`)**: Access permissions for door 1 (0:none, 1:24/7, 2-254: time profile).
- **`Door2` (`uint8`)**: Access permissions for door 2 (0:none, 1:24/7, 2-254: time profile).
- **`Door3` (`uint8`)**: Access permissions for door 3 (0:none, 1:24/7, 2-254: time profile).
- **`Door4` (`uint8`)**: Access permissions for door 4 (0:none, 1:24/7, 2-254: time profile).
- **`PIN` (`uint32`)**: Optional PIN code for card reader keypad (1-999999, 0 for none).


### `TimeProfile`
Time profile configuration information.

- **`Profile` (`uint8`)**: Time profile ID (2-254)
- **`StartDate` (`DateOnly Nullable`)**: Date from which profile is active.
- **`EndDate` (`DateOnly Nullable`)**: Date after which profile is no longer active.
- **`Monday` (`bool`)**: Profile is active on Mondays if true.
- **`Tuesday` (`bool`)**: Profile is active on Tuesdays if true.
- **`Wednesday` (`bool`)**: Profile is active on Wednesdays if true.
- **`Thursday` (`bool`)**: Profile is active on Thursdays if true.
- **`Friday` (`bool`)**: Profile is active on Fridays if true.
- **`Saturday` (`bool`)**: Profile is active on Saturdays if true.
- **`Sunday` (`bool`)**: Profile is active on Sundays if true.
- **`Segment1Start` (`TimeOnly Nullable`)**: Start time for first active time segment (HH:mm).
- **`Segment1End` (`TimeOnly Nullable`)**: End time for first active time segment (HH:mm).
- **`Segment2Start` (`TimeOnly Nullable`)**: Start time for second active time segment (HH:mm).
- **`Segment2End` (`TimeOnly Nullable`)**: End time for second active time segment (HH:mm).
- **`Segment3Start` (`TimeOnly Nullable`)**: Start time for third active time segment (HH:mm).
- **`Segment3End` (`TimeOnly Nullable`)**: End time for third active time segment (HH:mm).
- **`LinkedProfile` (`uint8`)**: Profile ID of 'extension' profile (0 if none).


### `Task`
Scheduled task configuration information.

- **`Task` (`TaskCode`)**: Task activity code.
- **`Door` (`uint8`)**: Door (1-4) to which task is assigned.
- **`StartDate` (`DateOnly Nullable`)**: Date from which task is enabled.
- **`EndDate` (`DateOnly Nullable`)**: Date after which task is no longer enabled.
- **`StartTime` (`TimeOnly Nullable`)**: Time at which task triggers.
- **`Monday` (`bool`)**: Triggers on Mondays if true.
- **`Tuesday` (`bool`)**: Triggers on Tuesdays if true.
- **`Wednesday` (`bool`)**: Triggers on Wednesdays if true.
- **`Thursday` (`bool`)**: Triggers on Thursdays if true.
- **`Friday` (`bool`)**: Triggers on Fridays if true.
- **`Saturday` (`bool`)**: Triggers on Saturdays if true.
- **`Sunday` (`bool`)**: Triggers on Sundays if true.
- **`MoreCards` (`uint8`)**: Number of 'more cards' for 'more cards' task.


### `Listener`
Event listener configuration information.

- **`Endpoint` (`IPEndPoint`)**: IPv4 address:port of event listener.
- **`Interval` (`uint8`)**: Interval (seconds) at which the controller state is sent to the event listener.


### `ListenerEvent`
'Listen' event information.

- **`Controller` (`uint32`)**: Originating controller ID.
- **`Status` (`Status`)**: Controller state at time of event.

- **`Event` (`Nullable<Event>`)**: Event that triggered the event. May be null in the rare condition 
     that the auto-send interval is not zero and the controller has no events.


