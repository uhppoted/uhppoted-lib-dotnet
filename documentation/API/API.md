# API

- [Functions](#functions)
- [Options](#options)
- [_controller_ struct](#c)
- [Types](#types)
- [Internationalisation](#internationalisation)

## Functions
- [`FindControllers`](find-controllers.md)
- [`GetController`](get-controller.md)
- [`SetIPv4`](set-IPv4.md)
- [`GetListener`](get-listener.md)
- [`SetListener`](set-listener.md)
- [`GetTime`](get-time.md)
- [`SetTime`](set-time.md)
- [`GetDoor`](get-door.md)
- [`SetDoor`](set-door.md)
- [`SetDoorPasscodes`](set-door-passcodes.md)
- [`OpenDoor`](open_door.md)
- [`GetStatus`](get-status.md)
- [`GetCards`](get-cards.md)
- [`GetCard`](get-card.md)
- [`GetCardAtIndex`](get-card-at-index.md)
- [`PutCard`](put-card.md)
- [`DeleteCard`](delete-card.md)
- [`DeleteAllCards`](delete-all-cards.md)
- [`GetEvent`](get-event.md)
- [`GetEventIndex`](get-event-index.md)
- [`SetEventIndex`](set-event-index.md)
- [`RecordSpecialEvents`](record-special-events.md)
- [`GetTimeProfile`](get-time-profile.md)
- [`SetTimeProfile`](set-time-profile.md)
- [`ClearTimeProfiles`](clear-time-profiles.md)
- [`AddTask`](add-task.md)
- [`ClearTaskList`](clear-tasklist.md)
- [`RefreshTaskList`](refresh-tasklist.md)
- [`SetPCControl`](set-pc-control.md)
- [`SetInterlock`](set-interlock.md)
- [`ActivateKeypads`](activate-keypads.md)
- [`RestoreDefaultParameters`](restore-default-parameters.md)
- [`Listen`](listen.md)

## Options

Every API call takes an `Options` parameter that sets the network configuration used to connect to the
access controller. The `Options` struct comprises the following fields:

- **`bind` (`IPEndPoint`)**: IPv4 endpoint to which to bind. Default value is INADDR_ANY (0.0.0.0:0).
- **`broadcast (`IPEndPoint`)**:  IPv4 endpoint to which to broadcast UDP requests. Default value is '255.255.255.255:60000'.
- **`listen (`IPEndPoint`)**: IPv4 endpoint on which to listen for controller events. Defaults to '0.0.0.0:60001.
- **`timeout` (`int`)**: Operation timeout (ms).
- **`debug (`bool`)**: Logs controller requests and responses to the console if enabled.

e.g.
```
let options: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      timeout = 1000
      debug = true }
```

**NB**: 

1. Using a bind port of `60000` is not recommended unless you have superior network-fu (it will receive all UDP broadcasts to the 
   controllers which is not what you want). In general bind to port `0` and if necessary exclude port `60000` from the ephemeral 
   port list (see [_Ephemeral ports and binding to `0.0.0.0:0`_](https://github.com/uhppoted/uhppoted-lib-dotnet?tab=readme-ov-file#ephemeral-ports-and-binding-to-00000))

2. A listen port of `60000` is less catastrophic but also not recommended (for the same reason). You do need use the `set-listener` API
   function to set the controller listener IPv4 address:port to match your chosen listen endpoint though.

### OptionsBuilder

`OptionsBuilder` is a utility class to simplify construction of an `Options` struct when using C# or VB.NET:

- **`WithBind(endpoint: IPEndPoint)`: Sets the `bind` IPv4 endpoint.
- **`WithBroadcast(endpoint: IPEndPoint)`: Sets the `broadcast` IPv4 endpoint.
- **`WithListen(endpoint: IPEndPoint)`**: Sets the `listen` endpoint.
- **`WithTimeout(ms: int)`**: Sets the operation timeout (milliseconds).
- **`WithDebug(enable: bool)`**: Enables (or disables) logging of controller requests and responses to the console.
- **`Build()`**: Builds the `Options` struct.

```csharp
static readonly Options options = new OptionsBuilder()
                                      .WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000"))
                                      .WithProtocol("udp")
                                      .WithTimeout(1250)
                                      .WithDebug(true)
                                      .Build();
```

```vb
Private ReadOnly Dim options = New OptionsBuilder().
                                   WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000")).
                                   WithProtocol("udp").
                                   WithTimeout(1250).
                                   WithDebug(True).
                                   Build()
```

## C 

The first parameter of every API call (other than `FindControllers` and `Listen`, i.e. every API call
addressed to a specific controller) is a _controller_ value which may be:

- controller ID (`uint32`): the controller serial number
- a controller `C` struct which specifies:
    - the controller ID (`uint32`): the controller serial number
    - an optional IPv4 endpoint (`IPEndPoint`): the controller IPv4 address:port
    - an optional protocol (`string`): the controller connection protocol ("udp" or "tcp")

| Configuration                                            | Network Transport |
|----------------------------------------------------------|-------------------|
| `uint32` controller ID                                   | UDP broadcast     |
| `struct` with controller ID                              | UDP broadcast     |
| `struct` with controller ID and endpoint                 | connected UDP     |
| `struct` with controller ID, endpoint and 'tcp' protocol | TCP/IP            |

e.g.
```
match GetController 405419896u options with
| Ok record -> printfn "get-controller: ok %A" record
| Error err -> printfn "get-controller: error %A" err
```

```
let controller = { 
    controller=405419896u; 
    endpoint=Some(IPEndPoint.Parse("192.168.1.100:60000")); 
    protocol=Some("tcp") }

match GetController controller options with
| Ok record -> printfn "get-controller: ok %A" record
| Error err -> printfn "get-controller: error %A" err
```

### ControllerBuilder

`ControllerBuilder` is a utility class to simplify construction of a `C` struct when using C# or VB.NET:

- **`WithEndpoint(endpoint: IPEndPoint)`**: Sets the optional controller endpoint.
- **`WithProtocol(protocol: string)`**: Sets the optional controller protocol ('udp' or 'tcp').
- **`Build()`**: Builds the `C` struct.

```csharp
static readonly controller = new ControllerBuilder(405419896u)
                                 .WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000"))
                                 .WithProtocol("tcp")
                                 .Build();
```

```vb
Private ReadOnly Dim controller = New ControllerBuilder(405419896u).
                                      WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000")).
                                      WithProtocol("udp").
                                      Build()
```

## Types

- [`Controller`](types.md#controller)
- [`Door`](types.md#door)
- [`Status`](types.md#status)
- [`Event`](types.md#event)
- [`Card`](types.md#card)
- [`TimeProfile`](types.md#timeprofile)
- [`Task`](types.md#task)
- [`Listener`](types.md#listener)
- [`ListenerEvent`](types.md#listenerevent)


### enums

#### _DoorMode_

Defines the door control modes:
- `Unknown`: unknown door control mode
- `NormallyOpen`: Door is always unlocked
- `NormallyClosed`: Door is always locked
- `Controlled`: Door lock is managed by access controller

#### _Direction_

Defines whether a door was unlocked for entrance or exit:
- `Unknown`: unknown direction
- `In`: access granted for entrance
- `Out`: access granted for exit

#### Interlock

Defines the door interlocks for an access controller:

- `None`: no interlock
- `Doors12`: interlocks doors 1 & 2
- `Doors34`: interlocks doors 3 & 4
- `Doors12And34`: interlocks doors 1 & 2 and doors 3 & 4
- `Doors123`: interlock between doors 1, 2 & 3
- `Doors1234`: interlocks all doors

#### Relay

Defines the relay contact states for the door unlock relays:

- `Unknown`: the relay state is undefined
- `Inactive`: the relay is deactivated i.e. the _normally open_ contact is open and the _normally closed_ contact is closed
- `Active`: the relay is activated i.e. the _normally open_ contact is closed  and the _normally closed_ contact is open

#### Input

Defines the states for external inputs:

- `Unknown`: unknown input contact state
- `Clear`: input contact is not 'made'
- `Set`: input contact is 'made'

#### TaskCode

Defines the known task codes for the scheduled tasks:
- `ControlDoor`: sets a door mode to 'controlled
- `UnlockDoor`: sets a door mode to 'normally open'
- `LockDoor`: sets a door mode to 'normally closed'
- `DisableTimeProfiles`: disables time profiles
- `EnableTimeProfiles`: enables time profiles
- `EnableCardNoPIN`: allows card entry without a PIN
- `EnableCardInPIN`: requires a card+PIN for IN access
- `EnableCardInOutPIN`: requires a card+PIN for both IN and OUT access
- `EnableMoreCards`: enables 'more cards' access
- `DisableMoreCards`: disables 'more cards' access
- `TriggerOnce`: trigger 'once'
- `DisablePushbutton`: disables pushbutton access
- `EnablePushbutton`: enables pushbutton access

## Internationalisation

The API includes a `translate` function to translate error codes, event reasons, etc into the local language set by
the .NET `CurrentCulture` and `CurrentUICulture` values, e.g.:

``` fsharp
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
    Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

    WriteLine ($"ERROR {Uhppoted.Translate err}")

```

```csharp
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
    Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

    WriteLine ($"ERROR {Uhppoted.Translate(result.ErrorValue}")
```

```vb.net
    Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
    Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

    WriteLine ($"ERROR {UHPPOTE.Translate(result.ErrorValue}")

```

Translations are defined for the following values:
- errors
- event types
- event reasons
- door directions
- door control modes
- door interlocks
- door relay states
- external inputs
- task codes

