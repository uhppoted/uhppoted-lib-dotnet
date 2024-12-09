# API

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

## Options

Every API call takes an `Options` parameter that sets the network configuration used to connect to the
access controller. The `Options` struct comprises the following fields:

- **`bind` (`IPEndPoint`)**: IPv4 endpoint to which to bind. Default value is INADDR_ANY (0.0.0.0:0).
- **`broadcast (`IPEndPoint`)**:  IPv4 endpoint to which to broadcast UDP requests. Default value is '255.255.255.255:60000'.
- **`listen (`IPEndPoint`)**: IPv4 endpoint on which to listen for controller events. Defaults to '0.0.0.0:60001.
- **`endpoint (`Option<IPEndPoint>`)**: Optional IPv4 controller address:port. Required if the controller is not accessible via UDP broadcast.
- **`protocol (`Option<string>`)**: Optional 'protocol' to connect to controller. Valid values are currently 'udp' or 'tcp', defaulting to 'udp'.
- **`debug (`bool`)**: Logs controller requests and responses to the console if enabled.

e.g.
```
let options: Options =
    { bind = IPEndPoint(IPAddress.Any, 0)
      broadcast = IPEndPoint(IPAddress.Broadcast, 60000)
      listen = IPEndPoint(IPAddress.Any, 60001)
      endpoint = Some(IPEndPoint.Parse("192.168.1.100:60000"))
      protocol = 'tcp'
      debug = true }
```

### OptionsBuilder

`OptionsBuilder` is a utility class to simplify construction of an `Options` struct when using C# or VB.NET.

    
- **`WithBind(endpoint: IPEndPoint)`: Sets the `bind` IPv4 endpoint.
- **`WithBroadcast(endpoint: IPEndPoint)`: Sets the `broadcast` IPv4 endpoint.
- **`WithListen(endpoint: IPEndPoint)`**: Sets the `listen` endpoint.
- **`WithEndpoint(endpoint: IPEndPoint)`**: Sets the optional controller endpoint.
- **`WithProtocol(protocol: string)`**: Sets the optional controller protocol ('udp' or 'tcp').
- **`WithDebug(enable: bool)`**: Enables (or disables) logging of controller requests and responses to the console.
- **`Build()`**: Builds the `Options` struct.

```csharp
static readonly Options options = new OptionsBuilder()
                                      .WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000"))
                                      .WithProtocol("udp")
                                      .WithDebug(true)
                                      .Build();
```

```vb
Private ReadOnly Dim options = New OptionsBuilder().
                                   WithEndpoint(IPEndPoint.Parse("192.168.1.100:60000")).
                                   WithProtocol("udp").
                                   WithDebug(True).
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
