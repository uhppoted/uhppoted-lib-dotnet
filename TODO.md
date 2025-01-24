# TODO

- [x] Package for NuGet
      - [x] "klingon is an invalid culture identifier."
      - [x] Copy README on build/release (generate ?)
      - [x] README: github registry install instructions
      - [x] README: fix documentation links
      - [x] Clean up integration test warning on terminate
      - [x] Publish from github workflow
      - [x] Test install from NuGet
      - [x] Test install from Github packages
      - [x] Test publish from release
      - [x] Test F# with published package
      - [x] Test C# with published package
      - [x] Test VB.NET with published package
      - [x] "hello world" examples
      - [ ] Version weirdness
```
warning NU1608: Detected package version outside of dependency constraint: uhppoted requires FSharp.Core (>= 8.0.0 && < 9.0.0) but version FSharp.Core 9.0.101 was resolved. [D:\a\uhppoted-lib-dotnet\uhppoted-lib-dotnet\uhppoted\uhppoted.sln]

warning NU1603: uhppoted depends on FSharp.Core (>= 8.0.0 && < 9.0.0) but FSharp.Core 8.0.0 was not found. FSharp.Core 8.0.100 was resolved instead. [TargetFramework=net9.0]
```

- [x] nightly/release builds: integration tests
```
 Failed TestGetCards [29 ms]
  Error Message:
   ReceiveError "port 60000 is invalid for a UDP bind address"
```
```
netsh int ipv4 set dynamicport tcp start=20000 num=10000
netsh int ipv4 set dynamicport udp start=20000 num=10000
```

- [ ] Release
      - [ ] Bump project and version
      - [ ] Update version in README and release notes