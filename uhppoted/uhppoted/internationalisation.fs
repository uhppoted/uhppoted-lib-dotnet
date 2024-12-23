namespace uhppoted

open System.Resources
open System.Threading
open System.Globalization
open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("uhppoted.tests")>]
do ()

module internal internationalisation =
    let translate (e: string) : string =
        let rm =
            ResourceManager("uhppoted.strings", System.Reflection.Assembly.GetExecutingAssembly())

        rm.GetString(e)
