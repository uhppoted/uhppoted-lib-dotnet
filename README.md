![build](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/build/badge.svg)

# uhppoted-lib-dotnet

** IN DEVELOPMENT **

.NET package for the UHPPOTE access controller API.

The API is described in [API.md](documentation/API/API.md) and example CLI implementations in F#, C# and VB.NET that 
illustrate the use of the API can be found in the [examples](examples) folder.

## Installation

## Release Notes

#### Current Release

## API summary

### Usage


### [`find-controllers`](documentation/API/find-controllers.md)
'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### `get-controller`
Gets a controller IPv4 configuration, MAC address and version information.

### `set-IPv4`
Sets a controller IPv4 configuration.

### `get-listener`
Gets a controller event listener address:port and auto-send interval.

### `set-listener`
Sets a controller event listener address:port and auto-send interval.

### `get-time`
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

### `get-cards`
Retrieves the number of card records stored on a controller.

### [`get-card`](documentation/API/get-card.md)
Retrieves a card record by card number.

### [`get-card-at-index`](documentation/API/get-card-at-index.md)
Retrieves the card record (if any) at the index in the cards list stored on the controller.

### [`put-card`](documentation/API/put-card.md)
Adds or updates a card record on a controller.

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
