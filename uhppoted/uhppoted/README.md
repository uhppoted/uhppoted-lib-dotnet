# uhppoted

.NET package for the UHPPOTE access controller API.

The API is described in [API](API.md) and example CLI implementations in F#, C# and VB.NET that 
illustrate the use of the API can be found in the [examples](https://github.com/uhppoted/uhppoted-lib-dotnet/examples)
folder on the github [repository](https://github.com/uhppoted/uhppoted-lib-dotnet).

## Installation

## Release Notes

## API summary

### [`get-all-controllers`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-all-controllers.md))
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

### `get-card`
Retrieves a card record by card number.

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
