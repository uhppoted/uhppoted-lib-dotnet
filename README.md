![build](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/build/badge.svg)
![integration-tests](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/integration-tests/badge.svg)
![nightly](https://github.com/uhppoted/uhppoted-lib-dotnet/workflows/nightly/badge.svg)

# uhppoted-lib-dotnet

.NET package for the UHPPOTE UT0311-L0x* TCP/IP Wiegand access controller API.

The API is described in detail in the [API](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/API.md) documentation and examples 
in [_F#_](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/fsharp), [_C#_](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/csharp) 
and [_VB.NET_](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/vb) are included to illustrate the use of the API.

---
- [Release Notes](#release-notes)
- [Installation](#installation)
   - [Installing from nuget.org](#installing-from-the-nuget-registry)
   - [Installing from Github Packages](#installing-from-github-packages)
   - [Building from source](#building-from-source)
   - [C#](#c)
   - [VB.NET](vbnet)
- [API summary](#api-summary)
- [API documentation](https://github.com/uhppoted/uhppoted-lib-dotnet/blob/main/documentation/API/API.md)
- [Examples](#examples)
- [Notes](#notes)
   - [Ephemeral ports](#ephemeral-ports-and-binding-to-00000)

## Release Notes

#### Current Release

**[v0.8.10](https://github.com/uhppoted/uhppoted-lib-dotnet/releases/tag/v0.8.10) - 2025-01-30**

1. Initial release.


## Installation

The package can be installed from either the [_NuGet_](https://www.nuget.org/packages/uhppoted) registry or
[_Github Packages_](https://github.com/uhppoted/uhppoted-lib-dotnet/pkgs/nuget/uhppoted):

### Installing from the NuGet Registry

Use either the _dotnet CLI_ or _Package Manager_:

- CLI:
```
dotnet add package uhppoted --version 0.8.10

```

- _Package Manager_ (console):
```
NuGet\Install-Package uhppoted -Version 0.8.10
```

After installing the package it should be referenced in the _project_ file, e.g.:
```
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted" Version="0.8.10" />
  </ItemGroup>
  ...
```

### Installing from Github Packages

Installing from the _Github Packages_ _NuGet_ registry requires a [_Personal Access Token (classic)_](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic)
with at least `package: read` permissions.

- _dotnet CLI_:
   ```
   dotnet nuget add source --username <username> --password <personal-access-token> --store-password-in-clear-text --name uhppoted "https://nuget.pkg.github.com/uhppoted/index.json"
   dotnet nuget add source https://nuget.pkg.github.com/uhppoted/index.json
   dotnet add package uhppoted --version 0.8.10

   ```

- _Package Manager_ (console):
   ```
   NuGet\Set-Source -Name "uhppoted" -Source "https://nuget.pkg.github.com/uhppoted/index.json" -Username "<username>" -Password "<personal-access-token>"
   NuGet\Install-Package uhppoted -Version 0.8.10
   ```

After installing the package it should be referenced in the _project_ file, e.g:
```
  ...
  <ItemGroup>
    <PackageReference Include="uhppoted" Version="0.8.10" />
  </ItemGroup>
  ...
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

#### C#

C# applications _may_ additionally require installing the FSharp.Core package:
```
dotnet add package FSharp.Core
```

#### VB.NET

VB.NET applications _may_ additionally require installing the FSharp.Core package:
```
dotnet add package FSharp.Core
```

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

#### F#

- [hello world](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/fsharp/hello-world)
- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/fsharp/cli)

#### C#

- [hello world](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/csharp/hello-world)
- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/csharp/cli)

#### VB.NET

- [hello world](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/vb/hello-world)
- [CLI](https://github.com/uhppoted/uhppoted-lib-dotnet/tree/main/examples/vb/cli)

## Notes

#### Ephemeral ports and binding to `0.0.0.0:0`

As per [Microsoft Knowledgebase Article 929851](https://learn.microsoft.com/en-us/troubleshoot/windows-server/networking/default-dynamic-port-range-tcpip-chang),
the default Windows ephemeral port range extends from 49152 to 65535, which includes the default UHPPOTE UDP port (`60000`). Present-day BSD and Linux
have similar ranges.

If an application is assigned port `60000` when binding to e.g. `0.0.0.0:0` it will receive the any outgoing UDP broadcast requests and interpret
them as replies - which will be, uh, a little confusing, e.g.:
```
request:
   17 94 00 00 00 00 00 00  00 00 00 00 00 00 00 00
   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00
   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00
   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00

reply:
   17 94 00 00 78 37 2a 18  c0 a8 01 64 ff ff ff 00
   c0 a8 01 01 00 12 23 34  45 56 08 92 20 18 11 05
   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00
   00 00 00 00 00 00 00 00  00 00 00 00 00 00 00 00
      
get-all-controllers:
   controller: 0
      address: 0.0.0.0
      netmask: 0.0.0.0
      gateway: 0.0.0.0
          MAC: 00:00:00:00:00:00
      version: v0.00
         date: ---

   controller: 405419896
      address: 192.168.1.100
      netmask: 255.255.255.0
      gateway: 192.168.1.1
          MAC: 00:12:23:34:45:56
      version: v8.92
         date: 2018-11-05
```

In general this doesn't seem to have been a problem (or at least nobody has raised it as an issue) and the implementation will
return an error if the bind port for a UDP broadcast is 60000. It can be mitigated by:
- Excluding port `60000` from the ephemeral range using whatever method is recommended for your platform of choice.
- (OR) Reduce (or move) the ephemeral port range (again using whatever method is recommended for your platform of choice).
- (OR) (**really not recommended except as a quick hack**) Bind a netcat listener to port `60000` before running the application:
```
nc -lu 600000
```

References:
1. [_The Ephemeral Port Range_](https://www.ncftp.com/ncftpd/doc/misc/ephemeral_ports.html)
2. [_How to change/view the ephemeral port range on Windows machines?_](https://stackoverflow.com/questions/7006939/how-to-change-view-the-ephemeral-port-range-on-windows-machines#7007159)
3. [_You cannot exclude ports by using the ReservedPorts registry key in Windows Server 2008 or in Windows Server 2008 R2_](https://support.microsoft.com/en-us/topic/you-cannot-exclude-ports-by-using-the-reservedports-registry-key-in-windows-server-2008-or-in-windows-server-2008-r2-a68373fd-9f64-4bde-9d68-c5eded74ea35)
4. [_Listen to UDP data on local port with netcat_](https://serverfault.com/questions/207683/listen-to-udp-data-on-local-port-with-netcat)
