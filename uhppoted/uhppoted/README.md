# uhppoted

.NET package for the UHPPOTE access controller API.

The API is described in [API](API.md) and example CLI implementations in F#, C# and VB.NET that 
illustrate the use of the API can be found in the [examples](https://github.com/uhppoted/uhppoted-lib-dotnet/examples)
folder on the github [repository](https://github.com/uhppoted/uhppoted-lib-dotnet).

## Installation

## Release Notes

## API summary

### [`FindControllers`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/find-controllers.md))
'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

### [`GetController`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-controller.md))
Retrieves the IPv4 configuration, MAC address and version information for an access controller.

### [`SetIPv4`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/set-IPv4.md))
Sets a controller IPv4 address, netmask and gateway address.

### [`GetListener`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-listener.md))
Gets a controller event listener endpoint and auto-send interval.

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

### [`GetCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-card.md))
Retrieves a card record from a controller by card number.

### [`GetCardAtIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-card-at-index.md))
Retrieves the card record (if any) at the index from a controller.

### [`PutCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/put-card.md))
Adds or updates a card record stored on a controller.

### [`DeleteCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/delete-card.md))
Deletes a card record from a controller.

### [`DeleteAllCards`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/delete-all-cards.md))
Deletes all card records from a controller.

### [`GetEvent`](https://github.com/uhppoted/uhppoted-lib-dotnet/documentation/API/get-event.md))
Retrieves the event record (if any) at the index from a controller.

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
