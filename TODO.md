# TODO

- [ ] camelCase internal functions

- [ ] API
    - [ ] Rework endpoint/protocol as controller struct
          - [x] F# examples
          - [x] C# examples
          - [x] VB.NET examples
          - [x] API doc
          - [ ] Rework integration tests as seperate broadcast, UDP and TCP test suites
                - https://stackoverflow.com/questions/35733544/testcases-for-nunit-3-tests-in-f
          - [ ] Remove endpoint and protocol from Options
    - [ ] _Cannot access a disposed object._

- [ ] argparse
    - [ ] --weekdays
        - [ ] F#
        - [ ] C#
        - [ ] VB.NET
    - [ ] --segments
    - [ ] --task
    - [ ] --interlock
    - [ ] --keypads

- [ ] translations & enums
    - [ ] door mode
    - [ ] event types
    - [ ] event reasons
    - [ ] event direction
    - [ ] tasks
    - [ ] interlock
    - [ ] relays
    - [ ] inputs
    - https://stackoverflow.com/questions/2689446/how-to-manage-resources-in-an-f-project
    - https://fsprojects.github.io/FSharp.Configuration/ResXProvider.html
    - https://poeditor.com/kb/resx-editor
    - https://localizely.com/getting-started

- [ ] function signature file
- [ ] Represent error cases and illegal state in types intrinsic to your domain
      - https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions#represent-error-cases-and-illegal-state-in-types-intrinsic-to-your-domain
- [ ] Prefer namespaces at the top level
      - https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/conventions
- [ ] Add .NET 9 as a target framework
- (?) Cross-platform builds
      - https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions

