namespace uhppoted

module Uhppoted =
    let get_controllers () =
        printfn "uhppoted::get-controllers"
        let packet = Encode.get_controller_request 0
        UDP.broadcast packet
