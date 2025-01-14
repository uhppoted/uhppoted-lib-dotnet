# TODO

- [ ] Package for NuGet
      - [x] "klingon is an invalid culture identifier."
      - [x] Copy README on build/release (generate ?)
      - [x] README: github registry install instructions
      - [x] README: fix documentation links
      - [ ] Publish from github workflow
            - [x] Github packages
            - [x] nuget.org
            - [x] Check snupkg is uploaded
            - [ ] Remove version extension
      - [ ] Test install from NuGet
      - [ ] Test install from Github packages
      - [x] Clean up integration test warning on terminate

- [ ] github workflow
```
  Determining projects to restore...
  All projects are up-to-date for restore.
  stub -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/stub/bin/Debug/net8.0/stub.dll
  stub -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/stub/bin/Debug/net9.0/stub.dll
  uhppoted -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/uhppoted/uhppoted/bin/Debug/net8.0/uhppoted.dll
  uhppoted -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/uhppoted/uhppoted/bin/Debug/net9.0/uhppoted.dll
  fs -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/fs/bin/Debug/net8.0/fs.dll
  fs -> /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/fs/bin/Debug/net9.0/fs.dll
Test run for /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/fs/bin/Debug/net8.0/fs.dll (.NETCoreApp,Version=v8.0)
Test run for /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/fs/bin/Debug/net9.0/fs.dll (.NETCoreApp,Version=v9.0)
VSTest version 17.12.0 (x64)

VSTest version 17.12.0 (x64)

Starting test execution, please wait...
Starting test execution, please wait...
A total of 1 test files matched the specified pattern.
A total of 1 test files matched the specified pattern.
  ** WARN Address already in use ... retrying...
  Failed TestFindControllers [533 ms]
  Error Message:
   ReceiveError "Operation canceled"
  Stack Trace:
     at FSharp.Tests.TestFindAPI.TestFindControllers() in /home/runner/work/uhppoted-lib-dotnet/uhppoted-lib-dotnet/integration-tests/fs/UhppotedTest.fs:line 76
```

- [ ] API
    - [ ] _Cannot access a disposed object._
        - https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socket.receiveasync?view=net-8.0
        - https://stackoverflow.com/questions/41019997/udpclient-receiveasync-correct-early-termination/41041601?noredirect=1#comment69291144_41041601
        - https://stackoverflow.com/questions/1921611/c-how-do-i-terminate-a-socket-before-socket-beginreceive-calls-back
        - https://stackoverflow.com/questions/60083939/what-is-the-best-way-to-cancel-a-socket-receive-request

- [ ] examples
    - [ ] GCI
          - https://mauiman.dev/maui_cli_commandlineinterface.html


