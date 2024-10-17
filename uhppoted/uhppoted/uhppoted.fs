namespace uhppoted

open System

module Uhppoted =
    let dump (packet: byte array) =
        let hex = packet |> Array.map (fun (x: byte) -> String.Format("{0:X2}", x))

        for ix in 0..4 do
            let p = 16 * ix
            let q = 16 * ix + 7
            let left = String.Join(" ", hex.[p..q])
            let right = String.Join(" ", hex.[p + 8 .. q + 8])
            printfn "    %s  %s" left right

    let get_controllers () =
        printfn "uhppoted::get-controllers"
        let packet = Encode.get_controller_request 0
        dump packet
