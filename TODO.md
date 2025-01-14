# TODO

- [ ] Package for NuGet
      - [x] "klingon is an invalid culture identifier."
      - [x] Copy README on build/release (generate ?)
      - [ ] README: github registry install instructions
      - [ ] README: fix documentation links
      - [ ] Publish from github workflow
            - [ ] Github packages
            - [ ] nuget.org
            - [ ] Check snupkg is uploaded
            - [ ] Automate version
      - [ ] Test install from Github packages
      - [ ] Test install from NuGet
      - [ ] Clean up integration test warning on terminate
```
** ERROR/1 System.Net.Sockets.SocketException (995): The I/O operation has been aborted because of either a thread exit or an application request.
   at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException(SocketError error, CancellationToken cancellationToken)
   at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.System.Threading.Tasks.Sources.IValueTaskSource<System.Net.Sockets.SocketReceiveFromResult>.GetResult(Int16 token)
   at System.Threading.Tasks.ValueTask`1.ValueTaskSourceAsTask.<>c.<.cctor>b__4_0(Object state)
--- End of stack trace from previous location ---
   at System.Threading.Tasks.TaskToAsyncResult.End[TResult](IAsyncResult asyncResult)
   at System.Net.Sockets.Socket.EndReceiveFrom(IAsyncResult asyncResult, EndPoint& endPoint)
   at System.Net.Sockets.UdpClient.EndReceive(IAsyncResult asyncResult, IPEndPoint& remoteEP)
   at stub.Stub.recv@36-3.Invoke(IAsyncResult iar) in D:\a\uhppoted-lib-dotnet\uhppoted-lib-dotnet\integration-tests\stub\stub.fs:line 38
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


