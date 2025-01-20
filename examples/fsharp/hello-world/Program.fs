open System.Net
open uhppoted

printfn "uhppoted-lib-dotnet"

let OPTIONS = OptionsBuilder()
                  .WithBind(new IPEndPoint(IPAddress.Any, 0))
                  .WithBroadcast(new IPEndPoint(IPAddress.Broadcast, 60000))
                  .WithTimeout(1000)
                  .WithDebug(true)
                  .Build()

match Uhppoted.FindControllers(OPTIONS) with
| Ok controllers ->
    printfn "find-controllers: %d" controllers.Length

    controllers
    |> Array.iter (fun v ->
        printfn "  controller %u" v.Controller
        printfn "    address  %A" v.Address
        printfn "    netmask  %A" v.Netmask
        printfn "    gateway  %A" v.Gateway
        printfn "    MAC      %A" v.MAC
        printfn "    version  %s" v.Version
        printfn "    date     %A" v.Date
        printfn "")

| Error err -> 
    printfn "*** ERROR %s" (Uhppoted.Translate err)
