namespace uhppoted

open System.Resources
open System.Threading
open System.Globalization

module internal internationalisation =
    let translate () =
        Thread.CurrentThread.CurrentCulture <- CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture <- CultureInfo("en-US")

        let rm =
            ResourceManager("uhppoted.strings", System.Reflection.Assembly.GetExecutingAssembly())

        printfn ">>>>>>>>>>>>>>> translate %A" rm
        let s = rm.GetString("event.reason.1")
        printfn ">>>>>>>>>>>>>>> event.reason.1 %A" s
        printfn ""
