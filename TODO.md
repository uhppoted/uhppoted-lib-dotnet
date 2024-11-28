# TODO

- [ ] Add .NET 9 as a target framework

- [ ] function signature file
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

- [ ] translations & enums
      - [ ] door mode
      - [ ] event types
      - [ ] event reasons
      - [ ] event direction
      - https://stackoverflow.com/questions/2689446/how-to-manage-resources-in-an-f-project
      - https://fsprojects.github.io/FSharp.Configuration/ResXProvider.html
      - https://poeditor.com/kb/resx-editor
      - https://localizely.com/getting-started

- [ ] argparse
      - [x] --controller
      - [x] --card
      - [x] --address
      - [x] --netmask
      - [x] --gateway
      - [x] --listener
      - [x] --interval
      - [x] --index
      - [x] --time
      - [x] --enable
      - [x] --door
      - [ ] --mode
            - [ ] F#
            - [ ] C#
            - [ ] VB.NET
      - [ ] --delay
      - [ ] --passcodes
      - [ ] --start-date
      - [ ] --end-date
      - [ ] --permissions
      - [ ] --PIN
      - [ ] --weekdays
      - [ ] --segments

- [ ] API
      - [x] find-controllers
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
      - [x] get-status
      - [x] get-cards
      - [x] get-card
      - [x] get-card-at-index
      - [x] put-card
            - [ ] rework to take `Card` struct
      - [x] delete-card
      - [x] delete-all-cards
      - [x] get-event
      - [x] get-event-index
      - [x] set-event-index
      - [x] record-special-events
      - [x] get-time-profile
      - [x] set-time-profile
      - [x] clear-time-profiles
      - [ ] add-task
            - [ ] API function
            - [ ] CLI
                  - [ ] F#
                  - [ ] C#
                  - [ ] VB.NET
            - [ ] integration test
            - [ ] API doc
            - [ ] README
      
      - [ ] clear-task-list
      - [ ] refresh-task-list
      - [ ] set-task-list
      - [ ] set-pc-control
      - [ ] set-interlock
      - [ ] activate-keypads
      - [ ] restore-default-parameters
      - [ ] listen

## Notes
