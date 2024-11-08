# TODO

- [x] dump
- [x] `Controller` record
- [x] Options
- [x] decode
- [x] bind to bind-addr
- [x] integration tests
- [x] Add .NET 8 as a target framework on github

- [ ] github _nightly_ build
- [ ] function signature file
- [ ] README
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

- [ ] translations/enums
      - [ ] door mode

- (?) verify controller ID in response

- [x] get-all-controllers
      - [x] pprint Nullable DateOnly
      - [x] Return array of controllers and remove FSharpCollections dependency
      - [x] Return Result
      - [x] Handle errors in receive-all

- [ ] Restructure API fns to use |>

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
      - [x] open-door
      - [ ] get-status
            - [ ] API fn
            - [ ] CLI
                  - [ ] F#
                  - [ ] C#
                  - [ ] VB.NET
            - [ ] integration test
            - [ ] API doc
      
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

## Notes
