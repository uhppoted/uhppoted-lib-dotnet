# TODO

- [x] dump
- [x] `Controller` record
- [x] Options
- [x] decode
- [x] bind to bind-addr
- [ ] Add .NET 8 as a target framework on github
```
    <TargetFrameworks>net7.0;net8.0</TargetFrameworks>
```


- [ ] translations/enums
      - [ ] door mode
- [ ] function signature file

- [x] integration tests
      - [x] UDP broadcast
      - [x] UDP broadcast-to
      - [x] UDP send-to
      - [x] TCP send-to
      - [x] Check TCP client is closing network connection

- [x] get-all-controllers
      - [x] pprint Nullable DateOnly
      - [x] Return array of controllers and remove FSharpCollections dependency
      - [ ] return Result
      - [ ] error handling

- [x] get-controller
- [x] set-IPv4
- [x] get-listener
- [x] set-listener
- [x] get-time
- [x] set-time
- [x] get-door
- [x] set-door
- [ ] set-door-passcodes
      - [ ] API fn
      - [ ] CLI
            - [ ] F#
            - [ ] C#
            - [ ] VB.NET
      - [ ] integration test
      - [ ] API doc

- [ ] record-special-events
- [ ] get-status
- [ ] get-cards
- [ ] get-card
- [ ] put-card
- [ ] delete-card
- [ ] delete-cards
- [ ] get-time-profile
- [ ] set-time-profile
- [ ] clear-time-profiles
- [ ] clear-task-list
- [ ] add-task
- [ ] refresh-task-list
- [ ] set-task-list
- [ ] get-events
- [ ] get-event
- [ ] get-event-index
- [ ] set-event-index
- [ ] open-door
- [ ] set-pc-control
- [ ] set-interlock
- [ ] activate-keypads
- [ ] restore-default-parameters
- [ ] listen

## CS
- https://stackoverflow.com/questions/35163327/working-with-f-options-in-c-e-g-fsharpoptiondictionaryguid-membershipuser
- https://bizmonger.wordpress.com/2016/09/27/accessing-fsharps-option-type-from-c/
- https://gsscoder.github.io/consuming-fsharp-results-in-c/

## Notes
- https://stackoverflow.com/questions/10110174/best-approach-for-designing-f-libraries-for-use-from-both-f-and-c-sharp
- https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/component-design-guidelines
- https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/
- https://github.com/knocte/2fsharp/blob/master/csharp2fsharp.md
- https://forums.fsharp.org/t/how-to-use-f-in-c-and-conversely/1274
- https://github.com/dandereggK/FSharp_Types_In_CSharp
- https://stackoverflow.com/questions/75451183/how-to-access-f-results-from-c-sharp
- https://bizmonger.wordpress.com/2016/09/27/accessing-fsharps-option-type-from-c/
