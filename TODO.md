# TODO

- [x] dump
- [x] `Controller` record
- [x] Options
- [x] decode
- [x] bind to bind-addr
- [x] integration tests
- [x] Add .NET 8 as a target framework on github
- [x] Add .NET 9 as a target framework on github
- [x] verify controller ID in response
- [x] README

- [ ] github _nightly_ build
      - [x] Fix version
      - [x] Fix missing README
      - [ ] Upload artifacts
      - https://devblogs.microsoft.com/dotnet/dotnet-loves-github-actions
      - https://www.dotnetcurry.com/dotnetcore/github-actions-for-dotnet-developers
      - https://samlearnsazure.blog/2021/07/27/publishing-a-nuget-package-to-github-packages
      - https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry
      - https://www.meziantou.net/publishing-a-nuget-package-following-best-practices-using-github.htm

- [ ] function signature file
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

- [ ] translations/enums
      - [ ] door mode

- [ ] F#: argparse
      - [x] --controller
      - [x] --address
      - [x] --netmask
      - [x] --gateway
      - [ ] --listener
      - [ ] --interval
      - [ ] --time
      - [ ] --door
      - [ ] --mode
      - [ ] --delay
      - [ ] --passcode
      - [ ] --card
      - [ ] --card-index

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
      - [ ] delete-card
            - [ ] API function
            - [ ] CLI
                  - [ ] F#
                  - [ ] C#
                  - [ ] VB.NET
            - [ ] integration test
            - [ ] API doc
            - [ ] README
      
      - [ ] delete-all-cards
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
