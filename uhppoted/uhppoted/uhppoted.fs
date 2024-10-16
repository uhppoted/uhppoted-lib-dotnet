namespace uhppoted

module Uhppoted =
    let dump (packet: byte array) =
        printf "    %s" (packet[0..7] |> Array.fold (fun state x -> state + sprintf "%02x " x) "")
        printf "%s\n" (packet[8..15] |> Array.fold (fun state x -> state + sprintf " %02x" x) "")
        printf "    %s" (packet[16..23] |> Array.fold (fun state x -> state + sprintf "%02x " x) "")
        printf "%s\n" (packet[24..31] |> Array.fold (fun state x -> state + sprintf " %02x" x) "")
        printf "    %s" (packet[32..39] |> Array.fold (fun state x -> state + sprintf "%02x " x) "")
        printf "%s\n" (packet[40..47] |> Array.fold (fun state x -> state + sprintf " %02x" x) "")
        printf "    %s" (packet[48..55] |> Array.fold (fun state x -> state + sprintf "%02x " x) "")
        printf "%s\n" (packet[56..] |> Array.fold (fun state x -> state + sprintf " %02x" x) "")

    let get_controllers () =
        printfn "uhppoted::get-controllers"
        let packet = Encode.get_controller_request 0
        dump packet
