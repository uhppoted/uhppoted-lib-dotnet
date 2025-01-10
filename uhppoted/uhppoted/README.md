# uhppoted-dotnet

.NET package for the UHPPOTE access controller API:

- the API is described in [API.md](documentation/API/API.md)
- example CLI implementations in F#, C# and VB.NET that illustrate the use of the API can be found in the [examples](examples) folder.

## Release Notes

#### Current Release

_ALPHA_


## Installation

The package can be installed from both [_Nuget_](https://www.nuget.org/packages/uhppoted-dotnet) and 
_Github Packages_:

### Installing from NuGet

CLI:
```
dotnet add package uhppoted-dotnet --version 0.8.9.2-alpha
```

Package Manager:
```
NuGet\Install-Package uhppoted-dotnet -Version 0.8.9.2-alpha
```

After installing the package add it the _project_ file:
```
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted-dotnet" Version="0.8.9.*" />
  </ItemGroup>
  ...
```

### Installing from Github Packages

CLI:
```
```

Package Manager:
```
```

After installing the package add it the _project_ file:
```
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted-dotnet" Version="0.8.9.*" />
  </ItemGroup>
  ...
```

### Building from source

Requirements:
- .NET SDK 7.0+
- (optional) make

```
git clone https://github.com/uhppoted/uhppoted-lib-dotnet.git
cd uhppoted-lib-dotnet/uhppoted
make build
```

If you prefer to build manually:
```
   git clone https://github.com/uhppoted/uhppoted-lib-dotnet.git
   cd uhppoted-lib-dotnet/uhppoted
   dotnet build
```

### Notes

#### C#

C# applications may require installing the FSharp.Core package:
```
dotnet add package FSharp.Core
```

#### VB.NET

VB.NET applications may require installing the FSharp.Core package:
```
dotnet add package FSharp.Core
```


## API summary

#### [`FindControllers`](documentation/API/find-controllers.md)
'Discovers' all controllers accessible via a UDP broadcast on the local LAN.

#### [`GetController`](documentation/API/get-controller.md)
Retrieves the IPv4 configuration, MAC address and version information for an access controller.

#### [`SetIPv4`](documentation/API/set-IPv4.md)
Sets a controller IPv4 address, netmask and gateway address.

#### [`GetListener`](documentation/API/get-listener.md)
Gets a controller event listener address:port and auto-send interval.

#### [`SetListener`](documentation/API/set-listener.md)
Sets a controller event listener endpoint and auto-send interval.

#### [`GetTime`](documentation/API/get-time.md)
Gets a controller current date and time.

#### [`SetTime`](documentation/API/set-time.md)
Sets a controller current date and time.

#### [`GetDoor`](documentation/API/get-door.md)
Gets a controller door operational mode and unlocked delay.

#### [`SetDoor`](documentation/API/set-door.md)
Sets a controller door operational mode and unlocked delay.

#### [`SetDoorPasscodes`](documentation/API/set-door-passcodes.md)
Sets up to 4 passcodes for a controller door.

#### [`OpenDoor`](documentation/API/open-door.md)
Unlocks a door controlled by a controller.

#### [`GetStatus`](documentation/API/get-status.md)
Retrieves a controller status and most recent event (if any).

#### [`GetCards`](documentation/API/get-cards.md)
Retrieves the number of card records stored on a controller.

#### [`GetCard`](documentation/API/get-card.md)
Retrieves a card record by card number.

#### [`GetCardAtIndex`](documentation/API/get-card-at-index.md)
Retrieves the card record (if any) at the index from a controller.

#### [`PutCard`](documentation/API/put-card.md)
Adds or updates a card record on a controller.

#### [`DeleteCard`](documentation/API/delete-card.md)
Deletes a card record from a controller.

#### [`DeleteAllCards`](documentation/API/delete-all-cards.md)
Deletes all card records from a controller.

#### [`GetEvent`](documentation/API/get-event.md)
Retrieves the event record (if any) at the index from a controller.

#### [`GetEventIndex`](documentation/API/get-event-index.md)
Retrieves the current event index from a controller.

#### [`SetEventIndex`](documentation/API/set-event-index.md)
Sets a controller event index.

#### [`RecordSpecialEvents`](documentation/API/record-special-events.md)
Enables (or disables) events for door open/close, button press, etc.

#### [`GetTimeProfile`](documentation/API/get-time-profile.md)
Retrieves an access time profile from a controller.

#### [`GetTimeProfile`](documentation/API/set-time-profile.md)
Adds or updates an access time profile on a controller.

#### [`ClearTimeProfiles`](documentation/API/clear-time-profiles.md)
Clears all access time profiles stored on a controller.

#### [`AddTask`](documentation/API/add-task.md)
Adds or updates a scheduled task on a controller.

#### [`ClearTaskList`](documentation/API/clear-tasklist.md)
Clears all scheduled tasks from a controller tasklist.

#### [`RefreshTaskList`](documentation/API/refresh-tasklist.md)
Schedules added tasks.

#### [`SetPCControl`](documentation/API/set-pc-control.md)
Enables/disables remote access control management. 

#### [`SetInterlock`](sdocumentation/API/set-interlock.md)
Sets the door interlock mode for an access controller.

#### [`ActivateKeypads`](documentation/API/activate-keypads.md)
Activates/deactivates the access reader keypads attached to an access controller.

#### [`RestoreDefaultSettings`](documentation/API/restore-default-settings.md)
Restores the manufacturer default settings.

#### [`Listen`](documentation/API/listen.md)
Listens for access controller events.

## Examples

### F#

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/fsharp/cli)

### C#

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/csharp/cli)

### VB.NET

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/vb/cli)

## Notes

