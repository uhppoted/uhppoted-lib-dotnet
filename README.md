![build](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/build/badge.svg)
![nightly](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/nightly/badge.svg)

# uhppoted-lib-dotnet

** IN DEVELOPMENT **

.NET package for the UHPPOTE access controller API.

The API is described in [API.md](documentation/API/API.md) and example CLI implementations in F#, C# and VB.NET that 
illustrate the use of the API can be found in the [examples](examples) folder.

## Installation

### Building from source

NTS:
1. To build with .NET 7 SDK
```
export DOTNET7=true
```

## Release Notes

#### Current Release

## API summary

### Usage


### [`FindControllers`](documentation/API/find-controllers.md)
'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### [`GetController`](documentation/API/get-controller.md)
Retrieves the IPv4 configuration, MAC address and version information for an access controller.

### [`SetIPv4`](documentation/API/set-IPv4.md)
Sets a controller IPv4 address, netmask and gateway address.

### [`GetListener`](documentation/API/get-listener.md)
Gets a controller event listener address:port and auto-send interval.

### [`SetListener`](documentation/API/set-listener.md)
Sets a controller event listener endpoint and auto-send interval.

### [`GetTime`](documentation/API/get-time.md)
Gets a controller current date and time.

### [`SetTime`](documentation/API/set-time.md)
Sets a controller current date and time.

### [`GetDoor`](documentation/API/get-door.md)
Gets a controller door operational mode and unlocked delay.

### [`SetDoor`](documentation/API/set-door.md)
Sets a controller door operational mode and unlocked delay.

### [`SetDoorPasscodes`](documentation/API/set-door-passcodes.md)
Sets up to 4 passcodes for a controller door.

### [`OpenDoor`](documentation/API/open-door.md)
Unlocks a door controlled by a controller.

### [`GetStatus`](documentation/API/get-status.md)
Retrieves a controller status and most recent event (if any).

### [`GetCards`](documentation/API/get-cards.md)
Retrieves the number of card records stored on a controller.

### [`GetCard`](documentation/API/get-card.md)
Retrieves a card record by card number.

### [`GetCardAtIndex`](documentation/API/get-card-at-index.md)
Retrieves the card record (if any) at the index from a controller.

### [`PutCard`](documentation/API/put-card.md)
Adds or updates a card record on a controller.

### [`DeleteCard`](documentation/API/delete-card.md)
Deletes a card record from a controller.

### [`DeleteAllCards`](documentation/API/delete-all-cards.md)
Deletes all card records from a controller.

### [`GetEvent`](documentation/API/get-event.md)
Retrieves the event record (if any) at the index from a controller.

### [`GetEventIndex`](documentation/API/get-event-index.md)
Retrieves the current event index from a controller.

### [`SetEventIndex`](documentation/API/set-event-index.md)
Sets a controller event index.

### [`RecordSpecialEvents`](documentation/API/record-special-events.md)
Enables (or disables) events for door open/close, button press, etc.

### [`GetTimeProfile`](documentation/API/get-time-profile.md)
Retrieves an access time profile from a controller.

### [`GetTimeProfile`](documentation/API/set-time-profile.md)
Adds or updates an access time profile on a controller.

### [`ClearTimeProfiles`](documentation/API/clear-time-profiles.md)
Clears all access time profiles stored on a controller.

### [`AddTask`](documentation/API/add-task.md)
Adds or updates a scheduled task on a controller.

### [`ClearTaskList`](documentation/API/clear-tasklist.md)
Clears all scheduled tasks from a controller tasklist.

### [`RefreshTaskList`](documentation/API/refresh-tasklist.md)
Schedules added tasks.


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
