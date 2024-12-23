# uhppoted

.NET package for the UHPPOTE access controller API.

The API is described in [API](API.md) and example CLI implementations in F#, C# and VB.NET that 
illustrate the use of the API can be found in the [examples](https://github.com/uhppoted/uhppoted-lib-dotnet/examples)
folder on the github [repository](https://github.com/uhppoted/uhppoted-lib-dotnet).

## Installation

## Release Notes

## API summary

### [`FindControllers`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/find-controllers.md)
'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### [`GetController`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-controller.md)
Retrieves the IPv4 configuration, MAC address and version information for an access controller.

### [`SetIPv4`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-IPv4.md)
Sets a controller IPv4 address, netmask and gateway address.

### [`GetListener`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-listener.md)
Gets a controller event listener endpoint and auto-send interval.

### [`SetListener`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-listener.md)
Sets a controller event listener endpoint and auto-send interval.

### [`GetTime`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-time.md)
Gets a controller current date and time.

### [`SetTime`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-time.md)
Sets a controller current date and time.

### [`GetDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-door.md)
Gets a controller door operational mode and unlocked delay.

### [`SetDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-door.md)
Sets a controller door operational mode and unlocked delay.

### [`SetDoorPasscodes`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-door-passcodes.md)
Sets up to 4 passcodes for a controller door.

### [`OpenDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/open-door.md)
Unlocks a door controlled by a controller.

### [`GetStatus`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-status.md)
Retrieves a controller status and most recent event (if any).

### [`GetCards`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-cards.md)
Retrieves the number of card records stored on a controller.

### [`GetCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-card.md)
Retrieves a card record from a controller by card number.

### [`GetCardAtIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-card-at-index.md)
Retrieves the card record (if any) at the index from a controller.

### [`PutCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/put-card.md)
Adds or updates a card record stored on a controller.

### [`DeleteCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/delete-card.md)
Deletes a card record from a controller.

### [`DeleteAllCards`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/delete-all-cards.md)
Deletes all card records from a controller.

### [`GetEvent`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-event.md)
Retrieves the event record (if any) at the index from a controller.

### [`GetEventIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-event-index.md)
Retrieves the current event index from a controller.

### [`SetEventIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-event-index.md)
Set a controller event index.

### [`RecordSpecialEvents`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/record-special-events.md)
Enables (or disables) events for door open/close, button press, etc.

### [`GetTimeProfile`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-time-profile.md)
Retrieves an access time profile from a controller.

### [`SetTimeProfile`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-time-profile.md)
Adds or updates an access time profile on a controller.

### [`ClearTimeProfiles`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/clear-time-profiles.md)
Clears all access time profiles stored on a controller.

### [`AddTask`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/add-task.md)
Adds or updates a scheduled task on a controller.

### [`ClearTaskList`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/clear-tasklist.md)
Clears all scheduled tasks from a controller tasklist.

### [`RefreshTasklList`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/refresh-tasklist.md)
Schedules added tasks.

### [`SetPCControl`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-pc-control.md)
Enables/disables remote access control management. 

### [`SetInterlock`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-interlock.md)
Sets the door interlock mode for an access controller.

### [`ActivateKeypads`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/activate-keypads.md)
Activates/deactivates the access reader keypads attached to an access controller.

### [`RestoreDefaultSettings`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/restore-default-settings.md)
Restores the manufacturer default settings.

### [`Listen`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/listen.md)
Listens for access controller events.

## Notes

### C# Interop

1. May require the FSharp.Core package
```
dotnet add package FSharp.Core
```

### VB.NET Interop

1. May require the FSharp.Core package
```
dotnet add package FSharp.Core
```

[examples]:  https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples
