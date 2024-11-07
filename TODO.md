# TODO

- [x] dump
- [x] `Controller` record
- [x] Options
- [x] decode
- [x] bind to bind-addr
- [x] integration tests
- (?) verify controller ID in response
- [ ] Restructure API fns to use |>

- [ ] Add .NET 8 as a target framework on github
- [ ] github _nightly_ build
- [ ] function signature file
- [ ] README
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

- [ ] translations/enums
      - [ ] door mode

- [ ] get-all-controllers
      - [x] pprint Nullable DateOnly
      - [x] Return array of controllers and remove FSharpCollections dependency
      - [x] Return Result
      - [ ] Handle errors in receive-all

- [ ] API
      - [x] get-controller
      - [x] set-IPv4
      - [x] get-listener
      - [x] set-listener
      - [x] get-time
      - [x] set-time
      - [x] get-door
      - [x] set-door
      - [x] set-door-passcodes
      - [ ] open-door
            - [ ] API fn
            - [ ] CLI
                  - [ ] F#
                  - [ ] C#
                  - [ ] VB.NET
            - [ ] integration test
            - [ ] API doc
      
      - [ ] get-status
      - [ ] get-cards
      - [ ] get-card
      - [ ] put-card
      - [ ] delete-card
      - [ ] delete-cards
      - [ ] get-events
      - [ ] get-event
      - [ ] get-event-index
      - [ ] set-event-index
      - [ ] record-special-events
      - [ ] get-time-profile
      - [ ] set-time-profile
      - [ ] clear-time-profiles
      - [ ] clear-task-list
      - [ ] add-task
      - [ ] refresh-task-list
      - [ ] set-task-list
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
