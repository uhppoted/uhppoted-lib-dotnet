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

### `set-time`
Sets a controller current date and time.

### `get-door`
Gets a controller door operational mode and unlocked delay.

### `set-door`
Sets a controller door operational mode and unlocked delay.

### `set-door-passcodes`
Sets up to 4 passcodes for a controller door.

### `open-door`
Unlocks a door controlled by a controller.

### `get-status`
Retrieves a controller status record.

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

### [`RecordSpecialEvents`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/record-special-events.md))
Enables (or disables) events for door open/close, button press, etc.


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
