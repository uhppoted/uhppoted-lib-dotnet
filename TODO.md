# TODO

- [ ] nightly builds
      - (?) maybe use different ports integration tests (59997 seems to be in use)
      - [x] check open sockets (lsof -i :8000)
      - [x] set SO_REUSEADDR

- [ ] camelCase internal functions

- [ ] API
    - [ ] _Cannot access a disposed object._
    - [ ] Rework inputs:
          - [ ] fire alarm
          - [ ] tamper detect
          - [ ] set of bool

- [ ] argparse
    - [x] --interlock
    - [ ] --weekdays
        - [ ] F#
        - [ ] C#
        - [ ] VB.NET
    - [ ] --segments
    - [ ] --task
    - [ ] --keypads

- [ ] translations & enums
    - [x] door mode
    - [x] door direction
    - [x] interlock
    - [x] relays
    - [x] inputs
    - [ ] event types
    - [ ] event reasons
    - [ ] tasks

- [ ] function signature file
- [ ] Represent error cases and illegal state in types intrinsic to your domain
      - https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions#represent-error-cases-and-illegal-state-in-types-intrinsic-to-your-domain
- [ ] Prefer namespaces at the top level
      - https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions
- [ ] Add .NET 9 as a target framework
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

