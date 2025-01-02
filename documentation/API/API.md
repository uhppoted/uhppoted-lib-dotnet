# API

**Contents**:
1. [Types](#types)
2. [Functions](#functions)
3. [Options](#options)
4. [C](#c)

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
    protocol:Some("tcp") }

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

## enums

### Interlock

Defines the door interlocks for an access controller:

- `None`: no interlock
- `Doors12`: interlocks doors 1 & 2
- `Doors34`: interlocks doors 3 & 4
- `Doors12And34`: interlocks doors 1 & 2 and doors 3 & 4
- `Doors123`: interlock between doors 1, 2 & 3
- `Doors1234`: interlocks all doors

### Relay

Defines the relay contact states for the door unlock relays:

- `Open`: the relay is deactivated i.e. the _normally open_ contact is open and the _normally closed_ contact is closed
- `Closed`: the relay is activated i.e. the _normally open_ contact is closed  and the _normally closed_ contact is open
