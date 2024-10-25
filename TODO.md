# TODO

- [ ] get-all-controllers
      - [x] encode request packet
      - [x] UDP broadcast
      - [x] decode packets
      - [ ] pprint Option<DateOnly>
            - [x] F#
            - [ ] C#
            - [ ] VB
      - [ ] error handling
      - [ ] integration test

- [ ] get-controller
      - [x] UDP broadcast_to
            - [x] timeout OR receive
            - [x] recurse if packet is not 64 bytes
      - [x] return Result
      - [ ] UDP sendto
      - [ ] TCP send
      - [ ] Controller struct parameter

- [x] dump
- [ ] Add .NET 8 as a target framework on github
```
    <TargetFrameworks>net7.0;net8.0</TargetFrameworks>
```

## Notes
- https://stackoverflow.com/questions/10110174/best-approach-for-designing-f-libraries-for-use-from-both-f-and-c-sharp
- https://learn.microsoft.com/en-us/dotnet/fsharp/style-guide/component-design-guidelines
- https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/
- https://github.com/knocte/2fsharp/blob/master/csharp2fsharp.md
