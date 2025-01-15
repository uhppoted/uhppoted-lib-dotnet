# uhppoted

.NET package for the UHPPOTE access controller API.

The API is described in [API.md](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/API.md) and 
example CLI implementations in F#, C# and VB.NET that illustrate the use of the API can be found in the 
[examples](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples) folder.

- [Release Notes](#release-notes)
- [Installation](#installation)
- [API summary](#api-summary)
- [API documentation](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/API.md)
- [Examples](#examples)

## Release Notes

#### Current Release

_BETA_


## Installation

The package can be installed from either the [_NuGet_](https://www.nuget.org/packages/uhppoted) registry or
[_Github Packages_](https://github.com/uhppoted/uhppoted-lib-dotnet/pkgs/nuget/uhppoted):

### Installing from the NuGet Registry

Use either the dotnet CLI or _Package Manager_:

- CLI:
```
dotnet add package uhppoted
```

- Package Manager:
```
NuGet\Install-Package uhppoted -Version <version>
```

After installing the package add it the _project_ file:
```
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted" Version="<version>" />
  </ItemGroup>
  ...
```

### Installing from Github Packages

Installing from the _Github Packages_ _NuGet_ registry is a more complicated. Either:

- Follow the steps under 
[_Installing a package_](https://docs.github.com/en/enterprise-server@3.12/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry#installing-a-package) and add the repository URL and package to the project file:

```
  ...
  <PropertyGroup>
    <RepositoryUrl>https://githbuHOSTNAME/OWNER/REPOSITORY</RepositoryUrl>
  </PropertyGroup>
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted" Version="<version>" />
  </ItemGroup>
  ...
```

- (OR) use the dotnet CLI:
   - Create a _personal access token (classic)_
   - Add a _NuGet_ source:
   ```
   dotnet nuget add source https://nuget.pkg.github.com/uhppoted/index.json --name github-uhppoted --username <user-id> --password <personal-access-token>
   ```
   - Install the package:
   ```
   dotnet add package uhppoted
   ```

### Building from source

Requirements:
- .NET SDK 7.0+
- (_optional_) make

```
git clone https://github.com/uhppoted/uhppoted-lib-dotnet.git
cd uhppoted-lib-dotnet/uhppoted
make build
```

If you prefer to build without _make_:
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

#### Windows

The dynamic port range in _Windows_ has been extended to include the default _listen_ port (60001):

- [The default dynamic port range for TCP/IP has changed...](https://learn.microsoft.com/en-us/troubleshoot/windows-server/networking/default-dynamic-port-range-tcpip-chang)

Applications running on Windows _may_ need to either provision a different UDP port when listening for 
events or add port 60001 to the _allowed_ list.


## API summary

#### [`FindControllers`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/find-controllers.md)
Discovers all controllers accessible via a UDP broadcast on the local LAN.

#### [`GetController`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-controller.md)
Retrieves the IPv4 configuration, MAC address and version information for an access controller.

#### [`SetIPv4`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-IPv4.md)
Sets a controller IPv4 address, netmask and gateway address.

#### [`GetListener`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-listener.md)
Gets a controller event listener address:port and auto-send interval.

#### [`SetListener`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-listener.md)
Sets a controller event listener endpoint and auto-send interval.

#### [`GetTime`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-time.md)
Gets a controller current date and time.

#### [`SetTime`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-time.md)
Sets a controller current date and time.

#### [`GetDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-door.md)
Gets a controller door operational mode and unlocked delay.

#### [`SetDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-door.md)
Sets a controller door operational mode and unlocked delay.

#### [`SetDoorPasscodes`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-door-passcodes.md)
Sets up to 4 passcodes for a controller door.

#### [`OpenDoor`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/open-door.md)
Unlocks a door controlled by a controller.

#### [`GetStatus`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-status.md)
Retrieves a controller status and most recent event (if any).

#### [`GetCards`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-cards.md)
Retrieves the number of card records stored on a controller.

#### [`GetCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-card.md)
Retrieves a card record by card number.

#### [`GetCardAtIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-card-at-index.md)
Retrieves the card record (if any) at the index from a controller.

#### [`PutCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/put-card.md)
Adds or updates a card record on a controller.

#### [`DeleteCard`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/delete-card.md)
Deletes a card record from a controller.

#### [`DeleteAllCards`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/delete-all-cards.md)
Deletes all card records from a controller.

#### [`GetEvent`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-event.md)
Retrieves the event record (if any) at the index from a controller.

#### [`GetEventIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-event-index.md)
Retrieves the current event index from a controller.

#### [`SetEventIndex`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-event-index.md)
Sets a controller event index.

#### [`RecordSpecialEvents`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/record-special-events.md)
Enables (or disables) events for door open/close, button press, etc.

#### [`GetTimeProfile`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/get-time-profile.md)
Retrieves an access time profile from a controller.

#### [`GetTimeProfile`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-time-profile.md)
Adds or updates an access time profile on a controller.

#### [`ClearTimeProfiles`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/clear-time-profiles.md)
Clears all access time profiles stored on a controller.

#### [`AddTask`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/add-task.md)
Adds or updates a scheduled task on a controller.

#### [`ClearTaskList`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/clear-tasklist.md)
Clears all scheduled tasks from a controller tasklist.

#### [`RefreshTaskList`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/refresh-tasklist.md)
Schedules added tasks.

#### [`SetPCControl`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/set-pc-control.md)
Enables/disables remote access control management. 

#### [`SetInterlock`](sdocumentation/API/set-interlock.md)
Sets the door interlock mode for an access controller.

#### [`ActivateKeypads`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/activate-keypads.md)
Activates/deactivates the access reader keypads attached to an access controller.

#### [`RestoreDefaultSettings`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/restore-default-settings.md)
Restores the manufacturer default settings.

#### [`Listen`](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/listen.md)
Listens for access controller events.

## Examples

### F#

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/fsharp/cli)

### C#

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/csharp/cli)

### VB.NET

- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/vb/cli)

